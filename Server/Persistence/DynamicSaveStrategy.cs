using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nelderim;
using Server.Guilds;

namespace Server
{
	public sealed class DynamicSaveStrategy : SaveStrategy
	{
		private readonly ConcurrentBag<Item> _decayBag;

		private readonly BlockingCollection<QueuedMemoryWriter> _itemThreadWriters;
		private readonly BlockingCollection<QueuedMemoryWriter> _mobileThreadWriters;
		private readonly BlockingCollection<QueuedMemoryWriter> _guildThreadWriters;

		private SaveMetrics _metrics;

		private SequentialFileWriter _itemData, _itemIndex;
		private SequentialFileWriter _mobileData, _mobileIndex;
		private SequentialFileWriter _guildData, _guildIndex;

		public DynamicSaveStrategy()
		{
			_decayBag = new ConcurrentBag<Item>();

			_itemThreadWriters = new BlockingCollection<QueuedMemoryWriter>();
			_mobileThreadWriters = new BlockingCollection<QueuedMemoryWriter>();
			_guildThreadWriters = new BlockingCollection<QueuedMemoryWriter>();
		}

		public override string Name => "Dynamic";

		public override void Save(SaveMetrics metrics, bool permitBackgroundWrite)
		{
			_metrics = metrics;

			OpenFiles();

			Task[] saveTasks =
			[
				SaveItems(),
				SaveMobiles(),
				SaveGuilds(),
				Task.Factory.StartNew(NExtension.SaveAll)
			];

			SaveTypeDatabases();

			if (permitBackgroundWrite)
			{
				//This option makes it finish the writing to disk in the background, continuing even after Save() returns.
				Task.Factory.ContinueWhenAll(saveTasks, _ =>
				{
					CloseFiles();

					World.NotifyDiskWriteComplete();
				});
			}
			else
			{
				Task.WaitAll(saveTasks);    //Waits for the completion of all of the tasks(committing to disk)

				CloseFiles();
			}
		}

		public override void ProcessDecay()
		{
			while (_decayBag.TryTake(out var item))
			{
				if (item.OnDecay())
				{
					item.Delete();
				}
			}
		}

		private Task StartCommitTask(BlockingCollection<QueuedMemoryWriter> threadWriter, SequentialFileWriter data, SequentialFileWriter index)
		{
			var commitTask = Task.Factory.StartNew(() =>
			{
				while (!threadWriter.IsCompleted)
				{
					QueuedMemoryWriter writer;

					try
					{
						writer = threadWriter.Take();
					}
					catch (InvalidOperationException)
					{
						//Per MSDN, it's fine if we're here, successful completion of adding can rarely put us into this state.
						break;
					}

					writer.CommitTo(data, index);
				}
			});

			return commitTask;
		}

		private Task SaveItems()
		{
			//Start the blocking consumer; this runs in background.
			var commitTask = StartCommitTask(_itemThreadWriters, _itemData, _itemIndex);

			IEnumerable<Item> items = World.Items.Values;

			//Start the producer.
			Parallel.ForEach(items, () => new QueuedMemoryWriter(), (item, state, writer) =>
			{
				var startPosition = writer.Position;

				item.Serialize(writer);

				var size = (int)(writer.Position - startPosition);

				writer.QueueForIndex(item, size);

				if (item.Decays && item.Parent == null && item.Map != Map.Internal && DateTime.UtcNow > (item.LastMoved + item.DecayTime))
				{
					_decayBag.Add(item);
				}

				if (_metrics != null)
				{
					_metrics.OnItemSaved(size);
				}

				return writer;
			}, writer =>
			{
				writer.Flush();

				_itemThreadWriters.Add(writer);
			});

			_itemThreadWriters.CompleteAdding();    //We only get here after the Parallel.ForEach completes.  Lets our task 

			return commitTask;
		}

		private Task SaveMobiles()
		{
			//Start the blocking consumer; this runs in background.
			var commitTask = StartCommitTask(_mobileThreadWriters, _mobileData, _mobileIndex);

			IEnumerable<Mobile> mobiles = World.Mobiles.Values;

			//Start the producer.
			Parallel.ForEach(mobiles, () => new QueuedMemoryWriter(), (mobile, state, writer) =>
			{
				var startPosition = writer.Position;

				mobile.Serialize(writer);

				var size = (int)(writer.Position - startPosition);

				writer.QueueForIndex(mobile, size);

				if (_metrics != null)
				{
					_metrics.OnMobileSaved(size);
				}

				return writer;
			},
			writer =>
			{
				writer.Flush();

				_mobileThreadWriters.Add(writer);
			});

			_mobileThreadWriters.CompleteAdding();  //We only get here after the Parallel.ForEach completes.  Lets our task tell the consumer that we're done

			return commitTask;
		}

		private Task SaveGuilds()
		{
			//Start the blocking consumer; this runs in background.
			var commitTask = StartCommitTask(_guildThreadWriters, _guildData, _guildIndex);

			IEnumerable<BaseGuild> guilds = BaseGuild.List.Values;

			//Start the producer.
			Parallel.ForEach(guilds, () => new QueuedMemoryWriter(), (guild, state, writer) =>
			{
				var startPosition = writer.Position;

				guild.Serialize(writer);

				var size = (int)(writer.Position - startPosition);

				writer.QueueForIndex(guild, size);

				if (_metrics != null)
				{
					_metrics.OnGuildSaved(size);
				}

				return writer;
			}, writer =>
			{
				writer.Flush();

				_guildThreadWriters.Add(writer);
			});

			_guildThreadWriters.CompleteAdding();   //We only get here after the Parallel.ForEach completes.  Lets our task 

			return commitTask;
		}

		private void OpenFiles()
		{
			_itemData = new SequentialFileWriter(World.ItemDataPath, _metrics);
			_itemIndex = new SequentialFileWriter(World.ItemIndexPath, _metrics);

			_mobileData = new SequentialFileWriter(World.MobileDataPath, _metrics);
			_mobileIndex = new SequentialFileWriter(World.MobileIndexPath, _metrics);

			_guildData = new SequentialFileWriter(World.GuildDataPath, _metrics);
			_guildIndex = new SequentialFileWriter(World.GuildIndexPath, _metrics);

			WriteCount(_itemIndex, World.Items.Count);
			WriteCount(_mobileIndex, World.Mobiles.Count);
			WriteCount(_guildIndex, BaseGuild.List.Count);
		}

		private void CloseFiles()
		{
			_itemData.Close();
			_itemIndex.Close();

			_mobileData.Close();
			_mobileIndex.Close();

			_guildData.Close();
			_guildIndex.Close();
		}

		private void WriteCount(SequentialFileWriter indexFile, int count)
		{
			//Equiv to GenericWriter.Write( (int)count );
			var buffer = new byte[4];

			buffer[0] = (byte)count;
			buffer[1] = (byte)(count >> 8);
			buffer[2] = (byte)(count >> 16);
			buffer[3] = (byte)(count >> 24);

			indexFile.Write(buffer, 0, buffer.Length);
		}

		private void SaveTypeDatabases()
		{
			SaveTypeDatabase(World.ItemTypesPath, World.m_ItemTypes);
			SaveTypeDatabase(World.MobileTypesPath, World.m_MobileTypes);
		}

		private void SaveTypeDatabase(string path, ICollection<Type> types)
		{
			var bfw = new BinaryFileWriter(path, false);

			bfw.Write(types.Count);

			foreach (var type in types)
			{
				bfw.Write(type.FullName);
			}

			bfw.Flush();
			bfw.Close();
		}
	}
}
