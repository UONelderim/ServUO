using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nelderim;
using Nelderim.Towns;
using Server.Accounting;
using Server.Engines.CannedEvil;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Nelderim;

namespace Server.Commands
{
	public class Migrate
	{
		private static Dictionary<int, int> _HueMap = new()
		{
			{ 1457, 2663 },
			{ 1458, 2664 },
			{ 1459, 2665 },
			{ 1460, 2666 },
			{ 1461, 2667 },
			{ 1462, 2668 },
			{ 1463, 2669 },
			{ 1464, 2670 },
			{ 1755, 1488 },
			{ 1756, 1489 },
			{ 1757, 1490 },
			{ 1758, 1491 },
			{ 1759, 1492 },
			{ 1760, 1493 },
			{ 1761, 1494 },
			{ 1762, 1495 },
			{ 1763, 1496 },
			{ 1764, 1497 },
			{ 1765, 1498 },
			{ 1766, 1499 },
			{ 1767, 1500 },
			{ 1872, 1455 },
			{ 1909, 1583 },
			{ 1910, 1584 },
			{ 1911, 1585 },
			{ 1912, 1586 },
			{ 1913, 1587 },
			{ 1914, 1588 },
			{ 1915, 1589 },
			{ 1916, 1590 },
			{ 1917, 1591 },
			{ 1918, 1592 },
			{ 1919, 1593 },
			{ 1920, 1594 },
			{ 1921, 1595 },
			{ 1922, 1596 },
			{ 1923, 1597 },
			{ 1924, 1598 },
			{ 1925, 1599 },
			{ 1926, 1600 },
			{ 1927, 2131 },
			{ 1928, 2132 },
			{ 1929, 2133 },
			{ 1930, 2134 },
			{ 1931, 2135 },
			{ 1932, 2136 },
			{ 1933, 2137 },
			{ 1934, 2138 },
			{ 1935, 2139 },
			{ 1936, 2140 },
			{ 1937, 2141 },
			{ 1938, 2142 },
			{ 1939, 2143 },
			{ 1940, 2144 },
			{ 1941, 2077 },
			{ 1942, 2078 },
			{ 1943, 2079 },
			{ 1944, 2080 },
			{ 1945, 2081 },
			{ 1946, 2082 },
			{ 1947, 2083 },
			{ 1948, 2084 },
			{ 1949, 2085 },
			{ 1950, 2086 },
			{ 1951, 2087 },
			{ 1952, 2088 },
			{ 1953, 2089 },
			{ 1954, 1282 },
			{ 1955, 1283 },
			{ 1956, 1284 },
			{ 1957, 1285 },
			{ 1958, 1286 },
			{ 1959, 1287 },
			{ 1960, 1288 },
			{ 1961, 1289 },
			{ 1962, 1290 },
			{ 1963, 1291 },
			{ 1964, 1292 },
			{ 1965, 1293 },
			{ 1966, 1294 },
			{ 1967, 1295 },
			{ 1968, 1296 },
			{ 1969, 1297 },
			{ 1970, 1298 },
			{ 2057, 1780 },
			{ 2058, 1781 },
			{ 2059, 1782 },
			{ 2060, 1783 },
			{ 2061, 1784 },
			{ 2062, 1785 },
			{ 2063, 1786 },
			{ 2064, 1787 },
			{ 2065, 1788 },
			{ 2066, 1789 },
			{ 2067, 1790 },
			{ 2068, 1791 },
			{ 2069, 1792 },
			{ 2070, 1793 },
			{ 2071, 1794 },
			{ 2072, 1795 },
			{ 2073, 1796 },
			{ 2074, 1797 },
			{ 2075, 1798 },
			{ 2076, 1799 },
			{ 2077, 1800 },
			{ 2078, 1279 },
			{ 2079, 1280 },
			{ 2498, 2225 },
			{ 2499, 2226 },
			{ 2500, 2227 },
			{ 2501, 2228 },
			{ 2502, 2229 },
			{ 2503, 2230 },
			{ 2504, 2231 },
			{ 2505, 2232 },
			{ 2506, 2233 },
			{ 2507, 2234 },
			{ 2508, 2235 },
			{ 2509, 2236 },
			{ 2510, 2237 },
			{ 2511, 2238 },
			{ 2512, 2239 },
			{ 2513, 2240 },
			{ 2514, 2241 },
			{ 2515, 2242 },
			{ 2516, 2243 },
			{ 2517, 2244 },
			{ 2518, 2245 },
			{ 2519, 2246 },
			{ 2520, 2247 },
			{ 2521, 2248 },
			{ 2522, 2249 },
			{ 2523, 2250 },
			{ 2524, 2251 },
			{ 2525, 2252 },
			{ 2526, 2253 },
			{ 2527, 2254 },
			{ 2528, 2255 },
			{ 2529, 2256 },
			{ 2530, 2257 },
			{ 2531, 2258 },
			{ 2532, 2259 },
			{ 2533, 2260 },
			{ 2534, 2261 },
			{ 2535, 2262 },
			{ 2536, 2263 },
			{ 2537, 2264 },
			{ 2538, 2265 },
			{ 2539, 2266 },
			{ 2540, 2267 },
			{ 2541, 2268 },
			{ 2542, 2269 },
			{ 2543, 2270 },
			{ 2544, 2271 },
			{ 2545, 2272 },
			{ 2546, 2273 },
			{ 2547, 2274 },
			{ 2548, 2275 },
			{ 2549, 2276 },
			{ 2550, 2277 },
			{ 2551, 2278 },
			{ 2552, 2279 },
			{ 2553, 2280 },
			{ 2554, 2281 },
			{ 2555, 2282 },
			{ 2556, 2283 },
			{ 2557, 2284 },
			{ 2558, 2285 },
			{ 2559, 2286 },
			{ 2560, 2287 },
			{ 2561, 2288 },
			{ 2562, 2289 },
			{ 2563, 2290 },
			{ 2564, 2291 },
			{ 2565, 2292 },
			{ 2566, 2293 },
			{ 2567, 2294 },
			{ 2568, 2295 },
			{ 2569, 2296 },
			{ 2570, 2297 },
			{ 2571, 2298 },
			{ 2572, 2299 },
			{ 2573, 2300 },
			{ 2574, 1059 },
			{ 2575, 1060 },
			{ 2576, 1061 },
			{ 2577, 1062 },
			{ 2578, 1063 },
			{ 2579, 1064 },
			{ 2580, 1065 },
			{ 2581, 1066 },
			{ 2582, 1067 },
			{ 2583, 1068 },
			{ 2584, 1069 },
			{ 2585, 1070 },
			{ 2586, 1071 },
			{ 2587, 1072 },
			{ 2588, 1073 },
			{ 2589, 1074 },
			{ 2590, 1075 },
			{ 2591, 1076 },
			{ 2592, 1077 },
			{ 2593, 1078 },
			{ 2594, 1079 },
			{ 2595, 1080 },
			{ 2596, 1081 },
			{ 2597, 1082 },
			{ 2598, 1083 },
			{ 2599, 1084 },
			{ 2600, 1085 },
			{ 2601, 1086 },
			{ 2602, 1087 },
			{ 2603, 1088 },
			{ 2604, 1089 },
			{ 2605, 1090 },
			{ 2606, 1091 },
			{ 2607, 1092 },
			{ 2608, 1093 },
			{ 2609, 1094 },
			{ 2610, 1095 },
			{ 2611, 1096 },
			{ 2612, 1097 },
			{ 2613, 1098 },
			{ 2614, 1099 },
			{ 2615, 2161 },
			{ 2616, 2162 },
			{ 2617, 2163 },
			{ 2618, 2164 },
			{ 2619, 2165 },
			{ 2620, 2166 },
			{ 2621, 2167 },
			{ 2622, 2168 },
			{ 2623, 2169 },
			{ 2624, 2170 },
			{ 2625, 2171 },
			{ 2626, 2172 },
			{ 2627, 2173 },
			{ 2628, 2174 },
			{ 2629, 2175 },
			{ 2630, 2176 },
			{ 2631, 2177 },
			{ 2632, 2178 },
			{ 2633, 2179 },
			{ 2634, 2180 },
			{ 2635, 2181 },
			{ 2636, 2182 },
			{ 2637, 2183 },
			{ 2638, 2184 },
			{ 2639, 2185 },
			{ 2640, 2186 },
			{ 2641, 2187 },
			{ 2642, 2188 },
			{ 2643, 2189 },
			{ 2644, 2190 },
			{ 2645, 2191 },
			{ 2646, 2192 },
			{ 2647, 2193 },
			{ 2648, 2194 },
			{ 2649, 2195 },
			{ 2650, 2196 },
			{ 2651, 2197 },
			{ 2652, 2198 },
			{ 2653, 2199 },
			{ 2654, 2200 },
			{ 2655, 1671 },
			{ 2656, 1672 },
			{ 2657, 1673 },
			{ 2658, 1674 },
			{ 2659, 1675 },
			{ 2660, 1676 },
			{ 2661, 1677 },
			{ 2662, 1678 },
			{ 2663, 1679 },
			{ 2664, 1680 },
			{ 2665, 1681 },
			{ 2666, 1682 },
			{ 2667, 1683 },
			{ 2668, 1684 },
			{ 2669, 1685 },
			{ 2670, 1686 },
			{ 2671, 1687 },
			{ 2672, 1688 },
			{ 2673, 1689 },
			{ 2674, 1690 },
			{ 2675, 1691 },
			{ 2676, 1692 },
			{ 2677, 1693 },
			{ 2678, 1694 },
			{ 2679, 1695 },
			{ 2680, 1696 },
			{ 2681, 1697 },
			{ 2682, 1698 },
			{ 2683, 1699 },
			{ 2684, 1700 },
			{ 2685, 1555 },
			{ 2686, 1556 },
			{ 2687, 1557 },
			{ 2688, 1558 },
			{ 2689, 1559 },
			{ 2690, 1560 },
			{ 2691, 1561 },
			{ 2692, 1562 },
			{ 2693, 1563 },
			{ 2694, 1564 },
			{ 2695, 1565 },
			{ 2696, 1566 },
			{ 2697, 1567 },
			{ 2698, 1178 },
			{ 2699, 1179 },
			{ 2700, 1180 },
			{ 2701, 1181 },
			{ 2702, 1182 },
			{ 2703, 1183 },
			{ 2704, 1184 },
			{ 2705, 1185 },
			{ 2706, 1186 },
			{ 2707, 1187 },
			{ 2708, 1188 },
			{ 2709, 1189 },
			{ 2717, 1456 },
			{ 2947, 1379 },
			{ 2948, 1380 },
			{ 2949, 1381 },
			{ 2950, 1382 },
			{ 2951, 1383 },
			{ 2952, 1384 },
			{ 2953, 1385 },
			{ 2954, 1386 },
			{ 2955, 1387 },
			{ 2956, 1388 },
			{ 2957, 1389 },
			{ 2958, 1390 },
			{ 2959, 1391 },
			{ 2960, 1392 },
			{ 2961, 1393 },
			{ 2962, 1394 },
			{ 2963, 1395 },
			{ 2964, 1396 },
			{ 2965, 1397 },
			{ 2966, 1398 },
			{ 2967, 1399 },
			{ 2968, 1400 },
			{ 2969, 1998 },
			{ 2970, 1999 }
		};

