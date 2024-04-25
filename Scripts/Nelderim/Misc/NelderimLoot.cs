#region References

using System.Collections.Generic;
using Nelderim.Configuration;
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
using static System.Math;

#endregion

namespace Nelderim
{
	public class NelderimLootPackEntry : LootPackEntry
	{
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

		public int MinProps { get; }

		public override int GetBonusProperties()
		{
			return Utility.RandomMinMax(MinProps, MinProps);
		}
	}

	public class NelderimLoot
	{
		public static LootPack Generate(BaseCreature bc, LootStage stage)
		{
			if (bc.Controlled || bc.Summoned || bc.AI == AIType.AI_Animal)
				return LootPack.Empty;

			var entries = new List<LootPackEntry>();

			if (stage == LootStage.Spawning)
				GenerateGold(bc, ref entries);
			else
			{
				//GenerateSpellbooks( bc, ref entries );
				GenerateMagicItems(bc, ref entries);
				GenerateGems(bc, ref entries);
				GenerateScrolls(bc, ref entries);
				GeneratePotions(bc, ref entries);
				GenerateInstruments(bc, ref entries);
				GenerateReagents(bc, ref entries);
				GenerateArrows(bc, ref entries);
				GenerateRecallRunes(bc, ref entries);
			}

			if (entries.Count > 0)
				return new LootPack(entries.ToArray());

			return LootPack.Empty;
		}

		private static void GenerateGold(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			var minGold = (int)Ceiling(Pow(bc.Difficulty * 10, 0.5));
			var maxGold = (int)Ceiling(minGold * 1.3);

			var amount = (int)(Utility.RandomMinMax(minGold, maxGold) * NConfig.Loot.GoldModifier);

			entries.Add(new LootPackEntry(true, true, LootPack.Gold, 100.0, amount));
		}

		private static void GenerateRecallRunes(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			var count = (int)Min(5, 1 + bc.Difficulty * 0.002);
			var chance = Pow(bc.Difficulty, 0.6) * 0.025;

			entries.Add(new LootPackEntry(false, true, RecallRune, chance, count));
		}

		private static void GenerateGems(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			var count = (int)Ceiling(Pow(bc.Difficulty, 0.25) * 0.5);
			var chance = Min(1, 0.1 + Pow(bc.Difficulty, 0.25) * 0.05) * 100;

			for (var i = 0; i < count; i++)
				entries.Add(new LootPackEntry(false, true,
					Utility.RandomDouble() > 0.01 ? LootPack.GemItems : LootPack.RareGemItems, chance, 1));
		}

		private static void GenerateReagents(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			LootPackItem[] lootItems;
			switch (bc.AI)
			{
				case AIType.AI_Mage:
					lootItems = LootPack.MageryRegItems;
					break;
				case AIType.AI_Necro:
					lootItems = LootPack.NecroRegItems;
					break;
				case AIType.AI_NecroMage:
					lootItems = Utility.RandomBool() ? LootPack.MageryRegItems : LootPack.NecroRegItems;
					break;
				case AIType.AI_Mystic:
					lootItems = LootPack.MysticRegItems;
					break;
				default:
					return;
			}

			var count = (int)Ceiling(Pow(bc.Difficulty, 0.2)) * 5;
			entries.Add(new LootPackEntry(false, true, lootItems, 100, count));
		}

		private static void GenerateArrows(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			BaseRanged weapon = bc.FindItemOnLayer(Layer.TwoHanded) as BaseRanged;
			if (weapon != null)
			{
				var count = (int)(Ceiling(Pow(bc.Difficulty, 0.2)) * 10 * (Utility.RandomDouble() + 0.5)); // +/- 50%

				entries.Add(
					new LootPackEntry(false, true, new[] { new LootPackItem(weapon.AmmoType, 100) }, 100, count));
			}
		}

