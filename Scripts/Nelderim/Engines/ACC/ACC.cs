#region References

using System;
using System.Collections.Generic;
using System.IO;

#endregion

namespace Server.ACC
{
	public partial class ACC
	{
		public static void Initialize()
		{
			EventSink.WorldSave += Save;
			Load();
		}

		public static Dictionary<string, bool> RegisteredSystems { get; private set; } = new Dictionary<string, bool>();

		public static void RegisterSystem(string system)
		{
			if (RegisteredSystems.ContainsKey(system))
				return;

			Type t = Type.GetType(system);
			if (t == null)
			{
				Console.WriteLine("Invalid System String: " + system);
				return;
			}

			ACCSystem sys = (ACCSystem)Activator.CreateInstance(t);
			if (sys != null)
			{
				RegisteredSystems.Add(system, true);
				Console.WriteLine("ACC Registered: " + system);
			}
		}

		public static bool SysEnabled(string system)
		{
			return RegisteredSystems.ContainsKey(system) && RegisteredSystems[system];
		}

		public static void DisableSystem(string system)
		{
			if (RegisteredSystems.ContainsKey(system))
			{
				Type t = ScriptCompiler.FindTypeByFullName(system);
				if (t != null)
				{
					if (!Directory.Exists("ACC Backups"))
						Directory.CreateDirectory("ACC Backups");

					ACCSystem sys = (ACCSystem)Activator.CreateInstance(t);
					if (sys != null)
					{
						sys.StartSave("ACC Backups/");
						sys.Disable();
					}

					RegisteredSystems[system] = false;
				}
			}
			else
				Console.WriteLine("Invalid System - {0} - Cannot disable.", system);
		}

		public static void EnableSystem(string system)
		{
			if (RegisteredSystems.ContainsKey(system))
			{
				Type t = ScriptCompiler.FindTypeByFullName(system);
				if (t != null)
				{
					if (!Directory.Exists("ACC Backups"))
						Directory.CreateDirectory("ACC Backups");

					ACCSystem sys = (ACCSystem)Activator.CreateInstance(t);
					if (sys != null)
					{
						sys.StartLoad("ACC Backups/");
						sys.Enable();
					}

					RegisteredSystems[system] = true;
				}
			}
			else
				Console.WriteLine("Invalid System - {0} - Cannot enable.", system);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			if (!Directory.Exists("Saves/ACC"))
				Directory.CreateDirectory("Saves/ACC");

			string filename = "acc.sav";
			string path = @"Saves/ACC/";
			string pathNfile = path + filename;
			DateTime start = DateTime.Now;

			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("----------");
			Console.WriteLine("Saving ACC...");

			try
			{
				using (FileStream m_FileStream = new FileStream(pathNfile, FileMode.OpenOrCreate, FileAccess.Write))
				{
					BinaryFileWriter writer = new BinaryFileWriter(m_FileStream, true);

					writer.Write(RegisteredSystems.Count);
					foreach (KeyValuePair<string, bool> kvp in RegisteredSystems)
					{
						Type t = ScriptCompiler.FindTypeByFullName(kvp.Key);
						if (t != null)
						{
							writer.Write(kvp.Key);
							writer.Write(kvp.Value);

							if (kvp.Value)
							{
								ACCSystem system = (ACCSystem)Activator.CreateInstance(t);
								if (system != null)
									system.StartSave(path);
							}
						}
					}

					writer.Close();
					m_FileStream.Close();
				}

				Console.WriteLine("Done in {0:F1} seconds.", (DateTime.Now - start).TotalSeconds);
				Console.WriteLine("----------");
				Console.WriteLine();
			}
			catch (Exception err)
			{
				Console.WriteLine("Failed. Exception: " + err);
			}
		}

		public static void Load()
		{
			if (!Directory.Exists("Saves/ACC"))
				return;

			string filename = "acc.sav";
			string path = @"Saves/ACC/";
			string pathNfile = path + filename;
			DateTime start = DateTime.Now;

			Console.WriteLine();
			Console.WriteLine("----------");
			Console.WriteLine("Loading ACC...");

			try
			{
				using (FileStream m_FileStream = new FileStream(pathNfile, FileMode.Open, FileAccess.Read))
				{
					BinaryReader m_BinaryReader = new BinaryReader(m_FileStream);
					BinaryFileReader reader = new BinaryFileReader(m_BinaryReader);

					if (RegisteredSystems == null)
						RegisteredSystems = new Dictionary<string, bool>();

					int Count = reader.ReadInt();
					for (int i = 0; i < Count; i++)
					{
						string system = reader.ReadString();
						Type t = ScriptCompiler.FindTypeByFullName(system);
						bool enabled = reader.ReadBool();

						if (t != null)
						{
							RegisteredSystems[system] = enabled;

							if (RegisteredSystems[system])
							{
								ACCSystem sys = (ACCSystem)Activator.CreateInstance(t);
								if (sys != null)
									sys.StartLoad(path);
							}
						}
					}

					reader.Close();
					m_FileStream.Close();
				}

				Console.WriteLine("Done in {0:F1} seconds.", (DateTime.Now - start).TotalSeconds);
				Console.WriteLine("----------");
				Console.WriteLine();
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed. Exception: " + e);
			}
		}
	}
}
