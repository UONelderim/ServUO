#region References

using Server.Mobiles;
using System;
using System.Collections.Generic;
using Server.Engines.ArenaSystem;
using Server.Engines.Quests;
using Server.Mobiles.Swiateczne;

#endregion

namespace Server
{
	public static class SpeedInfo
	{
		private const double DefaultSpeed = 1.0;
		
		private const double MinDelay = 0.1;
		private const double MaxDelay = 0.5;
		private const double MinDelayWild = 0.15;
		private const double MaxDelayWild = 0.8;

		public static bool GetSpeeds(BaseCreature bc, ref double activeSpeed, ref double passiveSpeed)
		{
			if (!_SpeedTable.TryGetValue(bc.GetType(), out activeSpeed))
			{
				var parentType = bc.GetType().BaseType;
				if (parentType == null || !_SpeedTable.TryGetValue(parentType, out activeSpeed))
				{
					activeSpeed = DefaultSpeed;
					Console.WriteLine($"No speed defined for {bc.GetType().Name} or its parent {parentType.Name}");
				}
			}
			passiveSpeed = activeSpeed * 2f;

			if (bc.IsMonster)
			{
				activeSpeed = Math.Clamp(activeSpeed * 1.3f, MinDelayWild, MaxDelayWild);
				passiveSpeed = Math.Clamp(passiveSpeed * 1.3f, MinDelayWild, MaxDelayWild);
			}

			return true;
		}

		public static double TransformMoveDelay(BaseCreature bc, double delay)
		{
			double max = bc.IsMonster ? MaxDelayWild : MaxDelay;

			if (!bc.IsDeadPet && (bc.ReduceSpeedWithDamage || bc.IsSubdued))
			{
				double offset = bc.Stam / (double)bc.StamMax;

				if (offset < 1.0)
				{
					delay += ((max - delay) * (1.0 - offset));
				}

				var hitsScalar = bc.Hits < (bc.HitsMax * 0.25f) ? 1.2 : 1.0;
				delay *= hitsScalar;
			}

			if (bc.IsMonster)
			{
				delay = Math.Clamp(delay, MinDelayWild, MaxDelayWild);
			}
			else
			{
				delay = Math.Clamp(delay, MinDelay, MaxDelay);
			}

			return delay;
		}

		private static Dictionary<Type, double> _SpeedTable = new();
		
		static SpeedInfo()
		{
			foreach (var speedInfoEntry in _SpeedDefinitions)
			{
				foreach (var type in speedInfoEntry.types)
				{
					//We crash on purpose if we have duplicates
					_SpeedTable.Add(type, speedInfoEntry.activeSpeed);
				}
			}
		}

		private record SpeedInfoEntry(double activeSpeed, Type[] types);

