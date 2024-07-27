using System;
using Server;
using System.Collections.Generic;
using Server.Network;
using Server.Gumps;
using Server.Items; 
using Server.Mobiles;
using Server.Engines;
using Server.Engines.Harvest;
using System.Text.RegularExpressions;

/*
TODO:
  * - Dokonczyc klasy CustomPlant (dodac duzo zmiennych, konstruktorow, pola typu Type? Przerobic wtedy m_SkillMin w zwyklym BasePlant na statyczne.)
*/



namespace Server.Items.Crops 
{ 
	delegate bool WeedTimerAnimation(Mobile from);
	delegate void WeedTimerAction(Mobile from);
	delegate void WeedTimerFail(Mobile from);
	class WeedTimer : Timer
	{
			private Mobile m_From;
			private Item m_Plant;
			WeedTimerAnimation m_Animation;
			WeedTimerAction m_Action;
			WeedTimerFail m_Fail;
			int m_Count;

			public WeedTimer( Mobile from, Item plant, WeedTimerAnimation animation, WeedTimerAction action, WeedTimerFail fail, TimeSpan delay, TimeSpan interval, int count  ) : base( delay, interval, count )
			{
				m_From = from;
				m_Plant = plant;
				m_Count = count;
				m_Action = action;
				m_Animation = animation;
				m_Fail = fail;
			}

			protected override void OnTick()
			{
				if( m_Plant == null || m_Plant.Deleted || m_From == null || !m_From.Alive )
				{
					Stop();
					m_Fail(m_From);
					return;
				}

				if( !m_Animation(m_From) )
				{
					Stop();
					m_Fail(m_From);
					return;
				}

				--m_Count;
				if( m_Count < 1 )
				{
					m_Action(m_From);
				}
			}
	}


	// Funkcje pomocne do sprawdzania terenu pod uprawe ziol.
	class WeedHelper
	{
		public static TimeSpan DefaultHerbGrowingTimeInSeconds = TimeSpan.FromMinutes(15);
		public static TimeSpan DefaultVegetableGrowingTimeInSeconds = TimeSpan.FromHours(1);


		public static int GardenTileID { get { return 13001; } }
        public static SkillName MainWeedSkill { get { return SkillName.Herbalism; } }

		public static double GetMainSkillValue(Mobile from)
		{
			return from.Skills[MainWeedSkill].Value;
		}

		public static double GetHighestSkillValue(Mobile from, SkillName[] SkillsRequired)
        {
            double highest = 0.0;
            foreach (SkillName sn in SkillsRequired)
            {
                if (from.Skills[sn].Value > highest)
                    highest = from.Skills[sn].Value;
            }
            return highest;
        }

        private static Random m_Random = new Random();

        public static bool CheckSkills(Mobile from, SkillName[] SkillsRequired, double minSkill, double chancePercentAtMin, double maxSkill, double chancePercentAtMax)
        {
            double skill = GetHighestSkillValue(from, SkillsRequired);

            if (skill < minSkill)
                return false;

            if (skill >= maxSkill)
                return chancePercentAtMax > m_Random.Next(100);

            double chance = chancePercentAtMin + (skill - minSkill) / (maxSkill - minSkill) * (chancePercentAtMax - chancePercentAtMin);

			return chance > m_Random.Next(100);
        }

        public static bool CheckSkills(Mobile from, SkillName[] SkillsRequired, double minSkill, double maxSkill)
        {
            return CheckSkills(from, SkillsRequired, minSkill, 0.0, maxSkill, 100.0);
        }

		public static bool Check(double successChance)
		{
			return successChance > m_Random.NextDouble();
		}

		public static int Bonus(int crop, double bonus)
		{
			if (crop <= 0 && bonus <= 0)
				return 0;

			int bonusPercent = (int)(bonus * 100);

			int m = crop * bonusPercent;  // if crop==12 and bonusPercent==15 then m==180
			int a = m / 100;              // if crop==12 and bonusPercent==15 then a==1
			double b = (m % 100) * 0.01;  // if crop==12 and bonusPercent==15 then b==0.8

			return a + (WeedHelper.Check(b * 0.01) ? 1 : 0); // if crop==10 and bonusPercent==15 then return 1 (20% chance) or 2 (80% chance) hence 1.8 average
		}

