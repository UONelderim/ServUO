using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Engines.BulkOrders
{
	public sealed class HunterRewardCalculator : RewardCalculator
	{
		private static Item SelectRandomType(Dictionary<Type, int> objects)
		{
			int rand;
			while (true)
			{
				rand = Utility.Random(100);

				List<Type> keys = new List<Type>(objects.Keys);
				int size = objects.Count;
				Random randa = new Random();
				Type randomKey = keys[randa.Next(size)];
				int randomeElement = objects[randomKey];

				if (randomeElement < rand)
					return (Item)Activator.CreateInstance(randomKey);
			}
		}

		private static readonly ConstructCallback Pigments = CreatePigments;
		private static readonly ConstructCallback TransPowders = CreateTransPowders;
		private static readonly ConstructCallback DurabilityPowder = CreateDurabilityPowder;
		private static readonly ConstructCallback DecoMinor = CreateDecoMinor;
		private static readonly ConstructCallback Talismans = CreateTalismans;
		private static readonly ConstructCallback PetResurrectPotion = CreatePetResurrectPotion;
		private static readonly ConstructCallback DecoMajor = CreateDecoMajor;
		private static readonly ConstructCallback Artifacts = CreateArtifacts;

		private static Item CreatePigments(int type)
		{
			return new BasePigment(type);
		}

		private static Item CreateTransPowders(int type)
		{
			return new PowderOfTranslocation(type);
		}

		private static Item CreateDurabilityPowder(int type)
		{
			var uses = Utility.Random(2) + 1; // 1-2
			var reward = Utility.RandomList(
				typeof(BlacksmithyPowderOfTemperament), 
				typeof(BowFletchingPowderOfTemperament), 
				typeof(CarpentryPowderOfTemperament),
				typeof(TailoringPowderOfTemperament),
				typeof(TinkeringPowderOfTemperament));
			if (reward != null)
			{
				return (Item)Activator.CreateInstance(reward, uses);
			}

			return null;
		}

		private static Item CreateDecoMinor(int type)
		{
			Dictionary<Type, int> objects = new Dictionary<Type, int>();
			objects.Add(typeof(FurCape), 15);
			objects.Add(typeof(NBearMask), 15);
			objects.Add(typeof(NDeerMask), 15);
			objects.Add(typeof(Arrows), 10);
			objects.Add(typeof(CrossBowBolts), 10);
			objects.Add(typeof(Rope), 10);
			objects.Add(typeof(Whip), 10);
			objects.Add(typeof(WhisperingRose), 5);
			objects.Add(typeof(RoseOfTrinsic), 5);
			switch (Utility.Random(4))
			{
				default:
				case 0:
					objects.Add(typeof(carpet3sDeed), 5);
					break;
				case 1:
					objects.Add(typeof(carpet4sDeed), 5);
					break;
				case 2:
					objects.Add(typeof(carpet5sDeed), 5);
					break;
				case 3:
					objects.Add(typeof(carpet6sDeed), 5);
					break;
			}

			return SelectRandomType(objects);
		}

		private enum TalizmanType
		{
			Level2,
			Level3,
		}

		private static Item CreateTalismans(int type)
		{
			switch ((TalizmanType)type)
			{
				case TalizmanType.Level3: return new TalismanLevel3();
				default: return new TalismanLevel2();
			}
		}

		private static Item CreatePetResurrectPotion(int type)
		{
			return new PetResurrectPotion();
		}

		private static Item CreateDecoMajor(int type)
		{
			Dictionary<Type, int> objects = new Dictionary<Type, int>();
			switch (Utility.Random(30))
			{
				default:
				case 0:
					objects.Add(typeof(figurka01), 50);
					break;
				case 1:
					objects.Add(typeof(figurka02), 50);
					break;
				case 2:
					objects.Add(typeof(figurka03), 50);
					break;
				case 3:
					objects.Add(typeof(figurka04), 50);
					break;
				case 4:
					objects.Add(typeof(figurka05), 50);
					break;
				case 5:
					objects.Add(typeof(figurka06), 50);
					break;
				case 6:
					objects.Add(typeof(figurka07), 50);
					break;
				case 7:
					objects.Add(typeof(figurka08), 50);
					break;
				case 8:
					objects.Add(typeof(figurka09), 50);
					break;
				case 9:
					objects.Add(typeof(figurka10), 50);
					break;
				case 10:
					objects.Add(typeof(figurka11), 50);
					break;
				case 11:
					objects.Add(typeof(figurka12), 50);
					break;
				case 12:
					objects.Add(typeof(figurka13), 50);
					break;
				case 13:
					objects.Add(typeof(figurka14), 50);
					break;
				case 14:
					objects.Add(typeof(figurka15), 50);
					break;
				case 15:
					objects.Add(typeof(figurka16), 50);
					break;
				case 16:
					objects.Add(typeof(figurka17), 50);
					break;
				case 17:
					objects.Add(typeof(figurka18), 50);
					break;
				case 18:
					objects.Add(typeof(figurka19), 50);
					break;
				case 19:
					objects.Add(typeof(figurka20), 50);
					break;
				case 20:
					objects.Add(typeof(figurka21), 50);
					break;
				case 21:
					objects.Add(typeof(figurka22), 50);
					break;
				case 22:
					objects.Add(typeof(figurka23), 50);
					break;
				case 23:
					objects.Add(typeof(figurka24), 50);
					break;
				case 24:
					objects.Add(typeof(figurka25), 50);
					break;
				case 25:
					objects.Add(typeof(figurka26), 50);
					break;
				case 26:
					objects.Add(typeof(figurka27), 50);
					break;
				case 27:
					objects.Add(typeof(figurka28), 50);
					break;
				case 28:
					objects.Add(typeof(figurka29), 50);
					break;
				case 29:
					objects.Add(typeof(figurka30), 50);
					break;
			}

			switch (Utility.Random(10))
			{
				default:
				case 0:
					objects.Add(typeof(SmallEmptyPot), 20);
					break;
				case 1:
					objects.Add(typeof(LargeEmptyPot), 20);
					break;
				case 2:
					objects.Add(typeof(PottedPlant), 20);
					break;
				case 3:
					objects.Add(typeof(PottedPlant1), 20);
					break;
				case 4:
					objects.Add(typeof(PottedPlant2), 20);
					break;
				case 5:
					objects.Add(typeof(PottedTree), 20);
					break;
				case 6:
					objects.Add(typeof(PottedTree1), 20);
					break;
				case 7:
					objects.Add(typeof(PottedTree2), 20);
					break;
				case 8:
					objects.Add(typeof(PottedTree3), 20);
					break;
				case 9:
					objects.Add(typeof(PottedTree4), 20);
					break;
			}

			switch (Utility.Random(2))
			{
				default:
				case 0:
					objects.Add(typeof(BoilingCauldronEastAddonDeed), 10);
					break;
				case 1:
					objects.Add(typeof(BoilingCauldronNorthAddonDeed), 10);
					break;
			}

			switch (Utility.Random(4))
			{
				default:
				case 0:
					objects.Add(typeof(IronWire), 10);
					break;
				case 1:
					objects.Add(typeof(CopperWire), 10);
					break;
				case 2:
					objects.Add(typeof(SilverWire), 10);
					break;
				case 3:
					objects.Add(typeof(GoldWire), 10);
					break;
			}

			switch (Utility.Random(4))
			{
				default:
				case 0:
					objects.Add(typeof(carpet3mDeed), 5);
					break;
				case 1:
					objects.Add(typeof(carpet4mDeed), 5);
					break;
				case 2:
					objects.Add(typeof(carpet5mDeed), 5);
					break;
				case 3:
					objects.Add(typeof(carpet6mDeed), 5);
					break;
			}

			switch (Utility.Random(6))
			{
				default:
				case 0:
					objects.Add(typeof(CreepyPortraitE), 5);
					break;
				case 1:
					objects.Add(typeof(CreepyPortraitS), 5);
					break;
				case 2:
					objects.Add(typeof(DisturbingPortraitE), 5);
					break;
				case 3:
					objects.Add(typeof(DisturbingPortraitS), 5);
					break;
				case 4:
					objects.Add(typeof(UnsettlingPortraitE), 5);
					break;
				case 5:
					objects.Add(typeof(UnsettlingPortraitS), 5);
					break;
			}

			return SelectRandomType(objects);
		}

		public static readonly Type[] ArtLvl1 =
		{
			typeof(Raikiri),
			typeof(PeasantsBokuto),
			typeof(PixieSwatter),
			typeof(Frostbringer),
			typeof(SzyjaGeriadoru),
			typeof(BlazenskieSzczescie),
			typeof(KulawyMagik),
			typeof(KilofZRuinTwierdzy),
			typeof(SkalpelDoktoraBrandona),
			typeof(JaszczurzySzal),
			typeof(OblivionsNeedle),
			typeof(Bonesmasher),
			typeof(DaimyosHelm),
			typeof(LegsOfStability),
			typeof(AegisOfGrace),
			typeof(AncientFarmersKasa),
			typeof(StudniaOdnowy)
		};

		public static readonly Type[] ArtLvl2 =
		{
			typeof(Tyrfing),
			typeof(Arteria),
			typeof(ArcticDeathDealer),
			typeof(CavortingClub),
			typeof(Quernbiter),
			typeof(PromienSlonca),
			typeof(SwordsOfProsperity),
			typeof(TeczowaNarzuta),
			typeof(SmoczeKosci),
			typeof(RekawiceFredericka),
			typeof(OdbijajacyStrzaly),
			typeof(HuntersHeaddress),
			typeof(BurglarsBandana),
			typeof(SpodnieOswiecenia),
			typeof(KiltZycia),
			typeof(ArkanaZywiolow),
			typeof(OstrzeCienia),
			typeof(TalonBite),
			typeof(OrcChieftainHelm),
			typeof(ShroudOfDeciet),
			typeof(CaptainJohnsHat),
			typeof(EssenceOfBattle),
		};

		public static readonly Type[] ArtLvl3 =
		{
			typeof(HebanowyPlomien),
			typeof(PomstaGrima),
			typeof(MaskaSmierci),
			typeof(SmoczyNos),
			typeof(StudniaOdnowy),
			typeof(Aegis),
			typeof(HanzosBow),
			typeof(MagicznySaif),
			typeof(StrzalaAbarisa),
			typeof(FangOfRactus),
			typeof(RighteousAnger),
			typeof(Stormgrip),
			typeof(LeggingsOfEmbers),
			typeof(SmoczeJelita),
			typeof(FeyLeggings),
            typeof(DjinnisRing),
			typeof(PendantOfTheMagi),
		};

		public static readonly Type[] ArtLvl4 =
		{
			typeof(Stormgrip),
			typeof(FeyLeggings),
			typeof(HuntersHeaddress),
			typeof(SpodnieOswiecenia),
			typeof(OrcChieftainHelm),
			typeof(KulawyMagik),
			typeof(SmoczyNos),
        };

		private enum ArtType
		{
			Art1,
			Art2,
			Art3,
			Art4
		}

		public static Item CreateArtifacts(int type)
		{
			Type itemType;
			switch ((ArtType)type)
			{
				case ArtType.Art4:
					itemType = Utility.RandomList(ArtLvl4);
					break;
				case ArtType.Art3:
					itemType = Utility.RandomList(ArtLvl3);
					break;
				case ArtType.Art2:
					itemType = Utility.RandomList(ArtLvl2);
					break;
				default:
					itemType = Utility.RandomList(ArtLvl1);
					break;
			}

			Item art = (Item)Activator.CreateInstance(itemType);
			return art;
		}

		public static readonly HunterRewardCalculator Instance = new();

		public override int ComputePoints(int quantity, bool exceptional, BulkMaterialType material, int itemCount,
			Type type)
		{
			return 0;
		}

		public override int ComputeGold(int quantity, bool exceptional, BulkMaterialType material, int itemCount,
			Type type)
		{
			return 0;
		}

		private static readonly int[] GoldPerCreature = { 30, 100, 300, 600 };

		public int ComputeGold(double points)
		{
			return Utility.RandomMinMax((int)(points * 0.95), (int)(points * 1.05));
		}

		public override int ComputeGold(SmallBOD bod)
		{
			return ComputeGold((bod as SmallHunterBOD)?.CollectedPoints ?? 0);
		}

		public override int ComputeGold(LargeBOD bod)
		{
			return bod.Entries.Length * ComputeGold((bod as LargeHunterBOD)?.CollectedPoints ?? 0);
		}

		public override int ComputePoints(SmallBOD bod)
		{
			// int level = (bod as SmallHunterBOD)?.Level ?? 1;
			// int basePoints = 0;
			// int levelPoints = 100 * (level - 1);
			// int amountPoints;
			// switch (bod.AmountMax)
			// {
			// 	case 10:
			// 		amountPoints = 10;y
			// 		break;
			// 	case 15:
			// 		amountPoints = 25;
			// 		break;
			// 	case 20:
			// 		amountPoints = 50;
			// 		break;
			// 	default:
			// 		amountPoints = 0;
			// 		break;
			// }
			//
			// return basePoints + levelPoints + amountPoints;
			return (int)(((SmallHunterBOD)bod)?.CollectedPoints ?? 0);
		}

		public override int ComputePoints(LargeBOD bod)
		{
			// int level = (bod as LargeHunterBOD)?.Level ?? 1;
			// int basePoints = 200;
			// int levelPoints = 100 * (level - 1);
			// int amountPoints;
			// switch (bod.AmountMax)
			// {
			// 	case 10:
			// 		amountPoints = 10;
			// 		break;
			// 	case 15:
			// 		amountPoints = 25;
			// 		break;
			// 	case 20:
			// 		amountPoints = 50;
			// 		break;
			// 	default:
			// 		amountPoints = 0;
			// 		break;
			// }
			//
			// return basePoints + levelPoints + amountPoints;
			return (int)(((LargeHunterBOD)bod)?.CollectedPoints ?? 0);
		}

		public HunterRewardCalculator()
		{
			const int PIGMENT_1 = 0;
			const int PIGMENT_2 = 1;

			// Konstrukcja new RewardItem( ilosc procent ze zostanie wybrany, grupa)
			// Konstrukcja new RewardItem( ilosc procent ze zostanie wybrany, grupa, typ) // typ moze byc uzyty np przy rozroznieniu poziomu talizmanow czy losowania artefaktow
			// Pierwszy zawsze musi byc ten z najwiekszym prawdopodobnienstwem, reszta bez znaczenia
			Groups = new[]
			{
					new RewardGroup(  0, new RewardItem(60, DecoMinor), new RewardItem(20, Pigments, PIGMENT_1), new RewardItem(20, TransPowders, 10)),
					new RewardGroup( 25, new RewardItem(50, DecoMinor), new RewardItem(30, Pigments, PIGMENT_1), new RewardItem(20, TransPowders, 13)),
					new RewardGroup( 50, new RewardItem(40, DecoMinor), new RewardItem(40, Pigments, PIGMENT_1), new RewardItem(20, TransPowders, 15)),

					new RewardGroup(100, new RewardItem(20, DecoMinor), new RewardItem(20, DecoMajor), new RewardItem(20, Pigments, PIGMENT_1), new RewardItem(20, Pigments, PIGMENT_2), new RewardItem(20, TransPowders, 20)),
					new RewardGroup(125, new RewardItem(30, DecoMajor), new RewardItem(30, Pigments, PIGMENT_2), new RewardItem(10, Pigments, PIGMENT_1), new RewardItem(10, DecoMinor), new RewardItem(10, TransPowders, 20), new RewardItem(10, DurabilityPowder)),
					new RewardGroup(150, new RewardItem(40, DecoMajor), new RewardItem(40, Pigments, PIGMENT_2), new RewardItem(20, DurabilityPowder)),

					new RewardGroup(200, new RewardItem(60, DecoMajor), new RewardItem(20, DurabilityPowder), new RewardItem(20, Artifacts, (int)ArtType.Art1)),
					new RewardGroup(225, new RewardItem(40, Artifacts, (int)ArtType.Art1), new RewardItem(30, DecoMajor), new RewardItem(15, Talismans, (int)TalizmanType.Level2), new RewardItem(10, DurabilityPowder), new RewardItem(5, PetResurrectPotion)),
					new RewardGroup(250, new RewardItem(60, Artifacts, (int)ArtType.Art1), new RewardItem(30, Talismans, (int)TalizmanType.Level2), new RewardItem(10, PetResurrectPotion)),

					new RewardGroup(300, new RewardItem(40, Talismans, (int)TalizmanType.Level2), new RewardItem(40, Artifacts, (int)ArtType.Art2), new RewardItem(20, Artifacts, (int)ArtType.Art1)),
					new RewardGroup(325, new RewardItem(50, Artifacts, (int)ArtType.Art2), new RewardItem(35, Talismans, (int)TalizmanType.Level2), new RewardItem(10, Artifacts, (int)ArtType.Art1), new RewardItem(5, PetResurrectPotion)),
					new RewardGroup(350, new RewardItem(60, Artifacts, (int)ArtType.Art2), new RewardItem(30, Talismans, (int)TalizmanType.Level2), new RewardItem(10, PetResurrectPotion)),

					new RewardGroup(400, new RewardItem(40, Talismans, (int)TalizmanType.Level3), new RewardItem(40, Artifacts, (int)ArtType.Art3), new RewardItem(20, Artifacts, (int)ArtType.Art2)),
					new RewardGroup(425, new RewardItem(55, Artifacts, (int)ArtType.Art3), new RewardItem(35, Talismans, (int)TalizmanType.Level3), new RewardItem(10, Artifacts, (int)ArtType.Art2)),
					new RewardGroup(450, new RewardItem(70, Artifacts, (int)ArtType.Art3), new RewardItem(30, Talismans, (int)TalizmanType.Level3)),

					new RewardGroup(500, new RewardItem(60, Talismans, (int)TalizmanType.Level3), new RewardItem(10, Talismans, (int)TalizmanType.Level2), new RewardItem(20, Artifacts, (int)ArtType.Art2), new RewardItem(10, Artifacts, (int)ArtType.Art1)),
					new RewardGroup(525, new RewardItem(70, Talismans, (int)TalizmanType.Level3), new RewardItem(15, Artifacts, (int)ArtType.Art2), new RewardItem(5, Artifacts, (int)ArtType.Art1), new RewardItem(5, Artifacts, (int)ArtType.Art3), new RewardItem(5, Artifacts, (int)ArtType.Art4)),
					new RewardGroup(550, new RewardItem(80, Talismans, (int)TalizmanType.Level3), new RewardItem(10, Artifacts, (int)ArtType.Art3), new RewardItem(10, Artifacts, (int)ArtType.Art4)),

					new RewardGroup(700, new RewardItem(60, Artifacts, (int)ArtType.Art4), new RewardItem(40, Artifacts, (int)ArtType.Art3)),
					new RewardGroup(725, new RewardItem(70, Artifacts, (int)ArtType.Art4), new RewardItem(30, Artifacts, (int)ArtType.Art3)),
					new RewardGroup(750, new RewardItem(80, Artifacts, (int)ArtType.Art4), new RewardItem(20, Artifacts, (int)ArtType.Art3)),
			};
		}
	}
}