		private static SpeedInfoEntry[] _SpeedDefinitions =
		[
			/* Veeeeery Slooooow */
			new(0.55,
			[
				typeof(Aminia), typeof(Mummy2), typeof(Mummy3), typeof(Tangle), typeof(UnfrozenMummy),
				typeof(XmlQuestNPC)
			]),
			/* Very Slow */
			new(0.3,
			[
				typeof(Boar), typeof(BogThing), typeof(FrostOoze), typeof(FrostTroll), typeof(GazerLarva),
				typeof(Ghoul), typeof(HeadlessOne), typeof(Juggernaut), typeof(Jwilson), typeof(MoundOfMaggots),
				typeof(PlagueBeast), typeof(Quagmire), typeof(RestlessSoul), typeof(RottingCorpse),
				typeof(ShadowWisp), typeof(Skeleton), typeof(Slime), typeof(Walrus), typeof(Zombie)
			]),
			/* Slow */
			new(0.25,
			[
				typeof(AntLion), typeof(xCommonArcaneDaemon), typeof(ArcticOgreLord), typeof(BlackSolenWorker),
				typeof(BloodElemental), typeof(Bogle), typeof(BoneKnight), typeof(BoneMagi), typeof(Brigand),
				typeof(BullFrog), typeof(Cat), typeof(Chicken), typeof(Cow), typeof(CrystalElemental),
				typeof(Cyclops), typeof(DarknightCreeper), typeof(EarthElemental), typeof(ElderGazer),
				typeof(EnslavedGargoyle), typeof(Ettin), typeof(FleshGolem), typeof(Gazer), typeof(GiantSpider),
				typeof(Gibberling), typeof(Golem), typeof(GolemController), typeof(GreaterDragon),
				typeof(Feniks), typeof(LesserHordeDaemon), typeof(IceElemental), typeof(JukaLord),
				typeof(JukaMage), typeof(LichLord), typeof(Lizardman), typeof(Mongbat), typeof(Mummy),
				typeof(NDeloth), typeof(NDzahhar), typeof(NKatrill), typeof(NBurugh),
				typeof(NSkeletalDragon), typeof(NGorogon), typeof(NSilshashaszals),
				typeof(NStarozytnyLodowySmok), typeof(NStarozytnySmok), typeof(NSzeol), typeof(WladcaDemonow),
				typeof(Zhoaminth), typeof(Ogre), typeof(OgreLord), typeof(Orc), typeof(OrcBomber),
				typeof(OrcishMage), typeof(PatchworkSkeleton), typeof(Pig), typeof(Rat), typeof(Ratman),
				typeof(RatmanMage), typeof(RedSolenWorker), typeof(Serado), typeof(Sewerrat), typeof(Sheep),
				typeof(SkitteringHopper), typeof(StrongMongbat), typeof(SummonedDaemon),
				typeof(SummonedEarthElemental), typeof(SummonedFireElemental), typeof(SummonedWaterElemental),
				typeof(SwampTentacle), typeof(TerathanDrone), typeof(Titan), typeof(Treefellow), typeof(Troll),
				typeof(WhippingVine),
			]),
			/* Medium */
			new(0.2,
			[
				typeof(Abscess), typeof(Actor), typeof(AgapiteElemental), typeof(AlbinoSquirrel),
				typeof(Alligator), typeof(AmethystDragon), typeof(AmethystDrake), typeof(ArenaManager), typeof(DiamondDragon),
				typeof(DiamondDrake), typeof(EmeraldDragon), typeof(EmeraldDrake), typeof(RubyDragon),
				typeof(RubyDrake), typeof(SapphireDragon), typeof(SapphireDrake), typeof(AncientLich),
				typeof(AncientSeaMonster), typeof(Arachne), typeof(ArcaneFey), typeof(ArcaneFiend),
				typeof(Artist), typeof(BagiennaLama), typeof(BakeKitsune), typeof(BaseVendor), typeof(NBaseTalkingNPC),
				typeof(Betrayer), typeof(Bird), typeof(BlackBear),
				typeof(BlackSolenInfiltratorWarrior), typeof(BlackSolenWarrior), typeof(BladeSpirits),
				typeof(Bogling), typeof(BronzeElemental), typeof(BrownBear),
				typeof(BulbousPutrification), typeof(Bull), typeof(Centaur),
				typeof(xCommonChaosDaemon), typeof(ChiefParoxysmus), typeof(ChiefParoxysmusAvatar),
				typeof(Chiikkaha), typeof(CopperElemental), typeof(CorporealBrume), typeof(Corpser),
				typeof(CorrosiveSlime), typeof(Crane), typeof(CrystalDaemon),
				typeof(CrystalHydra), typeof(CrystalLatticeSeeker), typeof(CrystalSeaSerpent),
				typeof(CrystalVortex), typeof(CrystalWisp), typeof(Cursed), typeof(CuSidhe),
				typeof(CommonDaemon), typeof(DarkWisp), typeof(DarkWolfFamiliar), typeof(DeathAdder),
				typeof(DeathSpiderFamiliar), typeof(DeathwatchBeetle), typeof(DeathwatchBeetleHatchling),
				typeof(DeepSeaSerpent), typeof(DemonicznySluga), typeof(DesertOstard), typeof(Devourer),
				typeof(Dog), typeof(Dolphin), typeof(Doppleganger), typeof(Dragon),
				typeof(DragonsFlameGrandMage), typeof(DragonsFlameMage), typeof(Drake), typeof(DreadHorn),
				typeof(DullCopperElemental), typeof(ElfBrigand), typeof(EnslavedSatyr), typeof(EscortableMage),
				typeof(EttinLord), typeof(EvilHealer), typeof(EvilMage), typeof(EvilMageLord),
				typeof(EvilWanderingHealer), typeof(Executioner), typeof(ExodusOverseer),
				typeof(Ferret), typeof(FetidEssence), typeof(FieryGoblinSapper),
				typeof(FireBeetle), typeof(FireGargoyle), typeof(Flurry), typeof(FrostSpider), typeof(Gaman),
				typeof(Gargoyle), typeof(GargoyleEnforcer), typeof(GiantIceWorm), typeof(GiantRat),
				typeof(GiantSerpent), typeof(GiantToad), typeof(Gnaw), typeof(Goat), typeof(GorskiWilk),
				typeof(Goblin), typeof(GoblinSapper), typeof(GoblinWarrior), typeof(GoldenElemental),
				typeof(GoreFiend), typeof(Gorilla), typeof(Gregorio), typeof(Grim), typeof(GrimmochDrummel),
				typeof(Grobu), typeof(Guardian), typeof(Gypsy), typeof(HarborMaster), typeof(Harpy),
				typeof(Harrower), typeof(HarrowerTentacles), typeof(HellHorse), typeof(HellHound), typeof(Hind),
				typeof(CommonHordeDaemon), typeof(HordeMinionFamiliar),
				typeof(Hydra), typeof(IceFiend), typeof(IceSerpent), typeof(Imp),
				typeof(Impaler), typeof(InsaneDryad), typeof(InterredGrizzle), typeof(JackRabbit),
				typeof(JukaWarrior), typeof(Kappa), typeof(KamiennaWyverna), typeof(KazeKemono),
				typeof(KhaldunSummoner), typeof(KhaldunZealot), typeof(Kraken), typeof(KusznikMorrlok),
				typeof(LadyLissith), typeof(LadyMelisande), typeof(LadySabrix), typeof(LavaLizard),
				typeof(LavaSerpent), typeof(LesserArcaneDaemon), typeof(LesserChaosDaemon),
				typeof(LesserDaemon), typeof(LesserGoblinSapper), typeof(LesserMoloch), typeof(Llama),
				typeof(LodowySmok), typeof(LordMorrlok), typeof(LucznikMorrlok), typeof(Lurg),
				typeof(LysanderGathenwale), typeof(MagMorrlok), typeof(MantraEffervescence),
				typeof(MasterJonath), typeof(MasterTheophilus), typeof(MeerCaptain), typeof(MeerEternal),
				typeof(MeerMage), typeof(MeerWarrior), typeof(Mistral),
				typeof(MLDryad), typeof(MlodyLodowySmok), typeof(MlodyOgnistySmok), typeof(MlodyGorskiSmok),
				typeof(MlodaKobra), typeof(xMoloch), typeof(MonstrousInterredGrizzle), typeof(MordercaMorrlok),
				typeof(MorgBergen), typeof(MorrlokWarHorse), typeof(Motyl), typeof(MougGuur),
				typeof(MountainGoat), typeof(MrocznaCzelusc), typeof(NatureFury), typeof(NChimera),
				typeof(NecroMageLord), typeof(NelderimAncientLich), typeof(GreaterArcaneDaemon),
				typeof(GreaterChaosDaemon), typeof(GreaterDaemon), typeof(NelderimDragon),
				typeof(GreaterHordeDaemon), typeof(GreaterMoloch), typeof(Ninja), typeof(Minotaur),
				typeof(MinotaurBoss), typeof(MinotaurCaptain), typeof(MinotaurLord), typeof(MinotaurMage),
				typeof(MinotaurScout), typeof(CommonMoloch), typeof(NPrzeklety), typeof(NSarag),
				typeof(NZapomniany), typeof(OgnistaWyverna), typeof(OgnistyNiewolnik), typeof(OgnistySmok),
				typeof(OgromnyWilk), typeof(OgnistySzaman), typeof(OgnistyWojownik), typeof(OrcBrute),
				typeof(OrcCaptain), typeof(OrcishLord), typeof(OrcScout), typeof(PackLlama), typeof(PanSmierc),
				typeof(ParoxysmusSwampDragon), typeof(PestilentBandage),
				typeof(PetParrot), typeof(NPirateCaptain), typeof(NPirateCrew), typeof(PomiotPajaka),
				typeof(PrastaryLodowySmok), typeof(PrastaryOgnistySmok), typeof(PRidgeback), typeof(Protector),
				typeof(PSavage), typeof(PSavage1), typeof(PSavageShaman), typeof(Putrefier), typeof(Rabbit),
				typeof(RagingGrizzlyBear), typeof(Reaper), typeof(RedBeetle), typeof(RedDeath),
				typeof(RedSolenInfiltratorWarrior), typeof(RedSolenWarrior), typeof(Rend), typeof(Reptalon),
				typeof(RidableLlama), typeof(Ridgeback), typeof(Saliva), typeof(Samurai), typeof(Satyr),
				typeof(Savage), typeof(SavageRidgeback), typeof(ScaledSwampDragon), typeof(Scorpion),
				typeof(Sculptor), typeof(SeaHorse), typeof(SeaSerpent),
				typeof(SerpentsFangAssassin), typeof(SerpentsFangHighExecutioner), typeof(ServantOfSemidar),
				typeof(ShadowFiend), typeof(ShadowIronElemental), typeof(ShadowWispFamiliar),
				typeof(ShadowWyrm), typeof(ShimmeringEffusion), typeof(ShimmeringFerret), typeof(Silk),
				typeof(SkeletalDragon), typeof(SkeletalMount), typeof(SkalnyJaszczur), typeof(SkalnyKocur),
				typeof(SkalnyLew), typeof(SkalnyOgar), typeof(SkorpionKrolewski), typeof(Snake),
				typeof(SnieznaLama), typeof(SnowBakeKitsune), typeof(SzafirowyJednorozec),
				typeof(SpawnedOrcishLord), typeof(SpeckledScorpion), typeof(Sphynx), typeof(Squirrel),
				typeof(StaryLodowySmok), typeof(StaryOgnistySmok), typeof(StoneGargoyle), typeof(StoneHarpy),
				typeof(SwampDragon), typeof(Swoop), typeof(Szavetra), typeof(TalkingBaseEscortable),
				typeof(TalkingDrake), typeof(TalkingSeekerOfAdventure), typeof(TavaraSewel), typeof(Tempest),
				typeof(TerathanMatriarch), typeof(TerathanWarrior), typeof(TigersClawMaster),
				typeof(TigersClawThief), typeof(TormentedMinotaur), typeof(Travesty), typeof(TravestyDog),
				typeof(Troglodyte), typeof(TrollLord), typeof(TropicalBird),
				typeof(UpadlyJednorozec), typeof(UpadlyKirin), typeof(ValoriteElemental), typeof(VampireBat),
				typeof(VampireBatFamiliar), typeof(VeriteElemental), typeof(Virulent), typeof(VitVarg),
				typeof(VitVargArcher), typeof(VitVargLord), typeof(VitVargMage), typeof(VitVargWarrior),
				typeof(WailingBanshee), typeof(WandererOfTheVoid), typeof(WanderingHealer), typeof(WarHorse),
				typeof(WarLlama), typeof(Widmak),
				typeof(WaterElemental), typeof(WojownikMorrlok), typeof(WolfHunter), typeof(WolfShaman),
				typeof(WolfWarrior), typeof(Wraith), typeof(Wyvern), typeof(YomotsuElder),
				typeof(YomotsuPriest), typeof(YomotsuWarrior), typeof(ZimowyOgre), typeof(ZimowyOgreLord),
				typeof(DullCopperColossus), typeof(ShadowIronColossus), typeof(CopperColossus), typeof(BronzeColossus), 
				typeof(GoldenColossus), typeof(AgapiteColossus), typeof(VeriteColossus), typeof(ValoriteColossus)
			]),
			/* Fast */
			new(0.175,
			[
				typeof(AbysmalHorror), typeof(AirElemental), typeof(Anchimayen), typeof(AncientWyrm), typeof(Balron),
				typeof(BaronowaFrozen), typeof(BlackSolenInfiltratorQueen), typeof(BlackSolenQueen),
				typeof(BoneDemon), typeof(Cougar), typeof(DemonKnight), typeof(DireWolf),
				typeof(DreadSpider), typeof(Eagle), typeof(EnergyVortex), typeof(ExodusMinion),
				typeof(ExodusMinionArmored), typeof(ExodusBoss), typeof(FireElemental), typeof(FireSteed),
				typeof(ForestOstard), typeof(FrenziedOstard), typeof(GargoyleDestroyer),
				typeof(GiantBlackWidow), typeof(GreatHart), typeof(GreyWolf), typeof(GrizzlyBear),
				typeof(HellCat), typeof(HellSteed), typeof(Horse), typeof(JaskiniowyJaszczur), typeof(IceSnake),
				typeof(Kirin), typeof(LadyOfTheSnow), typeof(LavaSnake), typeof(LesserHiryu), typeof(Worg),
				typeof(Lich), typeof(LordOaks), typeof(Oni), typeof(OphidianKnight),
				typeof(OphidianMage), typeof(OphidianWarrior), typeof(PackHorse), typeof(JaskiniowyZukJuczny),
				typeof(PlagueSpawn), typeof(PolarBear), typeof(RatmanArcher), typeof(Ravager),
				typeof(RedSolenInfiltratorQueen), typeof(RedSolenQueen), typeof(RevenantLion), typeof(Ronin),
				typeof(RuneBeetle), typeof(SavageRider), typeof(SavageShaman), typeof(SerpentineDragon),
				typeof(ShadowKnight), typeof(Silvani), typeof(SilverSteed),
				typeof(SnowElemental), typeof(SnowLeopard), typeof(SnieznyKirin), typeof(SnieznyRumak),
				typeof(SpectralArmour), typeof(StormElemental), typeof(Succubus), typeof(SummonedAirElemental),
				typeof(TerathanAvenger), typeof(TimberWolf), typeof(ToxicElemental),
				typeof(TsukiWolf), typeof(Unicorn), typeof(WhiteWolf), typeof(WhiteWyrm), typeof(Yamandon),
				typeof(HireBard), typeof(HireBardArcher), typeof(HireBeggar), typeof(HireFighter),
				typeof(HireMage), typeof(HirePaladin), typeof(HirePeasant), typeof(HireRanger),
				typeof(HireRangerArcher), typeof(HireSailor), typeof(HireThief), typeof(ArcherNelderimGuard), typeof(MageNelderimGuard)
			]),
			/* Very Fast */
			new(0.15,
			[
				typeof(AntyMageMob), typeof(AntyTamerMob), typeof(AntyWarriorMob),
				typeof(BagiennyKoszmar), typeof(Barracoon), typeof(Beetle), typeof(ChaosDragoon),
				typeof(ChaosDragoonElite), typeof(Efreet), typeof(EliteNinja), typeof(EtherealWarrior),
				typeof(FanDancer), typeof(FleshRenderer), typeof(Hiryu),
				typeof(Leviathan), typeof(Mephitis), typeof(Neira), typeof(Nightmare), typeof(OphidianArchmage),
				typeof(OphidianMatriarch), typeof(Panther), typeof(PoisonElemental), typeof(PredatorHellCat),
				typeof(PSavageRider), typeof(RaiJu), typeof(Rikktor), typeof(SandVortex), typeof(Semidar),
				typeof(SilverSerpent), typeof(SkoczekZPodmroku), typeof(VitVargAmazon),
				typeof(VitVargBerserker), typeof(VitVargCook), typeof(VitVargCutler), typeof(Wisp),
				typeof(StandardNelderimGuard), typeof(HeavyNelderimGuard), 
				typeof(EliteNelderimGuard)
			]),
			/* Ultra Fast */
			new(0.1,
			[
				typeof(Changeling), typeof(Coil), typeof(CorruptedSoul), typeof(Guile), typeof(Ilhenir),
				typeof(Irk), typeof(JezdziecMorrlok), typeof(KhaldunRevenant), typeof(LadyJennifyr),
				typeof(LadyMarai), typeof(Malefic), typeof(MasterMikael), typeof(Meraktus), typeof(Miasma),
				typeof(NDuchNatury), typeof(NPoleAntymagiczne), typeof(NUpiorWalki), typeof(Phoenix),
				typeof(Pixie), typeof(Pyre), typeof(Revenant), typeof(SirPatrick), typeof(Spite),
				typeof(Thrasher), typeof(Twaulo), typeof(VorpalBunny), typeof(MountedNelderimGuard)
			]),
			/* Fast and Furious */
			new(0.05,
			[
				typeof(SpecialNelderimGuard)
			])
		];
	}
}
