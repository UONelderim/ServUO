using System;
using System.Collections.Generic;
using System.Linq;
using Nelderim;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Nelderim;
using Server.Nelderim.Misc;

namespace Server.Commands
{
	public class Migrate
	{
		public static void Initialize()
		{
			//TODO: CHANGE ME TO OWNER ONLY
			CommandSystem.Register("DoMigrate", AccessLevel.Administrator, DoMigrate);
		}

		private static void DoMigrate(CommandEventArgs e)
		{
			var from = e.Mobile;

			RenamePlayers();
			ConfigureMoongates(from);
			MigrateMalasDungeons(from);
			AlignSpawnersAndRespawn(from);
		}

		private static void ConfigureMoongates(Mobile from)
		{
			//New Haven -> RaceRoom
			var newHavenMap = Map.Trammel;
			var newHavenPortalLocation = new Point3D(3506, 2574, 18);
			var raceRoomMap = Map.Felucca;
			var raceRoomStartLocation = new Point3D(5176, 230, 0);
			var raceRoomPortalLocation = new Point3D(5156, 254, 50);
			from.SendMessage("Cleaning space for newHaven Portal");
			foreach (var item in newHavenMap.FindItems<Item>(newHavenPortalLocation).ToArray())
			{
				from.SendMessage($"Removing {item}@{item.Location}");
				item.Delete();
			}

			from.SendMessage("Creating newHaven Portal");
			var newHavenMoongate = new ConfirmationMoongate()
			{
				GumpWidth = 300,
				GumpHeight = 200,
				TitleColor = 0xFFFFFF,
				MessageColor = 0xFFFFFF,
				TitleNumber = 1124183,
				MessageNumber = 1124184,
				TargetMap = raceRoomMap,
				Target = raceRoomStartLocation
			};
			newHavenMoongate.MoveToWorld(newHavenPortalLocation, newHavenMap);

			from.SendMessage("Cleaning space for RaceRoom Portal");
			foreach (var item in raceRoomMap.FindItems<Item>(raceRoomPortalLocation).ToArray())
			{
				from.SendMessage($"Removing {item}@{item.Location}");
				item.Delete();
			}

			from.SendMessage("Creating RaceRoom Portal");
			var raceRoomMoongate = new RaceRoomMoongate
			{
				//It have to go somewhere to not give an error
				TargetMap = raceRoomMap, Target = raceRoomPortalLocation
			};
			raceRoomMoongate.MoveToWorld(raceRoomPortalLocation, raceRoomMap);
		}

		private static void RefundLanguages(PlayerMobile pm)
		{
			foreach (var lang in Enum.GetValues<NLanguage>())
			{
				if (lang is NLanguage.Belkot or NLanguage.Powszechny)
					continue;
				if (pm.Race.DefaultLanguages.ContainsKey(lang))
					continue;
				var value = pm.LanguagesKnown[lang];
				if (value == 1000)
				{
					pm.LanguagesKnown[lang] = 0;
					pm.QuestPoints += 150;
					pm.QuestPointsHistory.Add(
						new QuestPointsHistoryEntry(
							DateTime.Now,
							"Nelderim",
							150,
							$"Zwrot PD za jezyk: {lang}",
							pm.Name));
				}
			}
		}

