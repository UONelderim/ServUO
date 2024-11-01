#region References

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Server;

#endregion

namespace Nelderim
{
	public abstract class NExtension
	{
		public const string BasePath = "Saves/Nelderim";
		
		private static List<NExtension> m_Extensions = [];
		public static void Register(NExtension extension)
		{
			m_Extensions.Add(extension);
		}
		
		public static void Initialize()
		{
			foreach (var extension in m_Extensions)
				extension.Load();
		}
		
		public static void SaveAll()
		{
			foreach (var extension in m_Extensions)
				extension.Save();
		}

		public abstract void Load();
		public abstract void Save();
	}
	
	public abstract class NExtension<T> : NExtension where T : NExtensionInfo, new()
	{
		private string ModuleName;
		private static NExtension<T> Instance;
		public NExtension(string moduleName)
		{
			if(Instance != null)
				throw new Exception("Only one instance of NExtension allowed!");
			ModuleName = moduleName;
			Instance = this;
		}
		
		protected ConcurrentDictionary<Serial, T> ExtensionInfo = new();

		public static bool Delete(IEntity entity)
		{
			return Instance.ExtensionInfo.TryRemove(entity.Serial, out _);
		}

		public static T Get(IEntity entity)
		{
			return Instance.InternalGet(entity);
		}

		public virtual T InternalGet(IEntity entity)
		{
			T result;
			if (!ExtensionInfo.TryGetValue(entity.Serial, out result))
			{
				result = new T { Serial = entity.Serial };
				ExtensionInfo.TryAdd(entity.Serial, result);
			}

			return result;
		}
		
		private void Cleanup()
		{
			var toRemove = new List<Serial>();
			foreach (var serial in ExtensionInfo.Keys)
				if (World.FindEntity(serial) == null)
					toRemove.Add(serial);

			foreach (var serial in toRemove)
				ExtensionInfo.TryRemove(serial, out _);
		}

		public override void Save()
		{
			Cleanup();
			if (!Directory.Exists(BasePath))
				Directory.CreateDirectory(BasePath);

			string pathNfile = $"{BasePath}/{ModuleName}.sav";

			try
			{
				using (FileStream m_FileStream = new FileStream(pathNfile, FileMode.OpenOrCreate, FileAccess.Write))
				{
					BinaryFileWriter writer = new BinaryFileWriter(m_FileStream, true);

					writer.Write(1); //version
					writer.Write(ExtensionInfo.Count);
					foreach (T info in ExtensionInfo.Values)
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

		public override void Load()
		{
			if (!File.Exists($"{BasePath}/{ModuleName}.sav"))
				ModuleName = Char.ToLower(ModuleName[0]) + ModuleName.Substring(1); // 1st letter lowercase
			if (!File.Exists($"{BasePath}/{ModuleName}.sav"))
				return;


			string pathNfile = $"{BasePath}/{ModuleName}.sav";

			Console.Write(ModuleName + ": Wczytywanie...");
			using (FileStream m_FileStream = new FileStream(pathNfile, FileMode.Open, FileAccess.Read))
			{
				BinaryReader m_BinaryReader = new BinaryReader(m_FileStream);
				BinaryFileReader reader = new BinaryFileReader(m_BinaryReader);

				if (ExtensionInfo == null)
					ExtensionInfo = new ConcurrentDictionary<Serial, T>();

				int version = reader.ReadInt();
				int count = reader.ReadInt();
				for (int i = 0; i < count; i++)
				{
					Serial serial = new Serial(reader.ReadInt());

					T info = new T { Serial = serial };

					if (version > 0)
						info.Fix = true;

					info.Deserialize(reader);

					ExtensionInfo[serial] = info;
				}

				reader.Close();
				m_BinaryReader.Close();
				m_FileStream.Close();
			}
			Console.WriteLine("Done");
		}
	}
}