		private static void GenerateScrolls(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			var count = (int)Floor(Pow((1 + bc.Difficulty) * 5, 0.15));
			var scrollChance = Max(0.01, Min(0.8, Pow(bc.Difficulty * 0.5, 0.29) * 0.025));

			var chances = GetChances(Min(1, Pow(bc.Difficulty, 0.7) * 0.0001), 2.0, NL_scrolls.Length);

			for (var i = 0; i < count; i++)
			{
				if (Utility.RandomDouble() >= scrollChance)
					continue;

				entries.Add(new LootPackEntry(false, true, NL_scrolls[Utility.RandomIndex(chances)], 100, 1));
			}
		}

		private static void GeneratePotions(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			var count = (int)Floor(Pow((1 + bc.Difficulty) * 5, 0.15));
			var potionChance = Max(0.01, Min(0.8, Pow(bc.Difficulty * 0.5, 0.29) * 0.1)) * 100;

			for (var i = 0; i < count; i++)
				entries.Add(new LootPackEntry(false, true, LootPack.PotionItems, potionChance, 1));
		}

		private static void GenerateInstruments(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			var chance = Max(0.01, Min(1, Pow(bc.Difficulty, 0.25) * 0.1)) * 100;
			entries.Add(new LootPackEntry(false, true, LootPack.Instruments, chance, 1));
		}

		public static void GenerateMagicItems(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			var countModifier = NConfig.Loot.ItemsCountModifier;
			var itemsMin = (int)Floor(Pow(bc.Difficulty, 0.2) * countModifier);
			var itemsMax = (int)Max(1, itemsMin + Ceiling((double)itemsMin / 3 * countModifier));
			var itemsCount = Utility.RandomMinMax(itemsMin, itemsMax);

			var propsModifier = NConfig.Loot.PropsCountModifier;
			var minProps = (int)Utility.Clamp(Log(bc.Difficulty * 0.1) * propsModifier, 1, 5);
			var maxProps = (int)Utility.Clamp(Log(bc.Difficulty) * propsModifier, 1, 5);

			var minIntensityModifier = NConfig.Loot.MinIntensityModifier;
			var maxIntensityModifier = NConfig.Loot.MaxIntensityModifier;
			var minIntensity = Utility.Clamp(Pow(bc.Difficulty, 0.6) * minIntensityModifier, 5, 30);
			var maxIntensity = Utility.Clamp((Pow(bc.Difficulty, 0.75) + 30) * maxIntensityModifier, 50, 100);
			var reduceStep = (int)Max(1, itemsCount * 0.2); // Po kazdym 20% itemow oslabiamy loot
			for (var i = 0; i < itemsCount; i++)
			{
				if (i % reduceStep == 0)
					switch (Utility.Random(4))
					{
						case 0:
							minProps = Max(1, minProps - 1);
							break;
						case 1:
							if (maxProps == minProps) goto case 0;
							maxProps = Max(1, maxProps - 1);
							break;
						case 2:
							minIntensity *= 0.8;
							break;
						case 3:
							maxIntensity *= 0.8;
							break;
					}

				entries.Add(new NelderimLootPackEntry(false, false, NelderimItems, 100.0, 1, minProps, maxProps,
					(int)minIntensity, (int)maxIntensity, true));
			}
		}

		private static double[] GetChances(double baseChance, double factor, int count)
		{
			var chances = new double[count];
			double sum = 0;

			chances[0] = baseChance;
			for (var i = 1; i < count; i++)
			{
				chances[i] = Min(1 - sum, chances[i - 1] * factor);
				if (chances[i - 1] < 0.01)
				{
					chances[i] += chances[i - 1];
					chances[i - 1] = 0;
				}
				else
					sum += chances[i];
			}

			return chances;
		}

		#region LootPacks

		public static readonly LootPackItem[] NelderimItems =
		{
			new LootPackItem(typeof(BaseWeapon), 20), new LootPackItem(typeof(BaseRanged), 4),
			new LootPackItem(typeof(BaseArmor), 60), new LootPackItem(typeof(BaseShield), 7),
			new LootPackItem(typeof(BaseJewel), 20)
		};

		public static readonly LootPackItem[] RecallRune = { new LootPackItem(typeof(RecallRune)) };