		private static void RenamePlayers()
		{
			foreach (var pm in PlayerMobile.Instances)
			{
				if (pm.IsStaff())
					continue;
				RefundLanguages(pm);
				pm.Kills = 0;
				pm.Fame = 0;
				pm.Karma = 0;
				if (pm.Account is Account a)
				{
					for (int i = 0; i < a.Length; i++)
					{
						if (a[i] == pm)
						{
							pm.Name = $"{RenameGump.DEFAULT_PREFIX} {i + 1}";
							break;
						}
					}
				}

				if (!pm.Name.StartsWith(RenameGump.DEFAULT_PREFIX))
				{
					pm.Name = RenameGump.DEFAULT_PREFIX;
				}

				pm.Profile = "";
				var characterSheet = CharacterSheet.Get(pm);
				characterSheet.AppearanceAndCharacteristic = "";
				characterSheet.HistoryAndProfession = "";
				pm.Race = Race.None;
				pm.Faction = Faction.None;

				pm.LogoutMap = AccountHandler.StartingCities[0].Map;
				pm.LogoutLocation = AccountHandler.StartingCities[0].Location;
				BaseMount.Dismount(pm);
				//Stable all pets
				if (pm.AllFollowers.Count > 0)
				{
					for (int i = pm.AllFollowers.Count - 1; i >= 0; --i)
					{
						var pet = pm.AllFollowers[i] as BaseCreature;

						if (pet == null)
						{
							continue;
						}

						pet.ControlTarget = null;
						pet.ControlOrder = OrderType.Stay;
						pet.Internalize();

						pet.SetControlMaster(null);
						pet.SummonMaster = null;

						pet.IsStabled = true;
						pet.StabledBy = null;

						pm.Stabled.Add(pet);
					}
				}
			}
		}

		public record DungMigrationInfo(string name, int x1, int y1, int x2, int y2, int xto, int yto);

		public static List<DungMigrationInfo> dungInfos =
		[
			new("elghin", 0, 0, 231, 367, 6144, 0),
			new("melisande & shimmering", 0, 688, 407, 1023, 6376, 0),
			new("grizzle & parox", 0, 1112, 175, 1375, 6784, 0),
			new("vox", 64, 1776, 175, 1927, 6960, 0),
			new("zoo", 552, 1880, 607, 1951, 6280, 280),
			new("talus", 2000, 1848, 2199, 2047, 6144, 600),
			new("arena", 2232, 1792, 2551, 2047, 6144, 800),
			new("wampir1", 0, 480, 95, 631, 6376, 192),
			new("wampir2", 0, 1936, 79, 2047, 7072, 0),
			new("lotharn", 0, 384, 55, 431, 6480, 168)
		];

		public static List<DungMigrationInfo> additonalFixes =
		[
			new("melisandeFix", 6376, 0, 6456, 180, 6386, 0), //Melisande x+10
			new("shimmeringFix1", 6468, 0, 6663, 165, 6478, 0), //ShimmeringLv1 x+10
			new("shimmeringFix2", 6629, 171, 6793, 330, 6649, 0), //ShimmeringLv2 x+20, y-171
		];

		private static void MigrateMalasDungeons(Mobile from)
		{
			foreach (var dmi in dungInfos)
			{
				Console.WriteLine("Migrating " + dmi.name);
				MigrateDungeon(dmi, Map.Malas, Map.Felucca);
			}
			Console.WriteLine("Applying additional fixes");
			foreach (var dmi in additonalFixes)
			{
				Console.WriteLine("Migrating " + dmi.name);
				MigrateDungeon(dmi, Map.Felucca, Map.Felucca);
			}
		}

		private static void MigrateDungeon(DungMigrationInfo dmi, Map fromMap, Map toMap)
		{
			var width = dmi.x2 - dmi.x1;
			var height = dmi.y2 - dmi.y1;
			var xDiff = dmi.xto - dmi.x1;
			var yDiff = dmi.yto - dmi.y1;
			var items = fromMap.GetItemsInBounds(new Rectangle2D(dmi.x1, dmi.y1, width, height));
			foreach (var item in items)
			{
				var newX = item.X + xDiff;
				var newY = item.Y + yDiff;
				item.MoveToWorld(new Point3D(newX, newY, item.Z), toMap);
				if (item is XmlSpawner spawner)
				{
					foreach (var spawnObject in spawner.SpawnObjects)
					{
						var text = spawnObject.TypeName.ToLower();
						var mapKey = "";
						var pointKey = "";
						if (text.StartsWith("teleporter"))
						{
							mapKey = "mapdest";
							pointKey = "pointdest";
						}

						if (text.StartsWith("moongate"))
						{
							mapKey = "targetmap";
							pointKey = "target";
						}

						if (mapKey != "" && pointKey != "")
						{
							text = ReplaceToken(text, mapKey, fromMap.Name.ToLower(), toMap.Name.ToLower());
							var pointText = GetTokenValue(text,
								pointKey,
								out _,
								out _);
							if(pointText != "")
							{
								var point = Point3D.Parse(pointText);
								point = new Point3D(point.X + xDiff, point.Y + yDiff, point.Z);
								text = ReplaceToken(text, pointKey, pointText, point.ToString());
							}
						}

						var x = GetTokenValue(text, "x", out _, out _);
						if (x != "")
						{
							text = ReplaceToken(text, "x", x, (int.Parse(x) + xDiff).ToString());
						}

						var y = GetTokenValue(text, "y", out _, out _);
						if (y != "")
						{
							text = ReplaceToken(text, "y", y, (int.Parse(y) + yDiff).ToString());
						}
						spawnObject.TypeName = text;
					}

					spawner.Respawn();
				}
				if (item is BossSpawner bossSpawner)
				{
					if (bossSpawner.SealTargetMap == fromMap)
					{
						bossSpawner.SealTargetMap = toMap;
						bossSpawner.SealTargetLocation = new Point3D(
							bossSpawner.SealTargetLocation.X + xDiff,
							bossSpawner.SealTargetLocation.Y + yDiff,
							bossSpawner.SealTargetLocation.Z);
					}
				}
				
			}

			items.Free();
		}

