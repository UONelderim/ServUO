#region References

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#endregion

namespace Server.Engines.BulkOrders
{
	public partial class SmallBulkEntry
	{
		private static readonly Dictionary<Type, int> m_HunterLevelCache = new Dictionary<Type, int>();

		public static int GetHunterBulkLevel(SmallBOD bod)
		{
			int result;
			if (bod != null && bod is SmallHunterBOD && m_HunterLevelCache.TryGetValue(bod.Type, out result))
				return result;
			Console.WriteLine("!!!WARNING!!!");
			Console.WriteLine($"Not found HunterBulkLevel for {bod}!!!");
			return 1;
		}

		public static int GetHunterBulkLevel(LargeBOD bod)
		{
			int result;
			if (bod != null && bod is LargeHunterBOD && bod.Entries != null && bod.Entries.Length > 0 &&
			    bod.Entries[0] != null && bod.Entries[0].Details != null)
				if (m_HunterLevelCache.TryGetValue(bod.Entries[0].Details.Type, out result))
					return result;
			Console.WriteLine("!!!WARNING!!!");
			Console.WriteLine($"Not found HunterBulkLevel for {bod}!!!");
			return 1;
		}

		static SmallBulkEntry()
		{
			//To fill HunterLevelCache
			_ = Hunter1;
			_ = Hunter2;
			_ = Hunter3;
		}

		public static SmallBulkEntry[] Hunter1
		{
			get { return GetHunterEntries("Hunting", "small-1-lvl"); }
		}

		public static SmallBulkEntry[] Hunter2
		{
			get { return GetHunterEntries("Hunting", "small-2-lvl"); }
		}

		public static SmallBulkEntry[] Hunter3
		{
			get { return GetHunterEntries("Hunting", "small-3-lvl"); }
		}

		public static SmallBulkEntry[] GetHunterEntries(string type, string name)
		{
			if (m_Cache == null)
				m_Cache = new Hashtable();

			Hashtable table = (Hashtable)m_Cache[type];

			if (table == null)
				m_Cache[type] = table = new Hashtable();

			SmallBulkEntry[] entries = (SmallBulkEntry[])table[name];

			if (entries == null)
			{
				table[name] = entries = LoadHunterEntries(type, name);
			}

			return entries;
		}

		public static SmallBulkEntry[] LoadHunterEntries(string type, string name)
		{
			return LoadHunterEntries(String.Format("Data/Bulk Orders/{0}/{1}.cfg", type, name));
		}

		public static SmallBulkEntry[] LoadHunterEntries(string path)
		{
			path = Path.Combine(Core.BaseDirectory, path);

			ArrayList list = new ArrayList();

			if (File.Exists(path))
			{
				using (StreamReader ip = new StreamReader(path))
				{
					string line;

					while ((line = ip.ReadLine()) != null)
					{
						if (line.Length == 0 || line.StartsWith("#"))
							continue;
						try
						{
							string[] split = line.Split('\t');

							if (split.Length == 3)
							{
								Type type = ScriptCompiler.FindTypeByName(split[0]);

								if (type != null)
								{
									var level = Utility.ToInt32(split[2]);

									if (m_HunterLevelCache.ContainsKey(type))
									{
										if (m_HunterLevelCache[type] != level)
											throw new InvalidDataException(
												"Niejednolita konfiguracja HunterBulk dla typu : " + type);
									}
									else
										m_HunterLevelCache.Add(type, level);

									list.Add(new SmallBulkEntry(type, Utility.ToInt32(split[1]), 0, 0));
								}
							}
						}
						catch (Exception e)
						{
							Console.WriteLine(e.ToString());
						}
					}
				}
			}

			return (SmallBulkEntry[])list.ToArray(typeof(SmallBulkEntry));
		}
	}

	public partial class LargeBulkEntry
	{
		public static SmallBulkEntry[] Animal_1
		{
			get { return GetHunterEntries("Hunting", "Animal_1"); }
		}

		public static SmallBulkEntry[] Animal_2
		{
			get { return GetHunterEntries("Hunting", "Animal_2"); }
		}

		public static SmallBulkEntry[] Ants
		{
			get { return GetHunterEntries("Hunting", "Ants"); }
		}

		public static SmallBulkEntry[] Elementals_1
		{
			get { return GetHunterEntries("Hunting", "Elementals_1"); }
		}

		public static SmallBulkEntry[] Elementals_2
		{
			get { return GetHunterEntries("Hunting", "Elementals_2"); }
		}