		public static readonly LootPackItem[] Scrolls1 =
		{
			new LootPackItem(typeof(ClumsyScroll)), new LootPackItem(typeof(CreateFoodScroll)),
			new LootPackItem(typeof(FeeblemindScroll)), new LootPackItem(typeof(HealScroll)),
			new LootPackItem(typeof(MagicArrowScroll)), new LootPackItem(typeof(NightSightScroll)),
			new LootPackItem(typeof(ReactiveArmorScroll)), new LootPackItem(typeof(WeakenScroll)),
			//Chivalry
			new LootPackItem(typeof(CloseWoundsScroll)),
			//Necromancy
			new LootPackItem(typeof(CurseWeaponScroll)),
			//Ninjitsu
			new LootPackItem(typeof(AnimalFormScroll)),
			//Spellweaving
			//new LootPackItem(typeof(ArcaneCircleScroll)), new LootPackItem(typeof(AttuneWeaponScroll)),
			//new LootPackItem(typeof(GiftOfRenewalScroll)), new LootPackItem(typeof(NatureFuryScroll)),
			//Mysticism
			// new LootPackItem(typeof(HealingStoneScroll)), new LootPackItem(typeof(NetherBoltScroll)),
		};

		public static readonly LootPackItem[] Scrolls2 =
		{
			new LootPackItem(typeof(AgilityScroll)), new LootPackItem(typeof(CunningScroll)),
			new LootPackItem(typeof(CureScroll)), new LootPackItem(typeof(HarmScroll)),
			new LootPackItem(typeof(MagicTrapScroll)), new LootPackItem(typeof(MagicUnTrapScroll)),
			new LootPackItem(typeof(ProtectionScroll)), new LootPackItem(typeof(StrengthScroll)),
			//Chivalry
			new LootPackItem(typeof(RemoveCurseScroll)), new LootPackItem(typeof(CleanseByFireScroll)),
			//Mysticism
			// new LootPackItem(typeof(EnchantScroll)), new LootPackItem(typeof(PurgeMagicScroll)),
		};

		public static readonly LootPackItem[] Scrolls3 =
		{
			new LootPackItem(typeof(BlessScroll)), new LootPackItem(typeof(FireballScroll)),
			new LootPackItem(typeof(MagicLockScroll)), new LootPackItem(typeof(PoisonScroll)),
			new LootPackItem(typeof(TelekinisisScroll)), new LootPackItem(typeof(TeleportScroll)),
			new LootPackItem(typeof(UnlockScroll)), new LootPackItem(typeof(WallOfStoneScroll)),
			//Chivalry
			new LootPackItem(typeof(ConsecrateWeaponScroll)), new LootPackItem(typeof(SacredJourneyScroll)),
			//Necromancy
			new LootPackItem(typeof(BloodOathScroll)), new LootPackItem(typeof(CorpseSkinScroll)),
			new LootPackItem(typeof(EvilOmenScroll)), new LootPackItem(typeof(PainSpikeScroll)),
			new LootPackItem(typeof(WraithFormScroll)),
			//Ninjitsu
			new LootPackItem(typeof(MirrorImageScroll)),
			//Spellweaving
			//new LootPackItem(typeof(ImmolatingWeaponScroll)), new LootPackItem(typeof(ThunderstormScroll)),
			//Mysticism
			// new LootPackItem(typeof(EagleStrikeScroll)), new LootPackItem(typeof(SleepScroll)),
		};

		public static readonly LootPackItem[] Scrolls4 =
		{
			new LootPackItem(typeof(ArchCureScroll)), new LootPackItem(typeof(ArchProtectionScroll)),
			new LootPackItem(typeof(CurseScroll)), new LootPackItem(typeof(FireFieldScroll)),
			new LootPackItem(typeof(GreaterHealScroll)), new LootPackItem(typeof(LightningScroll)),
			new LootPackItem(typeof(ManaDrainScroll)), new LootPackItem(typeof(RecallScroll)),
			//Chivalry
			new LootPackItem(typeof(DivineFuryScroll)),
			//Necromancy
			new LootPackItem(typeof(MindRotScroll)), new LootPackItem(typeof(SummonFamiliarScroll)),
			//Bushido
			new LootPackItem(typeof(HonorableExecutionScroll)), new LootPackItem(typeof(ConfidenceScroll)),
			//Ninjitsu
			new LootPackItem(typeof(FocusAttackScroll)),
			//Spellweaving
			//new LootPackItem(typeof(ArcaneEmpowermentScroll)), new LootPackItem(typeof(EtherealVoyageScroll)),
			//new LootPackItem(typeof(ReaperFormScroll)),
			//Mysticism
			// new LootPackItem(typeof(AnimatedWeaponScroll)), new LootPackItem(typeof(StoneFormScroll)),
		};

