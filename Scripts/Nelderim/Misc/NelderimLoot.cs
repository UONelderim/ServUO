using System;
using System.Collections.Generic;
using Server;
using Server.ACC.CSS.Systems.Ancient;
using Server.ACC.CSS.Systems.Avatar;
using Server.ACC.CSS.Systems.Bard;
using Server.ACC.CSS.Systems.Cleric;
using Server.ACC.CSS.Systems.Druid;
using Server.ACC.CSS.Systems.Ranger;
using Server.ACC.CSS.Systems.Undead;
using Server.Items;
using Server.Mobiles;

namespace Nelderim
{
	public class NelderimLootPackEntry : LootPackEntry
	{
		public int MinProps { get; set; }
		
		public NelderimLootPackEntry(
			bool atSpawnTime,
			bool onStolen,
			LootPackItem[] items,
			double chance,
			int quantity,
			int minProps,
			int maxProps,
			int minIntensity,
			int maxIntensity,
			bool standardLootItem) : 
			base(atSpawnTime, onStolen, items, chance, quantity, 
			maxProps, minIntensity, maxIntensity, standardLootItem)
		{
			MinProps = minProps;
		}

		public override int GetBonusProperties()
		{
			return Utility.RandomMinMax(MinProps, MinProps);
		}
	}
	
	public class NelderimLoot
	{
		public static LootPack Generate(BaseCreature bc, LootStage stage)
		{
			if (Config.Get("NelderimLoot.Enabled", false))
			{
				if (bc.Controlled || bc.Summoned || bc.AI == AIType.AI_Animal)
					return LootPack.Empty;

				List<LootPackEntry> entries = new List<LootPackEntry>();

				if (stage == LootStage.Spawning)
					GenerateGold(bc, ref entries);
				else
				{
					//GenerateSpellbooks( bc, ref entries );
					GenerateRecallRunes(bc, ref entries);
					GenerateGems(bc, ref entries);
					GenerateScrolls(bc, ref entries);
					GeneratePotions(bc, ref entries);
					GenerateInstruments(bc, ref entries);
					GenerateMagicItems(bc, ref entries);
				}

				if (entries.Count > 0)
					return new LootPack(entries.ToArray());
			}

			return LootPack.Empty;
		}

		private static void GenerateGold(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			int minGold = (int)Math.Ceiling(Math.Pow((bc.Difficulty * 630), 0.49));
			int maxGold = (int)Math.Ceiling(minGold * 1.3);

			int gold = (int) (Utility.RandomMinMax(minGold, maxGold) * Config.Get("NelderimLoot.GoldModifier", 1.0));

			entries.Add(new LootPackEntry(true, true, LootPack.Gold, 100.0, gold));
		}

		private static void GenerateRecallRunes(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			int count = (int)Math.Min(5, 1 + bc.Difficulty / 500);
			double chance = Math.Pow(bc.Difficulty, 0.6) / 10;

			for (int i = 0; i < count; i++)
				entries.Add(new LootPackEntry(false, true, RecallRune, chance, 1));
		}

		private static void GenerateGems(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			int count = (int)Math.Ceiling(Math.Pow(bc.Difficulty, 0.35));
			double chance = Math.Max(0.01, Math.Min(1, 0.1 + Math.Pow(bc.Difficulty, 0.3) / 20)) * 100;

			for (int i = 0; i < count; i++)
				entries.Add(new LootPackEntry(false, true, LootPack.GemItems, chance, 1));
		}
		
		private static void GenerateScrolls(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			int count = (int)Math.Floor(Math.Pow((1 + bc.Difficulty) * 39.0, 0.15));
			double scrollChance = Math.Max(0.01, Math.Min(0.8, Math.Pow(bc.Difficulty * 0.5, 0.29) * 0.03));
			
			double[] chances = GetChances(Math.Min(1, Math.Pow(bc.Difficulty, 0.9) / 10000), 2.0, NL_scrolls.Length);

			for ( int i = 0 ; i < count; i++) 
			{
				if (Utility.RandomDouble() >= scrollChance)
					continue;

				entries.Add(new LootPackEntry(false, true, NL_scrolls[Utility.RandomIndex(chances)], 100, 1));
			}
		}
		
