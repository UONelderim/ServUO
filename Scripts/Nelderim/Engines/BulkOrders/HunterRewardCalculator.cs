using System;
using System.Collections.Generic;
using Server.Diagnostics;
using Server.Items;

namespace Server.Engines.BulkOrders
{
	public sealed class HunterRewardCalculator : RewardCalculator
	{
		public HunterRewardCalculator()
		{
			RewardCollection =
			[
				new BODCollectionItem(0, 0, 0, 30, BagOfPotions, 0),
				new BODCollectionItem(0x18E9, 1159541, 0, 100, DecoMinor),
				new BODCollectionItem(0, 0, 0, 125, BagOfPotions, 1),
				new BODCollectionItem(0xEFF, 1159542, 0x07A1, 150, Pigment, 0),
				new BODCollectionItem(0x26B8, 1159543, 0, 250, TranslocationPowder, 20),
				new BODCollectionItem(0, 0, 0, 300, BagOfPotions, 2),
				new BODCollectionItem(0xEFF, 1159544, 0x486, 350, Pigment, 1),
				new BODCollectionItem(0x1006, 1159545, 0, 400, DurabilityPowder),
				new BODCollectionItem(0x11CC, 1159546, 0, 450, DecoMajor),
				new BODCollectionItem(0xF0B, 1159547, 0x367, 500, PetResurrectPotion),
				new BODCollectionItem(0x1415, 1159548, 0x21E, 550, Artifact, (int)ArtType.Art1),
				new BODCollectionItem(0x1415, 1159549, 0xBAF, 650, Artifact, (int)ArtType.Art2),
				new BODCollectionItem(0x1415, 1159550, 0x499, 800, Artifact, (int)ArtType.Art3),
				new BODCollectionItem(0x2F58, 1159551, 0, 900, Talisman),
				new BODCollectionItem(0x1415, 1159552, 0x445, 1000, Artifact, (int)ArtType.Art4)
			];
		}

		private static Type[][] _PotionTypes =
		{
			[
				typeof(LesserCurePotion), 
				typeof(LesserHealPotion), 
				typeof(LesserPoisonPotion), 
				typeof(StrengthPotion), 
				typeof(AgilityPotion),
			],
			[
				typeof(CurePotion),
				typeof(HealPotion),
				typeof(PoisonPotion),
				typeof(RefreshPotion),
				typeof(GreaterStrengthPotion),
				typeof(GreaterAgilityPotion),
			],
			[
				typeof(GreaterCurePotion),
				typeof(GreaterHealPotion),
				typeof(GreaterPoisonPotion),
				typeof(TotalRefreshPotion),
				typeof(NGreaterStrengthPotion),
				typeof(NGreaterAgilityPotion),
				typeof(InvisibilityPotion),
				typeof(EarthElementalPotion),
				typeof(FireElementalPotion),
				typeof(WaterElementalPotion),
			]
		};
		
		
		private static Item BagOfPotions(int type)
		{
			var bag = new Bag();
			try
			{
				var types = _PotionTypes[type];
				for (int i = 0; i < 4; i++)
				{
					var potionType = Utility.RandomList(types);
					var potion = (Item)Activator.CreateInstance(potionType);
					potion.Amount = 5;
					bag.DropItem(potion);
				}
			}
			catch (Exception e)
			{
				ExceptionLogging.LogException(e);
			}

			return bag;
		}
		