		public static SmallBulkEntry[] Gargoyles
		{
			get { return GetHunterEntries("Hunting", "Gargoyles"); }
		}

		public static SmallBulkEntry[] Horda_1
		{
			get { return GetHunterEntries("Hunting", "Horda_1"); }
		}

		public static SmallBulkEntry[] Horda_2
		{
			get { return GetHunterEntries("Hunting", "Horda_2"); }
		}

		public static SmallBulkEntry[] Horda_3
		{
			get { return GetHunterEntries("Hunting", "Horda_3"); }
		}

		public static SmallBulkEntry[] Jukas
		{
			get { return GetHunterEntries("Hunting", "Jukas"); }
		}

		public static SmallBulkEntry[] Kox_1
		{
			get { return GetHunterEntries("Hunting", "Kox_1"); }
		}

		public static SmallBulkEntry[] Kox_2
		{
			get { return GetHunterEntries("Hunting", "Kox_2"); }
		}

		public static SmallBulkEntry[] Kox_3
		{
			get { return GetHunterEntries("Hunting", "Kox_3"); }
		}

		public static SmallBulkEntry[] Kox_4
		{
			get { return GetHunterEntries("Hunting", "Kox_4"); }
		}

		public static SmallBulkEntry[] Kox_5
		{
			get { return GetHunterEntries("Hunting", "Kox_5"); }
		}

		public static SmallBulkEntry[] Mech
		{
			get { return GetHunterEntries("Hunting", "Mech"); }
		}

		public static SmallBulkEntry[] Minotaurs
		{
			get { return GetHunterEntries("Hunting", "Minotaurs"); }
		}

		public static SmallBulkEntry[] Ophidians
		{
			get { return GetHunterEntries("Hunting", "Ophidians"); }
		}

		public static SmallBulkEntry[] Orcs
		{
			get { return GetHunterEntries("Hunting", "Orcs"); }
		}

		public static SmallBulkEntry[] OreElementals
		{
			get { return GetHunterEntries("Hunting", "OreElementals"); }
		}

		public static SmallBulkEntry[] Plants
		{
			get { return GetHunterEntries("Hunting", "Plants"); }
		}

		public static SmallBulkEntry[] Rozne
		{
			get { return GetHunterEntries("Hunting", "Rozne"); }
		}

		public static SmallBulkEntry[] Strong
		{
			get { return GetHunterEntries("Hunting", "Strong"); }
		}

		public static SmallBulkEntry[] Terathans
		{
			get { return GetHunterEntries("Hunting", "Terathans"); }
		}

		public static SmallBulkEntry[] Undead_1
		{
			get { return GetHunterEntries("Hunting", "Undead_1"); }
		}

		public static SmallBulkEntry[] Undead_2
		{
			get { return GetHunterEntries("Hunting", "Undead_2"); }
		}

		public static SmallBulkEntry[] GetHunterEntries(string type, string name)
		{
			if (m_Cache == null)
			{
				m_Cache = new Hashtable();
			}

			Hashtable table = (Hashtable)m_Cache[type]; // lista zlecen dla danego typu rzemiosla

			if (table == null)
				m_Cache[type] = table = new Hashtable();

			SmallBulkEntry[] entries = (SmallBulkEntry[])table[name];

			if (entries == null)
			{
				if (!Directory.Exists("Logs"))
					Directory.CreateDirectory("Logs");

				string directory = "Logs/HunterBulkOrders";

				if (!Directory.Exists(directory))
					Directory.CreateDirectory(directory);

				StreamWriter m_Output = null;

				try
				{
					m_Output = new StreamWriter(Path.Combine(directory, "HunterBODs.log"), true);
					m_Output.AutoFlush = true;
					m_Output.WriteLine("#################################");
					m_Output.WriteLine("Log started on {0}", DateTime.Now);
					m_Output.WriteLine();
					m_Output.WriteLine("### LARGE BOD ###");
					m_Output.Close();
				}
				catch
				{
				}

				table[name] = entries = SmallBulkEntry.LoadHunterEntries(type, name);

				try
				{
					m_Output = new StreamWriter(Path.Combine(directory, "HunterBODs.log"), true);
					m_Output.AutoFlush = true;
					m_Output.WriteLine("### LARGE BOD END ###");
					m_Output.Close();
				}
				catch
				{
				}
			}

			return entries;
		}
	}
}