		private static void GeneratePotions( BaseCreature bc, ref List<LootPackEntry> entries )
		{
			int count = (int)Math.Floor( Math.Pow( (1+bc.Difficulty) * 39.0 , 0.15 ) );
			double potionChance =  Math.Max( 0.01, Math.Min( 0.8, Math.Pow( bc.Difficulty * 0.5 , 0.29 ) * 0.1 )) * 100;

			for( int i = 0; i < count; i++ )
				entries.Add( new LootPackEntry( false, true, LootPack.PotionItems, potionChance, 1 ) );
		}
		
		private static void GenerateInstruments( BaseCreature bc, ref List<LootPackEntry> entries )
		{
			double chance = (double)Math.Max( 0.01, Math.Min( 1, Math.Pow( bc.Difficulty, 0.3 ) / 30 ) ) * 100;
			entries.Add( new LootPackEntry( false, true, LootPack.Instruments, chance, 1 ) );
		}
		
		public static void GenerateMagicItems(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			double countModifier = Config.Get("NelderimLoot.ItemsCountModifier", 1.0);
			int itemsMin = (int)Math.Floor(Math.Pow(bc.Difficulty, 0.275) * countModifier);
			int itemsMax = (int)Math.Max(1, itemsMin + Math.Ceiling((double)itemsMin / 3 * countModifier));
			int itemsCount = Utility.RandomMinMax(itemsMin, itemsMax);

			double propsModifier = Config.Get("NelderimLoot.PropsCountModifier", 1.0);
			int minProps = (int)Utility.Clamp(Math.Log(bc.Difficulty * 0.1) * propsModifier, 1, 5);
			int maxProps = (int)Utility.Clamp(Math.Log(bc.Difficulty) * propsModifier, 1, 5);
			
			double minIntensityModifier = Config.Get("NelderimLoot.MinPropsIntensityModifier", 1.0);
			double maxIntensityModifier = Config.Get("NelderimLoot.MaxPropsIntensityModifier", 1.0);
			double minIntensity = Utility.Clamp( Math.Pow(bc.Difficulty, 0.6) * minIntensityModifier, 5, 30);
			double maxIntensity = Utility.Clamp( (Math.Pow(bc.Difficulty, 0.75) + 30) * maxIntensityModifier, 50, 100);
			int reduceStep = (int)(Math.Max(1, itemsCount * 0.2)); // Po kazdym 20% itemow oslabiamy loot
			for ( int i = 0; i < itemsCount; i++ )
			{
				if (i % reduceStep == 0)
				{
					switch (Utility.Random(4))
					{
						case 0:
							minProps = Math.Max(1, minProps - 1);
							break;
						case 1:
							if (maxProps == minProps) goto case 0;
							maxProps = Math.Max(1, maxProps - 1);
							break;
						case 2:
							minIntensity *= 0.8;
							break;
						case 3:
							maxIntensity *= 0.8;
							break;
					}
				}
				entries.Add( new NelderimLootPackEntry( false, false, NelderimItems, 100.0, 1, minProps, maxProps, (int)minIntensity, (int)maxIntensity, true ) );
			}
		}
		
		private static double[] GetChances( double baseChance, double factor, int count )
		{
			double[] chances = new double[ count ];
			double sum = 0;
			
			chances[0] = baseChance;
			for ( int i = 1; i < count; i++ )
			{
				chances[i] = Math.Min(1 - sum, chances[i - 1] * factor);
				if (chances[i - 1] < 0.01) 
				{
					chances[i] += chances[i - 1];
					chances[i - 1] = 0;
				}
				else
					sum += chances[ i ];
			}
			
			return chances;
		}