		private static Item SelectRandomType(Dictionary<Type, int> objects)
		{
			var selected = Utility.RandomWeighted(objects);
			if (selected != null)
			{
				return Activator.CreateInstance(selected) as Item;
			}
			return null;
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

		private static readonly Dictionary<Type, int> _MinorDecorations = new()
		{
			{ typeof(FurCape), 15 },
			{ typeof(NBearMask), 15 },
			{ typeof(NDeerMask), 15 },
			{ typeof(Arrows), 10 },
			{ typeof(CrossBowBolts), 10 },
			{ typeof(Rope), 10 },
			{ typeof(Whip), 10 },
			{ typeof(WhisperingRose), 5 },
			{ typeof(RoseOfTrinsic), 5 },
			{ typeof(carpet3sDeed), 2 },
			{ typeof(carpet4sDeed), 2 },
			{ typeof(carpet5sDeed), 2 },
			{ typeof(carpet6sDeed), 2 }
		};

		
		private static Item DecoMinor(int type)
		{
			return SelectRandomType(_MinorDecorations);
		}

		private static Item Talisman(int type)
		{
			return new RandomTalisman();
		}

		private static Item PetResurrectPotion(int type)
		{
			return new PetResurrectPotion();
		}

		private static readonly Dictionary<Type, int> _MajorDecorations = new()
		{
			{ typeof(figurka01), 50 },
			{ typeof(figurka02), 50 },
			{ typeof(figurka03), 50 },
			{ typeof(figurka04), 50 },
			{ typeof(figurka05), 50 },
			{ typeof(figurka06), 50 },
			{ typeof(figurka07), 50 },
			{ typeof(figurka08), 50 },
			{ typeof(figurka09), 50 },
			{ typeof(figurka10), 50 },
			{ typeof(figurka11), 50 },
			{ typeof(figurka12), 50 },
			{ typeof(figurka13), 50 },
			{ typeof(figurka14), 50 },
			{ typeof(figurka15), 50 },
			{ typeof(figurka16), 50 },
			{ typeof(figurka17), 50 },
			{ typeof(figurka18), 50 },
			{ typeof(figurka19), 50 },
			{ typeof(figurka20), 50 },
			{ typeof(figurka21), 50 },
			{ typeof(figurka22), 50 },
			{ typeof(figurka23), 50 },
			{ typeof(figurka24), 50 },
			{ typeof(figurka25), 50 },
			{ typeof(figurka26), 50 },
			{ typeof(figurka27), 50 },
			{ typeof(figurka28), 50 },
			{ typeof(figurka29), 50 },
			{ typeof(figurka30), 50 },
			{ typeof(SmallEmptyPot), 20 },
			{ typeof(LargeEmptyPot), 20 },
			{ typeof(PottedPlant), 20 },
			{ typeof(PottedPlant1), 20 },
			{ typeof(PottedPlant2), 20 },
			{ typeof(PottedTree), 20 },
			{ typeof(PottedTree1), 20 },
			{ typeof(PottedTree2), 20 },
			{ typeof(PottedTree3), 20 },
			{ typeof(PottedTree4), 20 },
			{ typeof(BoilingCauldronEastAddonDeed), 10 },
			{ typeof(BoilingCauldronNorthAddonDeed), 10 },
			{ typeof(IronWire), 10 },
			{ typeof(CopperWire), 10 },
			{ typeof(SilverWire), 10 },
			{ typeof(GoldWire), 10 },
			{ typeof(carpet3mDeed), 5 },
			{ typeof(carpet4mDeed), 5 },
			{ typeof(carpet5mDeed), 5 },
			{ typeof(carpet6mDeed), 5 },
			{ typeof(CreepyPortraitE), 5 },
			{ typeof(CreepyPortraitS), 5 },
			{ typeof(DisturbingPortraitE), 5 },
			{ typeof(DisturbingPortraitS), 5 },
			{ typeof(UnsettlingPortraitE), 5 },
			{ typeof(UnsettlingPortraitS), 5 },
		};

		private static Item DecoMajor(int type)
		{
			return SelectRandomType(_MajorDecorations);
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
			var levelFactor = SmallHunterBOD.GetBODLevel(type) switch
			{
				SmallHunterBOD.HunterBODLevel.Easy => 3,
				SmallHunterBOD.HunterBODLevel.Medium => 6,
				SmallHunterBOD.HunterBODLevel.Hard => 10,
				SmallHunterBOD.HunterBODLevel.Boss => 15,
				_ => 6
			};
			
			return levelFactor * quantity * itemCount;
		}

		public override int ComputeGold(int quantity, bool exceptional, BulkMaterialType material, int itemCount,
			Type type)
		{
			var points = ComputePoints(quantity, exceptional, material, itemCount, type);
			return (int)Utility.RandomMinMax(points * 9.5, points * 10.5);
		}
	}
}