		private static Dictionary<int, int> _ArtMap = new() { {643, 5190},
			{644, 5191},
			{645, 5192},
			{646, 5194},
			{647, 5195},
			{648, 5196},
			{649, 5197},
			{650, 5275},
			{769, 5276},
			{770, 5277},
			{771, 5278},
			{772, 5401},
			{773, 5416},
			{774, 5417},
			{775, 5418},
			{776, 5419},
			{777, 5411},
			{778, 5427},
			{779, 5428},
			{780, 5466},
			{781, 5467},
			{782, 5732},
			{784, 5744},
			{785, 5745},
			{1027, 5777},
			{1028, 5778},
			{1029, 5779},
			{1030, 5813},
			{1031, 5814},
			{1032, 5815},
			{1033, 5850},
			{1034, 5851},
			{1168, 5816},
			{2188, 5852},
			{2301, 5853},
			{2302, 5854},
			{2303, 5855},
			{2304, 5856},
			{2305, 5857},
			{2306, 5858},
			{2307, 5859},
			{2308, 5860},
			{2309, 5861},
			{2310, 5862},
			{2311, 5863},
			{2312, 5864},
			{2313, 5865},
			{2314, 5866},
			{2315, 5867},
			{2316, 5868},
			{3511, 5869},
			{3524, 5870},
			{3525, 5871},
			{3563, 5872},
			{3564, 5873},
			{3565, 5874},
			{3566, 5875},
			{3664, 5876},
			{3665, 5877},
			{3666, 5878},
			{3667, 5879},
			{3760, 5897},
			{3902, 5898}
		};