		#region LootPacks
		
		public static readonly LootPackItem[] NelderimItems =
		{
			new LootPackItem( typeof( BaseWeapon ), 20 ),
			new LootPackItem( typeof( BaseRanged ), 4 ),
			new LootPackItem( typeof( BaseArmor ), 60 ),
			new LootPackItem( typeof( BaseShield ), 7 ),
			new LootPackItem( typeof( BaseJewel ), 20 )
		};

		public static readonly LootPackItem[] RecallRune = {new LootPackItem(typeof(RecallRune), 1)};
		
		public static readonly LootPackItem[] NL_Scrolls1 =
		{
			new LootPackItem(typeof(ClumsyScroll), 1), new LootPackItem(typeof(CreateFoodScroll), 1),
			new LootPackItem(typeof(FeeblemindScroll), 1), new LootPackItem(typeof(HealScroll), 1),
			new LootPackItem(typeof(MagicArrowScroll), 1), new LootPackItem(typeof(NightSightScroll), 1),
			new LootPackItem(typeof(ReactiveArmorScroll), 1), new LootPackItem(typeof(WeakenScroll), 1),
			new LootPackItem(typeof(CleanseByFireScroll), 1), new LootPackItem(typeof(CurseWeaponScroll), 1),
			new LootPackItem(typeof(AnimalFormScroll), 1)
		};

		public static readonly LootPackItem[] NL_Scrolls2 =
		{
			new LootPackItem(typeof(AgilityScroll), 1), new LootPackItem(typeof(CunningScroll), 1),
			new LootPackItem(typeof(CureScroll), 1), new LootPackItem(typeof(HarmScroll), 1),
			new LootPackItem(typeof(MagicTrapScroll), 1), new LootPackItem(typeof(MagicUnTrapScroll), 1),
			new LootPackItem(typeof(ProtectionScroll), 1), new LootPackItem(typeof(StrengthScroll), 1),
			new LootPackItem(typeof(RemoveCurseScroll), 1), new LootPackItem(typeof(CorpseSkinScroll), 1),
			new LootPackItem(typeof(WraithFormScroll), 1), new LootPackItem(typeof(EvilOmenScroll), 1),
			new LootPackItem(typeof(BackstabScroll), 1)
		};

		public static readonly LootPackItem[] NL_Scrolls3 =
		{
			new LootPackItem(typeof(BlessScroll), 1), new LootPackItem(typeof(FireballScroll), 1),
			new LootPackItem(typeof(MagicLockScroll), 1), new LootPackItem(typeof(PoisonScroll), 1),
			new LootPackItem(typeof(TelekinisisScroll), 1), new LootPackItem(typeof(TeleportScroll), 1),
			new LootPackItem(typeof(UnlockScroll), 1), new LootPackItem(typeof(WallOfStoneScroll), 1),
			new LootPackItem(typeof(ConsecrateWeaponScroll), 1), new LootPackItem(typeof(CloseWoundsScroll), 1),
			new LootPackItem(typeof(BloodOathScroll), 1), new LootPackItem(typeof(PainSpikeScroll), 1),
			new LootPackItem(typeof(SummonFamiliarScroll), 1), new LootPackItem(typeof(ConfidenceScroll), 1),
			new LootPackItem(typeof(HonorableExecutionScroll), 1), new LootPackItem(typeof(SurpriseAttackScroll), 1)
		};

