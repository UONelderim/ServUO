#region References

using System;
using System.Linq;
using Server.ACC.CSS.Systems.Ancient;
using Server.ACC.CSS.Systems.Avatar;
using Server.ACC.CSS.Systems.Bard;
using Server.ACC.CSS.Systems.Cleric;
using Server.ACC.CSS.Systems.Druid;
using Server.ACC.CSS.Systems.Ranger;
using Server.ACC.CSS.Systems.Undead;
using Server.ACC.CSS.Systems.Rogue;
using Server.Items;
using Server.Spells.DeathKnight;

#endregion

namespace Server
{
	public partial class LootPack
	{
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

        public static readonly LootPackItem[] RogueScrollItems = new LootPackItem[]
        {
            new LootPackItem( typeof( RogueCharmScroll ), 1 ),
            new LootPackItem( typeof( RogueFalseCoinScroll ), 1 ),
            new LootPackItem( typeof( RogueIntimidationScroll ), 1 ),
            new LootPackItem( typeof( RogueShadowScroll ), 1 ),   
            new LootPackItem( typeof( RogueShieldOfEarthScroll ), 1 ),   
        	new LootPackItem( typeof( RogueSlyFoxScroll ), 1 )
        };

        public static readonly LootPackItem[] DeathKnightItems = new LootPackItem[]
        {
            new LootPackItem( typeof( BanishSkull ), 1 ),
            new LootPackItem( typeof( DemonicTouchSkull ), 1 ),
            new LootPackItem( typeof( DevilPactSkull ), 1 ),
            new LootPackItem( typeof( GrimReaperSkull ), 1 ),
            new LootPackItem( typeof( HagHandSkull ), 1 ),
            new LootPackItem( typeof( HellfireSpell ), 1 ),
            new LootPackItem( typeof( LucifersBoltSkull ), 1 ),
            new LootPackItem( typeof( OrbOfOrcusSkull ), 1 ),
            new LootPackItem( typeof( ShieldOfHateSkull ), 1 ),
            new LootPackItem( typeof( SoulReaperSkull ), 1 ),
            new LootPackItem( typeof( StrengthOfSteelSkull ), 1 ),
            new LootPackItem( typeof( StrikeSkull ), 1 ),
            new LootPackItem( typeof( SuccubusSkinSkull ), 1 ),
            new LootPackItem( typeof( WrathSkull ), 1 )

        };
        
		public static readonly LootPackItem[] BardScrollItems =
		{
			new LootPackItem(typeof(BardArmysPaeonScroll), 1),
			new LootPackItem(typeof(BardEnchantingEtudeScroll), 1),
			new LootPackItem(typeof(BardEnergyCarolScroll), 1),
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


		public static readonly LootPack Empty = new LootPack(new LootPackEntry[0]);

		public static readonly LootPack AvatarScrolls = new LootPack(new[]
		{
			new LootPackEntry(false, true, AvatarScrollItems, 30.00, 1)
		});

		public static readonly LootPack DruidScrolls = new LootPack(new[]
		{
			new LootPackEntry(false, true, DruidScrollItems, 30.00, 1)
		});

		public static readonly LootPack UndeadScrolls = new LootPack(new[]
		{
			new LootPackEntry(false, true, UndeadScrollItems, 30.00, 1)
		});

		public static readonly LootPack RangerScrolls = new LootPack(new[]
		{
			new LootPackEntry(false, true, RangerScrollItems, 30.00, 1)
		});

		public static readonly LootPack ClericScrolls = new LootPack(new[]
		{
			new LootPackEntry(false, true, ClericScrollItems, 30.00, 1)
		});

		public static readonly LootPack BardScrolls = new LootPack(new[]
		{
			new LootPackEntry(false, true, BardScrollItems, 30.00, 1)
		});

		public static readonly LootPack AncientScrolls = new LootPack(new[]
		{
			new LootPackEntry(false, true, AncientScrollItems, 30.00, 1)
		});
		
		public static readonly LootPack DeathKnightScrolls = new LootPack( new LootPackEntry[]
		{
			new LootPackEntry( false, true,  DeathKnightItems, 30.00, 1 )
		} );
		
		
		private static Type[] _NelderimExclusiveLootTypes = { typeof(Arrow), typeof(Bolt) };

		public bool IsNelderimLootOnly => m_Entries.Length == 1 && m_Entries[0].Items.Length == 1 &&
		                                 _NelderimExclusiveLootTypes.Contains(m_Entries[0].Items[0].Type);
	}
}
