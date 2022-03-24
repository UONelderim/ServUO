#region References

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Server;

#endregion

namespace Nelderim
{
	public abstract class NExtension<T> where T : NExtensionInfo, new()
	{
		protected static ConcurrentDictionary<Serial, T> m_ExtensionInfo = new ConcurrentDictionary<Serial, T>();

		public static bool Delete(IEntity entity)
		{
			return m_ExtensionInfo.TryRemove(entity.Serial, out _);
		}

		public static T Get(IEntity entity)
		{
			T result;
			if (!m_ExtensionInfo.TryGetValue(entity.Serial, out result))
			{
				result = new T { Serial = entity.Serial };
				m_ExtensionInfo.TryAdd(entity.Serial, result);
			}

			return result;
		}
		
		private static void Cleanup()
		{
			var toRemove = new List<Serial>();
			foreach (var kvp in m_ExtensionInfo)
				if (World.FindEntity(kvp.Key) == null)
					toRemove.Add(kvp.Key);

			foreach (var serial in toRemove)
				m_ExtensionInfo.TryRemove(serial, out _);
		}

		public static void Save(WorldSaveEventArgs args, string moduleName)
		{
			Cleanup();
			if (!Directory.Exists("Saves/Nelderim"))
				Directory.CreateDirectory("Saves/Nelderim");

			string pathNfile = @"Saves/Nelderim/" + moduleName + ".sav";

			Console.WriteLine(moduleName + ": Zapisywanie...");
			try
			{
				using (FileStream m_FileStream = new FileStream(pathNfile, FileMode.OpenOrCreate, FileAccess.Write))
				{
					BinaryFileWriter writer = new BinaryFileWriter(m_FileStream, true);

					writer.Write(1); //version
					writer.Write(m_ExtensionInfo.Count);
					foreach (T info in m_ExtensionInfo.Values)
					{
						writer.Write(info.Serial);
						info.Serialize(writer);
					}

					writer.Close();
					m_FileStream.Close();
				}
			}
			catch (Exception err)
			{
				Console.WriteLine("Failed. Exception: " + err);
			}
		}

		public static void Load(string moduleName)
		{
			if (!File.Exists(@"Saves/Nelderim/" + moduleName + ".sav"))
				moduleName = Char.ToLower(moduleName[0]) + moduleName.Substring(1); // 1st letter lowercase
			if (!File.Exists(@"Saves/Nelderim/" + moduleName + ".sav"))
				return;


			string pathNfile = @"Saves/Nelderim/" + moduleName + ".sav";

			Console.WriteLine(moduleName + ": Wczytywanie...");
			using (FileStream m_FileStream = new FileStream(pathNfile, FileMode.Open, FileAccess.Read))
			{
				BinaryReader m_BinaryReader = new BinaryReader(m_FileStream);
				BinaryFileReader reader = new BinaryFileReader(m_BinaryReader);

				if (m_ExtensionInfo == null)
					m_ExtensionInfo = new ConcurrentDictionary<Serial, T>();

				int version = reader.ReadInt();
				int count = reader.ReadInt();
				for (int i = 0; i < count; i++)
				{
					Serial serial = new Serial(reader.ReadInt());

					T info = new T { Serial = serial };

					if (version > 0)
						info.Fix = true;

					info.Deserialize(reader);

					m_ExtensionInfo[serial] = info;
				}

				reader.Close();
				m_BinaryReader.Close();
				m_FileStream.Close();
			}
		}
	}
}