		public static readonly LootPackItem[] NL_Scrolls4 =
		{
			new LootPackItem(typeof(ArchCureScroll), 1), new LootPackItem(typeof(ArchProtectionScroll), 1),
			new LootPackItem(typeof(CurseScroll), 1), new LootPackItem(typeof(FireFieldScroll), 1),
			new LootPackItem(typeof(GreaterHealScroll), 1), new LootPackItem(typeof(LightningScroll), 1),
			new LootPackItem(typeof(ManaDrainScroll), 1), new LootPackItem(typeof(RecallScroll), 1),
			new LootPackItem(typeof(DivineFuryScroll), 1), new LootPackItem(typeof(SacredJourneyScroll), 1),
			new LootPackItem(typeof(MindRotScroll), 1), new LootPackItem(typeof(HorrificBeastScroll), 1),
			new LootPackItem(typeof(AnimateDeadScroll), 1), new LootPackItem(typeof(CounterAttackScroll), 1),
			new LootPackItem(typeof(MirrorImageScroll), 1)
		};

		public static readonly LootPackItem[] NL_Scrolls5 =
		{
			new LootPackItem(typeof(BladeSpiritsScroll), 1), new LootPackItem(typeof(DispelFieldScroll), 1),
			new LootPackItem(typeof(IncognitoScroll), 1), new LootPackItem(typeof(MagicReflectScroll), 1),
			new LootPackItem(typeof(MindBlastScroll), 1), new LootPackItem(typeof(ParalyzeScroll), 1),
			new LootPackItem(typeof(PoisonFieldScroll), 1), new LootPackItem(typeof(SummonCreatureScroll), 1),
			new LootPackItem(typeof(DispelEvilScroll), 1), new LootPackItem(typeof(WitherScroll), 1),
			new LootPackItem(typeof(PoisonStrikeScroll), 1), new LootPackItem(typeof(LightningStrikeScroll), 1),
			new LootPackItem(typeof(ShadowJumpScroll), 1)
		};

		public static readonly LootPackItem[] NL_Scrolls6 =
		{
			new LootPackItem(typeof(DispelScroll), 1), new LootPackItem(typeof(EnergyBoltScroll), 1),
			new LootPackItem(typeof(ExplosionScroll), 1), new LootPackItem(typeof(InvisibilityScroll), 1),
			new LootPackItem(typeof(MarkScroll), 1), new LootPackItem(typeof(MassCurseScroll), 1),
			new LootPackItem(typeof(ParalyzeFieldScroll), 1), new LootPackItem(typeof(RevealScroll), 1),
			new LootPackItem(typeof(EnemyOfOneScroll), 1), new LootPackItem(typeof(StrangleScroll), 1),
			new LootPackItem(typeof(LichFormScroll), 1), new LootPackItem(typeof(EvasionScroll), 1),
			new LootPackItem(typeof(FocusAttackScroll), 1)
		};

		public static readonly LootPackItem[] NL_Scrolls7 = 
		{
			new LootPackItem(typeof(ChainLightningScroll), 1), new LootPackItem(typeof(EnergyFieldScroll), 1),
			new LootPackItem(typeof(FlamestrikeScroll), 1), new LootPackItem(typeof(GateTravelScroll), 1),
			new LootPackItem(typeof(ManaVampireScroll), 1), new LootPackItem(typeof(MassDispelScroll), 1),
			new LootPackItem(typeof(MeteorSwarmScroll), 1), new LootPackItem(typeof(PolymorphScroll), 1),
			new LootPackItem(typeof(HolyLightScroll), 1), new LootPackItem(typeof(ExorcismScroll), 1),
			new LootPackItem(typeof(VengefulSpiritScroll), 1), new LootPackItem(typeof(MomentumStrikeScroll), 1),
			new LootPackItem(typeof(KiAttackScroll), 1), new LootPackItem(typeof(DeathStrikeScroll), 1)
		};