		// 21.11.2012 mortuus: wygenerowalem grupy element'ow automatem bazujac na ich nazwach z pliku tiledata.mul. Nie wszystko sprawdzone.
		private static int[] LandTilesFurrows	= new int[] { 0x0009, 0x000A, 0x000B, 0x000C, 0x000D, 0x000E, 0x000F, 0x0010, 0x0011, 0x0012, 0x0013, 0x0014, 0x0015, 0x0150, 0x0151, 0x0152, 0x0153, 0x0154, 0x0155, 0x0156, 0x0157, 0x0158, 0x0159, 0x015A, 0x015B, 0x015C };
		private static int[] LandTilesGrass	= new int[] { 0x0003, 0x0004, 0x0005, 0x0006, 0x003B, 0x003C, 0x003D, 0x003E, 0x007D, 0x007E, 0x007F, 0x00C0, 0x00C1, 0x00C2, 0x00C3, 0x00D8, 0x00D9, 0x00DA, 0x00DB, 0x01A4, 0x01A5, 0x01A6, 0x01A7, 0x0242, 0x0243, 0x036F, 0x0370, 0x0371, 0x0372, 0x0373, 0x0374, 0x0375, 0x0376, 0x037B, 0x037C, 0x037D, 0x037E, 0x03BF, 0x03C0, 0x03C1, 0x03C2, 0x03C3, 0x03C4, 0x03C5, 0x03C6, 0x03CB, 0x03CC, 0x03CD, 0x03CE, 0x0579, 0x057A, 0x057B, 0x057C, 0x057D, 0x057E, 0x057F, 0x0580, 0x058B, 0x058C, 0x05D7, 0x05D8, 0x05D9, 0x05DA, 0x05DB, 0x05DC, 0x05DD, 0x05DE, 0x05E3, 0x05E4, 0x05E5, 0x05E6, 0x067D, 0x067E, 0x067F, 0x0680, 0x0681, 0x0682, 0x0683, 0x0684, 0x0689, 0x068A, 0x068B, 0x068C, 0x0695, 0x0696, 0x0697, 0x0698, 0x0699, 0x069A, 0x069B, 0x069C, 0x06A1, 0x06A2, 0x06A3, 0x06A4, 0x06B5, 0x06B6, 0x06B7, 0x06B8, 0x06B9, 0x06BA, 0x06BF, 0x06C0, 0x06C1, 0x06C2, 0x06DE, 0x06DF, 0x06E0, 0x06E1 };
		private static int[] LandTilesForest	= new int[] { 0x00C4, 0x00C5, 0x00C6, 0x00C7, 0x00C8, 0x00C9, 0x00CA, 0x00CB, 0x00CC, 0x00CD, 0x00CE, 0x00CF, 0x00D0, 0x00D1, 0x00D2, 0x00D3, 0x00D4, 0x00D5, 0x00D6, 0x00D7, 0x00EF, 0x00F0, 0x00F1, 0x00F2, 0x00F3, 0x00F8, 0x00F9, 0x00FA, 0x00FB, 0x015D, 0x015E, 0x015F, 0x0160, 0x0161, 0x0162, 0x0163, 0x0164, 0x0165, 0x0166, 0x0167, 0x0168, 0x0324, 0x0325, 0x0326, 0x0327, 0x0328, 0x0329, 0x032A, 0x032B, 0x054F, 0x0550, 0x0551, 0x0552, 0x05F1, 0x05F2, 0x05F3, 0x05F4, 0x05F9, 0x05FA, 0x05FB, 0x05FC, 0x05FD, 0x05FE, 0x05FF, 0x0600, 0x0601, 0x0602, 0x0603, 0x0604, 0x0611, 0x0612, 0x0613, 0x0614, 0x0653, 0x0654, 0x0655, 0x0656, 0x065B, 0x065C, 0x065D, 0x065E, 0x065F, 0x0660, 0x0661, 0x0662, 0x066B, 0x066C, 0x066D, 0x066E, 0x06AF, 0x06B0, 0x06B1, 0x06B2, 0x06B3, 0x06B4, 0x06BB, 0x06BC, 0x06BD, 0x06BE, 0x0709, 0x070A, 0x070B, 0x070C, 0x0715, 0x0716, 0x0717, 0x0718, 0x0719, 0x071A, 0x071B, 0x071C };
		private static int[] LandTilesJungle	= new int[] { 0x00AC, 0x00AD, 0x00AE, 0x00AF, 0x00B0, 0x00B3, 0x00B6, 0x00B9, 0x00BC, 0x00BD, 0x00BE, 0x00BF, 0x0100, 0x0101, 0x0102, 0x0103, 0x0108, 0x0109, 0x010A, 0x010B, 0x01F0, 0x01F1, 0x01F2, 0x01F3, 0x026E, 0x026F, 0x0270, 0x0271, 0x0276, 0x0277, 0x0278, 0x0279, 0x027A, 0x027B, 0x027C, 0x027D, 0x0286, 0x0287, 0x0288, 0x0289, 0x0292, 0x0293, 0x0294, 0x0295, 0x0581, 0x0582, 0x0583, 0x0584, 0x0585, 0x0586, 0x0587, 0x0588, 0x0589, 0x058A, 0x058D, 0x058E, 0x058F, 0x0590, 0x059F, 0x05A0, 0x05A1, 0x05A2, 0x05A3, 0x05A4, 0x05A5, 0x05A6, 0x05B3, 0x05B4, 0x05B5, 0x05B6, 0x05B7, 0x05B8, 0x05B9, 0x05BA, 0x05F5, 0x05F6, 0x05F7, 0x05F8, 0x0605, 0x0606, 0x0607, 0x0608, 0x0609, 0x060A, 0x060B, 0x060C, 0x060D, 0x060E, 0x060F, 0x0610, 0x0615, 0x0616, 0x0617, 0x0618, 0x0727, 0x0728, 0x0729, 0x0733, 0x0734, 0x0735, 0x0736, 0x0737, 0x0738, 0x0739, 0x073A };
		private static int[] LandTilesSwamp	= new int[] { 0x3DC0, 0x3DC1, 0x3DC2, 0x3DC3, 0x3DC4, 0x3DC5, 0x3DC6, 0x3DC7, 0x3DC8, 0x3DC9, 0x3DCA, 0x3DCB, 0x3DCC, 0x3DCD, 0x3DCE, 0x3DCF, 0x3DD0, 0x3DD1, 0x3DD2, 0x3DD3, 0x3DD4, 0x3DD5, 0x3DD6, 0x3DD7, 0x3DD8, 0x3DD9, 0x3DDA, 0x3DDB, 0x3DDC, 0x3DDD, 0x3DDE, 0x3DDF, 0x3DE0, 0x3DE1, 0x3DE2, 0x3DE3, 0x3DE4, 0x3DE5, 0x3DE6, 0x3DE7, 0x3DE8, 0x3DE9, 0x3DEA, 0x3DEB, 0x3DEC, 0x3DED, 0x3DEE, 0x3DEF, 0x3DF0, 0x3DF1 };
		private static int[] LandTilesCave	= new int[] { 0x0245, 0x0246, 0x0247, 0x0248, 0x0249, 0x063B, 0x063C, 0x063D, 0x063E };
		private static int[] LandTilesSand	= new int[] { 0x0016, 0x0017, 0x0018, 0x0019, 0x0033, 0x0034, 0x0035, 0x0036, 0x0037, 0x0038, 0x0039, 0x003A, 0x011E, 0x011F, 0x0120, 0x0121, 0x012A, 0x012B, 0x012C, 0x012D, 0x0192, 0x01A8, 0x01A9, 0x01AA, 0x01AB, 0x0282, 0x0283, 0x0284, 0x0285, 0x028A, 0x028B, 0x028C, 0x028D, 0x028E, 0x028F, 0x0290, 0x0291, 0x0335, 0x0336, 0x0337, 0x0338, 0x0339, 0x033A, 0x033B, 0x033C, 0x0341, 0x0342, 0x0343, 0x0344, 0x034D, 0x034E, 0x034F, 0x0350, 0x0351, 0x0352, 0x0353, 0x0354, 0x0359, 0x035A, 0x035B, 0x035C, 0x03B7, 0x03B8, 0x03B9, 0x03BA, 0x03BB, 0x03BC, 0x03BD, 0x03BE, 0x03C7, 0x03C8, 0x03C9, 0x03CA, 0x05A7, 0x05A8, 0x05A9, 0x05AA, 0x05AB, 0x05AC, 0x05AD, 0x05AE, 0x05AF, 0x05B0, 0x05B1, 0x05B2, 0x064B, 0x064C, 0x064D, 0x064E, 0x064F, 0x0650, 0x0651, 0x0652, 0x0657, 0x0658, 0x0659, 0x065A, 0x0663, 0x0664, 0x0665, 0x0666, 0x0667, 0x0668, 0x0669, 0x066A, 0x066F, 0x0670, 0x0671, 0x0672 };
		private static int[] LandTilesSnow	= new int[] { 0x011A, 0x011B, 0x011C, 0x011D, 0x0179, 0x017A, 0x017B, 0x0385, 0x0386, 0x0387, 0x0388, 0x0389, 0x038A, 0x038B, 0x038C, 0x0391, 0x0392, 0x0393, 0x0394, 0x039D, 0x039E, 0x039F, 0x03A0, 0x03A1, 0x03A2, 0x03A3, 0x03A4, 0x03A9, 0x03AA, 0x03AB, 0x03AC, 0x05BF, 0x05C0, 0x05C1, 0x05C2, 0x05C3, 0x05C4, 0x05C5, 0x05C6, 0x05C7, 0x05C8, 0x05C9, 0x05CA, 0x05CB, 0x05CC, 0x05CD, 0x05CE, 0x05CF, 0x05D0, 0x05D1, 0x05D2, 0x05D3, 0x05D4, 0x05D5, 0x05D6, 0x05DF, 0x05E0, 0x05E1, 0x05E2, 0x0745, 0x0746, 0x0747, 0x0748, 0x0751, 0x0752, 0x0753, 0x0754, 0x075D, 0x075E, 0x075F, 0x0760 };
		private static int[] LandTilesDirt = new int[] { 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0xE8, 0xE9, 0xEA, 0xEB, 0x27E, 0x27F, 0x280, 0x281, 0x637, 0x638, 0x639, 0x63A, 0x7AE, 0x7AF, 0x7B0, 0x7B1};
		//private static int[] LandTilesWater	= new int[] { 0x00A8, 0x00A9, 0x00AA, 0x00AB, 0x0136, 0x0137 };
		//private static int[] LandTilesLava	= new int[] { 0x01F4, 0x01F5, 0x01F6, 0x01F7 };
		//private static int[] LandTilesAcid	= new int[] { 0x2E02, 0x2E03, 0x2E04, 0x2E05, 0x2E06, 0x2E07, 0x2E08, 0x2E09, 0x2E0A, 0x2E0B, 0x2E0C, 0x2E0D, 0x2E0E, 0x2E0F, 0x2E10, 0x2E11, 0x2E12, 0x2E13, 0x2E14, 0x2E15, 0x2E16, 0x2E17, 0x2E18, 0x2E19, 0x2E1A, 0x2E1B, 0x2E1C, 0x2E1D, 0x2E1E, 0x2E1F, 0x2E20, 0x2E21, 0x2E22, 0x2E23, 0x2E24, 0x2E25, 0x2E26, 0x2E27, 0x2E28, 0x2E29, 0x2E2A, 0x2E2B, 0x2E2C, 0x2E2D, 0x2E2E, 0x2E2F, 0x2E30, 0x2E31, 0x2E32, 0x2E33, 0x2E34, 0x2E35, 0x2E36, 0x2E37, 0x2E38, 0x2E39, 0x2E3A, 0x2E3B };

