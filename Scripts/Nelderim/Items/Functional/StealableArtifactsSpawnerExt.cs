#region References

using static Server.Map;

#endregion

namespace Server.Items
{
	public partial class StealableArtifactsSpawner
	{
		private static readonly StealableEntry[] m_Entries =
		{
			new StealableEntry(Felucca, new Point3D(5302, 1648, 0), 18432, 27648,
				typeof(PrzekleteFeyLeggings)), //Saew
			new StealableEntry(Felucca, new Point3D(1547, 798, 0), 18432, 27648,
				typeof(PrzekletySoulSeeker)), // Elbrind
			new StealableEntry(Felucca, new Point3D(5702, 418, 4), 18432, 27648,
				typeof(PrzekletyMieczeAmrIbnLuhajj)), // Barad
			new StealableEntry(Felucca, new Point3D(665, 1476, 45), 18432, 27648,
				typeof(PrzekletaVioletCourage)), // Alcala
			new StealableEntry(Malas, new Point3D(258, 772, 1), 18432, 27648,
				typeof(PrzekletyArcticDeathDealer)), // Jaskina Blyskow
			new StealableEntry(Felucca, new Point3D(6004, 2551, 0), 18432, 27648,
				typeof(PrzekleteSongWovenMantle)), // Podmrok mrowki

			// Doom - Artifact rarity 1
			new StealableEntry(Malas, new Point3D(317, 56, -1), 72, 108, typeof(RockArtifact)), // doom
			new StealableEntry(Malas, new Point3D(360, 31, 8), 72, 108, typeof(SkullCandleArtifact)), // doom
			new StealableEntry(Malas, new Point3D(369, 372, -1), 72, 108, typeof(BottleArtifact)), // doom
			new StealableEntry(Malas, new Point3D(378, 372, 0), 72, 108, typeof(DamagedBooksArtifact)), // doom
			// Doom - Artifact rarity 2
			new StealableEntry(Malas, new Point3D(432, 16, -1), 144, 216, typeof(StretchedHideArtifact)), // doom
			new StealableEntry(Malas, new Point3D(489, 9, 0), 144, 216, typeof(BrazierArtifact)), // doom
			// Doom - Artifact rarity 3
			new StealableEntry(Malas, new Point3D(471, 96, -1), 288, 432, typeof(LampPostArtifact),
				GetLampPostHue()), // doom
			new StealableEntry(Malas, new Point3D(421, 198, 2), 288, 432, typeof(BooksNorthArtifact)), // doom
			new StealableEntry(Malas, new Point3D(431, 189, -1), 288, 432, typeof(BooksWestArtifact)), // doom
			new StealableEntry(Malas, new Point3D(435, 196, -1), 288, 432, typeof(BooksFaceDownArtifact)), // doom
			// Doom - Artifact rarity 5
			new StealableEntry(Malas, new Point3D(447, 9, 8), 1152, 1728, typeof(StuddedLeggingsArtifact)), // doom
			new StealableEntry(Malas, new Point3D(423, 28, 0), 1152, 1728, typeof(EggCaseArtifact)), // doom
			new StealableEntry(Malas, new Point3D(347, 44, 4), 1152, 1728, typeof(SkinnedGoatArtifact)), // doom
			new StealableEntry(Malas, new Point3D(497, 57, -1), 1152, 1728,
				typeof(GruesomeStandardArtifact)), //doom
			new StealableEntry(Malas, new Point3D(381, 375, 11), 1152, 1728, typeof(BloodyWaterArtifact)), // doom
			new StealableEntry(Malas, new Point3D(489, 369, 2), 1152, 1728, typeof(TarotCardsArtifact)), // doom
			new StealableEntry(Malas, new Point3D(497, 369, 5), 1152, 1728, typeof(BackpackArtifact)), // doom
			// Doom - Artifact rarity 7
			new StealableEntry(Malas, new Point3D(475, 23, 4), 4608, 6912, typeof(StuddedTunicArtifact)), // doom
			new StealableEntry(Malas, new Point3D(423, 28, 0), 4608, 6912, typeof(CocoonArtifact)), // doom
			// Doom - Artifact rarity 8
			new StealableEntry(Malas, new Point3D(354, 36, -1), 9216, 13824, typeof(SkinnedDeerArtifact)), // doom
			// Doom - Artifact rarity 9
			new StealableEntry(Malas, new Point3D(433, 11, -1), 18432, 27648, typeof(SaddleArtifact)), // doom
			new StealableEntry(Malas, new Point3D(403, 31, 4), 18432, 27648, typeof(LeatherTunicArtifact)), // doom
			// Doom - Artifact rarity 10
			new StealableEntry(Malas, new Point3D(257, 70, -2), 36864, 55296, typeof(ZyronicClaw)), // doom
			new StealableEntry(Malas, new Point3D(354, 176, 7), 36864, 55296, typeof(TitansHammer)), // doom
			new StealableEntry(Malas, new Point3D(369, 389, -1), 36864, 55296, typeof(BladeOfTheRighteous)), // doom
			new StealableEntry(Malas, new Point3D(469, 96, 5), 36864, 55296, typeof(InquisitorsResolution)), // doom
			// Doom - Artifact rarity 12
			new StealableEntry(Malas, new Point3D(487, 364, -1), 147456, 221184,
				typeof(RuinedPaintingArtifact)), // doom

			// Yomotsu Mines - Artifact rarity 1
			new StealableEntry(Felucca, new Point3D(5446, 503, 10), 72, 108,
				typeof(Basket1Artifact)), // Kanały Tasandora
			new StealableEntry(Felucca, new Point3D(5580, 496, 5), 72, 108,
				typeof(Basket2Artifact)), // Kanały Tasandora
			// Yomotsu Mines - Artifact rarity 2
			new StealableEntry(Felucca, new Point3D(5469, 572, 5), 144, 216,
				typeof(Basket4Artifact)), // Kanały Tasandora
			new StealableEntry(Felucca, new Point3D(5445, 590, 5), 144, 216,
				typeof(Basket5NorthArtifact)), // Kanały Tasandora
			new StealableEntry(Felucca, new Point3D(5405, 648, -25), 144, 216,
				typeof(Basket5WestArtifact)), // Kanały Tasandora
			// Yomotsu Mines - Artifact rarity 3
			new StealableEntry(Felucca, new Point3D(5389, 731, -25), 288, 432,
				typeof(Urn1Artifact)), // Kanały Tasandora
			new StealableEntry(Felucca, new Point3D(5494, 699, -26), 288, 432, typeof(Urn2Artifact)),
			new StealableEntry(Felucca, new Point3D(5299, 810, 4), 288, 432,
				typeof(Sculpture1Artifact)), // Hurengrav Lochy
			new StealableEntry(Felucca, new Point3D(1523, 818, 0), 288, 432, typeof(Sculpture2Artifact)), // Elbrind
			new StealableEntry(Felucca, new Point3D(1545, 797, 40), 288, 432,
				typeof(TeapotNorthArtifact)), // Elbrind
			new StealableEntry(Felucca, new Point3D(1633, 835, 15), 288, 432,
				typeof(TeapotWestArtifact)), // Elbrind
			new StealableEntry(Felucca, new Point3D(1685, 840, 20), 288, 432,
				typeof(TowerLanternArtifact)), // Elbrind
			// Yomotsu Mines - Artifact rarity 9
			new StealableEntry(Felucca, new Point3D(5355, 1695, 0), 18432, 27648,
				typeof(ManStatuetteSouthArtifact)), // Seaw

			// Fan Dancer's Dojo - Artifact rarity 1
			new StealableEntry(Felucca, new Point3D(5447, 1704, 0), 72, 108,
				typeof(Basket3NorthArtifact)), // Mrówki
			new StealableEntry(Felucca, new Point3D(5460, 2073, 0), 72, 108,
				typeof(Basket3WestArtifact)), // Smoczy Dung
			// Fan Dancer's Dojo - Artifact rarity 2
			new StealableEntry(Felucca, new Point3D(5473, 1963, 5), 144, 216,
				typeof(Basket6Artifact)), // Smoczy Dung
			new StealableEntry(Felucca, new Point3D(5496, 1964, 5), 144, 216,
				typeof(ZenRock1Artifact)), // Smoczy Dung
			// Fan Dancer's Dojo - Artifact rarity 3
			new StealableEntry(Felucca, new Point3D(5923, 2387, 0), 288, 432, typeof(FanNorthArtifact)), // Podmrok
			new StealableEntry(Felucca, new Point3D(5686, 2444, 48), 288, 432, typeof(FanWestArtifact)), // Podmrok
			new StealableEntry(Felucca, new Point3D(5686, 2460, 42), 288, 432,
				typeof(BowlsVerticalArtifact)), // Podmrok
			new StealableEntry(Felucca, new Point3D(5695, 3114, 5), 288, 432, typeof(ZenRock2Artifact)), // Podmrok
			new StealableEntry(Felucca, new Point3D(5492, 2936, 35), 288, 432, typeof(ZenRock3Artifact)), // Podmrok
			// Fan Dancer's Dojo - Artifact rarity 4
			new StealableEntry(Felucca, new Point3D(5574, 2054, 0), 576, 864,
				typeof(Painting1NorthArtifact)), // Smoczy
			new StealableEntry(Felucca, new Point3D(5795, 1896, 5), 576, 864,
				typeof(Painting1WestArtifact)), // Lodowy
			new StealableEntry(Felucca, new Point3D(5164, 2181, 25), 576, 864,
				typeof(Painting2NorthArtifact)), // Krysztalowe Smoki
			new StealableEntry(Felucca, new Point3D(5659, 1714, 0), 576, 864,
				typeof(Painting2WestArtifact)), // Hall Torech
			new StealableEntry(Felucca, new Point3D(5774, 1717, 0), 576, 864,
				typeof(TripleFanNorthArtifact)), // Hall Torech
			new StealableEntry(Felucca, new Point3D(5238, 1786, 13), 576, 864,
				typeof(TripleFanWestArtifact)), // Tyr Reviaren
			new StealableEntry(Felucca, new Point3D(5728, 1808, 0), 576, 864, typeof(BowlArtifact)), // Hall Torech
			new StealableEntry(Felucca, new Point3D(5893, 1093, 0), 576, 864,
				typeof(CupsArtifact)), // Świątynia Matki
			new StealableEntry(Felucca, new Point3D(5404, 1783, 0), 576, 864,
				typeof(BowlsHorizontalArtifact)), // Loen Torech
			new StealableEntry(Felucca, new Point3D(5900, 876, 4), 576, 864, typeof(SakeArtifact)), // Róża
			// Fan Dancer's Dojo - Artifact rarity 5
			new StealableEntry(Felucca, new Point3D(5358, 1666, 0), 1152, 1728,
				typeof(SwordDisplay1NorthArtifact)), // Saew
			new StealableEntry(Felucca, new Point3D(5325, 1454, 0), 1152, 1728,
				typeof(SwordDisplay1WestArtifact)), //  Swiatynia Smierci Tas
			new StealableEntry(Felucca, new Point3D(5270, 1410, 0), 1152, 1728,
				typeof(Painting3Artifact)), // Wulkan
			// Fan Dancer's Dojo - Artifact rarity 6
			new StealableEntry(Felucca, new Point3D(5538, 1467, 0), 2304, 3456,
				typeof(Painting4NorthArtifact)), // Podziemia Twierdzy
			new StealableEntry(Felucca, new Point3D(5883, 1177, 4), 2304, 3456,
				typeof(Painting4WestArtifact)), // Swiatynia Matki
			new StealableEntry(Felucca, new Point3D(5822, 1177, 0), 2304, 3456,
				typeof(SwordDisplay2NorthArtifact)), // Swiatynia Matki
			new StealableEntry(Felucca, new Point3D(5676, 741, 0), 2304, 3456,
				typeof(SwordDisplay2WestArtifact)), // Lochy Ophidian
			// Fan Dancer's Dojo - Artifact rarity 7
			new StealableEntry(Felucca, new Point3D(5315, 1786, 0), 4608, 6912, typeof(FlowersArtifact)),
			// Fan Dancer's Dojo - Artifact rarity 8
			new StealableEntry(Felucca, new Point3D(5379, 726, -25), 9216, 13824,
				typeof(DolphinLeftArtifact)), // Kanaly Tasandora
			new StealableEntry(Felucca, new Point3D(5384, 328, 5), 9216, 13824,
				typeof(DolphinRightArtifact)), // Cmentarz Tasandora
			new StealableEntry(Felucca, new Point3D(5238, 361, 0), 9216, 13824,
				typeof(SwordDisplay3SouthArtifact)), // Kopalnia Tasandora
			new StealableEntry(Felucca, new Point3D(5321, 56, 0), 9216, 13824,
				typeof(SwordDisplay3EastArtifact)), // Komnaty Bialego Wilka
			new StealableEntry(Malas, new Point3D(162, 647, -1), 9216, 13824, typeof(SwordDisplay4WestArtifact)),
			new StealableEntry(Malas, new Point3D(124, 624, 0), 9216, 13824, typeof(Painting5NorthArtifact)),
			new StealableEntry(Malas, new Point3D(146, 649, 2), 9216, 13824, typeof(Painting5WestArtifact)),
			// Fan Dancer's Dojo - Artifact rarity 9
			new StealableEntry(Malas, new Point3D(100, 488, -1), 18432, 27648, typeof(SwordDisplay4NorthArtifact)),
			new StealableEntry(Malas, new Point3D(175, 606, 0), 18432, 27648, typeof(SwordDisplay5NorthArtifact)),
			new StealableEntry(Malas, new Point3D(157, 608, -1), 18432, 27648, typeof(SwordDisplay5WestArtifact)),
			new StealableEntry(Malas, new Point3D(187, 643, 1), 18432, 27648, typeof(Painting6NorthArtifact)),
			new StealableEntry(Malas, new Point3D(146, 623, 1), 18432, 27648, typeof(Painting6WestArtifact)),
			new StealableEntry(Malas, new Point3D(178, 629, -1), 18432, 27648, typeof(ManStatuetteEastArtifact))
		};
	}
}