		public static void Initialize()
		{
			//TODO: CHANGE ME TO OWNER ONLY
			CommandSystem.Register("DoMigrate", AccessLevel.Administrator, DoMigrate);
			CommandSystem.Register("LockAccounts", AccessLevel.Administrator, LockAccounts);
		}

		private static void DoMigrate(CommandEventArgs e)
		{
			var from = e.Mobile;

			RenamePlayers();
			ConfigureMoongates(from);
			MigrateMalasDungeons(from);
			AlignTeleportersToMalasDungeons(from);
			AlignSpawnersAndRespawn(from);
			FixChampionSpawns(from);
			MigrateArtAndHues(from);
			RemoveCrafter(from);
			from.SendMessage("Done migrating!");
		}

		private static void LockAccounts(CommandEventArgs e)
		{
			var from = e.Mobile;
			var accountsFile = Path.Combine(Core.BaseDirectory, "Accounts.txt");
			var accountsText = "";
			if (File.Exists(accountsFile))
				accountsText = File.ReadAllText(accountsFile).ToLower();
			var allowedAccountNames =
				accountsText.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
			foreach (var account in Accounts.GetAccounts())
			{
				if (from.Account == account)
					account.Banned = false;
				else if (allowedAccountNames.Contains(account.Username.ToLower()))
				{
					account.Banned = false;
					from.SendMessage("Allowed account: " + account.Username);
				}
				else
					account.Banned = true;
			}

			from.SendMessage("Done");
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
				pm.Race.AssignDefaultLanguages(pm);
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

				TownDatabase.LeaveCurrentTown(pm);
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
			new("melisandeFix", 6376, 0, 6456, 181, 6386, 0), //Melisande x+10
			new("shimmeringFix1", 6468, 0, 6663, 160, 6478, 0), //ShimmeringLv1 x+10
			new("shimmeringFix2", 6619, 171, 6783, 330, 6639, 0), //ShimmeringLv2 x+20, y-171
		];