		public static readonly LootPackItem[] NL_Scrolls8 = 
		{
			new LootPackItem(typeof(EarthquakeScroll), 1), new LootPackItem(typeof(EnergyVortexScroll), 1),
			new LootPackItem(typeof(ResurrectionScroll), 1), new LootPackItem(typeof(SummonAirElementalScroll), 1),
			new LootPackItem(typeof(SummonDaemonScroll), 1),
			new LootPackItem(typeof(SummonEarthElementalScroll), 1),
			new LootPackItem(typeof(SummonFireElementalScroll), 1),
			new LootPackItem(typeof(SummonWaterElementalScroll), 1),
			new LootPackItem(typeof(NobleSacrificeScroll), 1), new LootPackItem(typeof(VampiricEmbraceScroll), 1)
		};
		
		private static readonly LootPackItem[][] NL_scrolls = 
		{ 
			NL_Scrolls8, NL_Scrolls7, NL_Scrolls6, NL_Scrolls5, NL_Scrolls4, NL_Scrolls3, NL_Scrolls2, NL_Scrolls1
		};

		public static readonly LootPackItem[] AncientScrollItems =
		{
			new LootPackItem(typeof(AncientCauseFearScroll), 1), new LootPackItem(typeof(AncientCloneScroll), 1),
			new LootPackItem(typeof(AncientDanceScroll), 1), new LootPackItem(typeof(AncientDeathVortexScroll), 1),
			new LootPackItem(typeof(AncientDouseScroll), 1), new LootPackItem(typeof(AncientEnchantScroll), 1),
			new LootPackItem(typeof(AncientFireRingScroll), 1),
			new LootPackItem(typeof(AncientGreatDouseScroll), 1),
			new LootPackItem(typeof(AncientGreatIgniteScroll), 1), new LootPackItem(typeof(AncientIgniteScroll), 1),
			new LootPackItem(typeof(AncientMassMightScroll), 1), new LootPackItem(typeof(AncientPeerScroll), 1),
			new LootPackItem(typeof(AncientSeanceScroll), 1), new LootPackItem(typeof(AncientSwarmScroll), 1),
			new LootPackItem(typeof(AncientDeathVortexScroll), 1),
			new LootPackItem(typeof(AncientDeathVortexScroll), 1)
		};

		public static readonly LootPackItem[] AvatarScrollItems =
		{
			new LootPackItem(typeof(AvatarHeavenlyLightScroll), 1),
			new LootPackItem(typeof(AvatarHeavensGateScroll), 1),
			new LootPackItem(typeof(AvatarMarkOfGodsScroll), 1),
			new LootPackItem(typeof(AvatarRestorationScroll), 1),
			new LootPackItem(typeof(AvatarSacredBoonScroll), 1)
		};

		public static readonly LootPackItem[] BardScrollItems =
		{
			new LootPackItem(typeof(BardArmysPaeonScroll), 1),
			new LootPackItem(typeof(BardEnchantingEtudeScroll), 1),
			new LootPackItem(typeof(BardEnergyCarolScroll), 1),
			new LootPackItem(typeof(AncientDeathVortexScroll), 1),
			new LootPackItem(typeof(BardEnergyThrenodyScroll), 1), new LootPackItem(typeof(BardFireCarolScroll), 1),
			new LootPackItem(typeof(BardFireThrenodyScroll), 1), new LootPackItem(typeof(BardFoeRequiemScroll), 1),
			new LootPackItem(typeof(BardIceCarolScroll), 1), new LootPackItem(typeof(BardIceThrenodyScroll), 1),
			new LootPackItem(typeof(BardKnightsMinneScroll), 1), new LootPackItem(typeof(BardMagesBalladScroll), 1),
			new LootPackItem(typeof(BardMagicFinaleScroll), 1), new LootPackItem(typeof(BardPoisonCarolScroll), 1),
			new LootPackItem(typeof(BardPoisonThrenodyScroll), 1),
			new LootPackItem(typeof(BardSheepfoeMamboScroll), 1), new LootPackItem(typeof(BardSinewyEtudeScroll), 1)
		};

