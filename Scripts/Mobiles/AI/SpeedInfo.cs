#region References

using Server.Mobiles;
using System;
using System.Collections.Generic;
using Server.Engines.HunterKiller;
using Server.Engines.Quests.RitualQuest;
using Server.Items;
using Server.Mobiles.Swiateczne;

#endregion

namespace Server
{
	public static class SpeedInfo
	{
		private const double DefaultSpeed = 0.2;

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
				}
			}

			passiveSpeed = activeSpeed * 2f;

			if (bc.IsMonster)
			{
				activeSpeed = Math.Clamp(activeSpeed * 1.3f, MinDelayWild, MaxDelayWild);
				passiveSpeed = Math.Clamp(passiveSpeed * 1.3f, MinDelayWild, MaxDelayWild);
			}

			if (bc is BaseNelderimGuard)
			{
				// Guards have to be faster when in fight
				activeSpeed = Math.Min(0.15, activeSpeed);
			}

			return true;
		}

		public static double TransformMoveDelay(BaseCreature bc, double delay)
		{
			var max = bc.IsMonster ? MaxDelayWild : MaxDelay;

			if (!bc.IsDeadPet && (bc.ReduceSpeedWithDamage || bc.IsSubdued))
			{
				var offset = bc.Stam / (double)bc.StamMax;

				if (offset < 1.0) delay += (max - delay) * (1.0 - offset);

				var hitsScalar = bc.Hits < bc.HitsMax * 0.25f ? 1.2 : 1.0;
				delay *= hitsScalar;
			}

			if (bc.IsMonster)
				delay = Math.Clamp(delay, MinDelayWild, MaxDelayWild);
			else
				delay = Math.Clamp(delay, MinDelay, MaxDelay);

			return delay;
		}

		private static Dictionary<Type, double> _SpeedTable = new();

		static SpeedInfo()
		{
			foreach (var speedInfoEntry in _SpeedDefinitions)
			{
				foreach (var type in speedInfoEntry.types)
					//We crash on purpose if we have duplicates
					_SpeedTable.Add(type, speedInfoEntry.activeSpeed);
			}
		}

		private record SpeedInfoEntry(double activeSpeed, Type[] types);

		private static SpeedInfoEntry[] _SpeedDefinitions =
		[
			/* Veeeeery Slooooow */
			new(0.55,
			[
				typeof(Aminia),
				typeof(BexilPunchingBag),
				typeof(MonsterNestEntity),
				typeof(Mummy2),
				typeof(Mummy3),
				typeof(Tangle),
				typeof(UnfrozenMummy),
				typeof(XmlQuestNPC)
			]),
			/* Very Slow */
			new(0.3,
			[
				typeof(BarakSlime),
				typeof(Boar),
				typeof(BogThing),
				typeof(FrostOoze),
				typeof(FrostTroll),
				typeof(GazerLarva),
				typeof(Ghoul),
				typeof(HeadlessOne),
				typeof(Juggernaut),
				typeof(Jwilson),
				typeof(MoundOfMaggots),
				typeof(PlagueBeast),
				typeof(Quagmire),
				typeof(RestlessSoul),
				typeof(RottingCorpse),
				typeof(ShadowWisp),
				typeof(Skeleton),
				typeof(Slime),
				typeof(Walrus),
				typeof(Zombie)
			]),
			/* Slow */
			new(0.25,
			[
				typeof(AntLion),
				typeof(ArcticOgreLord),
				typeof(BlackSolenWorker),
				typeof(BloodElemental),
				typeof(Bogle),
				typeof(BoneKnight),
				typeof(BoneMagi),
				typeof(Brigand),
				typeof(BullFrog),
				typeof(Cat),
				typeof(Chicken),
				typeof(Cow),
				typeof(CrystalElemental),
				typeof(Cyclops),
				typeof(DalharukElghinn),
				typeof(DarknightCreeper),
				typeof(EarthElemental),
				typeof(ElderGazer),
				typeof(EnslavedGargoyle),
				typeof(Ettin),
				typeof(Feniks),
				typeof(FleshGolem),
				typeof(Gazer),
				typeof(GiantSpider),
				typeof(Gibberling),
				typeof(Golem),
				typeof(GolemController),
				typeof(GreaterDragon),
				typeof(IceElemental),
				typeof(JukaLord),
				typeof(JukaMage),
				typeof(LesserHordeDaemon),
				typeof(LichLord),
				typeof(Lizardman),
				typeof(Mongbat),
				typeof(Mummy),
				typeof(NBurugh),
				typeof(NDeloth),
				typeof(NDzahhar),
				typeof(NGorogon),
				typeof(NKatrill),
				typeof(NSilshashaszals),
				typeof(NSkeletalDragon),
				typeof(NStarozytnyLodowySmok),
				typeof(NStarozytnySmok),
				typeof(NSzeol),
				typeof(Ogre),
				typeof(OgreLord),
				typeof(Orc),
				typeof(OrcBomber),
				typeof(OrcishMage),
				typeof(PatchworkSkeleton),
				typeof(Pig),
				typeof(Rat),
				typeof(Ratman),
				typeof(RatmanMage),
				typeof(RedSolenWorker),
				typeof(Serado),
				typeof(Sewerrat),
				typeof(Sheep),
				typeof(SkitteringHopper),
				typeof(StrongMongbat),
				typeof(SummonedDaemon),
				typeof(SummonedEarthElemental),
				typeof(SummonedFireElemental),
				typeof(SummonedWaterElemental),
				typeof(SwampTentacle),
				typeof(TerathanDrone),
				typeof(Titan),
				typeof(Treefellow),
				typeof(Troll),
				typeof(WhippingVine),
				typeof(WladcaDemonow),
				typeof(xCommonArcaneDaemon),
				typeof(Zhoaminth)
			]),
			/* Fast */
			new(0.175,
			[
				typeof(AbysmalHorror),
				typeof(AirElemental),
				typeof(Anchimayen),
				typeof(AncientWyrm),
				typeof(BagusGagakNinja),
				typeof(Balron),
				typeof(BaronowaFrozen),
				typeof(BlackSolenInfiltratorQueen),
				typeof(BlackSolenQueen),
				typeof(BoneDemon),
				typeof(Cougar),
				typeof(DemonKnight),
				typeof(DireWolf),
				typeof(DreadSpider),
				typeof(Eagle),
				typeof(EnergyVortex),
				typeof(ExodusBoss),
				typeof(ExodusMinion),
				typeof(ExodusMinionArmored),
				typeof(FireElemental),
				typeof(FireSteed),
				typeof(ForestOstard),
				typeof(FrenziedOstard),
				typeof(GargoyleDestroyer),
				typeof(GiantBlackWidow),
				typeof(GreatHart),
				typeof(GreyWolf),
				typeof(GrizzlyBear),
				typeof(HellCat),
				typeof(HellSteed),
				typeof(HireBard),
				typeof(HireBardArcher),
				typeof(HireBeggar),
				typeof(HireFighter),
				typeof(HireMage),
				typeof(HirePaladin),
				typeof(HirePeasant),
				typeof(HireRanger),
				typeof(HireRangerArcher),
				typeof(HireSailor),
				typeof(HireThief),
				typeof(HKMobile),
				typeof(Horse),
				typeof(IceSnake),
				typeof(JaskiniowyJaszczur),
				typeof(JaskiniowyZukJuczny),
				typeof(Kirin),
				typeof(KorahaTilkiDancer),
				typeof(LadyOfTheSnow),
				typeof(LavaSnake),
				typeof(LesserHiryu),
				typeof(Lich),
				typeof(LordOaks),
				typeof(MadPumpkinSpirit),
				typeof(Oni),
				typeof(OphidianKnight),
				typeof(OphidianMage),
				typeof(OphidianWarrior),
				typeof(PackHorse),
				typeof(PlagueSpawn),
				typeof(PolarBear),
				typeof(RatmanArcher),
				typeof(Ravager),
				typeof(RedSolenInfiltratorQueen),
				typeof(RedSolenQueen),
				typeof(RevenantLion),
				typeof(Ronin),
				typeof(RuneBeetle),
				typeof(SavageRider),
				typeof(SavageShaman),
				typeof(SerpentineDragon),
				typeof(ShadowKnight),
				typeof(Silvani),
				typeof(SilverSteed),
				typeof(SnieznyKirin),
				typeof(SnieznyRumak),
				typeof(SnowElemental),
				typeof(SnowLeopard),
				typeof(SpectralArmour),
				typeof(StormElemental),
				typeof(Succubus),
				typeof(SummonedAirElemental),
				typeof(TerathanAvenger),
				typeof(TimberWolf),
				typeof(ToxicElemental),
				typeof(TsukiWolf),
				typeof(Unicorn),
				typeof(WhiteWolf),
				typeof(WhiteWyrm),
				typeof(Worg),
				typeof(Yamandon)
			]),
			/* Very Fast */
			new(0.15,
			[
				typeof(AntyMageMob),
				typeof(AntyTamerMob),
				typeof(AntyWarriorMob),
				typeof(BagiennyKoszmar),
				typeof(BagusGagakLightCav),
				typeof(Barracoon),
				typeof(Beetle),
				typeof(ChaosDragoon),
				typeof(ChaosDragoonElite),
				typeof(Efreet),
				typeof(EliteNinja),
				typeof(EtherealWarrior),
				typeof(EvilSpellbook),
				typeof(FanDancer),
				typeof(FleshRenderer),
				typeof(Hiryu),
				typeof(KorahaTilkiPikador),
				typeof(Leviathan),
				typeof(Mephitis),
				typeof(MotherWolf),
				typeof(Neira),
				typeof(Nightmare),
				typeof(OphidianArchmage),
				typeof(OphidianMatriarch),
				typeof(Panther),
				typeof(PoisonElemental),
				typeof(PredatorHellCat),
				typeof(PSavageRider),
				typeof(RaiJu),
				typeof(Rikktor),
				typeof(SandVortex),
				typeof(Semidar),
				typeof(SilverSerpent),
				typeof(SkoczekZPodmroku),
				typeof(VitVargAmazon),
				typeof(VitVargBerserker),
				typeof(VitVargCook),
				typeof(VitVargCutler),
				typeof(Wisp)
			]),
			/* Ultra Fast */
			new(0.1,
			[
				typeof(Changeling),
				typeof(Coil),
				typeof(CorruptedSoul),
				typeof(Guile),
				typeof(Ilhenir),
				typeof(Irk),
				typeof(JezdziecMorrlok),
				typeof(KhaldunRevenant),
				typeof(LadyJennifyr),
				typeof(LadyMarai),
				typeof(Malefic),
				typeof(MasterMikael),
				typeof(Meraktus),
				typeof(Miasma),
				typeof(MountedNelderimGuard),
				typeof(NDuchNatury),
				typeof(NPoleAntymagiczne),
				typeof(NUpiorWalki),
				typeof(Phoenix),
				typeof(Pixie),
				typeof(Pyre),
				typeof(Revenant),
				typeof(SirPatrick),
				typeof(SpecialNelderimGuard),
				typeof(Spite),
				typeof(Thrasher),
				typeof(Twaulo),
				typeof(VorpalBunny)
			]),
		];
	}
}