		private static int[,] StaticTilesFurrows = new int[,] { { 0x32C9, 0x32CA } };
		private static int[,] StaticTilesGrass = new int[,] { { 0x177D, 0x1781 } };
		private static int[,] StaticTilesForest = new int[,] { { 0x337A, 0x337A } };
		private static int[,] StaticTilesJungle = new int[,] { };
		private static int[,] StaticTilesSwamp = new int[,] { { 0x3209, 0x322A }, { 0x3236, 0x324A }, { 0x3258, 0x3268 }, { 0x326A, 0x326F } };
		private static int[,] StaticTilesCave = new int[,] { { 0x53B, 0x53F } };
		private static int[,] StaticTilesSand = new int[,] { };
		private static int[,] StaticTilesSnow = new int[,] { { 0x17BD, 0x17C0 } };
		private static int[,] StaticTilesDirt = new int[,] { { 0x31F4, 0x31FB } };
		//private static int[,] StaticTilesWater = new int[,] { { 0x1797, 0x79C }, { 0x346E, 0x3485 }, { 0x3494, 0x34AB }, { 0x34B8, 0x34CA } };
		//private static int[,] StaticTilesLava = new int[,] { { 0x12EE, 0x12F2 }, { 0x12F4, 0x12FE }, { 0x1300, 0x1304 }, { 0x1306, 0x130A }, { 0x130C, 0x1310 }, { 0x1312, 0x1316 }, { 0x1318, 0x131C } }; // TODO: there is more lava element to add
		//private static int[,] StaticTilesAcid = new int[,] { { 0x2E0E, 0x2E0F }, { 0x2E1D, 0x2E2E }, { 0x2E2A, 0x2E39 } };