		public static readonly LootPackItem[] Scrolls5 =
		{
			new LootPackItem(typeof(BladeSpiritsScroll)), new LootPackItem(typeof(DispelFieldScroll)),
			new LootPackItem(typeof(IncognitoScroll)), new LootPackItem(typeof(MagicReflectScroll)),
			new LootPackItem(typeof(MindBlastScroll)), new LootPackItem(typeof(ParalyzeScroll)),
			new LootPackItem(typeof(PoisonFieldScroll)), new LootPackItem(typeof(SummonCreatureScroll)),
			//Chivalry
			new LootPackItem(typeof(DispelEvilScroll)), new LootPackItem(typeof(EnemyOfOneScroll)),
			//Necromancy
			new LootPackItem(typeof(AnimateDeadScroll)), new LootPackItem(typeof(HorrificBeastScroll)),
			//Bushido
			new LootPackItem(typeof(CounterAttackScroll)),
			//Ninjitsu
			new LootPackItem(typeof(BackstabScroll)),
			//Spellweaving
			//new LootPackItem(typeof(GiftOfLifeScroll)), new LootPackItem(typeof(SummonFeyScroll)),
			//new LootPackItem(typeof(SummonFiendScroll)),
			//Mysticism
			// new LootPackItem(typeof(MassSleepScroll)), new LootPackItem(typeof(SpellTriggerScroll)),
		};

		public static readonly LootPackItem[] Scrolls6 =
		{
			new LootPackItem(typeof(DispelScroll)), new LootPackItem(typeof(EnergyBoltScroll)),
			new LootPackItem(typeof(ExplosionScroll)), new LootPackItem(typeof(InvisibilityScroll)),
			new LootPackItem(typeof(MarkScroll)), new LootPackItem(typeof(MassCurseScroll)),
			new LootPackItem(typeof(ParalyzeFieldScroll)), new LootPackItem(typeof(RevealScroll)),
			//Chivalry
			new LootPackItem(typeof(HolyLightScroll)),
			//Necromancy
			new LootPackItem(typeof(WitherScroll)), new LootPackItem(typeof(StrangleScroll)),
			//Bushido
			new LootPackItem(typeof(LightningStrikeScroll)), new LootPackItem(typeof(EvasionScroll)),
			//Ninjitsu
			new LootPackItem(typeof(ShadowJumpScroll)), new LootPackItem(typeof(SurpriseAttackScroll)),
			//Spellweaving
			//new LootPackItem(typeof(DryadAllureScroll)), new LootPackItem(typeof(EssenceOfWindScroll)),
			//Mysticism
			// new LootPackItem(typeof(BombardScroll)), new LootPackItem(typeof(CleansingWindsScroll)),
		};

		public static readonly LootPackItem[] Scrolls7 =
		{
			new LootPackItem(typeof(ChainLightningScroll)), new LootPackItem(typeof(EnergyFieldScroll)),
			new LootPackItem(typeof(FlamestrikeScroll)), new LootPackItem(typeof(GateTravelScroll)),
			new LootPackItem(typeof(ManaVampireScroll)), new LootPackItem(typeof(MassDispelScroll)),
			new LootPackItem(typeof(MeteorSwarmScroll)), new LootPackItem(typeof(PolymorphScroll)),
			//Chivalry
			new LootPackItem(typeof(NobleSacrificeScroll)),
			//Necromancy
			new LootPackItem(typeof(LichFormScroll)),
			//Bushido
			new LootPackItem(typeof(MomentumStrikeScroll)),
			//Spellweaving
			//new LootPackItem(typeof(WildfireScroll)),
			//Mysticism
			// new LootPackItem(typeof(HailStormScroll)), new LootPackItem(typeof(SpellPlagueScroll)),
		};

