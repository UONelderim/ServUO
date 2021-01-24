using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Accounting;

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
        Stealing,
    }

    class ArtifactMonster
    {
        public class ArtInfo
        {
            private double m_Chance;
            private ArtGroup m_Group;

            public double PercentChance { get { return m_Chance; } }
            public ArtGroup Group { get { return m_Group; } }

            public ArtInfo( double percent, ArtGroup gr )
            {
                m_Chance = percent;
                m_Group = gr;
            }
        }

        private static Dictionary<Type, ArtInfo> m_CreatureInfo = new Dictionary<Type, ArtInfo>();

        static ArtifactMonster()
        {
            // Wzor:
            // m_CreatureInfo.Add(typeof(<KlasaPotwora>), new ArtInfo(<ProcentSzansy>, ArtGroup.<GrupaArtefaktow>));

            //Doom
            m_CreatureInfo.Add( typeof( DemonKnight ), new ArtInfo( 4, ArtGroup.Doom ) );
            m_CreatureInfo.Add( typeof( AbysmalHorror ), new ArtInfo( 1.5, ArtGroup.Doom ) );
            m_CreatureInfo.Add( typeof( DarknightCreeper ), new ArtInfo( 1.5, ArtGroup.Doom ) );
            m_CreatureInfo.Add( typeof( FleshRenderer ), new ArtInfo( 1.5, ArtGroup.Doom ) );
            m_CreatureInfo.Add( typeof( Impaler ), new ArtInfo( 1.5, ArtGroup.Doom ) );
            m_CreatureInfo.Add( typeof( ShadowKnight ), new ArtInfo( 1.5, ArtGroup.Doom ) );

            //Bossy
            m_CreatureInfo.Add( typeof( NGorogon ), new ArtInfo( 5, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( Sfinks ), new ArtInfo( 5, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( NSzeol ), new ArtInfo( 5, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( NBurugh ), new ArtInfo( 6, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( NKatrill ), new ArtInfo( 5, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( NDeloth ), new ArtInfo( 5, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( NDzahhar ), new ArtInfo( 5, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( NSarag ), new ArtInfo( 6, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( NelderimSkeletalDragon ), new ArtInfo( 7, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( NStarozytnyLodowySmok ), new ArtInfo( 8, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( StarozytnyDiamentowySmok ), new ArtInfo( 8, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( NStarozytnySmok ), new ArtInfo( 8, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( WladcaDemonow ), new ArtInfo( 10, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( MinotaurBoss ), new ArtInfo( 5, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( DreadHorn ), new ArtInfo( 5, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( LadyMelisande ), new ArtInfo( 7, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( Travesty ), new ArtInfo( 6, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( ChiefParoxysmus ), new ArtInfo( 10, ArtGroup.Boss ) );
            m_CreatureInfo.Add( typeof( Harrower ), new ArtInfo( 100, ArtGroup.Boss ) );

            //Mini Bossy
            m_CreatureInfo.Add( typeof( WladcaJezioraLawy ), new ArtInfo( 7, ArtGroup.Miniboss ) );
            m_CreatureInfo.Add( typeof( BagusGagakCreeper ), new ArtInfo( 7, ArtGroup.Miniboss ) );
            m_CreatureInfo.Add( typeof( VitVarg ), new ArtInfo( 7, ArtGroup.Miniboss ) );
            m_CreatureInfo.Add( typeof( TilkiBug ), new ArtInfo( 7, ArtGroup.Miniboss ) );
            m_CreatureInfo.Add( typeof( NelderimDragon ), new ArtInfo( 3, ArtGroup.Miniboss ) );
            m_CreatureInfo.Add( typeof( ShimmeringEffusion ), new ArtInfo( 9, ArtGroup.Miniboss ) );
            m_CreatureInfo.Add( typeof( MonstrousInterredGrizzle ), new ArtInfo( 9, ArtGroup.Miniboss ) );
            m_CreatureInfo.Add( typeof( NSilshashaszals ), new ArtInfo( 5, ArtGroup.Miniboss ) );
            m_CreatureInfo.Add( typeof( SaragAwatar ), new ArtInfo( 2, ArtGroup.Miniboss ) );
            m_CreatureInfo.Add( typeof( WladcaPiaskowBoss ), new ArtInfo( 4, ArtGroup.Miniboss ) );

            //Custom champy
            m_CreatureInfo.Add( typeof( KapitanIIILegionuOrkow ), new ArtInfo( 5, ArtGroup.CustomChamp ) );
            m_CreatureInfo.Add( typeof( MorenaAwatar ), new ArtInfo( 7, ArtGroup.CustomChamp ) );
            m_CreatureInfo.Add( typeof( Meraktus ), new ArtInfo( 10, ArtGroup.CustomChamp ) );
            m_CreatureInfo.Add( typeof( Ilhenir ), new ArtInfo( 10, ArtGroup.CustomChamp ) );
            m_CreatureInfo.Add( typeof( Twaulo ), new ArtInfo( 13, ArtGroup.CustomChamp ) );
            m_CreatureInfo.Add( typeof( Pyre ), new ArtInfo( 10, ArtGroup.CustomChamp ) );

            //Fishing Bossy
            m_CreatureInfo.Add( typeof( Leviathan ), new ArtInfo( 10, ArtGroup.Fishing ) );
        }

        public static double GetChanceFor( BaseCreature creature )
        {
            Type creatureType = creature.GetType();

            if ( m_CreatureInfo.ContainsKey( creatureType ) )
            {
                return m_CreatureInfo[creatureType].PercentChance / 100.0;
            }
            else
            {
                return 0;
            }
        }

        public static ArtGroup GetGroupFor( BaseCreature creature )
        {
            Type creatureType = creature.GetType();

            if ( m_CreatureInfo.ContainsKey( creatureType ) )
                return m_CreatureInfo[creatureType].Group;
            else
                return ArtGroup.None;
        }
    }

    class ArtifactHelper
    {
        #region Lista_artefaktow_Doom
        private static Type[] m_DoomArtifacts = new Type[]
        {
            typeof(Aegis),
            typeof (HolySword),
            typeof ( CrownOfTalKeesh ),
            typeof( ShadowDancerLeggings ),
            typeof( SpiritOfTheTotem ),
            typeof(HatOfTheMagi),
            typeof(LeggingsOfBane),
            typeof(TheTaskmaster),
            typeof(JackalsCollar),
            typeof(ArcaneShield),
            typeof(ArmorOfFortune),
            typeof(TheBeserkersMaul),
            typeof(BladeOfInsanity),
            typeof(BoneCrusher),
            typeof(BreathOfTheDead),
            typeof(AxeOfTheHeavens),
            typeof(BraceletOfHealth),
            typeof(DivineCountenance),
            typeof(TheDragonSlayer),
            typeof(TheDryadBow),
            typeof(Frostbringer),
            typeof(GauntletsOfNobility),
            typeof(HuntersHeaddress),
            typeof(HelmOfInsight),
            typeof(LegacyOfTheDreadLord),
            typeof(RingOfTheElements),
            typeof(MidnightBracers),
            typeof(HolyKnightsBreastplate),
            typeof(OrnateCrownOfTheHarrower),
            typeof(SerpentsFang),
            typeof(VoiceOfTheFallenKing),
            typeof(OrnamentOfTheMagician),
            typeof(StaffOfTheMagi),
            typeof(TunicOfFire),
            };

        #endregion


        #region Lista_artefaktow_Boss
        private static Type[] m_BossArtifacts = new Type[]
        {
            typeof( Aegis ),
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
            typeof ( GauntletsOfAnger ),
            typeof ( KasaOfTheRajin ),
            typeof ( CrownOfTalKeesh ),
            typeof ( CrimsonCincture),
            typeof ( DjinnisRing ),
            typeof ( PendantOfTheMagi ),
            typeof ( PocalunekBoginii ),
        };
        #endregion

        #region Lista_artefaktow_Mini_Boss
        private static Type[] m_MinibossArtifacts = new Type[] {
            typeof(GniewOceanu),
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
            typeof(TheHorselord),
            typeof(DemonForks),
            typeof(Exiler),
            typeof(GlovesOfTheSun),
            typeof(Arteria),
            typeof(OrleSkrzydla),
            typeof(Nasr),
            typeof(WidlyMroku),
            typeof(RoyalGuardSurvivalKnife),
            typeof(ZlamanyGungnir),
            typeof(MelisandesCorrodedHatchet),
            typeof(Bonesmasher),
            typeof(TalonBite),
            typeof(OverseerSunderedBlade),
            typeof(ShardThrasher),
            typeof(KosciLosu),
            typeof(BrambleCoat),
            typeof(GauntletsOfAnger),
            typeof(FeyLeggings),
            typeof(CaptainJohnsHat),
            typeof(PocalunekBoginii),
        };
        #endregion

        #region Lista_artefaktow_Custom_Champ
        private static Type[] m_CustomChampArtifacts = new Type[] {
            typeof(PrzekletaMaskaSmierci),

            typeof(PrzekletaStudniaOdnowy),
            typeof(PrzekleteOrleSkrzydla),
            typeof(PrzekletePogrobowce),
            typeof(PrzekleteWidlyMroku),
            typeof(PrzekletyKilofZRuinTwierdzy),
            typeof(PrzekletyMieczeAmrIbnLuhajj),
            typeof(PrzekletySoulSeeker),
            typeof(PrzekleteSongWovenMantle),
        };
        #endregion

        #region Lista_artefaktow_Fishing
        private static Type[] m_FishingArtifacts = new Type[] {
            typeof(CaptainQuacklebushsCutlass),
            typeof(NightsKiss),
            typeof(StraznikPolnocy),
            typeof(BlazeOfDeath),
            typeof(BowOfTheJukaKing),
            typeof(SpellWovenBritches),
            typeof(LegendaGenerala),
            typeof(BraceletOfHealth),
            typeof(Raikiri),
            typeof(SerpentsFang),
            typeof(OstrzePolksiezyca),
            typeof(ZdradzieckaSzata),
            typeof(OponczaEnergii),
            typeof(OponczaMrozu),
            typeof(OponczaOgnia),
            typeof(OponczaTrucizny),
            typeof(PrzysiegaTriamPergi),
            typeof(SzataHutum),
            typeof(RekawiceBulpa),
            typeof(DreadPirateHat),
            typeof(BlogoslawienstwoBogow),
            typeof(ArkanaZywiolow),
            typeof(DragonNunchaku),
            typeof(PilferedDancerFans),
            typeof(KlatwaMagow),
            typeof(LegendaStraznika),
            typeof(Pogrobowce),
            typeof(WidlyMroku),
            typeof(ZlamanyGungnir),
            typeof(Subdue),
            typeof(MelisandesCorrodedHatchet),
            typeof(RaedsGlory),
            typeof(SoulSeeker ),
            typeof(BlightGrippedLongbow),
            typeof(RuneBeetleCarapace),
            typeof(OponczaMrozu),
            typeof(LeggingsOfEmbers),
            typeof(IronwoodCrown),
            typeof(PasMurdulfaZlotobrodego),
            typeof(PadsOfTheCuSidhe),
            typeof(StudniaOdnowy),
        };
        #endregion

        #region Lista_artefaktow_Kartografia
        private static Type[] m_CartographyArtifacts = new Type[] {
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
            typeof(GauntletsOfAnger),
            typeof(CrownOfTalKeesh),

        };
        #endregion

        public static Type[] DoomArtifacts
        {
            get { return m_DoomArtifacts; }
        }

        public static Type[] BossArtifacts
        {
            get { return m_BossArtifacts; }
        }

        public static Type[] MinibossArtifacts
        {
            get { return m_MinibossArtifacts; }
        }

        public static Type[] FishingBossArtifacts
        {
            get { return m_FishingArtifacts; }
        }

        public static Type[] CartographyArtifacts
        {
            get { return m_CartographyArtifacts; }
        }
        public static Item CreateRandomDoomArtifact()
        {
            int random = Utility.Random( m_DoomArtifacts.Length );
            Type type = m_DoomArtifacts[random];

            return Loot.Construct( type );
        }


        public static Item CreateRandomBossArtifact()
        {
            int random = Utility.Random( m_BossArtifacts.Length );
            Type type = m_BossArtifacts[random];

            return Loot.Construct( type );
        }

        public static Item CreateRandomMinibossArtifact()
        {
            int random = Utility.Random( m_MinibossArtifacts.Length );
            Type type = m_MinibossArtifacts[random];

            return Loot.Construct( type );
        }

        public static Item CreateRandomFishingArtifact()
        {
            int random = Utility.Random( m_FishingArtifacts.Length );
            Type type = m_FishingArtifacts[random];

            return Loot.Construct( type );
        }

        public static Item CreateRandomCartographyArtifact()
        {
            int random = Utility.Random( m_CartographyArtifacts.Length );
            Type type = m_CartographyArtifacts[random];

            return Loot.Construct( type );
        }

        public static Mobile FindRandomPlayer( BaseCreature creature )
        {
            List<DamageStore> rights = creature.GetLootingRights();

            for ( int i = rights.Count - 1; i >= 0; --i )
            {
                DamageStore ds = rights[i];

                if ( !ds.m_HasRight )
                    rights.RemoveAt( i );
            }

            if ( rights.Count > 0 )
                return rights[Utility.Random( rights.Count )].m_Mobile;

            return null;
        }

        public static void ArtifactDistribution( BaseCreature creature )
        {
            if ( creature.Summoned || creature.NoKillAwards )
                return;

            if ( CheckArtifactChance( creature ) )
                DistributeArtifact( creature );
        }

        public static void DistributeArtifact( BaseCreature creature )
        {
            ArtGroup group = ArtifactMonster.GetGroupFor( creature );

            switch ( group )
            {
                case ArtGroup.Doom:
                    DistributeArtifact( creature, CreateRandomDoomArtifact() );
                    break;
                case ArtGroup.Boss:
                    DistributeArtifact( creature, CreateRandomBossArtifact() );
                    break;
                case ArtGroup.Miniboss:
                    DistributeArtifact( creature, CreateRandomMinibossArtifact() );
                    break;
                case ArtGroup.Fishing:
                    DistributeArtifact( creature, CreateRandomFishingArtifact() );
                    break;


                case ArtGroup.None:
                default:
                    break;
            }
        }

        public static void DistributeArtifact( BaseCreature creature, Item artifact )
        {
            DistributeArtifact( FindRandomPlayer( creature ), artifact );
        }

        public static void DistributeArtifact( Mobile to, Item artifact )
        {
            if ( to == null || artifact == null )
                return;

            if ( to.AccessLevel > AccessLevel.Player )
            {
                artifact.ModifiedBy = to.Account.Username;
                artifact.ModifiedDate = DateTime.Now;
            }

            bool message = true;

            Container pack = to.Backpack;

            if ( pack == null || !pack.TryDropItem( to, artifact, false ) )
            {
                if ( to.BankBox != null && to.BankBox.TryDropItem( to, artifact, false ) )
                {
                    to.BankBox.DropItem( artifact );
                    to.SendMessage( "W nagrode za pokonanie bestii otrzymujesz artefakt! Artefakt laduje w banku." );
                    to.PlaySound( 0x1F7 );
                    to.FixedParticles( 0x373A, 1, 15, 9913, 67, 7, EffectLayer.Head );
                }
                else
                {
                    to.SendLocalizedMessage( 1072523 ); // Otrzymujesz artefakt, lecz nie masz miejsca w plecaku ani banku. Artefakt upada na ziemie!
                    to.Emote( "Postac zdobyla artefakt, ktory upadl na ziemie!" );
                    to.PlaySound( 0x1F7 );
                    to.FixedParticles( 0x373A, 1, 15, 9913, 67, 7, EffectLayer.Head );
                    message = false;

                    artifact.MoveToWorld( to.Location, to.Map );
                }
            }

            if ( message )
                to.SendLocalizedMessage( 1062317 ); // W nagrode za pokonanie bestii otrzymujesz artefakt!
            to.Emote( "Postac zdobyla artefakt!" );
            to.PlaySound( 0x1F7 );
            to.FixedParticles( 0x373A, 1, 15, 9913, 67, 7, EffectLayer.Head );
            int itemID2 = 0xF5F;
            IEntity from = new Entity( Serial.Zero, new Point3D( to.X, to.Y, to.Z ), to.Map );
            IEntity too = new Entity( Serial.Zero, new Point3D( to.X, to.Y, to.Z + 50 ), to.Map );
            Effects.SendMovingParticles( from, too, itemID2, 1, 0, false, false, 33, 3, 9501, 1, 0, EffectLayer.Head, 0x100 );
        }

        public static double GetArtifactChance( BaseCreature boss )
        {
            double luck = LootPack.GetLuckChanceForKiller( boss );

            if ( luck > 1200 )
                luck = 1200;

            double chance = ArtifactMonster.GetChanceFor( boss );
            chance *= 1.0 + 0.25 * (luck / 1200); // luck zwieksza szanse maksymalnie do 125% 

            return chance;
        }

        public static bool CheckArtifactChance( BaseCreature boss )
        {
            return GetArtifactChance( boss ) > Utility.RandomDouble();
        }

    }
}