		private static List<int[,]> StaticTilesGroups = new List<int[,]> { StaticTilesFurrows, StaticTilesGrass, StaticTilesForest, StaticTilesJungle, StaticTilesSwamp, StaticTilesCave, StaticTilesSand, StaticTilesSnow };

		public static bool CheckCanGrow( BaseSeedling crop, Map map, Point3D p )
		{
			if (CanGrowOnLandTile(crop, map, p))
				return true;

			if (CanGrowOnStaticTile(crop, map, p))
				return true;

            if (crop.CanGrowGarden)
            {
                crop.BumpZ = ValidateGardenPlot(map, p);
                return crop.BumpZ;
            }
            return false;
        }

        private static bool ValidateGardenPlot(Map map, Point3D p)
        {
            bool ground = false;

            // Test for Dynamic Item
            IPooledEnumerable eable = map.GetItemsInBounds(new Rectangle2D(p.X, p.Y, 1, 1));
            foreach (Item item in eable)
            {
                if (item.ItemID == GardenTileID)
                    ground = true;
            }
            eable.Free();

            // Test for Frozen into Map
            if (!ground)
            {
                var tiles = map.Tiles.GetStaticTiles(p.X, p.Y);
                for (int i = 0; i < tiles.Length; ++i)
                {
                    if ((tiles[i].ID & 0x3FFF) == GardenTileID)
                        ground = true;
                }
            }

            return ground;
        }