		public static readonly LootPackItem[] Scrolls8 =
		{
			new LootPackItem(typeof(EarthquakeScroll)), new LootPackItem(typeof(EnergyVortexScroll)),
			new LootPackItem(typeof(ResurrectionScroll)), new LootPackItem(typeof(SummonAirElementalScroll)),
			new LootPackItem(typeof(SummonDaemonScroll)), new LootPackItem(typeof(SummonEarthElementalScroll)),
			new LootPackItem(typeof(SummonFireElementalScroll)), new LootPackItem(typeof(SummonWaterElementalScroll)),
			//Necromancy
			new LootPackItem(typeof(ExorcismScroll)), new LootPackItem(typeof(VengefulSpiritScroll)),
			new LootPackItem(typeof(VampiricEmbraceScroll)),
			//Ninjitsu
			new LootPackItem(typeof(KiAttackScroll)), new LootPackItem(typeof(DeathStrikeScroll)),
			//Spellweaving
		//	new LootPackItem(typeof(WordOfDeathScroll)),
			//Mysticism
			// new LootPackItem(typeof(NetherCycloneScroll)), new LootPackItem(typeof(RisingColossusScroll)),
		};

		private static readonly LootPackItem[][] NL_scrolls =
		{
			Scrolls8, Scrolls7, Scrolls6, Scrolls5, Scrolls4, Scrolls3, Scrolls2, Scrolls1
		};

		public static readonly LootPackItem[] AncientScrollItems =
		{
			new LootPackItem(typeof(AncientCauseFearScroll)), new LootPackItem(typeof(AncientCloneScroll)),
			new LootPackItem(typeof(AncientDanceScroll)), new LootPackItem(typeof(AncientDeathVortexScroll)),
			new LootPackItem(typeof(AncientDouseScroll)), new LootPackItem(typeof(AncientEnchantScroll)),
			new LootPackItem(typeof(AncientFireRingScroll)), new LootPackItem(typeof(AncientGreatDouseScroll)),
			new LootPackItem(typeof(AncientGreatIgniteScroll)), new LootPackItem(typeof(AncientIgniteScroll)),
			new LootPackItem(typeof(AncientMassMightScroll)), new LootPackItem(typeof(AncientPeerScroll)),
			new LootPackItem(typeof(AncientSeanceScroll)), new LootPackItem(typeof(AncientSwarmScroll)),
			new LootPackItem(typeof(AncientDeathVortexScroll)), new LootPackItem(typeof(AncientDeathVortexScroll))
		};

		public static readonly LootPackItem[] AvatarScrollItems =
		{
			new LootPackItem(typeof(AvatarHeavenlyLightScroll)), new LootPackItem(typeof(AvatarHeavensGateScroll)),
			new LootPackItem(typeof(AvatarMarkOfGodsScroll)), new LootPackItem(typeof(AvatarRestorationScroll)),
			new LootPackItem(typeof(AvatarSacredBoonScroll)), new LootPackItem(typeof(AvatarAngelicFaithScroll)),
			new LootPackItem(typeof(AvatarArmysPaeonScroll))
		};

		public static readonly LootPackItem[] BardScrollItems =
		{
			new LootPackItem(typeof(BardArmysPaeonScroll)), new LootPackItem(typeof(BardEnchantingEtudeScroll)),
			new LootPackItem(typeof(BardEnergyCarolScroll)), new LootPackItem(typeof(AncientDeathVortexScroll)),
			new LootPackItem(typeof(BardEnergyThrenodyScroll)), new LootPackItem(typeof(BardFireCarolScroll)),
			new LootPackItem(typeof(BardFireThrenodyScroll)), new LootPackItem(typeof(BardFoeRequiemScroll)),
			new LootPackItem(typeof(BardIceCarolScroll)), new LootPackItem(typeof(BardIceThrenodyScroll)),
			new LootPackItem(typeof(BardKnightsMinneScroll)), new LootPackItem(typeof(BardMagesBalladScroll)),
			new LootPackItem(typeof(BardMagicFinaleScroll)), new LootPackItem(typeof(BardPoisonCarolScroll)),
			new LootPackItem(typeof(BardPoisonThrenodyScroll)), new LootPackItem(typeof(BardSheepfoeMamboScroll)),
			new LootPackItem(typeof(BardSinewyEtudeScroll))
		};