		private static void AlignSpawnersAndRespawn(Mobile from)
		{
			var spawners = World.Items.OfType<XmlSpawner>();
			foreach (var spawner in spawners)
			{
				spawner.RegionName = spawner.RegionName switch
				{
					"Paroxumus_VeryEasy" => "Parox_VeryEasy",
					"Paroxymus_Easy" => "Parox_Easy",
					"Paroxumus_Medium" => "Parox_Medium",
					"Paroxumus_Difficult" => "Parox_Difficult",
					"Paroxysmus_VeryDifficult" => "Parox_VeryDifficult",
					"Voxy_VeryEasy" => "VoxPopuli_VeryEasy",
					"Voxy_Easy" => "VoxPopuli_Easy",
					"Voxy_Medium" => "VoxPopuli_Medium",
					"Voxy_Difficult" => "VoxPopuli_Difficult",
					"Voxy_VeryDifficult" => "VoxPopuli_VeryDifficult"
					
				};
				foreach (var spawnObject in spawner.SpawnObjects)
				{
					var text = spawnObject.TypeName.ToLower();
					text = ReplaceType(text, "wladcapiaskowboss", "wladcapiaskow");
					text = ReplaceType(text, "NelderimSkeletalDragon", "NSkeletalDragon");
					text = ReplaceType(text, "orccamp", "prisonercamp");
					text = ReplaceType(text, "ratcamp", "prisonercamp");
					if (text.StartsWith("raceteleporter") && spawner.Name == "KomnatyStworzenia")
					{
						text = "raceroommoongate";
					}
					spawnObject.TypeName = text;
				}

				if (spawner.Running)
					spawner.Respawn();
			}
		}
		
		//HELPERS
		private static string GetTokenValue(string text, string key, out int startIndex, out int length)
		{
			var fullKey = $"/{key}/";
			var tokenStartIndex = text.IndexOf(fullKey, StringComparison.Ordinal);
			if (tokenStartIndex == -1)
			{
				startIndex = -1;
				length = 0;
				return "";
			}

			startIndex = tokenStartIndex + fullKey.Length;
			var tokenEndIndex = text.IndexOf('/', startIndex);
			length = tokenEndIndex == -1 ? text.Length - startIndex : tokenEndIndex - startIndex;
			return text.Substring(startIndex, length);
		}

		private static string ReplaceToken(string text, string key, string oldValue, string newValue)
		{
			var tokenValue = GetTokenValue(text, key, out var startIndex, out var length);
			if (tokenValue.ToLower() == oldValue)
			{
				return text[..startIndex] + newValue + text.Substring(startIndex + length);
			}

			return text;
		}
		
		static string ReplaceType(string text, string oldType, string newType)
		{
			var index = text.IndexOf(oldType + '/', StringComparison.OrdinalIgnoreCase);
			if (index == -1)
			{
				return text;
			}
			return newType + text.Substring(index + oldType.Length);
		}
	}
}
