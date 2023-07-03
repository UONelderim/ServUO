#region References

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Server;

#endregion

namespace Nelderim
{
	public class LanguagesDictionary
	{
		public static Dictionary<string, string> Jarlowy = new Dictionary<string, string>();
		public static Dictionary<string, string> Krasnoludzki = new Dictionary<string, string>();
		public static Dictionary<string, string> Elficki = new Dictionary<string, string>();
		public static Dictionary<string, string> Drowi = new Dictionary<string, string>();
		public static Dictionary<string, string> Orkowy = new Dictionary<string, string>();
		public static List<string> Demoniczny = new List<string>();
		public static List<string> Nieumarlych = new List<string>();
		public static List<string> Belkot = new List<string>();

		public static void Initialize()
		{
			FillDictionary("Jarlowy", Jarlowy);
			FillDictionary("Krasnoludzki", Krasnoludzki);
			FillDictionary("Elficki", Elficki);
			FillDictionary("Drowi", Drowi);
			FillDictionary("Orkowy", Orkowy);
			FillList("Demoniczny", Demoniczny);
			FillList("Nieumarlych", Nieumarlych);
			FillList("Belkot", Belkot);
		}

		private static void FillDictionary(string filename, Dictionary<string, string> dict)
		{
			var list = new List<string>();
			try
			{
				using var reader = new StreamReader(Path.Combine(Core.BaseDirectory,"Data", "Languages", $"{filename}.txt"));
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					list.Add(line);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
			}

			foreach (var line in list)
			{
				var parts = line.Split('=');
				if (parts.Length != 2) continue;
				dict[parts[0].ToLower()] = parts[1].ToLower();
			}
		}

		private static void FillList(string filename, List<string> list)
		{
			try
			{
				using StreamReader reader = new StreamReader(Path.Combine(Core.BaseDirectory,"Data", "Languages", $"{filename}.txt"));
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (line.Trim().Length != 0)
						list.Add(line.ToLower());
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
			}
		}
	}
}