		public static readonly LootPackItem[] ClericScrollItems =
		{
			new LootPackItem(typeof(ClericAngelicFaithScroll)), new LootPackItem(typeof(ClericBanishEvilScroll)),
			new LootPackItem(typeof(ClericDampenSpiritScroll)), new LootPackItem(typeof(ClericDivineFocusScroll)),
			new LootPackItem(typeof(ClericPurgeScroll)), new LootPackItem(typeof(ClericHammerOfFaithScroll)),
			new LootPackItem(typeof(ClericRestorationScroll)), new LootPackItem(typeof(ClericSacredBoonScroll)),
			new LootPackItem(typeof(ClericSacrificeScroll)), new LootPackItem(typeof(ClericTouchOfLifeScroll)),
			new LootPackItem(typeof(ClericSmiteScroll)), new LootPackItem(typeof(ClericTrialByFireScroll))
		};

		public static readonly LootPackItem[] DruidScrollItems =
		{
			new LootPackItem(typeof(DruidBlendWithForestScroll)), new LootPackItem(typeof(DruidFamiliarScroll)),
			new LootPackItem(typeof(DruidEnchantedGroveScroll)), new LootPackItem(typeof(DruidGraspingRootsScroll)),
			new LootPackItem(typeof(DruidHollowReedScroll)), new LootPackItem(typeof(DruidLeafWhirlwindScroll)),
			new LootPackItem(typeof(DruidLureStoneScroll)), new LootPackItem(typeof(DruidMushroomGatewayScroll)),
			new LootPackItem(typeof(DruidNaturesPassageScroll)), new LootPackItem(typeof(DruidPackOfBeastScroll)),
			new LootPackItem(typeof(DruidRestorativeSoilScroll)),
			new LootPackItem(typeof(DruidShieldOfEarthScroll)), new LootPackItem(typeof(DruidSpringOfLifeScroll)),
			new LootPackItem(typeof(DruidStoneCircleScroll)), new LootPackItem(typeof(DruidSwarmOfInsectsScroll)),
			new LootPackItem(typeof(DruidVolcanicEruptionScroll))
		};

		public static readonly LootPackItem[] RangerScrollItems =
		{
			new LootPackItem(typeof(RangerFireBowScroll)), new LootPackItem(typeof(RangerHuntersAimScroll)),
			new LootPackItem(typeof(RangerIceBowScroll)), new LootPackItem(typeof(RangerLightningBowScroll)),
			new LootPackItem(typeof(RangerNoxBowScroll)), new LootPackItem(typeof(RangerPhoenixFlightScroll)),
			new LootPackItem(typeof(RangerFamiliarScroll)), new LootPackItem(typeof(RangerSummonMountScroll))
		};

		public static readonly LootPackItem[] UndeadScrollItems =
		{
			new LootPackItem(typeof(UndeadAngelicFaithScroll)), new LootPackItem(typeof(UndeadCauseFearScroll)),
			new LootPackItem(typeof(UndeadGraspingRootsScroll)),
			new LootPackItem(typeof(UndeadHammerOfFaithScroll)), new LootPackItem(typeof(UndeadHollowReedScroll)),
			new LootPackItem(typeof(UndeadLeafWhirlwindScroll)), new LootPackItem(typeof(UndeadLureStoneScroll)),
			new LootPackItem(typeof(UndeadMushroomGatewayScroll)),
			new LootPackItem(typeof(UndeadNaturesPassageScroll)), new LootPackItem(typeof(UndeadSeanceScroll)),
			new LootPackItem(typeof(UndeadSwarmOfInsectsScroll)),
			new LootPackItem(typeof(UndeadVolcanicEruptionScroll))
		};

		#endregion
	}
}
