#region References

using System;
using System.Collections.Generic;
using Server.Engines.BulkOrders;
using Server.Mobiles;
using Server.Mobiles.Swiateczne;

#endregion

namespace Server.Items
{
	public enum ArtGroup
	{
		None,
		Doom,
		Boss,
		Miniboss,
		Fishing,
		Cartography,
		Hunter,
		CustomChamp,
		Elghin,
		Stealing,
	}

	class ArtifactMonster
	{
		public class ArtInfo
		{
			public double PercentChance { get; }

			public ArtGroup Group { get; }

			public ArtInfo(double percent, ArtGroup gr)
			{
				PercentChance = percent;
				Group = gr;
			}
		}

		private static readonly Dictionary<Type, ArtInfo> m_CreatureInfo = new Dictionary<Type, ArtInfo>();

		static ArtifactMonster()
		{
			// Wzor:
			// m_CreatureInfo.Add(typeof(<KlasaPotwora>), new ArtInfo(<ProcentSzansy>, ArtGroup.<GrupaArtefaktow>));

			//Doom
			m_CreatureInfo.Add(typeof(DemonKnight), new ArtInfo(4, ArtGroup.Doom));
			m_CreatureInfo.Add(typeof(AbysmalHorror), new ArtInfo(1.5, ArtGroup.Doom));
			m_CreatureInfo.Add(typeof(DarknightCreeper), new ArtInfo(1.5, ArtGroup.Doom));
			m_CreatureInfo.Add(typeof(FleshRenderer), new ArtInfo(1.5, ArtGroup.Doom));
			m_CreatureInfo.Add(typeof(Impaler), new ArtInfo(1.5, ArtGroup.Doom));
			m_CreatureInfo.Add(typeof(ShadowKnight), new ArtInfo(1.5, ArtGroup.Doom));

			//Elghin
			m_CreatureInfo.Add(typeof(DalharukElghinn), new ArtInfo(10, ArtGroup.Elghin));
			m_CreatureInfo.Add(typeof(KevinBoss), new ArtInfo(5, ArtGroup.Elghin));


			//Bossy
			m_CreatureInfo.Add(typeof(NGorogon), new ArtInfo(5, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(Sfinks), new ArtInfo(5, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(NSzeol), new ArtInfo(5, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(NBurugh), new ArtInfo(6, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(NKatrill), new ArtInfo(5, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(NDeloth), new ArtInfo(5, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(NDzahhar), new ArtInfo(5, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(NSarag), new ArtInfo(6, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(NelderimSkeletalDragon), new ArtInfo(7, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(NStarozytnyLodowySmok), new ArtInfo(8, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(StarozytnyDiamentowySmok), new ArtInfo(8, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(NStarozytnySmok), new ArtInfo(8, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(WladcaDemonow), new ArtInfo(10, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(MinotaurBoss), new ArtInfo(5, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(DreadHorn), new ArtInfo(5, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(LadyMelisande), new ArtInfo(7, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(Travesty), new ArtInfo(6, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(ChiefParoxysmus), new ArtInfo(10, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(Harrower), new ArtInfo(100, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(AncientRuneBeetle), new ArtInfo(7, ArtGroup.Boss));
			m_CreatureInfo.Add(typeof(Serado), new ArtInfo(40, ArtGroup.Boss));

			//Mini Bossy
			m_CreatureInfo.Add(typeof(WladcaJezioraLawy), new ArtInfo(7, ArtGroup.Miniboss));
			m_CreatureInfo.Add(typeof(BagusGagakCreeper), new ArtInfo(7, ArtGroup.Miniboss));
			m_CreatureInfo.Add(typeof(VitVarg), new ArtInfo(7, ArtGroup.Miniboss));
			m_CreatureInfo.Add(typeof(TilkiBug), new ArtInfo(7, ArtGroup.Miniboss));
			m_CreatureInfo.Add(typeof(NelderimDragon), new ArtInfo(3, ArtGroup.Miniboss));
			m_CreatureInfo.Add(typeof(ShimmeringEffusion), new ArtInfo(9, ArtGroup.Miniboss));
			m_CreatureInfo.Add(typeof(MonstrousInterredGrizzle), new ArtInfo(9, ArtGroup.Miniboss));
			m_CreatureInfo.Add(typeof(NSilshashaszals), new ArtInfo(5, ArtGroup.Miniboss));
			m_CreatureInfo.Add(typeof(SaragAwatar), new ArtInfo(2, ArtGroup.Miniboss));
			m_CreatureInfo.Add(typeof(WladcaPiaskow), new ArtInfo(4, ArtGroup.Miniboss));
			// m_CreatureInfo.Add(typeof(IceDragon), new ArtInfo(5, ArtGroup.Miniboss));

			//Custom champy
			m_CreatureInfo.Add(typeof(KapitanIIILegionuOrkow), new ArtInfo(30, ArtGroup.CustomChamp));
			m_CreatureInfo.Add(typeof(MorenaAwatar), new ArtInfo(30, ArtGroup.CustomChamp));
			m_CreatureInfo.Add(typeof(Meraktus), new ArtInfo(20, ArtGroup.CustomChamp));
			m_CreatureInfo.Add(typeof(Ilhenir), new ArtInfo(20, ArtGroup.CustomChamp));
			m_CreatureInfo.Add(typeof(Twaulo), new ArtInfo(15, ArtGroup.CustomChamp));
			m_CreatureInfo.Add(typeof(Pyre), new ArtInfo(30, ArtGroup.CustomChamp));
			m_CreatureInfo.Add(typeof(MikolajBoss), new ArtInfo(5, ArtGroup.CustomChamp));

			//Fishing Bossy
			m_CreatureInfo.Add(typeof(Leviathan), new ArtInfo(10, ArtGroup.Fishing));
		}

		public static double GetChanceFor(BaseCreature creature)
		{
			Type creatureType = creature.GetType();

			if (m_CreatureInfo.ContainsKey(creatureType))
			{
				return m_CreatureInfo[creatureType].PercentChance / 100.0;
			}

			return 0;
		}

		public static ArtGroup GetGroupFor(BaseCreature creature)
		{
			Type creatureType = creature.GetType();

			if (m_CreatureInfo.ContainsKey(creatureType))
				return m_CreatureInfo[creatureType].Group;
			return ArtGroup.None;
		}
	}

	class ArtifactHelper
	{
		#region Lista_artefaktow_Doom

		#endregion

		#region Lista_artefaktow_Elghin

		#endregion

		#region Lista_artefaktow_Boss

		#endregion

		#region Lista_artefaktow_Mini_Boss

		#endregion

		#region Lista_artefaktow_Custom_Champ

		private static readonly Type[] m_CustomChampArtifacts =
		{
			typeof(PrzekletaMaskaSmierci), typeof(PrzekletaStudniaOdnowy), typeof(PrzekleteOrleSkrzydla),
			typeof(PrzekletePogrobowce), typeof(PrzekleteWidlyMroku), typeof(PrzekletyKilofZRuinTwierdzy),
			typeof(PrzekletyMieczeAmrIbnLuhajj), typeof(PrzekletySoulSeeker), typeof(PrzekleteSongWovenMantle),
			typeof(PrzekletaMaskaSmierci), typeof(PrzekletaArcaneShield), typeof(PrzekletaIolosLute),
			typeof(PrzekletaRetorta), typeof(PrzekletaSamaritanRobe), typeof(PrzekletaVioletCourage),
			typeof(PrzekleteFeyLeggings), typeof(PrzekleteGauntletsOfNobility), typeof(PrzekletyArcticDeathDealer),
			typeof(PrzekletyFleshRipper), typeof(PrzekletyQuell),
		};

		#endregion

		#region Lista_artefaktow_Fishing

		#endregion

		#region Lista_artefaktow_Kartografia

		private static readonly Type[] m_HunterArtifacts =
		{
			typeof(Raikiri), //Type1
			typeof(PeasantsBokuto), typeof(PixieSwatter), typeof(Frostbringer), typeof(SzyjaGeriadoru),
			typeof(BlazenskieSzczescie), typeof(KulawyMagik), typeof(KilofZRuinTwierdzy),
			typeof(SkalpelDoktoraBrandona), typeof(JaszczurzySzal), typeof(OblivionsNeedle), typeof(Bonesmasher),
			typeof(ColdForgedBlade), typeof(DaimyosHelm), typeof(LegsOfStability), typeof(AegisOfGrace),
			typeof(AncientFarmersKasa), typeof(StudniaOdnowy), typeof(Tyrfing), //Type 2
			typeof(Arteria), typeof(ArcticDeathDealer), typeof(CavortingClub), typeof(Quernbiter),
			typeof(PromienSlonca), typeof(SwordsOfProsperity), typeof(TeczowaNarzuta), typeof(SmoczeKosci),
			typeof(RekawiceFredericka), typeof(OdbijajacyStrzaly), typeof(HuntersHeaddress), typeof(BurglarsBandana),
			typeof(SpodnieOswiecenia), typeof(KiltZycia), typeof(ArkanaZywiolow), typeof(OstrzeCienia),
			typeof(TalonBite), typeof(SilvanisFeywoodBow), typeof(BrambleCoat), typeof(OrcChieftainHelm),
			typeof(ShroudOfDeciet), typeof(CaptainJohnsHat), typeof(EssenceOfBattle), typeof(HebanowyPlomien), //Type 3
			typeof(PomstaGrima), typeof(MaskaSmierci), typeof(SmoczyNos), typeof(StudniaOdnowy), typeof(Aegis),
			typeof(HanzosBow), typeof(MagicznySaif), typeof(StrzalaAbarisa), typeof(FangOfRactus),
			typeof(RighteousAnger), typeof(Stormgrip), typeof(LeggingsOfEmbers), typeof(SmoczeJelita),
			typeof(SongWovenMantle), typeof(StitchersMittens), typeof(FeyLeggings), typeof(DjinnisRing),
			typeof(PendantOfTheMagi)
		};

		#endregion

		public static void Configure()
		{
			List<Type> allArtifacts = new List<Type>();
			allArtifacts.AddRange(BossArtifacts);
			allArtifacts.AddRange(MinibossArtifacts);
			allArtifacts.AddRange(ParagonArtifacts);
			allArtifacts.AddRange(DoomArtifacts);
			allArtifacts.AddRange(m_HunterArtifacts);
			allArtifacts.AddRange(CartographyArtifacts);
			allArtifacts.AddRange(FishingBossArtifacts);
			AllArtifacts = allArtifacts.ToArray();
		}

		public static Type[] AllArtifacts { get; private set; }

		public static Type[] DoomArtifacts { get; } =
		{
			typeof(Aegis), typeof(HolySword), typeof(CrownOfTalKeesh), typeof(ShadowDancerLeggings),
			typeof(SpiritOfTheTotem), typeof(HatOfTheMagi), typeof(LeggingsOfBane), typeof(TheTaskmaster),
			typeof(JackalsCollar), typeof(ArcaneShield), typeof(ArmorOfFortune), typeof(TheBeserkersMaul),
			typeof(BladeOfInsanity), typeof(BoneCrusher), typeof(BreathOfTheDead), typeof(AxeOfTheHeavens),
			typeof(BraceletOfHealth), typeof(DivineCountenance), typeof(TheDragonSlayer), typeof(TheDryadBow),
			typeof(Frostbringer), typeof(GauntletsOfNobility), typeof(HuntersHeaddress), typeof(HelmOfInsight),
			typeof(LegacyOfTheDreadLord), typeof(RingOfTheElements), typeof(MidnightBracers),
			typeof(HolyKnightsBreastplate), typeof(OrnateCrownOfTheHarrower), typeof(SerpentsFang),
			typeof(VoiceOfTheFallenKing), typeof(OrnamentOfTheMagician), typeof(StaffOfTheMagi),
			typeof(TunicOfFire),
		};

		public static Type[] ElghinArtifacts { get; } =
		{
			typeof(AtrybutMysliwego), typeof(Belthor), typeof(FartuchMajstraZTasandory), typeof(HelmMagaBojowego),
			typeof(KoszulaZPajeczychNici), typeof(LuskiStarozytnegoSmoka), typeof(MaskaZabojcy),
			typeof(PlaszczPoszukiwaczaPrzygod), typeof(SpodenkiLukmistrza), typeof(SzarfaRegentaGarlan),
			typeof(TunikaNamiestnikaSnieznejPrzystani), typeof(WisiorMagowBialejWiezy),
		};

		public static Type[] BossArtifacts { get; } =
		{
			// Lato
			/* typeof( Aegis ),
			 typeof ( Manat ),
			 typeof( Draupnir ),
			 typeof( Gungnir ),
			 typeof( RekawyJingu ),
			 typeof( Hrunting ),
			 typeof( KulawyMagik ),
			 typeof( LegendaMedrcow ),
			 typeof( MieczeAmrIbnLuhajj ),
			 typeof( PostrachPrzekletych ),
			 typeof( Przysiega ),
			 typeof( Retorta ),
			 typeof ( ScrappersCompendium ),
			 typeof( RycerzeWojny ),
			 typeof( SerpentsFang ),
			 typeof( ShadowDancerLeggings ),
			 typeof( SmoczeJelita ),
			 typeof( SpodnieOswiecenia ),
			 typeof( SpodniePodstepu ),
			 typeof( TchnienieMatki ),
			 typeof ( TomeOfLostKnowledge ),
			 typeof( Svalinn ),
			 typeof( Vijaya ),
			 typeof( WiernyPrzysiedze ),
			 typeof( Wrzeciono ),
			 typeof( Zapomnienie ),
			 typeof( DreadsRevenge ),
			 typeof( DarkenedSky ),
			 typeof ( WindsEdge ),
			 typeof ( HanzosBow ),
			 typeof ( TheDestroyer ),
			 typeof ( HolySword ),
			 typeof ( ShaminoCrossbow ),
			 typeof ( LegendaStraznika ),
			 typeof ( MagicznySaif ),
			 typeof ( MlotPharrosa ),
			 typeof ( RoyalGuardSurvivalKnife ),
			 typeof ( Calm ),
			 typeof ( StrzalaAbarisa ),
			 typeof ( Pacify ),
			 typeof ( RighteousAnger ),
			 typeof ( SoulSeeker ),
			 typeof ( BlightGrippedLongbow ),
			 typeof ( TheNightReaper ),
			 typeof ( RuneBeetleCarapace ),
			 typeof ( Ancile ),
			 typeof ( KosciLosu ),
			 typeof ( LegendaMedrcow ),
			 typeof ( SmoczeJelita ),
			 typeof ( SongWovenMantle ),
			 typeof ( SpellWovenBritches ),
			 typeof ( GuantletsOfAnger ),
			 typeof ( KasaOfTheRajin ),
			 typeof ( CrownOfTalKeesh ),
			 typeof ( CrimsonCincture),
			 typeof ( DjinnisRing ),
			 typeof ( PendantOfTheMagi ),
			 typeof ( PocalunekBoginii ),*/

			/*   //Jesien
			   typeof(Aegis),
			   typeof(Manat),
			   typeof(Draupnir),
			   typeof(Gungnir),
			   typeof(RekawyJingu),
			   typeof(Hrunting),
			   typeof(KulawyMagik),
			   typeof(LegendaMedrcow),
			   typeof(PostrachPrzekletych),
			   typeof(Przysiega),
			   typeof(Retorta),
			   typeof(ScrappersCompendium),
			   typeof(RycerzeWojny),
			   typeof(SmoczeJelita),
			   typeof(SpodnieOswiecenia),
			   typeof(SpodniePodstepu),
			   typeof(BagiennaTunika),
			   typeof(BeretUczniaWeterynarza),
			   typeof(BerummTulThorok),
			   typeof(CzapkaRoduNolens),
			   typeof(DelikatnyDiamentowyNaszyjnikIgisZGarlan),
			   typeof(DotykDriad),
			   typeof(ElfickaSpodniczka),
			   typeof(HelmTarana),
			   typeof(HelmWladcyMorrlokow),
			   typeof(NaramiennikiStrazyObywatelskiej),
			   typeof(OgienZKuchniMurdulfa),
			   typeof(RekawiceStraznikaWulkanu),
			   typeof(ReliktDrowow),
			   typeof(SokoliWzork),
			   typeof(TrupieRece),
			   typeof(ZgubaSoteriosa),*/

			//Zima
			typeof(ZgubaSoteriosa), typeof(SpellWovenBritches), typeof(GuantletsOfAnger), typeof(KasaOfTheRajin),
			typeof(TomeOfLostKnowledge), typeof(Svalinn), typeof(Vijaya), typeof(WiernyPrzysiedze), typeof(Wrzeciono),
			typeof(Zapomnienie), typeof(DreadsRevenge), typeof(Calm), typeof(StrzalaAbarisa), typeof(Pacify),
			typeof(SmoczeJelita), typeof(SpodnieOswiecenia), typeof(SpodniePodstepu), typeof(BookOfKnowledge),
			typeof(KosturMagaZOrod), typeof(KrwaweNieszczescie), typeof(CzapkaRoduNolens),
			typeof(DelikatnyDiamentowyNaszyjnikIgisZGarlan),
		};

		public static Type[] MinibossArtifacts { get; } =
		{
			//Lato
			/*typeof(GniewOceanu),
			typeof(Tyrfing),
			typeof(BerloLitosci),
			typeof(KasraShamshir),
			typeof(KilofZRuinTwierdzy),
			typeof(ArcticDeathDealer),
			typeof(CaptainQuacklebushsCutlass),
			typeof(CavortingClub),
			typeof(NightsKiss),
			typeof(PixieSwatter),
			typeof(StraznikPolnocy),
			typeof(TomeOfEnlightenment),
			typeof(BlazeOfDeath),
			typeof(EnchantedTitanLegBone),
			typeof(StaffOfPower),
			typeof(WrathOfTheDryad),
			typeof(LunaLance),
			typeof(LowcaDusz),
			typeof(Saif),
			typeof(Gandiva),
			typeof(Sharanga),
			typeof(BowOfTheJukaKing),
			typeof(NoxRangersHeavyCrossbow),
			typeof(ZgubaDemonaOgnia),
			typeof(HeartOfTheLion),
			typeof(RekawiceAvadaGrava),
			typeof(SzponySzalenstwa),
			typeof(GlovesOfThePugilist),
			typeof(GrdykaZWiezyMagii),
			typeof(OrcishVisage),
			typeof(PolarBearMask),
			typeof(MlotPharrosa),
			typeof(LegendaGenerala),
			typeof(AlchemistsBauble),
			typeof(ShieldOfInvulnerability),
			typeof(DemonForks),
			typeof(Exiler),*/

			/* //Jesien
			 typeof(GlovesOfTheSun),
			 typeof(OrleSkrzydla),
			 typeof(Nasr),
			 typeof(WidlyMroku),
			 typeof(TheHorselord),
			 typeof(ZlamanyGungnir),
			 typeof(MelisandesCorrodedHatchet),
			 typeof(OverseerSunderedBlade),
			 typeof(PocalunekBoginii),
			 typeof(ChwytTeczy),
			 typeof(CorlrummEronDaUmri),
			 typeof(LodowaPoswiata),
			 typeof(OstrzanyKijMnichaZTasandory),
			 typeof(PasterzDuszPotepionych),
			 typeof(PikaZKolcemSkorpionaKrolewskiego),
			 typeof(PogromcaDrowow),
			 typeof(SiekieraDrwalaZCelendir),
			 typeof(SzponyOgnistegoSmoka),
			 typeof(ToporPierwszegoKrasnoluda),
			 typeof(ToporWysokichElfow),
			 typeof(SmoczyWrzask),*/

			// Zima
			typeof(LunaLance), typeof(CorlrummEronDaUmri), typeof(StraznikPolnocy), typeof(TomeOfEnlightenment),
			typeof(DemonForks), typeof(GniewOceanu), typeof(ArkanaZywiolow), typeof(FeyLeggings),
			typeof(WrathOfTheDryad), typeof(GrdykaZWiezyMagii), typeof(OverseerSunderedBlade), typeof(ResilientBracer),
			typeof(ColdForgedBlade), typeof(Aderthand), typeof(ArcticBeacon), typeof(ArmsOfToxicity),
			typeof(JaszczurzySzal),
		};

		public static Type[] ParagonArtifacts { get; } =
		{
			typeof(GoldBricks), typeof(AlchemistsBauble), typeof(ArcticDeathDealer), typeof(BlazeOfDeath),
			typeof(BowOfTheJukaKing), typeof(BurglarsBandana), typeof(CavortingClub), typeof(EnchantedTitanLegBone),
			typeof(GwennosHarp), typeof(IolosLute), typeof(LunaLance), typeof(NightsKiss),
			typeof(NoxRangersHeavyCrossbow), typeof(OrcishVisage), typeof(PolarBearMask),
			typeof(ShieldOfInvulnerability), typeof(StaffOfPower), typeof(VioletCourage), typeof(HeartOfTheLion),
			typeof(WrathOfTheDryad), typeof(PixieSwatter), typeof(GlovesOfThePugilist),
		};

		public static Type[] FishingBossArtifacts { get; } =
		{
			typeof(CaptainQuacklebushsCutlass), typeof(NightsKiss), typeof(StraznikPolnocy), typeof(BlazeOfDeath),
			typeof(BowOfTheJukaKing), typeof(SpellWovenBritches), typeof(LegendaGenerala), typeof(BraceletOfHealth),
			typeof(Raikiri), typeof(SerpentsFang), typeof(OstrzePolksiezyca), typeof(ZdradzieckaSzata),
			typeof(OponczaEnergii), typeof(OponczaMrozu), typeof(OponczaOgnia), typeof(OponczaTrucizny),
			typeof(PrzysiegaTriamPergi), typeof(SzataHutum), typeof(RekawiceBulpa), typeof(DreadPirateHat),
			typeof(BlogoslawienstwoBogow), typeof(ArkanaZywiolow), typeof(DragonNunchaku),
			typeof(PilferedDancerFans), typeof(KlatwaMagow), typeof(LegendaStraznika), typeof(Pogrobowce),
			typeof(WidlyMroku), typeof(ZlamanyGungnir), typeof(Subdue), typeof(MelisandesCorrodedHatchet),
			typeof(RaedsGlory), typeof(SoulSeeker), typeof(BlightGrippedLongbow), typeof(RuneBeetleCarapace),
			typeof(OponczaMrozu), typeof(LeggingsOfEmbers), typeof(IronwoodCrown), typeof(PasMurdulfaZlotobrodego),
			typeof(PadsOfTheCuSidhe), typeof(StudniaOdnowy),
		};

		public static Type[] CartographyArtifacts { get; } =
		{
			/* //Jesien
			 typeof(Retorta),
			 typeof(BoneCrusher),
			 typeof(CaptainQuacklebushsCutlass),
			 typeof(ColdBlood),
			 typeof(NightsKiss),
			 typeof(EnchantedTitanLegBone),
			 typeof(WrathOfTheDryad),
			 typeof(LunaLance),
			 typeof(OstrzePolksiezyca),
			 typeof(BowOfTheJukaKing),
			 typeof(NoxRangersHeavyCrossbow),
			 typeof(ZdradzieckaSzata),
			 typeof(PiecioMiloweSandaly),
			 typeof(GlovesOfTheSun),
			 typeof(OponczaOgnia),
			 typeof(OponczaTrucizny),
			 typeof(PrzysiegaTriamPergi),
			 typeof(SzataHutum),
			 typeof(BoskieNogawniceLodu),
			 typeof(DiabelskaSkora),
			 typeof(KrokWCieniu),
			 typeof(MyckaRybaka),
			 typeof(OchronaCialaIDucha),
			 typeof(RekawiceGornikaZOrod),
			 typeof(ZlotaSciana),
			 typeof(SrebrneOstrzeZEnedh),
 
			 //Lato
 
			 typeof(AncientSamuraiDo),
			 typeof(RekawiceBulpa),
			 typeof(BurglarsBandana),
			 typeof(PolarBearMask),
			 typeof(LegendaGenerala),
			 typeof(Bonesmasher),
			 typeof(BlogoslawienstwoBogow),
			 typeof(BraceletOfHealth),
			 typeof(AlchemistsBauble),
			 typeof(SwordsOfProsperity),
			 typeof(Exiler),
			 typeof(LegsOfStability),
			 typeof(TheDestroyer),
			 typeof(KonarMlodegoDrzewaZycia),
			 typeof(Nasr),
			 typeof(PalkaZAbadirem),
			 typeof(BraveKnightOfTheBritannia),
			 typeof(ZlamanyGungnir),
			 typeof(Pacify),
			 typeof(FleshRipper),
			 typeof(MelisandesCorrodedHatchet),
			 typeof(RighteousAnger),
			 typeof(LegendaMedrcow),
			 typeof(SongWovenMantle),
			 typeof(GuantletsOfAnger),
			 typeof(CrownOfTalKeesh),*/

			// Zima
			typeof(Subdue), typeof(MelisandesCorrodedHatchet), typeof(RaedsGlory), typeof(LukKrolaElfow),
			typeof(BoskieNogawniceLodu), typeof(DiabelskaSkora), typeof(KonarMlodegoDrzewaZycia), typeof(Nasr),
			typeof(PalkaZAbadirem), typeof(BraveKnightOfTheBritannia), typeof(OchronaPrzedZaraza),
			typeof(RytualnySztyletDruidow), typeof(SmoczaPrzywara), typeof(LegsOfStability), typeof(BladeDance),
			typeof(LegendaKapitana), typeof(IolosLute), typeof(StaffOfPower), typeof(KasraShamshir),
		};

		public static Item CreateRandomArtifact()
		{
			switch (Utility.Random(7))
			{
				case 0: return CreateRandomBossArtifact();
				case 1: return CreateRandomMinibossArtifact();
				case 2: return CreateRandomParagonArtifact();
				case 3: return CreateRandomDoomArtifact();
				case 4: return CreateRandomHunterArtifact();
				case 5: return CreateRandomCartographyArtifact();
				case 6: return CreateRandomFishingArtifact();
				default: return CreateRandomParagonArtifact();
			}
		}

		public static Item CreateRandomDoomArtifact()
		{
			int random = Utility.Random(DoomArtifacts.Length);
			Type type = DoomArtifacts[random];

			return Loot.Construct(type);
		}

		public static Item CreateRandomElghinArtifact()
		{
			int random = Utility.Random(ElghinArtifacts.Length);
			Type type = ElghinArtifacts[random];

			return Loot.Construct(type);
		}


		public static Item CreateRandomBossArtifact()
		{
			int random = Utility.Random(BossArtifacts.Length);
			Type type = BossArtifacts[random];

			return Loot.Construct(type);
		}

		public static Item CreateRandomMinibossArtifact()
		{
			int random = Utility.Random(MinibossArtifacts.Length);
			Type type = MinibossArtifacts[random];

			return Loot.Construct(type);
		}

		public static Item CreateRandomFishingArtifact()
		{
			int random = Utility.Random(FishingBossArtifacts.Length);
			Type type = FishingBossArtifacts[random];

			return Loot.Construct(type);
		}

		public static Item CreateRandomParagonArtifact()
		{
			int random = Utility.Random(ParagonArtifacts.Length);
			Type type = ParagonArtifacts[random];

			return Loot.Construct(type);
		}

		public static Item CreateRandomHunterArtifact()
		{
			return HunterRewardCalculator.CreateArtifacts(Utility.RandomList(10, 15, 20));
		}

		public static Item CreateRandomCustomChampArtifact()
		{
			int random = Utility.Random(m_CustomChampArtifacts.Length);
			Type type = m_CustomChampArtifacts[random];

			return Loot.Construct(type);
		}

		public static Item CreateRandomCartographyArtifact()
		{
			int random = Utility.Random(CartographyArtifacts.Length);
			Type type = CartographyArtifacts[random];

			return Loot.Construct(type);
		}

		public static Mobile FindRandomPlayer(BaseCreature creature)
		{
			List<DamageStore> rights = creature.GetLootingRights();

			for (int i = rights.Count - 1; i >= 0; --i)
			{
				DamageStore ds = rights[i];

				if (!ds.m_HasRight)
					rights.RemoveAt(i);
			}

			if (rights.Count > 0)
				return rights[Utility.Random(rights.Count)].m_Mobile;

			return null;
		}

		public static void ArtifactDistribution(BaseCreature creature)
		{
			if (creature.Summoned || creature.NoKillAwards)
				return;

			if (CheckArtifactChance(creature))
				DistributeArtifact(creature);
		}

		public static void DistributeArtifact(BaseCreature creature)
		{
			ArtGroup group = ArtifactMonster.GetGroupFor(creature);

			switch (group)
			{
				case ArtGroup.Doom:
					DistributeArtifact(creature, CreateRandomDoomArtifact());
					break;
				case ArtGroup.Boss:
					DistributeArtifact(creature, CreateRandomBossArtifact());
					break;
				case ArtGroup.Miniboss:
					DistributeArtifact(creature, CreateRandomMinibossArtifact());
					break;
				case ArtGroup.Fishing:
					DistributeArtifact(creature, CreateRandomFishingArtifact());
					break;


				case ArtGroup.None:
				default:
					break;
			}
		}

		public static void DistributeArtifact(BaseCreature creature, Item artifact)
		{
			DistributeArtifact(FindRandomPlayer(creature), artifact);
		}

		public static void DistributeArtifact(Mobile to, Item artifact)
		{
			if (to == null || artifact == null)
				return;

			if (to.AccessLevel > AccessLevel.Player)
			{
				artifact.ModifiedBy = to.Account.Username;
				artifact.ModifiedDate = DateTime.Now;
			}

			bool message = true;

			Container pack = to.Backpack;

			if (pack == null || !pack.TryDropItem(to, artifact, false))
			{
				if (to.BankBox != null && to.BankBox.TryDropItem(to, artifact, false))
				{
					to.BankBox.DropItem(artifact);
					to.SendMessage("W nagrode za pokonanie bestii otrzymujesz artefakt! Artefakt laduje w banku.");
					to.PlaySound(0x1F7);
					to.FixedParticles(0x373A, 1, 15, 9913, 67, 7, EffectLayer.Head);
				}
				else
				{
					to.SendLocalizedMessage(
						1072523); // Otrzymujesz artefakt, lecz nie masz miejsca w plecaku ani banku. Artefakt upada na ziemie!
					to.Emote("Postac zdobyla artefakt, ktory upadl na ziemie!");
					to.PlaySound(0x1F7);
					to.FixedParticles(0x373A, 1, 15, 9913, 67, 7, EffectLayer.Head);
					message = false;

					artifact.MoveToWorld(to.Location, to.Map);
				}
			}

			if (message)
				to.SendLocalizedMessage(1062317); // W nagrode za pokonanie bestii otrzymujesz artefakt!
			to.Emote("Postac zdobyla artefakt!");
			to.PlaySound(0x1F7);
			to.FixedParticles(0x373A, 1, 15, 9913, 67, 7, EffectLayer.Head);
			int itemID2 = 0xF5F;
			IEntity from = new Entity(Serial.Zero, new Point3D(to.X, to.Y, to.Z), to.Map);
			IEntity too = new Entity(Serial.Zero, new Point3D(to.X, to.Y, to.Z + 50), to.Map);
			Effects.SendMovingParticles(from, too, itemID2, 1, 0, false, false, 33, 3, 9501, 1, 0, EffectLayer.Head,
				0x100);
		}

		public static double GetArtifactChance(BaseCreature boss)
		{
			double luck = LootPack.GetLuckChanceForKiller(boss);

			if (luck > 1200)
				luck = 1200;

			double chance = ArtifactMonster.GetChanceFor(boss);
			chance *= 1.0 + 0.25 * (luck / 1200); // luck zwieksza szanse maksymalnie do 125% 

			return chance;
		}

		public static bool CheckArtifactChance(BaseCreature boss)
		{
			return GetArtifactChance(boss) > Utility.RandomDouble();
		}
	}
}
