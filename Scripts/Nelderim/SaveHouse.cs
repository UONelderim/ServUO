using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Server.Commands;
using Server.Items;
using Server.Multis;
using Server.Targeting;

namespace Server
{
	public class SaveHouse
	{
		public static void Initialize()
		{
			CommandSystem.Register("SaveHouse", AccessLevel.Owner, SaveHouse_OnCommand);
			CommandSystem.Register("LoadHouse", AccessLevel.Owner, LoadHouse_OnCommand);
		}

		private static void SaveHouse_OnCommand(CommandEventArgs e)
		{
			if (e.Arguments.Length != 1)
			{
				e.Mobile.SendMessage("Usage: SaveHouse <name>");
				return;
			}

			var name = e.Arguments[0];
			
			e.Mobile.BeginTarget(12,
				false,
				TargetFlags.None,
				(from, targeted) =>
				{
					if (targeted is HouseSign sign && sign.Owner is BaseHouse house)
					{
						SaveHouseInternal(from, house, name);
					}
				});
		}

		private static void SaveHouseInternal(Mobile m, BaseHouse house, string name)
		{
			var idx = new BinaryFileWriter($"{name}.idx", false);
			var tdb = new BinaryFileWriter($"{name}.tdb", false);
			var bin = new BinaryFileWriter($"{name}.bin", true);

			var toSave = new List<Item>();
			toSave.Add(house);
			toSave.Add(house.Sign);
			toSave.AddRange(house.LockDowns.Keys);
			var securedItems = house.Secures.Select(s => s.Item).ToArray();
			foreach (var securedItem in securedItems)
			{
				AddItemsRecursively(toSave, securedItem);
			}
			toSave.AddRange(house.Carpets);
			toSave.AddRange(house.Addons.Keys);
			toSave.AddRange(house.Addons.Keys.OfType<BaseAddon>().SelectMany(a => a.Components));
			toSave.AddRange(house.Doors);
			if (house is HouseFoundation foundation)
			{
				toSave.Add(foundation.Signpost);
				toSave.Add(foundation.SignHanger);
				toSave.AddRange(foundation.Fixtures);
			}

			var filtered = toSave.Where(item => item != null).DistinctBy(i => i.Serial).ToList();
			
			idx.Write(filtered.Count);
			foreach (var item in filtered)
			{
					var start = bin.Position;

					idx.Write(item.TypeReference);
					idx.Write(item.Serial);
					idx.Write(start);

					item.Serialize(bin);
					idx.Write((int)(bin.Position - start));
			}

			var types = World.ItemTypes;
			tdb.Write(types.Count);

			for (var i = 0; i < types.Count; ++i)
				tdb.Write(types[i].FullName);
			
			idx.Close();
			tdb.Close();
			bin.Close();
			m.SendMessage("Done");
		}

		private static void AddItemsRecursively(List<Item> items, Item item)
		{
			items.Add(item);
			item.Items.ForEach(i => AddItemsRecursively(items, i));
		}

