using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nelderim.Achievements
{
	public class AchievementRegistry<T> where T : IEntry
	{
		internal Dictionary<string, int> Index;
		internal Dictionary<int, T> Entries = new();

		private string _Name;
		private FileInfo _IndexFile;

		public AchievementRegistry(string name)
		{
			_Name = name;
			_IndexFile = new FileInfo($"{NExtension.BasePath}/{_Name}.csv");
		}

		public T Register(T entry)
		{
			int newIndex;
			if (!Index.TryGetValue(entry.Name, out newIndex))
			{
				newIndex = Index.Values.DefaultIfEmpty(0).Max() + 1;
				Index.Add(entry.Name, newIndex);
			}
			Entries.Add(newIndex, entry);
			entry.Id = newIndex;
			return entry;
		}

		public void Save()
		{
			if (!Directory.Exists(NExtension.BasePath))
			{
				Directory.CreateDirectory(NExtension.BasePath);
			}
			if (!_IndexFile.Exists)
			{
				using var fs = _IndexFile.Create();
				fs.Close();
			}
			File.WriteAllLines(_IndexFile.FullName, Index.Select(kvp => kvp.Value + "," + kvp.Key));
		}

		public void Load()
		{
			Index = new Dictionary<string, int>();
			if (_IndexFile.Exists)
			{
				var lines = File.ReadAllLines(_IndexFile.FullName);
				foreach (var line in lines)
				{
					var strings = line.Split(new[] { ',' }, 2);
					var id = Int32.Parse(strings[0]);
					var name = strings[1];
					Index.Add(name, id);
				}
			}
		}
	}
}
