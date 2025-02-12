using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Engines.BulkOrders
{
	public sealed class HunterRewardCalculator : RewardCalculator
	{
		public HunterRewardCalculator()
		{
			RewardCollection = new List<CollectionItem>();
			
			RewardCollection.Add(new BODCollectionItem(0x18E9, 1159541, 0, 100, DecoMinor));
			RewardCollection.Add(new BODCollectionItem(0xEFF, 1159542, 0x07A1, 150, Pigment, 0));
			RewardCollection.Add(new BODCollectionItem(0x26B8, 1159543, 0, 250, TranslocationPowder, 20));
			RewardCollection.Add(new BODCollectionItem(0xEFF, 1159544, 0x486, 350, Pigment, 1));
			RewardCollection.Add(new BODCollectionItem(0x1006, 1159545, 0, 400, DurabilityPowder));
			RewardCollection.Add(new BODCollectionItem(0x11CC, 1159546, 0, 450, DecoMajor));
			RewardCollection.Add(new BODCollectionItem(0xF0B, 1159547, 0x367, 500, PetResurrectPotion));
			RewardCollection.Add(new BODCollectionItem(0x1415, 1159548, 0x21E, 550, Artifact, (int)ArtType.Art1));
			RewardCollection.Add(new BODCollectionItem(0x1415, 1159549, 0xBAF, 650, Artifact, (int)ArtType.Art2));
			RewardCollection.Add(new BODCollectionItem(0x1415, 1159550, 0x499, 800, Artifact, (int)ArtType.Art3));
			RewardCollection.Add(new BODCollectionItem(0x2F58, 1159551, 0, 900, Talisman));
			RewardCollection.Add(new BODCollectionItem(0x1415, 1159552, 0x445, 1000, Artifact, (int)ArtType.Art4));
		}
		
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

		private static Item Pigment(int type)
		{
			return new BasePigment(type);
		}

		private static Item TranslocationPowder(int type)
		{
			return new PowderOfTranslocation(type);
		}

		private static Item DurabilityPowder(int type)
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

		private static Item DecoMinor(int type)
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

		private static Item Talisman(int type)
		{
			return new RandomTalisman();
		}

		private static Item PetResurrectPotion(int type)
		{
			return new PetResurrectPotion();
		}

		private static Item DecoMajor(int type)
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
			typeof(ArcaneTunic),
			typeof(AzysBracelet),
			typeof(BeaconOfHope),
			typeof(Beastmaster),
			typeof(Clasp),
			typeof(EarringsOfTheMagician),
			typeof(EverlastingBottle),
			typeof(GluttonousBlade),
			typeof(ObronaZywiolow),
			typeof(PancerzPrzodkaCzystejKrwi),
			typeof(RodOfResurrection),
			typeof(SoulRipper),
			typeof(StaffofSnakes),
			typeof(WyzywajaceOdzienieBarbarzyncy),
        };

		private enum ArtType
		{
			Art1,
			Art2,
			Art3,
			Art4
		}

		public static Item Artifact(int type)
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
			return (int)(((SmallHunterBOD)bod)?.CollectedPoints ?? 0);
		}

		public override int ComputePoints(LargeBOD bod)
		{
			return (int)(((LargeHunterBOD)bod)?.CollectedPoints ?? 0);
		}
	}
}