		private static void LoadHouse_OnCommand(CommandEventArgs args)
		{
			if (args.Arguments.Length != 1)
			{
				args.Mobile.SendMessage("Usage: LoadHouse <name>");
				return;
			}

			var name = args.Arguments[0];
			args.Mobile.SendMessage("Loading house: " + name);
			
			var items = new List<ItemEntry>();

			List<Tuple<ConstructorInfo, string>> types;

			using (var tdb = new FileStream($"{name}.tdb", FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				var tdbReader = new BinaryReader(tdb);
				types = ReadTypes(tdbReader);
				tdbReader.Close();
			}

			var serialMapping = new Dictionary<Serial, Serial>();
			var ctorArgs = new object[1];
			using (var idx = new FileStream($"{name}.idx", FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				var idxReader = new BinaryReader(idx);
				var itemCount = idxReader.ReadInt32();

				for (var i = 0; i < itemCount; ++i)
				{
					var typeID = idxReader.ReadInt32();
					var serial = new Serial(idxReader.ReadInt32());
					var pos = idxReader.ReadInt64();
					var length = idxReader.ReadInt32();
					
					if(World.Items.ContainsKey(serial))
					{
						var newSerial = Serial.NewItem;
						Console.WriteLine($"New serial for {serial}->{newSerial}");
						serialMapping[serial] = newSerial;
						serial = newSerial;
					}
					
					var objs = types[typeID];

					if (objs == null)
					{
						continue;
					}

					Item item = null;
					var ctor = objs.Item1;
					var typeName = objs.Item2;

					try
					{
						ctorArgs[0] = serial;
						item = (Item)ctor.Invoke(ctorArgs);
					}
					catch (Exception e)
					{
						Diagnostics.ExceptionLogging.LogException(e);
					}

					if (item != null)
					{
						items.Add(new ItemEntry(item, typeID, typeName, pos, length));
						World.AddItem(item);
					}
				}
				idxReader.Close();
			}
			
			using (var bin = new FileStream($"{name}.bin", FileMode.Open, FileAccess.Read, FileShare.Read, 81920))
			{
				var reader = new SerialFixingBinaryFileReader(new BinaryReader(bin), serialMapping);

				for (var i = 0; i < items.Count; ++i)
				{
					var entry = items[i];
					var item = entry.Item;

					if (item != null)
					{
						reader.Seek(entry.Position, SeekOrigin.Begin);

						try
						{
							item.Deserialize(reader);

							if (reader.Position != (entry.Position + entry.Length))
							{
								throw new Exception($"***** Bad serialize on {item.GetType()} *****");
							}
						}
						catch (Exception e)
						{
							items.RemoveAt(i);
							Console.WriteLine(e);
							break;
						}
					}
				}

				reader.Close();
			}
			args.Mobile.SendMessage("Done Loading");
	
			args.Mobile.SendEverything();

		}

		private sealed class ItemEntry
		{
			public Item Item { get; }

			public Serial Serial => Item == null ? Serial.MinusOne : Item.Serial;

			public int TypeID { get; }
			public string TypeName { get; }

			public long Position { get; }
			public int Length { get; }

			public ItemEntry(Item item, int typeID, string typeName, long pos, int length)
			{
				Item = item;
				TypeID = typeID;
				TypeName = typeName;
				Position = pos;
				Length = length;
			}
		}
		
		private static List<Tuple<ConstructorInfo, string>> ReadTypes(BinaryReader tdbReader)
		{
			var count = tdbReader.ReadInt32();

			var types = new List<Tuple<ConstructorInfo, string>>(count);

			for (var i = 0; i < count; ++i)
			{
				var typeName = tdbReader.ReadString();

				var t = ScriptCompiler.FindTypeByFullName(typeName);

				if (t == null)
				{
					Console.WriteLine("failed");

					if (!Core.Service)
					{
						Console.WriteLine($"Error: Type '{typeName}' was not found. Delete all of those types? (y/n)");
						
						if (Console.ReadKey(true).Key == ConsoleKey.Y)
						{
							types.Add(null);
							Utility.PushColor(ConsoleColor.Yellow);
							Console.Write("World: Loading...");
							Utility.PopColor();
							continue;
						}

						Console.WriteLine("Types will not be deleted. An exception will be thrown.");
					}
					else
					{
						Console.WriteLine($"Error: Type '{typeName}' was not found.");
					}

					throw new Exception($"Missing type '{typeName}'");
				}

				if (t.IsAbstract)
				{
					foreach (var at in ScriptCompiler.FindTypesByFullName(t.FullName))
					{
						if (at != t && !at.IsAbstract)
						{
							t = at;
							typeName = at.FullName;
							break;
						}
					}

					if (t.IsAbstract)
					{
						Console.WriteLine("failed");

						if (!Core.Service)
						{
							Console.WriteLine($"Error: Type '{typeName}' is abstract. Delete all of those types? (y/n)");

							if (Console.ReadKey(true).Key == ConsoleKey.Y)
							{
								types.Add(null);
								Utility.PushColor(ConsoleColor.Yellow);
								Console.Write("World: Loading...");
								Utility.PopColor();
								continue;
							}

							Console.WriteLine("Types will not be deleted. An exception will be thrown.");
						}
						else
						{
							Console.WriteLine($"Error: Type '{typeName}' is abstract.");
						}

						throw new Exception($"Abstract type '{typeName}'");
					}
				}

				var ctor = t.GetConstructor(new Type[1] { typeof(Serial) });

				if (ctor != null)
				{
					types.Add(new Tuple<ConstructorInfo, string>(ctor, typeName));
				}
				else
				{
					throw new Exception($"Type '{t}' does not have a serialization constructor");
				}
			}

			return types;
		}
	}
}
