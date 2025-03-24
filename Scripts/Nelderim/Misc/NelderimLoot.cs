#region References

using System;
using System.Collections.Generic;
using Nelderim.Configuration;
using Server;
using Server.ACC.CSS.Systems.Ancient;
using Server.ACC.CSS.Systems.Avatar;
using Server.ACC.CSS.Systems.Bard;
using Server.ACC.CSS.Systems.Cleric;
using Server.ACC.CSS.Systems.Druid;
using Server.ACC.CSS.Systems.Ranger;
using Server.ACC.CSS.Systems.Rogue;
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
				return Empty;

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

			return Empty;
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

			var count = (int)Ceiling(Pow(bc.Difficulty, 0.15)) * 5;
			entries.Add(new LootPackEntry(false, true, lootItems, 100, count));
		}

		private static void GenerateArrows(BaseCreature bc, ref List<LootPackEntry> entries)
		{
			BaseRanged weapon = bc.FindItemOnLayer(Layer.TwoHanded) as BaseRanged;
			if (weapon != null)
			{
				var count = (int)(Ceiling(Pow(bc.Difficulty, 0.2)) * 10 * (Utility.RandomDouble() + 0.5)); // +/- 50%

				entries.Add(
					new LootPackEntry(false, true, [new LootPackItem(weapon.AmmoType, 100)], 100, count));
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
		
		public static readonly LootPack Empty = new([]);

		public static readonly LootPackItem[] NelderimItems =
		[
			new(typeof(BaseWeapon), 20), new(typeof(BaseRanged), 4),
			new(typeof(BaseArmor), 60), new(typeof(BaseShield), 7),
			new(typeof(BaseJewel), 20)
		];

		public static readonly LootPackItem[] RecallRune = [new(typeof(RecallRune))];


		public static readonly LootPackItem[] Scrolls1 =
		[
			new(typeof(ClumsyScroll)), new(typeof(CreateFoodScroll)),
			new(typeof(FeeblemindScroll)), new(typeof(HealScroll)),
			new(typeof(MagicArrowScroll)), new(typeof(NightSightScroll)),
			new(typeof(ReactiveArmorScroll)), new(typeof(WeakenScroll)),
			//Chivalry
			new(typeof(CloseWoundsScroll)),
			//Necromancy
			new(typeof(CurseWeaponScroll)),
			//Ninjitsu
			new(typeof(AnimalFormScroll))
			//Spellweaving
			//new LootPackItem(typeof(ArcaneCircleScroll)), new LootPackItem(typeof(AttuneWeaponScroll)),
			//new LootPackItem(typeof(GiftOfRenewalScroll)), new LootPackItem(typeof(NatureFuryScroll)),
			//Mysticism
			// new LootPackItem(typeof(HealingStoneScroll)), new LootPackItem(typeof(NetherBoltScroll)),
		];

		public static readonly LootPackItem[] Scrolls2 =
		[
			new(typeof(AgilityScroll)), new(typeof(CunningScroll)),
			new(typeof(CureScroll)), new(typeof(HarmScroll)),
			new(typeof(MagicTrapScroll)), new(typeof(MagicUnTrapScroll)),
			new(typeof(ProtectionScroll)), new(typeof(StrengthScroll)),
			//Chivalry
			new(typeof(RemoveCurseScroll)), new(typeof(CleanseByFireScroll))
			//Mysticism
			// new LootPackItem(typeof(EnchantScroll)), new LootPackItem(typeof(PurgeMagicScroll)),
		];

		public static readonly LootPackItem[] Scrolls3 =
		[
			new(typeof(BlessScroll)), new(typeof(FireballScroll)),
			new(typeof(MagicLockScroll)), new(typeof(PoisonScroll)),
			new(typeof(TelekinisisScroll)), new(typeof(TeleportScroll)),
			new(typeof(UnlockScroll)), new(typeof(WallOfStoneScroll)),
			//Chivalry
			new(typeof(ConsecrateWeaponScroll)), new(typeof(SacredJourneyScroll)),
			//Necromancy
			new(typeof(BloodOathScroll)), new(typeof(CorpseSkinScroll)),
			new(typeof(EvilOmenScroll)), new(typeof(PainSpikeScroll)),
			new(typeof(WraithFormScroll)),
			//Ninjitsu
			new(typeof(MirrorImageScroll))
			//Spellweaving
			//new LootPackItem(typeof(ImmolatingWeaponScroll)), new LootPackItem(typeof(ThunderstormScroll)),
			//Mysticism
			// new LootPackItem(typeof(EagleStrikeScroll)), new LootPackItem(typeof(SleepScroll)),
		];

		public static readonly LootPackItem[] Scrolls4 =
		[
			new(typeof(ArchCureScroll)), new(typeof(ArchProtectionScroll)),
			new(typeof(CurseScroll)), new(typeof(FireFieldScroll)),
			new(typeof(GreaterHealScroll)), new(typeof(LightningScroll)),
			new(typeof(ManaDrainScroll)), new(typeof(RecallScroll)),
			//Chivalry
			new(typeof(DivineFuryScroll)),
			//Necromancy
			new(typeof(MindRotScroll)), new(typeof(SummonFamiliarScroll)),
			//Bushido
			new(typeof(HonorableExecutionScroll)), new(typeof(ConfidenceScroll)),
			//Ninjitsu
			new(typeof(FocusAttackScroll))
			//Spellweaving
			//new LootPackItem(typeof(ArcaneEmpowermentScroll)), new LootPackItem(typeof(EtherealVoyageScroll)),
			//new LootPackItem(typeof(ReaperFormScroll)),
			//Mysticism
			// new LootPackItem(typeof(AnimatedWeaponScroll)), new LootPackItem(typeof(StoneFormScroll)),
		];

		public static readonly LootPackItem[] Scrolls5 =
		[
			new(typeof(BladeSpiritsScroll)), new(typeof(DispelFieldScroll)),
			new(typeof(IncognitoScroll)), new(typeof(MagicReflectScroll)),
			new(typeof(MindBlastScroll)), new(typeof(ParalyzeScroll)),
			new(typeof(PoisonFieldScroll)), new(typeof(SummonCreatureScroll)),
			//Chivalry
			new(typeof(DispelEvilScroll)), new(typeof(EnemyOfOneScroll)),
			//Necromancy
			new(typeof(AnimateDeadScroll)), new(typeof(HorrificBeastScroll)),
			//Bushido
			new(typeof(CounterAttackScroll)),
			//Ninjitsu
			new(typeof(BackstabScroll))
			//Spellweaving
			//new LootPackItem(typeof(GiftOfLifeScroll)), new LootPackItem(typeof(SummonFeyScroll)),
			//new LootPackItem(typeof(SummonFiendScroll)),
			//Mysticism
			// new LootPackItem(typeof(MassSleepScroll)), new LootPackItem(typeof(SpellTriggerScroll)),
		];

		public static readonly LootPackItem[] Scrolls6 =
		[
			new(typeof(DispelScroll)), new(typeof(EnergyBoltScroll)),
			new(typeof(ExplosionScroll)), new(typeof(InvisibilityScroll)),
			new(typeof(MarkScroll)), new(typeof(MassCurseScroll)),
			new(typeof(ParalyzeFieldScroll)), new(typeof(RevealScroll)),
			//Chivalry
			new(typeof(HolyLightScroll)),
			//Necromancy
			new(typeof(WitherScroll)), new(typeof(StrangleScroll)),
			//Bushido
			new(typeof(LightningStrikeScroll)), new(typeof(EvasionScroll)),
			//Ninjitsu
			new(typeof(ShadowJumpScroll)), new(typeof(SurpriseAttackScroll))
			//Spellweaving
			//new LootPackItem(typeof(DryadAllureScroll)), new LootPackItem(typeof(EssenceOfWindScroll)),
			//Mysticism
			// new LootPackItem(typeof(BombardScroll)), new LootPackItem(typeof(CleansingWindsScroll)),
		];

		public static readonly LootPackItem[] Scrolls7 =
		[
			new(typeof(ChainLightningScroll)), new(typeof(EnergyFieldScroll)),
			new(typeof(FlamestrikeScroll)), new(typeof(GateTravelScroll)),
			new(typeof(ManaVampireScroll)), new(typeof(MassDispelScroll)),
			new(typeof(MeteorSwarmScroll)), new(typeof(PolymorphScroll)),
			//Chivalry
			new(typeof(NobleSacrificeScroll)),
			//Necromancy
			new(typeof(LichFormScroll)),
			//Bushido
			new(typeof(MomentumStrikeScroll))
			//Spellweaving
			//new LootPackItem(typeof(WildfireScroll)),
			//Mysticism
			// new LootPackItem(typeof(HailStormScroll)), new LootPackItem(typeof(SpellPlagueScroll)),
		];

		public static readonly LootPackItem[] Scrolls8 =
		[
			new(typeof(EarthquakeScroll)), new(typeof(EnergyVortexScroll)),
			new(typeof(ResurrectionScroll)), new(typeof(SummonAirElementalScroll)),
			new(typeof(SummonDaemonScroll)), new(typeof(SummonEarthElementalScroll)),
			new(typeof(SummonFireElementalScroll)), new(typeof(SummonWaterElementalScroll)),
			//Necromancy
			new(typeof(ExorcismScroll)), new(typeof(VengefulSpiritScroll)),
			new(typeof(VampiricEmbraceScroll)),
			//Ninjitsu
			new(typeof(KiAttackScroll)), new(typeof(DeathStrikeScroll))
			//Spellweaving
		//	new LootPackItem(typeof(WordOfDeathScroll)),
			//Mysticism
			// new LootPackItem(typeof(NetherCycloneScroll)), new LootPackItem(typeof(RisingColossusScroll)),
		];

		private static readonly LootPackItem[][] NL_scrolls =
		[
			Scrolls8, Scrolls7, Scrolls6, Scrolls5, Scrolls4, Scrolls3, Scrolls2, Scrolls1
		];

		public static readonly LootPackItem[] AncientScrollItems =
		[
			new(typeof(AncientCauseFearScroll)), new(typeof(AncientCloneScroll)),
			new(typeof(AncientDanceScroll)), new(typeof(AncientDeathVortexScroll)),
			new(typeof(AncientDouseScroll)), new(typeof(AncientEnchantScroll)),
			new(typeof(AncientFireRingScroll)), new(typeof(AncientGreatDouseScroll)),
			new(typeof(AncientGreatIgniteScroll)), new(typeof(AncientIgniteScroll)),
			new(typeof(AncientMassMightScroll)), new(typeof(AncientPeerScroll)),
			new(typeof(AncientSeanceScroll)), new(typeof(AncientSwarmScroll)),
			new(typeof(AncientDeathVortexScroll)), new(typeof(AncientDeathVortexScroll))
		];

		public static readonly LootPackItem[] AvatarScrollItems =
		[
			new(typeof(AvatarHeavenlyLightScroll)), new(typeof(AvatarHeavensGateScroll)),
			new(typeof(AvatarMarkOfGodsScroll)), new(typeof(AvatarRestorationScroll)),
			new(typeof(AvatarSacredBoonScroll)), new(typeof(AvatarAngelicFaithScroll)),
			new(typeof(AvatarArmysPaeonScroll)), new (typeof(AvatarEnemyOfOneSpell))
		];

		public static readonly LootPackItem[] BardScrollItems =
		[
			new(typeof(BardArmysPaeonScroll)), new(typeof(BardEnchantingEtudeScroll)),
			new(typeof(BardEnergyCarolScroll)), new(typeof(AncientDeathVortexScroll)),
			new(typeof(BardEnergyThrenodyScroll)), new(typeof(BardFireCarolScroll)),
			new(typeof(BardFireThrenodyScroll)), new(typeof(BardFoeRequiemScroll)),
			new(typeof(BardIceCarolScroll)), new(typeof(BardIceThrenodyScroll)),
			new(typeof(BardKnightsMinneScroll)), new(typeof(BardMagesBalladScroll)),
			new(typeof(BardMagicFinaleScroll)), new(typeof(BardPoisonCarolScroll)),
			new(typeof(BardPoisonThrenodyScroll)), new(typeof(BardSheepfoeMamboScroll)),
			new(typeof(BardSinewyEtudeScroll))
		];

		public static readonly LootPackItem[] ClericScrollItems =
		[
			new(typeof(ClericAngelicFaithScroll)), new(typeof(ClericBanishEvilScroll)),
			new(typeof(ClericDampenSpiritScroll)), new(typeof(ClericDivineFocusScroll)),
			new(typeof(ClericPurgeScroll)), new(typeof(ClericHammerOfFaithScroll)),
			new(typeof(ClericRestorationScroll)), new(typeof(ClericSacredBoonScroll)),
			new(typeof(ClericSacrificeScroll)), new(typeof(ClericTouchOfLifeScroll)),
			new(typeof(ClericSmiteScroll)), new(typeof(ClericTrialByFireScroll))
		];

		public static readonly LootPackItem[] DruidScrollItems =
		[
			new(typeof(DruidBlendWithForestScroll)), new(typeof(DruidFamiliarScroll)),
			new(typeof(DruidEnchantedGroveScroll)), new(typeof(DruidGraspingRootsScroll)),
			new(typeof(DruidHollowReedScroll)), new(typeof(DruidLeafWhirlwindScroll)),
			new(typeof(DruidLureStoneScroll)), new(typeof(DruidMushroomGatewayScroll)),
			new(typeof(DruidNaturesPassageScroll)), new(typeof(DruidPackOfBeastScroll)),
			new(typeof(DruidRestorativeSoilScroll)),
			new(typeof(DruidShieldOfEarthScroll)), new(typeof(DruidSpringOfLifeScroll)),
			new(typeof(DruidStoneCircleScroll)), new(typeof(DruidSwarmOfInsectsScroll)),
			new(typeof(DruidVolcanicEruptionScroll))
		];

		public static readonly LootPackItem[] RangerScrollItems =
		[
			new(typeof(RangerFireBowScroll)), new(typeof(RangerHuntersAimScroll)),
			new(typeof(RangerIceBowScroll)), new(typeof(RangerLightningBowScroll)),
			new(typeof(RangerNoxBowScroll)), new(typeof(RangerPhoenixFlightScroll)),
			new(typeof(RangerFamiliarScroll)), new(typeof(RangerSummonMountScroll))
		];
		
		public static readonly LootPackItem[] RogueScrollItems =
		[
			new( typeof( RogueCharmScroll ), 1 ),
			new( typeof( RogueFalseCoinScroll ), 1 ),
			new( typeof( RogueIntimidationScroll ), 1 ),
			new( typeof( RogueShadowScroll ), 1 ),   
			new( typeof( RogueShieldOfEarthScroll ), 1 ),   
			new( typeof( RogueSlyFoxScroll ), 1 )
		];

		public static readonly LootPackItem[] UndeadScrollItems =
		[
			new(typeof(UndeadAngelicFaithScroll)), new(typeof(UndeadCauseFearScroll)),
			new(typeof(UndeadGraspingRootsScroll)),
			new(typeof(UndeadHammerOfFaithScroll)), new(typeof(UndeadHollowReedScroll)),
			new(typeof(UndeadLeafWhirlwindScroll)), new(typeof(UndeadLureStoneScroll)),
			new(typeof(UndeadMushroomGatewayScroll)),
			new(typeof(UndeadNaturesPassageScroll)), new(typeof(UndeadSeanceScroll)),
			new(typeof(UndeadSwarmOfInsectsScroll)),
			new(typeof(UndeadVolcanicEruptionScroll))
		];
		
		public static readonly LootPackItem[] DeathKnightItems =
		[
			new( typeof( BanishSkull ), 1 ),
			new( typeof( DemonicTouchSkull ), 1 ),
			new( typeof( DevilPactSkull ), 1 ),
			new( typeof( GrimReaperSkull ), 1 ),
			new( typeof( HagHandSkull ), 1 ),
			new( typeof( HellfireSkull ), 1 ),
			new( typeof( LucifersBoltSkull ), 1 ),
			new( typeof( OrbOfOrcusSkull ), 1 ),
			new( typeof( ShieldOfHateSkull ), 1 ),
			new( typeof( SoulReaperSkull ), 1 ),
			new( typeof( StrengthOfSteelSkull ), 1 ),
			new( typeof( StrikeSkull ), 1 ),
			new( typeof( SuccubusSkinSkull ), 1 ),
			new( typeof( WrathSkull ), 1 )

		];

		public static readonly LootPack AncientScrolls = new([
			new LootPackEntry(false, true, AncientScrollItems, 30.00, 1)
		]);

		public static readonly LootPack AvatarScrolls = new([
			new LootPackEntry(false, true, AvatarScrollItems, 30.00, 1)
		]);

		public static readonly LootPack BardScrolls = new([
			new LootPackEntry(false, true, BardScrollItems, 30.00, 1)
		]);

		public static readonly LootPack ClericScrolls = new([
			new LootPackEntry(false, true, ClericScrollItems, 30.00, 1)
		]);

		public static readonly LootPack DruidScrolls = new([
			new LootPackEntry(false, true, DruidScrollItems, 30.00, 1)
		]);

		public static readonly LootPack RangerScrolls = new([
			new LootPackEntry(false, true, RangerScrollItems, 30.00, 1)
		]);

		public static readonly LootPack RogueScrolls = new([
			new LootPackEntry(false, true, RogueScrollItems, 30.00, 1)
		]);

		public static readonly LootPack UndeadScrolls = new([
			new LootPackEntry(false, true, UndeadScrollItems, 30.00, 1)
		]);

		public static readonly LootPack DeathKnightScrolls = new([
			new( false, true,  DeathKnightItems, 30.00, 1 )
		]);
		
		public static readonly LootPack ArcanistScrolls = new([new LootPackEntry(false, true, LootPack.ArcanistScrollItems, 30.00, 1)]);
		public static readonly LootPack MysticScrolls =  new([new LootPackEntry(false, true, LootPack.MysticScrollItems, 30.00, 1)]);
		
		#endregion
		
		public static List<Type> _NelderimExclusiveLootTypes = [typeof(Arrow), typeof(Bolt)];
	}
}
