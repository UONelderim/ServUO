using System;
using System.Collections.Generic;
using System.Linq;
using Nelderim;
using Server.Mobiles;
using Server.Engines.BulkOrders;
using Server.Engines.Points;
using Server.Mobiles.Swiateczne;

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
		Paragon
	}

	enum ArtSeason
	{
		Summer,
		Autumn,
		Winter,
		Spring
	}

	internal record struct ArtInfo(double Chance, ArtGroup Group);

	class ArtifactHelper
	{
		// TUTAJ PODMIENIAC SEZONY ARTEFAKTOW:
		private static readonly ArtSeason _CurrentSeason = ArtSeason.Winter;
		
		private static readonly ArtSeason[] _AllSeasons = { ArtSeason.Summer, ArtSeason.Autumn, ArtSeason.Winter, ArtSeason.Spring };

		private static readonly Dictionary<Type, ArtInfo> _CreatureInfo = new();
		
		public static Item GetRandomArtifact(ArtGroup group)
		{
			if (_Artifacts.TryGetValue(group, out var groupArtifacts))
			{
				if (groupArtifacts.TryGetValue(_CurrentSeason, out var seasonArtifacts))
				{
					return Loot.Construct(Utility.RandomList(seasonArtifacts));
				}
				Console.WriteLine($"No season {_CurrentSeason} for artifact group {group} ");
			}
			else
			{
				Console.WriteLine($"No artifact group definition {group} ");
			}

			return null;
		}
		
		public static void Configure()
		{
			//Elghin
			_CreatureInfo.Add(typeof(DalharukElghinn), new ArtInfo(10, ArtGroup.Elghin));
			_CreatureInfo.Add(typeof(KevinBoss), new ArtInfo(5, ArtGroup.Elghin));
			//Bossy
			_CreatureInfo.Add(typeof(NGorogon), new ArtInfo(5, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(Sfinks), new ArtInfo(5, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(NSzeol), new ArtInfo(5, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(NBurugh), new ArtInfo(6, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(NKatrill), new ArtInfo(5, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(NDeloth), new ArtInfo(5, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(NDzahhar), new ArtInfo(5, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(NSarag), new ArtInfo(6, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(NelderimSkeletalDragon), new ArtInfo(7, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(NStarozytnyLodowySmok), new ArtInfo(8, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(StarozytnyDiamentowySmok), new ArtInfo(8, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(NStarozytnySmok), new ArtInfo(8, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(WladcaDemonow), new ArtInfo(10, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(Zhoaminth), new ArtInfo(15, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(MinotaurBoss), new ArtInfo(5, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(DreadHorn), new ArtInfo(5, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(LadyMelisande), new ArtInfo(7, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(Travesty), new ArtInfo(6, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(ChiefParoxysmus), new ArtInfo(10, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(Harrower), new ArtInfo(100, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(AncientRuneBeetle), new ArtInfo(7, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(Serado), new ArtInfo(40, ArtGroup.Boss));
			_CreatureInfo.Add(typeof(BetrayerBoss), new ArtInfo(5, ArtGroup.Boss));
			//Mini Bossy
			_CreatureInfo.Add(typeof(WladcaJezioraLawy), new ArtInfo(7, ArtGroup.Miniboss));
			_CreatureInfo.Add(typeof(BagusGagakCreeper), new ArtInfo(7, ArtGroup.Miniboss));
			_CreatureInfo.Add(typeof(VitVarg), new ArtInfo(7, ArtGroup.Miniboss));
			_CreatureInfo.Add(typeof(TilkiBug), new ArtInfo(7, ArtGroup.Miniboss));
			_CreatureInfo.Add(typeof(NelderimDragon), new ArtInfo(3, ArtGroup.Miniboss));
			_CreatureInfo.Add(typeof(ShimmeringEffusion), new ArtInfo(9, ArtGroup.Miniboss));
			_CreatureInfo.Add(typeof(MonstrousInterredGrizzle), new ArtInfo(9, ArtGroup.Miniboss));
			_CreatureInfo.Add(typeof(NSilshashaszals), new ArtInfo(5, ArtGroup.Miniboss));
			_CreatureInfo.Add(typeof(SaragAwatar), new ArtInfo(2, ArtGroup.Miniboss));
			_CreatureInfo.Add(typeof(WladcaPiaskow), new ArtInfo(4, ArtGroup.Miniboss));
			// m_CreatureInfo.Add(typeof(IceDragon), new ArtInfo(5, ArtGroup.Miniboss));
			_CreatureInfo.Add(typeof(EvilSpellbook), new ArtInfo(5, ArtGroup.Miniboss));
			_CreatureInfo.Add(typeof(ExodusBoss), new ArtInfo(12, ArtGroup.Miniboss));
			//Custom champy
			_CreatureInfo.Add(typeof(KapitanIIILegionuOrkow), new ArtInfo(30, ArtGroup.CustomChamp));
			_CreatureInfo.Add(typeof(MorenaAwatar), new ArtInfo(30, ArtGroup.CustomChamp));
			_CreatureInfo.Add(typeof(Meraktus), new ArtInfo(20, ArtGroup.CustomChamp));
			_CreatureInfo.Add(typeof(Ilhenir), new ArtInfo(20, ArtGroup.CustomChamp));
			_CreatureInfo.Add(typeof(Twaulo), new ArtInfo(15, ArtGroup.CustomChamp));
			_CreatureInfo.Add(typeof(Pyre), new ArtInfo(30, ArtGroup.CustomChamp));
			_CreatureInfo.Add(typeof(MikolajBoss), new ArtInfo(5, ArtGroup.CustomChamp));
			//Fishing Bossy
			_CreatureInfo.Add(typeof(Leviathan), new ArtInfo(10, ArtGroup.Fishing));

			InitializeArtifacts();
			
			EventSink.CreatureDeath += OnCreatureDeath;
		}

		private static void OnCreatureDeath(CreatureDeathEventArgs e)
		{
			if (e.Creature is BaseCreature bc && _CreatureInfo.ContainsKey(bc.GetType()))
			{
				DistributeArtifact(bc);
			}
		}
		
		private static void DistributeArtifact(BaseCreature creature)
		{
			if (creature.Summoned || creature.NoKillAwards)
				return;

			if (CheckArtifactChance(creature))
			{
				var group = GetGroupFor(creature);
				DistributeArtifact(FindRandomPlayer(creature), GetRandomArtifact(group));
			}
		}

		private static bool CheckArtifactChance(BaseCreature boss)
		{
			//Luck chance is non linear
			//5136 for 1200 luck
			//10000 for 4000 luck
			var chance = GetChanceFor(boss);
			chance *= 1.0 + LootPack.GetLuckChanceForKiller(boss) / 10000f;

			return chance > Utility.RandomDouble();
		}

		private static double GetChanceFor(BaseCreature bc)
		{
			if (_CreatureInfo.TryGetValue(bc.GetType(), out var value))
			{
				return value.Chance / 100.0;
			}

			return 0;
		}
		
		private static ArtGroup GetGroupFor(BaseCreature bc)
		{
			if (_CreatureInfo.TryGetValue(bc.GetType(), out var value))
				return value.Group;
			
			Console.WriteLine("Unknown ArtGroup for " + bc.GetType().Name);
			return ArtGroup.None;
		}
		
		private static Mobile FindRandomPlayer(BaseCreature creature)
		{
			var rights = creature
				.GetLootingRights()
				.Where(ds => ds.m_HasRight)
				.ToList();

			if (rights.Count <= 0) 
				return null;
			
			return Utility.RandomList(rights).m_Mobile;
		}

		public static void DistributeArtifact(Mobile m, Item artifact)
		{
			if (m == null || artifact == null)
				return;

			LabelsConfig.AddCreationMark(artifact, m);
			if (m.IsStaff())
			{
				LabelsConfig.AddTamperingMark(artifact, m);
			}

			m.SendLocalizedMessage(1062317); // W nagrode za pokonanie bestii otrzymujesz artefakt!
			m.Emote("Postac zdobyla artefakt!");
			m.PlaySound(0x1F7);
			m.FixedParticles(0x373A, 1, 15, 9913, 67, 7, EffectLayer.Head);

			var pack = m.Backpack;
			if (pack == null || !pack.TryDropItem(m, artifact, false))
			{
				if (m.BankBox != null && m.BankBox.TryDropItem(m, artifact, false))
				{
					m.SendMessage("Artefakt laduje w banku.");
				}
				else
				{
					// Otrzymujesz artefakt, lecz nie masz miejsca w plecaku ani banku. Artefakt upada na ziemie!
					m.SendLocalizedMessage(1072523);
					m.Emote("Artefakt upadl na ziemie!");
					artifact.MoveToWorld(m.Location, m.Map);
				}
			}

			Effects.SendMovingParticles(m,
				new Entity(Serial.Zero, new Point3D(m.X, m.Y, m.Z + 50), m.Map),
				0xF5F,
				1,
				0,
				false,
				false,
				33,
				3,
				9501,
				1,
				0,
				EffectLayer.Head,
				0x100);
			Console.WriteLine($"ART: {m.Serial} {m.Name}: {artifact.GetType().Name}");
		}

		private static Dictionary<ArtGroup, Dictionary<ArtSeason, Type[]>> _Artifacts = new();

		private static void InitializeArtifacts()
		{
			Dictionary<ArtSeason, Type[]> ForAllSeasons(Type[] artifacts)
			{
				var dict = new Dictionary<ArtSeason, Type[]>();
				foreach (var season in _AllSeasons)
				{
					dict.Add(season, artifacts);
				}
				return dict;
			}

			_Artifacts.Add(ArtGroup.Boss, _BossArtifacts);
			_Artifacts.Add(ArtGroup.Miniboss, _MinibossArtifacts);
			_Artifacts.Add(ArtGroup.Fishing, ForAllSeasons(_FishingArtifacts));
			_Artifacts.Add(ArtGroup.Cartography, _CartographyArtifacts);
			_Artifacts.Add(ArtGroup.CustomChamp, ForAllSeasons(_CustomChampArtifacts));
			_Artifacts.Add(ArtGroup.Elghin, ForAllSeasons(_ElghinArtifacts));
			_Artifacts.Add(ArtGroup.Paragon, ForAllSeasons(_ParagonArtifacts));
			var allHunterArtifacts = new[]
				{
					HunterRewardCalculator.ArtLvl1, HunterRewardCalculator.ArtLvl2, HunterRewardCalculator.ArtLvl3,
					HunterRewardCalculator.ArtLvl4
				}
				.SelectMany(x => x) //flatten
				.ToArray();
			_Artifacts.Add(ArtGroup.Hunter, ForAllSeasons(allHunterArtifacts));
			var allDoomArtifacts = DoomGauntlet.RewardTable.SelectMany(x => x).Distinct().ToArray();
			_Artifacts.Add(ArtGroup.Doom, ForAllSeasons(allDoomArtifacts));

			_AllArtifactsCurrentSeason = _Artifacts
				.Values
				.SelectMany(x => 
					x
					.Where(p => p.Key == _CurrentSeason)
					.SelectMany(y => y.Value))
				.Distinct()
				.ToArray();
			
			_AllArtifacts = _Artifacts
				.Values
				.SelectMany(x => 
					x.SelectMany(y => y.Value))
				.Distinct()
				.ToArray();
		}

		private static Type[] _AllArtifacts;
		private static Type[] _AllArtifactsCurrentSeason;

		public static Type[] AllArtifacts => _AllArtifacts;
		public static Type[] AllArtifactsCurrentSeason => _AllArtifactsCurrentSeason;
		

		private static Type[] _ElghinArtifacts = {
			typeof(AtrybutMysliwego), typeof(Belthor), typeof(FartuchMajstraZTasandory), typeof(HelmMagaBojowego),
			typeof(KoszulaZPajeczychNici), typeof(LuskiStarozytnegoSmoka), typeof(MaskaZabojcy),
			typeof(PlaszczPoszukiwaczaPrzygod), typeof(SpodenkiLukmistrza), typeof(SzarfaRegentaGarlan),
			typeof(TunikaNamiestnikaSnieznejPrzystani), typeof(WisiorMagowBialejWiezy),
		};
		
		private static Dictionary<ArtSeason, Type[]> _BossArtifacts = new()
		{
			{
				ArtSeason.Summer,
				new[]
				{
					typeof(Aegis), typeof(Manat), typeof(Draupnir), typeof(Gungnir), typeof(RekawyJingu),
					typeof(Hrunting), typeof(KulawyMagik), typeof(LegendaMedrcow), typeof(MieczeAmrIbnLuhajj),
					typeof(PostrachPrzekletych), typeof(Przysiega), typeof(Retorta), typeof(RycerzeWojny),
					typeof(SerpentsFang), typeof(ShadowDancerLeggings), typeof(SmoczeJelita),
					typeof(SpodnieOswiecenia), typeof(SpodniePodstepu), typeof(TchnienieMatki),
					typeof(TomeOfLostKnowledge), typeof(Svalinn), typeof(Vijaya), typeof(WiernyPrzysiedze),
					typeof(Wrzeciono), typeof(Zapomnienie), typeof(DreadsRevenge), typeof(DarkenedSky),
					typeof(WindsEdge), typeof(HanzosBow), typeof(TheDestroyer), typeof(HolySword),
					typeof(ShaminoCrossbow), typeof(LegendaStraznika), typeof(MagicznySaif), typeof(MlotPharrosa),
					typeof(RoyalGuardSurvivalKnife), typeof(Calm), typeof(StrzalaAbarisa), typeof(Pacify),
					typeof(RighteousAnger), typeof(SoulSeeker), typeof(TheNightReaper), typeof(RuneBeetleCarapace),
					typeof(Ancile), typeof(KosciLosu), typeof(LegendaMedrcow), typeof(SmoczeJelita),
					typeof(GuantletsOfAnger), typeof(KasaOfTheRajin), typeof(CrownOfTalKeesh),
					typeof(CrimsonCincture), typeof(DjinnisRing), typeof(PocalunekBoginii),
				}
			},
			{
				ArtSeason.Spring,
				new[]
				{
					typeof(DreadsRevenge), typeof(Calm), typeof(StrzalaAbarisa), typeof(Pacify),
					typeof(SmoczeJelita), typeof(RycerzeWojny), typeof(SerpentsFang), typeof(ShadowDancerLeggings),
					typeof(SmoczeJelita), typeof(SpodnieOswiecenia), typeof(KosturMagaZOrod),
					typeof(TchnienieMatki), typeof(TomeOfLostKnowledge), typeof(Svalinn), typeof(Vijaya),
					typeof(WiernyPrzysiedze), typeof(Wrzeciono), typeof(Zapomnienie), typeof(DreadsRevenge),
					typeof(DarkenedSky), typeof(WindsEdge), typeof(HanzosBow), typeof(TheDestroyer),
					typeof(HolySword), typeof(ShaminoCrossbow), typeof(LegendaStraznika), typeof(MagicznySaif),
					typeof(MlotPharrosa), typeof(RoyalGuardSurvivalKnife),
				}
			},
			{
				ArtSeason.Autumn,
				new[]
				{
					typeof(Aegis), typeof(Manat), typeof(Draupnir), typeof(Gungnir), typeof(RekawyJingu),
					typeof(Hrunting), typeof(KulawyMagik), typeof(LegendaMedrcow), typeof(PostrachPrzekletych),
					typeof(Przysiega), typeof(Retorta), typeof(RycerzeWojny), typeof(SmoczeJelita),
					typeof(SpodnieOswiecenia), typeof(SpodniePodstepu), typeof(BagiennaTunika),
					typeof(BeretUczniaWeterynarza), typeof(BerummTulThorok), typeof(CzapkaRoduNolens),
					typeof(DelikatnyDiamentowyNaszyjnikIgisZGarlan), typeof(DotykDriad), typeof(ElfickaSpodniczka),
					typeof(HelmTarana), typeof(HelmWladcyMorrlokow), typeof(NaramiennikiStrazyObywatelskiej),
					typeof(OgienZKuchniMurdulfa), typeof(RekawiceStraznikaWulkanu), typeof(ReliktDrowow),
					typeof(SokoliWzrok), typeof(TrupieRece), typeof(ZgubaSoteriosa),
				}
			},
			{
				ArtSeason.Winter,
				new[]
				{
					typeof(ZgubaSoteriosa), typeof(GuantletsOfAnger), typeof(KasaOfTheRajin),
					typeof(TomeOfLostKnowledge), typeof(Svalinn), typeof(Vijaya), typeof(WiernyPrzysiedze),
					typeof(Wrzeciono), typeof(Zapomnienie), typeof(DreadsRevenge), typeof(Calm),
					typeof(StrzalaAbarisa), typeof(Pacify), typeof(SmoczeJelita), typeof(SpodnieOswiecenia),
					typeof(SpodniePodstepu), typeof(BookOfKnowledge), typeof(KosturMagaZOrod),
					typeof(KrwaweNieszczescie), typeof(CzapkaRoduNolens),
					typeof(DelikatnyDiamentowyNaszyjnikIgisZGarlan),
				}
			}
		};

		private static Dictionary<ArtSeason, Type[]> _MinibossArtifacts = new()
		{
			{
				ArtSeason.Summer,
				new[]
				{
					typeof(GniewOceanu), typeof(Tyrfing), typeof(BerloLitosci), typeof(KasraShamshir),
					typeof(KilofZRuinTwierdzy), typeof(ArcticDeathDealer), typeof(CaptainQuacklebushsCutlass),
					typeof(CavortingClub), typeof(NightsKiss), typeof(PixieSwatter), typeof(StraznikPolnocy),
					typeof(TomeOfEnlightenment), typeof(BlazeOfDeath), typeof(EnchantedTitanLegBone),
					typeof(StaffOfPower), typeof(WrathOfTheDryad), typeof(LunaLance), typeof(LowcaDusz),
					typeof(Saif), typeof(Gandiva), typeof(Sharanga), typeof(BowOfTheJukaKing),
					typeof(NoxRangersHeavyCrossbow), typeof(ZgubaDemonaOgnia), typeof(HeartOfTheLion),
					typeof(RekawiceAvadaGrava), typeof(SzponySzalenstwa), typeof(GlovesOfThePugilist),
					typeof(GrdykaZWiezyMagii), typeof(OrcishVisage), typeof(PolarBearMask), typeof(MlotPharrosa),
					typeof(LegendaGenerala), typeof(AlchemistsBauble), typeof(ShieldOfInvulnerability),
					typeof(DemonForks), typeof(Exiler),
				}
			},
			{
				ArtSeason.Spring,
				new[]
				{
					typeof(RuneBeetleCarapace), typeof(OponczaMrozu), typeof(LeggingsOfEmbers), typeof(DemonForks),
					typeof(GniewOceanu), typeof(StraznikPolnocy), typeof(TomeOfEnlightenment), typeof(BlazeOfDeath),
					typeof(EnchantedTitanLegBone), typeof(StaffOfPower), typeof(WrathOfTheDryad), typeof(LunaLance),
					typeof(LowcaDusz), typeof(Saif), typeof(Gandiva), typeof(Sharanga), typeof(BowOfTheJukaKing),
					typeof(NoxRangersHeavyCrossbow), typeof(ZlamanyGungnir), typeof(MelisandesCorrodedHatchet),
					typeof(PocalunekBoginii), typeof(ChwytTeczy),
				}
			},
			{
				ArtSeason.Autumn,
				new[]
				{
					typeof(GlovesOfTheSun), typeof(OrleSkrzydla), typeof(Nasr), typeof(WidlyMroku),
					typeof(TheHorselord), typeof(ZlamanyGungnir), typeof(MelisandesCorrodedHatchet),
					typeof(PocalunekBoginii), typeof(ChwytTeczy), typeof(CorlrummEronDaUmri),
					typeof(LodowaPoswiata), typeof(OstrzanyKijMnichaZTasandory), typeof(PasterzDuszPotepionych),
					typeof(PikaZKolcemSkorpionaKrolewskiego), typeof(PogromcaDrowow),
					typeof(SiekieraDrwalaZCelendir), typeof(SzponyOgnistegoSmoka),
					typeof(ToporPierwszegoKrasnoluda), typeof(ToporWysokichElfow), typeof(SmoczyWrzask),
				}
			},
			{
				ArtSeason.Winter,
				new[]
				{
					typeof(LunaLance), typeof(CorlrummEronDaUmri), typeof(StraznikPolnocy),
					typeof(TomeOfEnlightenment), typeof(DemonForks), typeof(GniewOceanu), typeof(ArkanaZywiolow),
					typeof(FeyLeggings), typeof(WrathOfTheDryad), typeof(GrdykaZWiezyMagii), typeof(Aderthand),
					typeof(ArcticBeacon), typeof(ArmsOfToxicity), typeof(JaszczurzySzal),
				}
			}
		};

		private static Type[] _CustomChampArtifacts =
		{
			typeof(PrzekletaMaskaSmierci), typeof(PrzekletaStudniaOdnowy), typeof(PrzekleteOrleSkrzydla),
			typeof(PrzekletePogrobowce), typeof(PrzekleteFeyLeggings), typeof(PrzekleteWidlyMroku),
			typeof(PrzekletyKilofZRuinTwierdzy), typeof(PrzekletyMieczeAmrIbnLuhajj), typeof(PrzekletySoulSeeker),
			typeof(PrzekleteSongWovenMantle), typeof(PrzekletaArcaneShield), typeof(PrzekletaIolosLute),
			typeof(PrzekletaRetorta), typeof(PrzekletaSamaritanRobe), typeof(PrzekletaVioletCourage),
			typeof(PrzekleteFeyLeggings), typeof(PrzekleteGauntletsOfNobility), typeof(PrzekletyArcticDeathDealer),
			typeof(PrzekletyFleshRipper), typeof(PrzekletyQuell),
		};

		private static Type[] _FishingArtifacts =
		{
			typeof(CaptainQuacklebushsCutlass), typeof(NightsKiss), typeof(StraznikPolnocy), typeof(BlazeOfDeath),
			typeof(BowOfTheJukaKing), typeof(LegendaGenerala), typeof(BraceletOfHealth), typeof(Raikiri),
			typeof(SerpentsFang), typeof(OstrzePolksiezyca), typeof(ZdradzieckaSzata), typeof(OponczaEnergii),
			typeof(OponczaMrozu), typeof(OponczaOgnia), typeof(OponczaTrucizny), typeof(PrzysiegaTriamPergi),
			typeof(SzataHutum), typeof(RekawiceBulpa), typeof(DreadPirateHat), typeof(BlogoslawienstwoBogow),
			typeof(ArkanaZywiolow), typeof(DragonNunchaku), typeof(PilferedDancerFans), typeof(KlatwaMagow),
			typeof(LegendaStraznika), typeof(Pogrobowce), typeof(WidlyMroku), typeof(ZlamanyGungnir),
			typeof(Subdue), typeof(MelisandesCorrodedHatchet), typeof(RaedsGlory), typeof(SoulSeeker),
			typeof(RuneBeetleCarapace), typeof(OponczaMrozu), typeof(LeggingsOfEmbers),
			typeof(PasMurdulfaZlotobrodego), typeof(PadsOfTheCuSidhe), typeof(StudniaOdnowy),
		};

		private static Dictionary<ArtSeason, Type[]> _CartographyArtifacts = new()
		{
			{
				ArtSeason.Autumn,
				new[]
				{
					typeof(Retorta), typeof(BoneCrusher), typeof(CaptainQuacklebushsCutlass), typeof(ColdBlood),
					typeof(NightsKiss), typeof(EnchantedTitanLegBone), typeof(WrathOfTheDryad), typeof(LunaLance),
					typeof(OstrzePolksiezyca), typeof(BowOfTheJukaKing), typeof(NoxRangersHeavyCrossbow),
					typeof(ZdradzieckaSzata), typeof(PiecioMiloweSandaly), typeof(GlovesOfTheSun),
					typeof(OponczaOgnia), typeof(OponczaTrucizny), typeof(PrzysiegaTriamPergi), typeof(SzataHutum),
					typeof(BoskieNogawniceLodu), typeof(DiabelskaSkora), typeof(KrokWCieniu), typeof(MyckaRybaka),
					typeof(OchronaCialaIDucha), typeof(RekawiceGornikaZOrod), typeof(ZlotaSciana),
					typeof(SrebrneOstrzeZEnedh),
				}
			},
			{
				ArtSeason.Spring,
				new[]
				{
					typeof(Retorta), typeof(BoneCrusher), typeof(CaptainQuacklebushsCutlass), typeof(ColdBlood),
					typeof(NightsKiss), typeof(EnchantedTitanLegBone), typeof(WrathOfTheDryad), typeof(LunaLance),
					typeof(OstrzePolksiezyca), typeof(BowOfTheJukaKing), typeof(NoxRangersHeavyCrossbow),
					typeof(ZdradzieckaSzata), typeof(PiecioMiloweSandaly), typeof(BraceletOfHealth),
					typeof(AlchemistsBauble), typeof(SwordsOfProsperity), typeof(Exiler), typeof(MyckaRybaka),
					typeof(OchronaCialaIDucha), typeof(RekawiceGornikaZOrod), typeof(ZlotaSciana),
					typeof(SrebrneOstrzeZEnedh), typeof(LegendaKapitana), typeof(IolosLute), typeof(StaffOfPower),
					typeof(KasraShamshir),
				}
			},
			{
				ArtSeason.Summer,
				new[]
				{
					typeof(AncientSamuraiDo), typeof(RekawiceBulpa), typeof(BurglarsBandana), typeof(PolarBearMask),
					typeof(LegendaGenerala), typeof(Bonesmasher), typeof(BlogoslawienstwoBogow),
					typeof(BraceletOfHealth), typeof(AlchemistsBauble), typeof(SwordsOfProsperity), typeof(Exiler),
					typeof(LegsOfStability), typeof(TheDestroyer), typeof(KonarMlodegoDrzewaZycia), typeof(Nasr),
					typeof(PalkaZAbadirem), typeof(BraveKnightOfTheBritannia), typeof(ZlamanyGungnir),
					typeof(Pacify), typeof(FleshRipper), typeof(MelisandesCorrodedHatchet), typeof(RighteousAnger),
					typeof(LegendaMedrcow), typeof(GuantletsOfAnger), typeof(CrownOfTalKeesh),
				}
			},
			{
				ArtSeason.Winter,
				new[]
				{
					typeof(Subdue), typeof(MelisandesCorrodedHatchet), typeof(RaedsGlory), typeof(LukKrolaElfow),
					typeof(BoskieNogawniceLodu), typeof(DiabelskaSkora), typeof(KonarMlodegoDrzewaZycia),
					typeof(Nasr), typeof(PalkaZAbadirem), typeof(BraveKnightOfTheBritannia),
					typeof(OchronaPrzedZaraza), typeof(RytualnySztyletDruidow), typeof(SmoczaPrzywara),
					typeof(LegsOfStability), typeof(BladeDance), typeof(LegendaKapitana), typeof(IolosLute),
					typeof(StaffOfPower), typeof(KasraShamshir),
				}
			}
		};

		private static Type[] _ParagonArtifacts =
		{
			typeof(GoldBricks), typeof(AlchemistsBauble), typeof(ArcticDeathDealer), typeof(BlazeOfDeath),
			typeof(BowOfTheJukaKing), typeof(BurglarsBandana), typeof(CavortingClub), typeof(EnchantedTitanLegBone),
			typeof(GwennosHarp), typeof(IolosLute), typeof(LunaLance), typeof(NightsKiss),
			typeof(NoxRangersHeavyCrossbow), typeof(OrcishVisage), typeof(PolarBearMask),
			typeof(ShieldOfInvulnerability), typeof(StaffOfPower), typeof(VioletCourage), typeof(HeartOfTheLion),
			typeof(WrathOfTheDryad), typeof(PixieSwatter), typeof(GlovesOfThePugilist),
		};
	}
}