		private static bool CanGrowOnLandTile(BaseSeedling crop, Map map, Point3D p)
		{
			var tile = map.Tiles.GetLandTile(p.X, p.Y);
			if (p.Z < tile.Z || p.Z > tile.Z + 5)
				return false;

			int tileID = tile.ID & 0x3FFF;

			if (crop.CanGrowFurrows && TilesListContains(LandTilesFurrows, tileID))
				return true;
			if (crop.CanGrowGrass && TilesListContains(LandTilesGrass, tileID))
				return true;
			if (crop.CanGrowForest && TilesListContains(LandTilesForest, tileID))
				return true;
			if (crop.CanGrowJungle && TilesListContains(LandTilesJungle, tileID))
				return true;
			if (crop.CanGrowCave && TilesListContains(LandTilesCave, tileID))
				return true;
			if (crop.CanGrowSand && TilesListContains(LandTilesSand, tileID))
				return true;
			if (crop.CanGrowSnow && TilesListContains(LandTilesSnow, tileID))
				return true;
			if (crop.CanGrowSwamp && TilesListContains(LandTilesSwamp, tileID))
				return true;
			if (crop.CanGrowDirt && TilesListContains(LandTilesDirt, tileID))
				return true;

			return false;
		}

        private static bool TilesListContains(int[] tileTab, int tileID)
		{
			for ( int i = 0; i < tileTab.Length; ++i )
				if( tileID == tileTab[i] )
					return true;

			return false;
		}

