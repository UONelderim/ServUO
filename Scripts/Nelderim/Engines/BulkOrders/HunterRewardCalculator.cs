#region References

using System;
using System.Collections.Generic;
using Server.Items;

#endregion

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

		#region Constructors

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

		private static Item CreateTalismans(int type)
		{
			Item toReceive;
			if (type == 2)
				toReceive = new TalismanLevel2();
			else if (type == 3)
				toReceive = new TalismanLevel3();
			else
				toReceive = new TalismanLevel2();
			return toReceive;
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

		public static Item CreateArtifacts(int type)
		{
			Type artifactType;

			List<Type> artifactLBODLevel1 = new List<Type>(); // najslabsze
			artifactLBODLevel1.Add(typeof(Raikiri));
			artifactLBODLevel1.Add(typeof(PeasantsBokuto));
			artifactLBODLevel1.Add(typeof(PixieSwatter));
			artifactLBODLevel1.Add(typeof(Frostbringer));
			artifactLBODLevel1.Add(typeof(SzyjaGeriadoru));
			artifactLBODLevel1.Add(typeof(BlazenskieSzczescie));
			artifactLBODLevel1.Add(typeof(KulawyMagik));
			artifactLBODLevel1.Add(typeof(KilofZRuinTwierdzy));
			artifactLBODLevel1.Add(typeof(SkalpelDoktoraBrandona));
			artifactLBODLevel1.Add(typeof(JaszczurzySzal));
			artifactLBODLevel1.Add(typeof(OblivionsNeedle));
			artifactLBODLevel1.Add(typeof(Bonesmasher));
			artifactLBODLevel1.Add(typeof(ColdForgedBlade));
			artifactLBODLevel1.Add(typeof(DaimyosHelm));
			artifactLBODLevel1.Add(typeof(LegsOfStability));
			artifactLBODLevel1.Add(typeof(AegisOfGrace));
			artifactLBODLevel1.Add(typeof(AncientFarmersKasa));
			artifactLBODLevel1.Add(typeof(StudniaOdnowy));


			List<Type> artifactLBODLevel2 = new List<Type>(); // srednie
			artifactLBODLevel2.Add(typeof(Tyrfing));
			artifactLBODLevel2.Add(typeof(Arteria));
			artifactLBODLevel2.Add(typeof(ArcticDeathDealer));
			artifactLBODLevel2.Add(typeof(CavortingClub));
			artifactLBODLevel2.Add(typeof(Quernbiter));
			artifactLBODLevel2.Add(typeof(PromienSlonca));
			artifactLBODLevel2.Add(typeof(SwordsOfProsperity));
			artifactLBODLevel2.Add(typeof(TeczowaNarzuta));
			artifactLBODLevel2.Add(typeof(SmoczeKosci));
			artifactLBODLevel2.Add(typeof(RekawiceFredericka));
			artifactLBODLevel2.Add(typeof(OdbijajacyStrzaly));
			artifactLBODLevel2.Add(typeof(HuntersHeaddress));
			artifactLBODLevel2.Add(typeof(BurglarsBandana));
			artifactLBODLevel2.Add(typeof(SpodnieOswiecenia));
			artifactLBODLevel2.Add(typeof(KiltZycia));
			artifactLBODLevel2.Add(typeof(ArkanaZywiolow));
			artifactLBODLevel2.Add(typeof(OstrzeCienia));
			artifactLBODLevel2.Add(typeof(TalonBite));
			artifactLBODLevel2.Add(typeof(SilvanisFeywoodBow));
			artifactLBODLevel2.Add(typeof(BrambleCoat));
			artifactLBODLevel2.Add(typeof(OrcChieftainHelm));
			artifactLBODLevel2.Add(typeof(ShroudOfDeceit));
			artifactLBODLevel2.Add(typeof(CaptainJohnsHat));
			artifactLBODLevel2.Add(typeof(EssenceOfBattle));

			List<Type> artifactLBODLevel3 = new List<Type>(); // najlepsze
			artifactLBODLevel3.Add(typeof(HebanowyPlomien));
			artifactLBODLevel3.Add(typeof(PomstaGrima));
			artifactLBODLevel3.Add(typeof(MaskaSmierci));
			artifactLBODLevel3.Add(typeof(SmoczyNos));
			artifactLBODLevel3.Add(typeof(StudniaOdnowy));
			artifactLBODLevel3.Add(typeof(Aegis));
			artifactLBODLevel3.Add(typeof(HanzosBow));
			artifactLBODLevel3.Add(typeof(MagicznySaif));
			artifactLBODLevel3.Add(typeof(StrzalaAbarisa));
			artifactLBODLevel3.Add(typeof(FangOfRactus));
			artifactLBODLevel3.Add(typeof(RighteousAnger));
			artifactLBODLevel3.Add(typeof(Stormgrip));
			artifactLBODLevel3.Add(typeof(LeggingsOfEmbers));
			artifactLBODLevel3.Add(typeof(SmoczeJelita));
			artifactLBODLevel3.Add(typeof(SongWovenMantle));
			artifactLBODLevel3.Add(typeof(StitchersMittens));
			artifactLBODLevel3.Add(typeof(FeyLeggings));
			//artifactLBODLevel3.Add(typeof(PadsOfTheCuSidhe));
			artifactLBODLevel3.Add(typeof(DjinnisRing));
			artifactLBODLevel3.Add(typeof(PendantOfTheMagi));

			int rand = Utility.RandomMinMax(0, 100);

			if (type == 10)
			{
				if (rand < 80) // 80% (lvl 1)
					artifactType = artifactLBODLevel1[Utility.Random(artifactLBODLevel1.Count)];
				else if (rand < 95) // 15% (lvl 2)
					artifactType = artifactLBODLevel2[Utility.Random(artifactLBODLevel2.Count)];
				else // 5% (lvl 3)
					artifactType = artifactLBODLevel3[Utility.Random(artifactLBODLevel3.Count)];
			}
			else if (type == 15)
			{
				if (rand < 5) // 5% (lvl 1: najslabsze)
					artifactType = artifactLBODLevel1[Utility.Random(artifactLBODLevel1.Count)];
				else if (rand < 85) // 80% (lvl 2: srednie)
					artifactType = artifactLBODLevel2[Utility.Random(artifactLBODLevel2.Count)];
				else // 15% (lvl 3: najlepsze)
					artifactType = artifactLBODLevel3[Utility.Random(artifactLBODLevel3.Count)];
			}
			else if (type == 20)
			{
				if (rand < 5) // 5% (lvl 1: najslabsze)
					artifactType = artifactLBODLevel1[Utility.Random(artifactLBODLevel1.Count)];
				else if (rand < 20) // 15% (lvl 2: srednie)
					artifactType = artifactLBODLevel2[Utility.Random(artifactLBODLevel2.Count)];
				else // 80% (lvl 3: najmocniejsze)
					artifactType = artifactLBODLevel3[Utility.Random(artifactLBODLevel3.Count)];
			}
			else
			{
				artifactType = artifactLBODLevel1[Utility.Random(artifactLBODLevel1.Count)];
			}

			Item art = (Item)Activator.CreateInstance(artifactType);
			return art;
		}


		//private static Item CreateDung( int type )
		//{
		//    return new HorseDung();
		//}

		//private static Item CreateShoes( int type )
		//{
		//    return new HorseShoes();
		//}

		//private static Item CreateAqFishingNet( int type )
		//{
		//    return new AquariumFishingNet();
		//}		

		#endregion

		public static readonly HunterRewardCalculator Instance = new HunterRewardCalculator();

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

		private static readonly int[] GoldPerCreature = { 30, 100, 300 };

		public int ComputeGold(int creatureAmount, int level)
		{
			int levelIndex = (level - 1);
			int gold = GoldPerCreature[levelIndex] * creatureAmount;

			return Utility.RandomMinMax((int)(gold * 0.95), (int)(gold * 1.05)); // plus minus 5%
		}

		public override int ComputeGold(SmallBOD bod)
		{
			return ComputeGold(bod.AmountMax, SmallBulkEntry.GetHunterBulkLevel(bod));
		}

		public override int ComputeGold(LargeBOD bod)
		{
			return bod.Entries.Length * ComputeGold(bod.AmountMax, SmallBulkEntry.GetHunterBulkLevel(bod));
		}

		public override int ComputePoints(SmallBOD bod)
		{
			int level = SmallBulkEntry.GetHunterBulkLevel(bod);
			int basePoints = 0;
			int levelPoints = 100 * (level - 1);
			int amountPoints;
			switch (bod.AmountMax)
			{
				case 10:
					amountPoints = 10;
					break;
				case 15:
					amountPoints = 25;
					break;
				case 20:
					amountPoints = 50;
					break;
				default:
					amountPoints = 0;
					break;
			}

			return basePoints + levelPoints + amountPoints;
		}

		public override int ComputePoints(LargeBOD bod)
		{
			int level = SmallBulkEntry.GetHunterBulkLevel(bod);
			int basePoints = 200;
			int levelPoints = 100 * (level - 1);
			int amountPoints;
			switch (bod.AmountMax)
			{
				case 10:
					amountPoints = 10;
					break;
				case 15:
					amountPoints = 25;
					break;
				case 20:
					amountPoints = 50;
					break;
				default:
					amountPoints = 0;
					break;
			}

			return basePoints + levelPoints + amountPoints;
		}

		public HunterRewardCalculator()
		{
			// Konstrukcja new RewardItem( ilosc procent ze zostanie wybrany, grupa)
			// Konstrukcja new RewardItem( ilosc procent ze zostanie wybrany, grupa, typ) // typ moze byc uzyty np przy rozroznieniu poziomu talizmanow czy losowania artefaktow
			// Pierwszy zawsze musi byc ten z najwiekszym prawdopodobnienstwem, reszta bez znaczenia
			Groups = new[]
			{
				new RewardGroup(0, new RewardItem(60, DecoMinor), new RewardItem(20, Pigments, 0),
					new RewardItem(20, TransPowders, 10)),
				new RewardGroup(50, new RewardItem(40, DecoMinor), new RewardItem(40, Pigments, 0),
					new RewardItem(20, TransPowders, 15)),
				new RewardGroup(80, new RewardItem(40, DecoMinor), new RewardItem(70, Pigments, 0),
					new RewardItem(10, TransPowders, 20)),
				new RewardGroup(100, new RewardItem(20, DecoMinor), new RewardItem(20, DecoMajor),
					new RewardItem(20, Pigments, 0), new RewardItem(20, Pigments, 1),
					new RewardItem(20, TransPowders, 20)),
				new RewardGroup(150, new RewardItem(40, DecoMajor), new RewardItem(40, Pigments, 1),
					new RewardItem(20, DurabilityPowder)),
				new RewardGroup(200, new RewardItem(60, DecoMajor), new RewardItem(20, DurabilityPowder),
					new RewardItem(20, Artifacts, 10)),
				new RewardGroup(230, new RewardItem(50, DecoMajor), new RewardItem(20, DurabilityPowder),
					new RewardItem(20, Artifacts, 10), new RewardItem(15, PetResurrectPotion)),
				new RewardGroup(250, new RewardItem(60, Artifacts, 10),
					new RewardItem(30, Talismans, 2) , new RewardItem(10, PetResurrectPotion)),
				new RewardGroup(300, new RewardItem(40, Talismans, 2), new RewardItem(40, Artifacts, 15),
					new RewardItem(20, Artifacts, 10)),
				new RewardGroup(350, new RewardItem(60, Artifacts, 15),
					new RewardItem(30, Talismans, 2) , new RewardItem(10, PetResurrectPotion)),
				new RewardGroup(380, new RewardItem(60, Artifacts, 15), new RewardItem(30, Talismans, 2),
					new RewardItem(10, PetResurrectPotion, 2)),
				new RewardGroup(400, new RewardItem(40, Talismans, 3), new RewardItem(40, Artifacts, 20),
					new RewardItem(20, Artifacts, 15)),
				new RewardGroup(450, new RewardItem(70, Artifacts, 20), new RewardItem(30, Talismans, 3)),

				//new RewardGroup(110, new RewardItem( 60, DecoMinor ), new RewardItem( 15, Pigments, 0), new RewardItem( 25, TransPowders, 10 )),
				//new RewardGroup(115, new RewardItem( 65, DecoMinor ), new RewardItem( 20, Pigments, 0), new RewardItem( 15, TransPowders, 15 )),
				//new RewardGroup(120, new RewardItem( 70, DecoMinor ), new RewardItem( 25, Pigments, 0), new RewardItem( 5, TransPowders, 20 )),

				//new RewardGroup(210, new RewardItem( 45, DecoMinor ), new RewardItem( 15, Pigments, 1), new RewardItem( 15, TransPowders, 20),   new RewardItem( 5, Talismans, 2),  new RewardItem( 15, PHS ), new RewardItem( 5, DecoMajor )),
				//new RewardGroup(215, new RewardItem( 40, DecoMinor ), new RewardItem( 20, Pigments, 1), new RewardItem( 5, TransPowders, 30),   new RewardItem( 10, Talismans, 2), new RewardItem( 15, PHS ), new RewardItem( 10, DecoMajor )),
				//new RewardGroup(220, new RewardItem( 35, DecoMinor ), new RewardItem( 20, Pigments, 1), new RewardItem( 0, TransPowders, 40),   new RewardItem( 15, Talismans, 2), new RewardItem( 15, PHS ), new RewardItem( 15, DecoMajor )),

				//new RewardGroup(310, new RewardItem( 55, DecoMajor ), new RewardItem( 15, Talismans, 3), new RewardItem( 30, Artifacts, 10)),
				//new RewardGroup(315, new RewardItem( 40, Artifacts, 15), new RewardItem( 20, Talismans, 3), new RewardItem( 40, DecoMajor)),
				//new RewardGroup(320, new RewardItem( 50, Artifacts, 20), new RewardItem( 25, Talismans, 3), new RewardItem( 25, DecoMajor)),
			};
		}
	}
}
