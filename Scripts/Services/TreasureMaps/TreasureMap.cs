#region References

using Server.ContextMenus;
using Server.Engines.CannedEvil;
using Server.Engines.Harvest;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Regions;
using Server.Spells;
using Server.Targeting;
using System;
using System.Collections.Generic;

#endregion

namespace Server.Items
{
	public class TreasureMap : MapItem
	{
		[ConfigProperty("TreasureMaps.LootChance")]
		public static double LootChance
		{
			get => Config.Get("TreasureMaps.LootChance", 0.01);
			set => Config.Set("TreasureMaps.LootChance", value);
		}

		[ConfigProperty("TreasureMaps.ResetTime")]
		public static TimeSpan ResetTime
		{
			get => TimeSpan.FromDays(Config.Get("TreasureMaps.ResetTime", 30.0));
			set => Config.Set("TreasureMaps.ResetTime", value.TotalDays);
		}

		#region Forgotten Treasures

		private TreasurePackage _Package;

		[CommandProperty(AccessLevel.GameMaster)]
		public TreasureLevel TreasureLevel
		{
			get { return (TreasureLevel)m_Level; }
			set
			{
				if ((int)value != Level)
				{
					Level = (int)value;
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public TreasurePackage Package
		{
			get { return _Package; }
			set
			{
				_Package = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public TreasureFacet TreasureFacet => TreasureMapInfo.GetFacet(ChestLocation, Facet);

		protected void AssignRandomPackage()
		{
			Package = (TreasurePackage)Utility.Random(5);
		}

		public void AssignChestQuality(Mobile digger, TreasureMapChest chest)
		{
			double skill = digger.Skills[SkillName.Cartography].Value;

			int dif;

			switch (TreasureLevel)
			{
				default:
				case TreasureLevel.Stash: dif = 100; break;
				case TreasureLevel.Supply: dif = 200; break;
				case TreasureLevel.Cache: dif = 300; break;
				case TreasureLevel.Hoard: dif = 400; break;
				case TreasureLevel.Trove: dif = 500; break;
			}

			if (Utility.Random(dif) <= skill)
			{
				chest.ChestQuality = ChestQuality.Gold;
			}
			else if (Utility.Random(dif) <= skill * 2)
			{
				chest.ChestQuality = ChestQuality.Standard;
			}
			else
			{
				chest.ChestQuality = ChestQuality.Rusty;
			}
		}

		#endregion

		private static readonly Type[][] m_SpawnTypes =
		[
			[typeof(HeadlessOne), typeof(Skeleton)],
			[typeof(Mongbat), typeof(Ratman), typeof(HeadlessOne), typeof(Skeleton), typeof(Zombie)],
			[typeof(OrcishMage), typeof(Gargoyle), typeof(Gazer), typeof(HellHound), typeof(EarthElemental)],
			[
				typeof(Lich), typeof(OgreLord), typeof(DreadSpider), typeof(AirElemental), typeof(FireElemental)
			],
			[typeof(DreadSpider), typeof(LichLord), typeof(Daemon), typeof(ElderGazer), typeof(OgreLord)],
			[
				typeof(LichLord), typeof(Daemon), typeof(ElderGazer), typeof(PoisonElemental), typeof(BloodElemental)
			],
			[
				typeof(AncientWyrm), typeof(Balron), typeof(BloodElemental), typeof(PoisonElemental), typeof(Titan)
			],
			[
				typeof(BloodElemental), typeof(ColdDrake), typeof(FrostDragon), typeof(FrostDrake),
				typeof(GreaterDragon), typeof(PoisonElemental)
			]
		];

		private static readonly Rectangle2D[] m_FelTramWrap =
		[
			new Rectangle2D(0, 0, 5119, 4095)
		];

		private int m_Level;
		private bool m_Completed;
		private Mobile m_CompletedBy;
		private Mobile m_Decoder;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Level
		{
			get { return m_Level; }
			set
			{
				m_Level = Math.Min(value, 4);

				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Completed
		{
			get { return m_Completed; }
			set
			{
				m_Completed = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile CompletedBy
		{
			get { return m_CompletedBy; }
			set
			{
				m_CompletedBy = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Decoder
		{
			get { return m_Decoder; }
			set
			{
				m_Decoder = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Point2D ChestLocation { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime NextReset { get; set; }

		public override int LabelNumber
		{
			get
			{
				if (m_Decoder != null)
				{
					if (m_Level == 6)
					{
						return 1063453;
					}
					else if (m_Level == 7)
					{
						return 1116773;
					}
					else
					{
						return 1041516 + m_Level;
					}
				}
				else if (m_Level == 6)
				{
					return 1063452;
				}
				else if (m_Level == 7)
				{
					return 1116790;
				}
				else
				{
					return 1041510 + m_Level;
				}
			}
		}

		public TreasureMap()
		{
		}

		[Constructable]
		public TreasureMap(int level, Map map)
			: this(level, map, false)
		{
		}

		[Constructable]
		public TreasureMap(int level, Map map, bool eodon)
		{
			Level = level;

			AssignRandomPackage();

			if (map == Map.Internal)
				map = GetRandomMap();

			Facet = map;
			ChestLocation = GetRandomLocation(map);

			Width = 300;
			Height = 300;
			int width, height;

			GetWidthAndHeight(map, out width, out height);

			int x1 = ChestLocation.X - Utility.RandomMinMax(width / 4, (width / 4) * 3);
			int y1 = ChestLocation.Y - Utility.RandomMinMax(height / 4, (height / 4) * 3);

			if (x1 < 0)
				x1 = 0;

			if (y1 < 0)
				y1 = 0;

			int x2;
			int y2;

			AdjustMap(map, out x2, out y2, x1, y1, width, height, eodon);

			x1 = x2 - width;
			y1 = y2 - height;

			Bounds = new Rectangle2D(x1, y1, width, height);
			Protected = true;

			AddWorldPin(ChestLocation.X, ChestLocation.Y);

			NextReset = DateTime.UtcNow + ResetTime;
		}

		public Map GetRandomMap()
		{
			return Map.Felucca;
		}

		public static Point2D GetRandomLocation(Map map)
		{
			Rectangle2D[] recs;

			int x = 0;
			int y = 0;

			recs = m_FelTramWrap;

			while (true)
			{
				Rectangle2D rec = recs[Utility.Random(recs.Length)];

				x = Utility.Random(rec.X, rec.Width);
				y = Utility.Random(rec.Y, rec.Height);

				if (ValidateLocation(x, y, map))
					return new Point2D(x, y);
			}
		}

		public static bool ValidateLocation(int x, int y, Map map)
		{
			LandTile lt = map.Tiles.GetLandTile(x, y);
			LandData ld = TileData.LandTable[lt.ID];

			//Checks for impassable flag..cant walk, cant have a chest
			if (lt.Ignored || (ld.Flags & TileFlag.Impassable) > 0)
			{
				return false;
			}

			//Checks for roads
			for (int i = 0; i < HousePlacement.RoadIDs.Length; i += 2)
			{
				if (lt.ID >= HousePlacement.RoadIDs[i] && lt.ID <= HousePlacement.RoadIDs[i + 1])
				{
					return false;
				}
			}

			Region reg = Region.Find(new Point3D(x, y, lt.Z), map);

			//no-go in towns, houses, dungeons and champspawns
			if (reg != null)
			{
				if (reg.IsPartOf<CityRegion>() || reg.IsPartOf<VillageRegion>() ||
				    reg.IsPartOf<DungeonRegion>() || reg.IsPartOf<ChampionSpawnRegion>() ||
				    reg.IsPartOf<HousingRegion>() || reg.IsPartOf<Jail>())

				{
					return false;
				}
			}

			string n = (ld.Name ?? string.Empty).ToLower();

			if (n != "dirt" && n != "grass" && n != "jungle" && n != "forest" && n != "snow" && n != "sand")
			{
				return false;
			}

			//Rare occrunces where a static tile needs to be checked
			foreach (StaticTile tile in map.Tiles.GetStaticTiles(x, y, true))
			{
				ItemData td = TileData.ItemTable[tile.ID & TileData.MaxItemValue];

				if ((td.Flags & TileFlag.Impassable) > 0)
				{
					return false;
				}

				n = (td.Name ?? string.Empty).ToLower();

				if (n != "dirt" && n != "grass" && n != "jungle" && n != "forest" && n != "snow" && n != "sand")
				{
					return false;
				}
			}

			//check for house within 5 tiles
			for (int xx = x - 5; xx <= x + 5; xx++)
			{
				for (int yy = y - 5; yy <= y + 5; yy++)
				{
					if (BaseHouse.FindHouseAt(new Point3D(xx, yy, lt.Z), map, Region.MaxZ - lt.Z) != null)
					{
						return false;
					}
				}
			}

			return true;
		}

		public void GetWidthAndHeight(Map map, out int width, out int height)
		{
			width = 600;
			height = 600;
		}

		public void AdjustMap(Map map, out int x2, out int y2, int x1, int y1, int width, int height)
		{
			AdjustMap(map, out x2, out y2, x1, y1, width, height, false);
		}

		public void AdjustMap(Map map, out int x2, out int y2, int x1, int y1, int width, int height, bool eodon)
		{
			x2 = x1 + width;
			y2 = y1 + height;

			if (x2 >= 5120)
				x2 = 5119;

			if (y2 >= 4096)
				y2 = 4095;
		}

		public virtual void OnMapComplete(Mobile from, TreasureMapChest chest)
		{
			if (from is PlayerMobile pm)
			{
				pm.Statistics.TreasureMapChestsDigged.Increment(Level);
			}
		}

		public virtual void OnChestOpened(Mobile from, TreasureMapChest chest)
		{
		}

		public TreasureMap(Serial serial)
			: base(serial)
		{
		}

		public static BaseCreature Spawn(int level, Point3D p, bool guardian, Map map)
		{
			Type[][] spawns;

			spawns = m_SpawnTypes;

			if (level >= 0 && level < spawns.Length)
			{
				BaseCreature bc;
				Type[] list = GetSpawnList(spawns, level);

				try
				{
					bc = (BaseCreature)Activator.CreateInstance(list[Utility.Random(list.Length)]);
				}
				catch (Exception e)
				{
					Diagnostics.ExceptionLogging.LogException(e);
					return null;
				}

				bc.Home = p;
				bc.RangeHome = 5;

				if (guardian)
				{
					bc.Title = "(Straznik)";

					if (BaseCreature.IsSoulboundEnemies && !bc.Tamable)
					{
						bc.IsSoulBound = true;
					}
				}

				return bc;
			}

			return null;
		}

		public static BaseCreature Spawn(int level, Point3D p, Map map, Mobile target, bool guardian)
		{
			if (map == null)
			{
				return null;
			}

			BaseCreature c = Spawn(level, p, guardian, map);

			if (c != null)
			{
				bool spawned = false;

				for (int i = 0; !spawned && i < 10; ++i)
				{
					int x = p.X - 3 + Utility.Random(7);
					int y = p.Y - 3 + Utility.Random(7);

					if (map.CanSpawnMobile(x, y, p.Z))
					{
						c.MoveToWorld(new Point3D(x, y, p.Z), map);
						spawned = true;
					}
					else
					{
						int z = map.GetAverageZ(x, y);

						if (map.CanSpawnMobile(x, y, z))
						{
							c.MoveToWorld(new Point3D(x, y, z), map);
							spawned = true;
						}
					}
				}

				if (!spawned)
				{
					c.Delete();
					return null;
				}

				if (target != null)
				{
					Timer.DelayCall(() => c.Combatant = target);
				}

				return c;
			}

			return null;
		}

		public static Type[] GetSpawnList(Type[][] table, int level)
		{
			Type[] array;

			switch (level)
			{
				default: array = table[level + 1]; break;
				case 2:
					List<Type> list1 = new List<Type>();
					list1.AddRange(table[2]);
					list1.AddRange(table[3]);

					array = list1.ToArray();
					break;
				case 3:
					List<Type> list2 = new List<Type>();
					list2.AddRange(table[4]);
					list2.AddRange(table[5]);

					array = list2.ToArray();
					break;
				case 4: array = table[6]; break;
				case 5: array = table[7]; break;
			}

			return array;
		}

		public static bool HasDiggingTool(Mobile m)
		{
			if (m.Backpack == null)
			{
				return false;
			}

			foreach (BaseHarvestTool tool in m.Backpack.FindItemsByType<BaseHarvestTool>())
			{
				if (tool.HarvestSystem == Mining.System)
				{
					return true;
				}
			}

			return false;
		}

		public virtual void OnBeginDig(Mobile from)
		{
			if (m_Completed)
			{
				from.SendLocalizedMessage(503028); // The treasure for this map has already been found.
			}
			else if (m_Level == 0 && !CheckYoung(from))
			{
				from.SendLocalizedMessage(1046447); // Only a young player may use this treasure map.
			}
			/*
			else if (from != m_Decoder)
			{
			    from.SendLocalizedMessage(503016); // Only the person who decoded this map may actually dig up the treasure.
			}
			*/
			else if (m_Decoder != from && !HasRequiredSkill(from))
			{
				from.SendLocalizedMessage(
					503031); // You did not decode this map and have no clue where to look for the treasure.
			}
			else if (!from.CanBeginAction(typeof(TreasureMap)))
			{
				from.SendLocalizedMessage(503020); // You are already digging treasure.
			}
			else if (from.Map != Facet)
			{
				from.SendLocalizedMessage(1010479); // You seem to be in the right place, but may be on the wrong facet!
			}
			else
			{
				from.SendLocalizedMessage(503033); // Where do you wish to dig?
				from.Target = new DigTarget(this);
			}
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(GetWorldLocation(), 2))
			{
				from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
				return;
			}

			if (!m_Completed && m_Decoder == null)
			{
				Decode(from);
			}
			else
			{
				DisplayTo(from);
			}
		}

		public virtual void Decode(Mobile from)
		{
			if (m_Completed || m_Decoder != null)
			{
				return;
			}

			if (m_Level == 0)
			{
				if (!CheckYoung(from))
				{
					from.SendLocalizedMessage(1046447); // Only a young player may use this treasure map.
					return;
				}
			}
			else
			{
				double minSkill = GetMinSkillLevel();

				if (from.Skills[SkillName.Cartography].Value < minSkill)
				{
					if (m_Level == 1)
					{
						from.CheckSkill(SkillName.Cartography, 0, minSkill);
					}
					else
					{
						from.SendLocalizedMessage(503013); // The map is too difficult to attempt to decode.
					}
				}

				if (!from.CheckSkill(SkillName.Cartography, minSkill - 10, minSkill + 30))
				{
					from.LocalOverheadMessage(MessageType.Regular,
						0x3B2,
						503018); // You fail to make anything of the map.
					return;
				}
			}

			from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 503019); // You successfully decode a treasure map!
			Decoder = from;

			LootType = LootType.Blessed;

			DisplayTo(from);
		}

		public void ResetLocation()
		{
			if (!m_Completed)
			{
				ClearPins();
				LootType = LootType.Regular;
				m_Decoder = null;
				GetRandomLocation(Facet);
				InvalidateProperties();
				NextReset = DateTime.UtcNow + ResetTime;
			}
		}

		public override void DisplayTo(Mobile from)
		{
			if (m_Completed)
			{
				SendLocalizedMessageTo(from, 503014); // This treasure hunt has already been completed.
			}
			else if (m_Level == 0 && !CheckYoung(from))
			{
				from.SendLocalizedMessage(1046447); // Only a young player may use this treasure map.
				return;
			}
			else if (m_Decoder != from && !HasRequiredSkill(from))
			{
				from.SendLocalizedMessage(
					503031); // You did not decode this map and have no clue where to look for the treasure.
				return;
			}
			else
			{
				SendLocalizedMessageTo(from,
					503017); // The treasure is marked by the red pin. Grab a shovel and go dig it up!
			}

			if (Pins.Count == 0)
			{
				AddWorldPin(ChestLocation.X, ChestLocation.Y);
			}

			from.PlaySound(0x249);
			base.DisplayTo(from);
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);

			if (!m_Completed)
			{
				if (m_Decoder == null)
				{
					list.Add(new DecodeMapEntry(this));
				}
				else
				{
					bool digTool = HasDiggingTool(from);

					list.Add(new OpenMapEntry(this));
					list.Add(new DigEntry(this, digTool));
				}
			}
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			list.Add(m_Decoder != null ? 1158980 + (int)TreasureLevel : 1158975 + (int)TreasureLevel,
				"#" + TreasureMapInfo.PackageLocalization(Package).ToString());
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			TreasureFacet facet = TreasureMapInfo.GetFacet(ChestLocation, Facet);

			switch (facet)
			{
				case TreasureFacet.Felucca: list.Add(1041502); break;
			}

			if (m_Completed)
			{
				list.Add(1041507, m_CompletedBy == null ? "someone" : m_CompletedBy.Name); // completed by ~1_val~
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(3);

			writer.Write((int)Package);

			writer.Write(NextReset);

			writer.Write(m_CompletedBy);

			writer.Write(m_Level);
			writer.Write(m_Completed);
			writer.Write(m_Decoder);

			writer.Write(ChestLocation);

			if (!Completed && NextReset != DateTime.MinValue && NextReset < DateTime.UtcNow)
				Timer.DelayCall(TimeSpan.FromSeconds(30), ResetLocation);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 3:
				{
					Package = (TreasurePackage)reader.ReadInt();
					goto case 2;
				}
				case 2:
				{
					NextReset = reader.ReadDateTime();
					goto case 1;
				}
				case 1:
				{
					m_CompletedBy = reader.ReadMobile();

					goto case 0;
				}
				case 0:
				{
					m_Level = reader.ReadInt();
					m_Completed = reader.ReadBool();
					m_Decoder = reader.ReadMobile();

					if (version == 1)
						Facet = reader.ReadMap();

					ChestLocation = reader.ReadPoint2D();

					if (version == 0 && m_Completed)
					{
						m_CompletedBy = m_Decoder;
					}

					break;
				}
			}

			if (version == 2)
			{
				Level = TreasureMapInfo.ConvertLevel(m_Level);
				AssignRandomPackage();
			}

			if (m_Decoder != null && LootType == LootType.Regular)
			{
				LootType = LootType.Blessed;
			}

			if (NextReset == DateTime.MinValue)
			{
				NextReset = DateTime.UtcNow + ResetTime;
			}
		}

		private bool CheckYoung(Mobile from)
		{
			return true;
		}

		private double GetMinSkillLevel()
		{
			switch (m_Level)
			{
				case 0:
					return 27;
				case 1:
					return 70;
				case 2:
					return 90;
				case 3:
				case 4:
					return 100.0;

				default:
					return 0.0;
			}
		}

		protected virtual bool HasRequiredSkill(Mobile from)
		{
			return (from.Skills[SkillName.Cartography].Value >= GetMinSkillLevel());
		}

		protected class DigTarget : Target
		{
			private readonly TreasureMap m_Map;

			public DigTarget(TreasureMap map)
				: base(6, true, TargetFlags.None)
			{
				m_Map = map;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (m_Map.Deleted)
				{
					return;
				}

				Map map = m_Map.Facet;

				if (m_Map.m_Completed)
				{
					from.SendLocalizedMessage(503028); // The treasure for this map has already been found.
				}
				/*
			else if ( from != m_Map.m_Decoder )
			{
			from.SendLocalizedMessage( 503016 ); // Only the person who decoded this map may actually dig up the treasure.
			}
			*/
				else if (m_Map.m_Decoder != from && !m_Map.HasRequiredSkill(from))
				{
					from.SendLocalizedMessage(
						503031); // You did not decode this map and have no clue where to look for the treasure.
					return;
				}
				else if (!from.CanBeginAction(typeof(TreasureMap)))
				{
					from.SendLocalizedMessage(503020); // You are already digging treasure.
				}
				else if (!HasDiggingTool(from))
				{
					from.SendMessage("Musisz miec cos przy sobie do kopania, by wykopac skarb.");
				}
				else if (from.Map != map)
				{
					from.SendLocalizedMessage(
						1010479); // You seem to be in the right place, but may be on the wrong facet!
				}
				else
				{
					IPoint3D p = targeted as IPoint3D;

					Point3D targ3D;
					if (p is Item)
					{
						targ3D = ((Item)p).GetWorldLocation();
					}
					else
					{
						targ3D = new Point3D(p);
					}

					int maxRange;
					double skillValue = from.Skills[SkillName.Cartography].Value;

					if (skillValue >= 100.0)
					{
						maxRange = 4;
					}
					else if (skillValue >= 81.0)
					{
						maxRange = 3;
					}
					else if (skillValue >= 51.0)
					{
						maxRange = 2;
					}
					else
					{
						maxRange = 1;
					}

					Point2D loc = m_Map.ChestLocation;
					int x = loc.X, y = loc.Y;

					Point3D chest3D0 = new Point3D(loc, 0);

					if (Utility.InRange(targ3D, chest3D0, maxRange))
					{
						if (from.Location.X == x && from.Location.Y == y)
						{
							from.SendLocalizedMessage(
								503030); // The chest can't be dug up because you are standing on top of it.
						}
						else if (map != null)
						{
							int z = map.GetAverageZ(x, y);

							if (!map.CanFit(x, y, z, 16, true, true))
							{
								from.SendLocalizedMessage(503021);
								// You have found the treasure chest but something is keeping it from being dug up.
							}
							else if (from.BeginAction(typeof(TreasureMap)))
							{
								new DigTimer(from, m_Map, new Point3D(x, y, z), map).Start();
							}
							else
							{
								from.SendLocalizedMessage(503020); // You are already digging treasure.
							}
						}
					}
					else if (m_Map.Level > 0)
					{
						if (Utility.InRange(targ3D, chest3D0, 8)) // We're close, but not quite
						{
							from.SendLocalizedMessage(503032); // You dig and dig but no treasure seems to be here.
						}
						else
						{
							from.SendLocalizedMessage(503035); // You dig and dig but fail to find any treasure.
						}
					}
					else
					{
						if (Utility.InRange(targ3D, chest3D0, 8)) // We're close, but not quite
						{
							from.SendAsciiMessage(0x44, "Skrzynia skarbu jest gdzies blisko!");
						}
						else
						{
							Direction dir = Utility.GetDirection(targ3D, chest3D0);

							string sDir;
							switch (dir)
							{
								case Direction.North:
									sDir = "polnoc";
									break;
								case Direction.Right:
									sDir = "polnocny-wschod";
									break;
								case Direction.East:
									sDir = "wschod";
									break;
								case Direction.Down:
									sDir = "poludniowy-wschod";
									break;
								case Direction.South:
									sDir = "poludnie";
									break;
								case Direction.Left:
									sDir = "poludniuwy-zachod";
									break;
								case Direction.West:
									sDir = "zachod";
									break;
								default:
									sDir = "polnocy-zachod";
									break;
							}

							from.SendAsciiMessage(0x44,
								"Sprobuj spojrzec w kierunku {0} w poszukiwaniu skrzyni.",
								sDir);
						}
					}
				}
			}
		}

		private class DigTimer : Timer
		{
			private readonly Mobile m_From;
			private readonly TreasureMap m_TreasureMap;
			private readonly Map m_Map;
			private readonly long m_NextSkillTime;
			private readonly long m_NextSpellTime;
			private readonly long m_NextActionTime;
			private readonly long m_LastMoveTime;
			private TreasureChestDirt m_Dirt1;
			private TreasureChestDirt m_Dirt2;
			private TreasureMapChest m_Chest;
			private int m_Count;

			public Point3D ChestLocation { get; private set; }

			public DigTimer(Mobile from, TreasureMap treasureMap, Point3D location, Map map)
				: base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
			{
				m_From = from;
				m_TreasureMap = treasureMap;

				ChestLocation = location;
				m_Map = map;

				m_NextSkillTime = from.NextSkillTime;
				m_NextSpellTime = from.NextSpellTime;
				m_NextActionTime = from.NextActionTime;
				m_LastMoveTime = from.LastMoveTime;

				Priority = TimerPriority.TenMS;
			}

			protected override void OnTick()
			{
				if (m_NextSkillTime != m_From.NextSkillTime || m_NextActionTime != m_From.NextActionTime)
				{
					Terminate();
					return;
				}

				if (m_LastMoveTime != m_From.LastMoveTime)
				{
					m_From.SendLocalizedMessage(503023);
					// You cannot move around while digging up treasure. You will need to start digging anew.
					Terminate();
					return;
				}

				int z = (m_Chest != null) ? m_Chest.Z + m_Chest.ItemData.Height : int.MinValue;
				int height = 16;

				if (z > ChestLocation.Z)
				{
					height -= (z - ChestLocation.Z);
				}
				else
				{
					z = ChestLocation.Z;
				}

				if (!m_Map.CanFit(ChestLocation.X, ChestLocation.Y, z, height, true, true, false))
				{
					m_From.SendLocalizedMessage(503024);
					// You stop digging because something is directly on top of the treasure chest.
					Terminate();
					return;
				}

				m_Count++;

				m_From.RevealingAction();
				m_From.Direction = m_From.GetDirectionTo(ChestLocation);

				if (m_Count > 1 && m_Dirt1 == null)
				{
					m_Dirt1 = new TreasureChestDirt();
					m_Dirt1.MoveToWorld(ChestLocation, m_Map);

					m_Dirt2 = new TreasureChestDirt();
					m_Dirt2.MoveToWorld(new Point3D(ChestLocation.X, ChestLocation.Y - 1, ChestLocation.Z), m_Map);
				}

				if (m_Count == 5)
				{
					m_Dirt1.Turn1();
				}
				else if (m_Count == 10)
				{
					m_Dirt1.Turn2();
					m_Dirt2.Turn2();
				}
				else if (m_Count > 10)
				{
					if (m_Chest == null)
					{
						m_Chest = new TreasureMapChest(m_From, m_TreasureMap.Level, true);

						m_TreasureMap.AssignChestQuality(m_From, m_Chest);

						m_Chest.MoveToWorld(new Point3D(ChestLocation.X, ChestLocation.Y, ChestLocation.Z - 15), m_Map);
					}
					else
					{
						m_Chest.Z++;
					}

					Effects.PlaySound(m_Chest, m_Map, 0x33B);
				}

				if (m_Chest != null && m_Chest.Location.Z >= ChestLocation.Z)
				{
					Stop();
					m_From.EndAction(typeof(TreasureMap));

					m_Chest.Temporary = false;
					m_Chest.TreasureMap = m_TreasureMap;
					m_Chest.DigTime = DateTime.UtcNow;
					m_TreasureMap.Completed = true;
					m_TreasureMap.CompletedBy = m_From;

					TreasureMapInfo.Fill(m_From, m_Chest, m_TreasureMap);

					m_TreasureMap.OnMapComplete(m_From, m_Chest);

					for (int i = 0; i < 4; ++i)
					{
						bool guardian = Utility.RandomDouble() >= 0.3;

						BaseCreature bc = Spawn(m_TreasureMap.Level, m_Chest.Location, m_Chest.Map, null, guardian);

						if (bc != null && guardian)
						{
							bc.Hue = 2725;
							m_Chest.Guardians.Add(bc);
						}
					}
				}
				else
				{
					if (m_From.Body.IsHuman && !m_From.Mounted)
					{
						m_From.Animate(AnimationType.Attack, 3);
					}

					new SoundTimer(m_From, 0x125 + (m_Count % 2)).Start();
				}
			}

			private void Terminate()
			{
				Stop();
				m_From.EndAction(typeof(TreasureMap));

				if (m_Chest != null)
				{
					m_Chest.Delete();
				}

				if (m_Dirt1 != null)
				{
					m_Dirt1.Delete();
					m_Dirt2.Delete();
				}
			}

			private class SoundTimer : Timer
			{
				private readonly Mobile m_From;
				private readonly int m_SoundID;

				public SoundTimer(Mobile from, int soundID)
					: base(TimeSpan.FromSeconds(0.9))
				{
					m_From = from;
					m_SoundID = soundID;

					Priority = TimerPriority.TenMS;
				}

				protected override void OnTick()
				{
					m_From.PlaySound(m_SoundID);
				}
			}
		}

		private class DecodeMapEntry : ContextMenuEntry
		{
			private readonly TreasureMap m_Map;

			public DecodeMapEntry(TreasureMap map)
				: base(6147, 2)
			{
				m_Map = map;
			}

			public override void OnClick()
			{
				if (!m_Map.Deleted)
				{
					m_Map.Decode(Owner.From);
				}
			}
		}

		private class OpenMapEntry : ContextMenuEntry
		{
			private readonly TreasureMap m_Map;

			public OpenMapEntry(TreasureMap map)
				: base(6150, 2)
			{
				m_Map = map;
			}

			public override void OnClick()
			{
				if (!m_Map.Deleted)
				{
					m_Map.DisplayTo(Owner.From);
				}
			}
		}

		private class DigEntry : ContextMenuEntry
		{
			private readonly TreasureMap m_Map;

			public DigEntry(TreasureMap map, bool enabled)
				: base(6148, 2)
			{
				m_Map = map;

				if (!enabled)
				{
					Flags |= CMEFlags.Disabled;
				}
			}

			public override void OnClick()
			{
				if (m_Map.Deleted)
				{
					return;
				}

				Mobile from = Owner.From;

				if (HasDiggingTool(from))
				{
					m_Map.OnBeginDig(from);
				}
				else
				{
					from.SendMessage("Aby wykopac skrzynie ze skarbem musisz miec cos do kopania.");
				}
			}
		}
	}

	public class TreasureChestDirt : Item
	{
		public TreasureChestDirt()
			: base(0x912)
		{
			Movable = false;

			Timer.DelayCall(TimeSpan.FromMinutes(2.0), Delete);
		}

		public TreasureChestDirt(Serial serial)
			: base(serial)
		{
		}

		public void Turn1()
		{
			ItemID = 0x913;
		}

		public void Turn2()
		{
			ItemID = 0x914;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();

			Delete();
		}
	}
}