		private static bool CanGrowOnStaticTile(BaseSeedling crop, Map map, Point3D p)
		{
			List<StaticTile> zTiles = new List<StaticTile>();
			var allTiles = map.Tiles.GetStaticTiles(p.X, p.Y, true);
			foreach (var t in allTiles)
			{
				if (t.Z == p.Z)
					zTiles.Add(t);
			}

			if (crop.CanGrowFurrows && TileListContains(StaticTilesFurrows, zTiles))
				return true;
			if (crop.CanGrowGrass && TileListContains(StaticTilesGrass, zTiles))
				return true;
			if (crop.CanGrowForest && TileListContains(StaticTilesForest, zTiles))
				return true;
			if (crop.CanGrowJungle && TileListContains(StaticTilesJungle, zTiles))
				return true;
			if (crop.CanGrowCave && TileListContains(StaticTilesCave, zTiles))
				return true;
			if (crop.CanGrowSand && TileListContains(StaticTilesSand, zTiles))
				return true;
			if (crop.CanGrowSnow && TileListContains(StaticTilesSnow, zTiles))
				return true;
			if (crop.CanGrowSwamp && TileListContains(StaticTilesSwamp, zTiles))
				return true;
			if (crop.CanGrowDirt && TileListContains(StaticTilesDirt, zTiles))
				return true;

			return false;
		}

		private static bool TileListContains(int[,] container, List<StaticTile> elements)
		{
			foreach (var tile in elements)
			{
				int tileID = tile.ID & 0x3FFF;
				for (int i = 0; i < container.GetLength(0); ++i)
					if (tileID >= container[i, 0] && tileID <= container[i, 1])
						return true;

			}
			return false;
		}

		public static bool CheckSpaceForPlants(Map map, Point3D pnt)
		{
			// Test for Plants
			bool freeSpace = true;
			IPooledEnumerable eable = map.GetItemsInRange( pnt, 0 );
			foreach ( Item item in eable ) 
			{ 
				if ( ( item != null ) && ( item is BasePlant ) && IsInHeightRange(pnt, item, 19))
				{
					freeSpace = false;
					break;
				}
			} 
			eable.Free();

			return freeSpace;
		}

		private static bool IsInHeightRange(Point3D pnt, Item it, int range)
		{
			int dist = Math.Abs(it.Z - pnt.Z);
			return dist <= range;
		}

		public static bool CheckSpaceForObstacles(Map map, Point3D pnt)
		{
			// Test for Frozen into Map
			var tiles = map.Tiles.GetStaticTiles(pnt.X, pnt.Y);
			foreach (var tile in tiles)
			{
				if (tile.Z > pnt.Z && tile.Z < pnt.Z + 20)
					return false;

				if (tile.Z == pnt.Z && !IsTileFloor(tile))
					return false;
			}
			return true;
		}

		private static bool IsTileFloor(StaticTile tile)
		{
			int tileID = tile.ID & 0x3FFF;
			foreach (int[,] tileGroup in StaticTilesGroups)
			{
				for (int i = 0; i < tileGroup.GetLength(0); ++i)
					if (tileID >= tileGroup[i, 0] && tileID <= tileGroup[i, 1])
						return true;
			}
			return false;
		}
	}


}
