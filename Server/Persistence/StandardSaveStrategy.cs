using System;
using System.Collections.Generic;
using Nelderim;
using Server.Guilds;

namespace Server
{
	public class StandardSaveStrategy : SaveStrategy
	{
		public static SaveOption SaveType = SaveOption.Normal;

		private readonly Queue<Item> _DecayQueue;

		public StandardSaveStrategy()
		{
			_DecayQueue = new Queue<Item>();
		}

		public enum SaveOption
		{
			Normal,
			Threaded
		}

		public override string Name => "Standard";

		protected bool PermitBackgroundWrite { get; set; }

		protected bool UseSequentialWriters => SaveType == SaveOption.Normal || !PermitBackgroundWrite;

		public override void Save(SaveMetrics metrics, bool permitBackgroundWrite)
		{
			PermitBackgroundWrite = permitBackgroundWrite;

			SaveMobiles(metrics);
			SaveItems(metrics);
			SaveGuilds(metrics);
			NExtension.SaveAll();

			if (permitBackgroundWrite && UseSequentialWriters)  //If we're permitted to write in the background, but we don't anyways, then notify.
				World.NotifyDiskWriteComplete();
		}

		public override void ProcessDecay()
		{
			while (_DecayQueue.Count > 0)
			{
				var item = _DecayQueue.Dequeue();

				if (item.OnDecay())
				{
					item.Delete();
				}
			}
		}

		protected void SaveMobiles(SaveMetrics metrics)
		{
			var mobiles = World.Mobiles;

			GenericWriter idx;
			GenericWriter tdb;
			GenericWriter bin;

			if (UseSequentialWriters)
			{
				idx = new BinaryFileWriter(World.MobileIndexPath, false);
				tdb = new BinaryFileWriter(World.MobileTypesPath, false);
				bin = new BinaryFileWriter(World.MobileDataPath, true);
			}
			else
			{
				idx = new AsyncWriter(World.MobileIndexPath, false);
				tdb = new AsyncWriter(World.MobileTypesPath, false);
				bin = new AsyncWriter(World.MobileDataPath, true);
			}

			idx.Write(mobiles.Count);

			foreach (var m in mobiles.Values)
			{
				var start = bin.Position;

				idx.Write(m.m_TypeRef);
				idx.Write(m.Serial);
				idx.Write(start);

				m.Serialize(bin);

				if (metrics != null)
				{
					metrics.OnMobileSaved((int)(bin.Position - start));
				}

				idx.Write((int)(bin.Position - start));

				m.FreeCache();
			}

			tdb.Write(World.m_MobileTypes.Count);

			for (var i = 0; i < World.m_MobileTypes.Count; ++i)
				tdb.Write(World.m_MobileTypes[i].FullName);

			idx.Close();
			tdb.Close();
			bin.Close();
		}

		protected void SaveItems(SaveMetrics metrics)
		{
			var items = World.Items;

			GenericWriter idx;
			GenericWriter tdb;
			GenericWriter bin;

			if (UseSequentialWriters)
			{
				idx = new BinaryFileWriter(World.ItemIndexPath, false);
				tdb = new BinaryFileWriter(World.ItemTypesPath, false);
				bin = new BinaryFileWriter(World.ItemDataPath, true);
			}
			else
			{
				idx = new AsyncWriter(World.ItemIndexPath, false);
				tdb = new AsyncWriter(World.ItemTypesPath, false);
				bin = new AsyncWriter(World.ItemDataPath, true);
			}

			idx.Write(items.Count);

			foreach (var item in items.Values)
			{
				if (item.Decays && item.Parent == null && item.Map != Map.Internal && (item.LastMoved + item.DecayTime) <= DateTime.UtcNow)
				{
					_DecayQueue.Enqueue(item);
				}

				var start = bin.Position;

				idx.Write(item.m_TypeRef);
				idx.Write(item.Serial);
				idx.Write(start);

				item.Serialize(bin);

				if (metrics != null)
				{
					metrics.OnItemSaved((int)(bin.Position - start));
				}

				idx.Write((int)(bin.Position - start));

				item.FreeCache();
			}

			tdb.Write(World.m_ItemTypes.Count);

			for (var i = 0; i < World.m_ItemTypes.Count; ++i)
				tdb.Write(World.m_ItemTypes[i].FullName);

			idx.Close();
			tdb.Close();
			bin.Close();
			Console.WriteLine("Finished saving items");
		}

		protected void SaveGuilds(SaveMetrics metrics)
		{
			GenericWriter idx;
			GenericWriter bin;

			if (UseSequentialWriters)
			{
				idx = new BinaryFileWriter(World.GuildIndexPath, false);
				bin = new BinaryFileWriter(World.GuildDataPath, true);
			}
			else
			{
				idx = new AsyncWriter(World.GuildIndexPath, false);
				bin = new AsyncWriter(World.GuildDataPath, true);
			}

			idx.Write(BaseGuild.List.Count);

			foreach (var guild in BaseGuild.List.Values)
			{
				var start = bin.Position;

				idx.Write(0);//guilds have no typeid
				idx.Write(guild.Id);
				idx.Write(start);

				guild.Serialize(bin);

				if (metrics != null)
				{
					metrics.OnGuildSaved((int)(bin.Position - start));
				}

				idx.Write((int)(bin.Position - start));
			}

			idx.Close();
			bin.Close();
		}
	}
}