		private static void MigrateMalasDungeons(Mobile from)
		{
			foreach (var dmi in dungInfos)
			{
				from.SendMessage("Migrating " + dmi.name);
				MigrateDungeon(dmi, Map.Malas, Map.Felucca, true);
			}

			from.SendMessage("Applying additional fixes");
			foreach (var dmi in additonalFixes)
			{
				from.SendMessage("Migrating " + dmi.name);
				MigrateDungeon(dmi, Map.Felucca, Map.Felucca, false);
			}
		}

		private static void MigrateDungeon(DungMigrationInfo dmi, Map fromMap, Map toMap, bool fixTeleporters)
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
				item.MoveToWorld(new Point3D(newX, newY, dmi.name == "zoo" ? 0 : item.Z), toMap);
				if (item is XmlSpawner spawner)
				{
					foreach (var spawnObject in spawner.SpawnObjects)
					{
						var text = spawnObject.TypeName;
						if (fixTeleporters)
						{
							var mapKey = "";
							var pointKey = "";
							if (text.StartsWith("teleporter", StringComparison.OrdinalIgnoreCase))
							{
								mapKey = "mapdest";
								pointKey = "pointdest";
							}

							if (text.StartsWith("moongate", StringComparison.OrdinalIgnoreCase))
							{
								mapKey = "targetmap";
								pointKey = "target";
							}

							if (mapKey != "" && pointKey != "")
							{
								var mapText = GetTokenValue(text,
									mapKey,
									out _,
									out _);
								if (mapText.Equals(fromMap.Name, StringComparison.OrdinalIgnoreCase))
								{
									text = ReplaceToken(text, mapKey, fromMap.Name, toMap.Name);
									var pointText = GetTokenValue(text,
										pointKey,
										out _,
										out _);
									if (pointText != "")
									{
										var point = Point3D.Parse(pointText);
										point = new Point3D(point.X + xDiff, point.Y + yDiff, point.Z);
										text = ReplaceToken(text, pointKey, pointText, point.ToString());
									}
								}
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

		public record TeleportFix(string name, int x, int y, int xto, int yto, int zto);

		private static List<TeleportFix> teleportFixes = new()
		{
			new("melisande", 3284, 1895, 6394, 179, 25),
			new("shimmering", 3384, 1954, 6505, 6, -1),
			new("lotharn_dungeon", 1950, 602, 6480, 185, 35),
			new("grizzle", 5924, 409, 6819, 105, 0),
			new("parox", 6052, 2427, 6785, 218, 0),
			new("vox", 4109, 3786, 6987, 139, 0),
			new("zoo", 1675, 2005, 6288, 316, 0),
		};

		private static void AlignTeleportersToMalasDungeons(Mobile from)
		{
			from.SendMessage("Fixing IN teleporters");
			foreach (var tf in teleportFixes)
			{
				var spawner = Map.Felucca.FindItem<XmlSpawner>(new Point3D(tf.x, tf.y, 0));
				foreach (var spawnObject in spawner.SpawnObjects)
				{
					var text = spawnObject.TypeName;
					var mapKey = "";
					var pointKey = "";
					if (text.StartsWith("teleporter", StringComparison.OrdinalIgnoreCase))
					{
						mapKey = "mapdest";
						pointKey = "pointdest";
					}

					if (text.StartsWith("moongate", StringComparison.OrdinalIgnoreCase))
					{
						mapKey = "targetmap";
						pointKey = "target";
					}

					if (mapKey != "" && pointKey != "")
					{
						text = ReplaceToken(text, mapKey, "malas", "felucca");
						var pointText = GetTokenValue(text,
							pointKey,
							out _,
							out _);
						var point = new Point3D(tf.xto, tf.yto, tf.zto);
						text = ReplaceToken(text, pointKey, pointText, point.ToString());
						spawnObject.TypeName = text;
					}
				}
			}
		}

		private static void AlignSpawnersAndRespawn(Mobile from)
		{
			var spawners = World.Items.Values.OfType<XmlSpawner>().ToArray();
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
					"Voxy_VeryDifficult" => "VoxPopuli_VeryDifficult",
					_ => spawner.RegionName
				};

				if (spawner.RegionName != null && spawner.RegionName.StartsWith("WielkaPokracznaBestia"))
				{
					spawner.RegionName = "Grizzle" + spawner.RegionName.Substring("WielkaPokracznaBestia".Length);
				}

				foreach (var spawnObject in spawner.SpawnObjects)
				{
					var text = spawnObject.TypeName;
					text = ReplaceType(text, "wladcapiaskowboss", "wladcapiaskow");
					text = ReplaceType(text, "NelderimSkeletalDragon", "NSkeletalDragon");
					text = ReplaceType(text, "orccamp", "prisonercamp");
					text = ReplaceType(text, "ratcamp", "prisonercamp");
					if (text.StartsWith("raceteleporter", StringComparison.OrdinalIgnoreCase) &&
					    spawner.Name == "KomnatyStworzenia")
					{
						text = "RaceRoomMoongate/Location/(5156,254,50)";
					}

					spawnObject.TypeName = text;
				}

				if (spawner.Running)
					spawner.Respawn();
			}
		}

		private static void FixChampionSpawns(Mobile from)
		{
			from.SendMessage("Fixing champion spawns");
			foreach (var championSpawn in ChampionSpawn.AllSpawns)
			{
				championSpawn.SpawnRadius = 35;
				championSpawn.SpawnMod = 1;
			}
		}

		private static void MigrateArtAndHues(Mobile from)
		{
			var hueCount = 0;
			var artCount = 0;
			from.SendMessage("Migrating hues");
			foreach (var item in World.Items.Values)
			{
				if (_HueMap.TryGetValue(item.RawHue, out var value))
				{
					item.Hue = value;
					hueCount++;
				}
				if(_ArtMap.TryGetValue(item.ItemID, out var artValue))
				{
					item.ItemID = artValue;
					artCount++;
				}
			}
			from.SendMessage($"Migrated {hueCount} hues");
			from.SendMessage($"Migrated {artCount} arts");
		}

		private static List<Type> _CrafterTypes =
		[
			typeof(BaseBeverage),
			typeof(DragonBardingDeed),
			typeof(RepairDeed),
			typeof(FurnitureContainer),
			typeof(LockableContainer),
			typeof(BaseLight),
			typeof(CraftableFurniture),
			typeof(Globe),
			typeof(BaseUtensil),
			typeof(BaseArmor),
			typeof(BaseClothing),
			typeof(BaseInstrument),
			typeof(BaseJewel),
			typeof(BaseQuiver),
			typeof(Runebook),
			typeof(Spellbook),
			typeof(BaseWeapon),
			typeof(BaseHarvestTool),
			typeof(BaseTool),
			typeof(FishingPole),
			typeof(Key),
			typeof(KeyRing),
			typeof(Scales),
			typeof(Scissors),
			typeof(Spyglass),
			typeof(WarHorseBardingDeed),
			typeof(LiquorBarrel)
		];
		
		private static void RemoveCrafter(Mobile from)
		{
			int count = 0;
			from.SendMessage("Removing crafter");
			foreach (var item in World.Items.Values)
			{
				var property = item.GetType().GetProperty("Crafter");
				if (property != null)
				{
					property.SetValue(item, null);
					count++;
				}
			}
			from.SendMessage($"Removed {count} crafters");
		}
		
		//HELPERS

		private static string GetTokenValue(string text, string key, out int startIndex, out int length)
		{
			var fullKey = $"/{key}/";
			var tokenStartIndex = text.IndexOf(fullKey, StringComparison.OrdinalIgnoreCase);
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
			if (tokenValue.Equals(oldValue, StringComparison.OrdinalIgnoreCase))
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