		public static readonly LootPackItem[] ClericScrollItems =
		{
			new LootPackItem(typeof(ClericAngelicFaithScroll), 1),
			new LootPackItem(typeof(ClericBanishEvilScroll), 1),
			new LootPackItem(typeof(ClericDampenSpiritScroll), 1),
			new LootPackItem(typeof(ClericDivineFocusScroll), 1), new LootPackItem(typeof(ClericPurgeScroll), 1),
			new LootPackItem(typeof(ClericHammerOfFaithScroll), 1),
			new LootPackItem(typeof(ClericRestorationScroll), 1),
			new LootPackItem(typeof(ClericSacredBoonScroll), 1), new LootPackItem(typeof(ClericSacrificeScroll), 1),
			new LootPackItem(typeof(ClericTouchOfLifeScroll), 1), new LootPackItem(typeof(ClericSmiteScroll), 1),
			new LootPackItem(typeof(ClericTrialByFireScroll), 1)
		};

		public static readonly LootPackItem[] DruidScrollItems =
		{
			new LootPackItem(typeof(DruidBlendWithForestScroll), 1),
			new LootPackItem(typeof(DruidFamiliarScroll), 1),
			new LootPackItem(typeof(DruidEnchantedGroveScroll), 1),
			new LootPackItem(typeof(DruidGraspingRootsScroll), 1),
			new LootPackItem(typeof(DruidHollowReedScroll), 1),
			new LootPackItem(typeof(DruidLeafWhirlwindScroll), 1),
			new LootPackItem(typeof(DruidLureStoneScroll), 1),
			new LootPackItem(typeof(DruidMushroomGatewayScroll), 1),
			new LootPackItem(typeof(DruidNaturesPassageScroll), 1),
			new LootPackItem(typeof(DruidPackOfBeastScroll), 1),
			new LootPackItem(typeof(DruidRestorativeSoilScroll), 1),
			new LootPackItem(typeof(DruidShieldOfEarthScroll), 1),
			new LootPackItem(typeof(DruidSpringOfLifeScroll), 1),
			new LootPackItem(typeof(DruidStoneCircleScroll), 1),
			new LootPackItem(typeof(DruidSwarmOfInsectsScroll), 1),
			new LootPackItem(typeof(DruidVolcanicEruptionScroll), 1)
		};

		public static readonly LootPackItem[] RangerScrollItems =
		{
			new LootPackItem(typeof(RangerFireBowScroll), 1), new LootPackItem(typeof(RangerHuntersAimScroll), 1),
			new LootPackItem(typeof(RangerIceBowScroll), 1), new LootPackItem(typeof(RangerLightningBowScroll), 1),
			new LootPackItem(typeof(RangerNoxBowScroll), 1), new LootPackItem(typeof(RangerPhoenixFlightScroll), 1),
			new LootPackItem(typeof(RangerFamiliarScroll), 1), new LootPackItem(typeof(RangerSummonMountScroll), 1)
		};

		public static readonly LootPackItem[] UndeadScrollItems =
		{
			new LootPackItem(typeof(UndeadAngelicFaithScroll), 1),
			new LootPackItem(typeof(UndeadCauseFearScroll), 1),
			new LootPackItem(typeof(UndeadGraspingRootsScroll), 1),
			new LootPackItem(typeof(UndeadHammerOfFaithScroll), 1),
			new LootPackItem(typeof(UndeadHollowReedScroll), 1),
			new LootPackItem(typeof(UndeadLeafWhirlwindScroll), 1),
			new LootPackItem(typeof(UndeadLureStoneScroll), 1),
			new LootPackItem(typeof(UndeadMushroomGatewayScroll), 1),
			new LootPackItem(typeof(UndeadNaturesPassageScroll), 1),
			new LootPackItem(typeof(UndeadSeanceScroll), 1),
			new LootPackItem(typeof(UndeadSwarmOfInsectsScroll), 1),
			new LootPackItem(typeof(UndeadVolcanicEruptionScroll), 1)
		};
		#endregion
	}
}
