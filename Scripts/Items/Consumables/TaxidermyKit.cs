using Server.Mobiles;
using Server.Multis;
using Server.Targeting;
using System;

namespace Server.Items
{
    [Flipable(0x1EBA, 0x1EBB)]
    public class TaxidermyKit : Item
    {
        public override int LabelNumber => 1041279;  // a taxidermy kit

        [Constructable]
        public TaxidermyKit() : base(0x1EBA)
        {
            Weight = 1.0;
        }

        public TaxidermyKit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else if (from.Skills[SkillName.Carpentry].Base < 90.0)
            {
                from.SendLocalizedMessage(1042594); // You do not understand how to use this.
            }
            else
            {
                from.SendLocalizedMessage(1042595); // Target the corpse to make a trophy out of.
                from.Target = new CorpseTarget(this);
            }
        }

        public static TrophyInfo[] TrophyInfos => m_Table;
        private static readonly TrophyInfo[] m_Table = new TrophyInfo[]
        {
            new TrophyInfo( typeof( BrownBear ),      0x1E60,       1041093, 1041107 ),
            new TrophyInfo( typeof( GreatHart ),      0x1E61,       1041095, 1041109 ),
            new TrophyInfo( typeof( BigFish ),        0x1E62,       1041096, 1041110 ),
            new TrophyInfo( typeof( Gorilla ),        0x1E63,       1041091, 1041105 ),
            new TrophyInfo( typeof( Orc ),            0x1E64,       1041090, 1041104 ),
            new TrophyInfo( typeof( PolarBear ),      0x1E65,       1041094, 1041108 ),
            new TrophyInfo( typeof( Troll ),          0x1E66,       1041092, 1041106 ),
            new TrophyInfo( typeof( RedHerring ),     0x1E62,       1113567, 1113569 ),
            new TrophyInfo( typeof( MudPuppy ),       0x1E62,       1113568, 1113570 ),

            new TrophyInfo( typeof( AutumnDragonfish),     0,       1116124, 1116185 ),
            new TrophyInfo( typeof( BullFish ),            1,       1116129, 1116190 ),
            new TrophyInfo( typeof( FireFish ),            2,       1116127, 1116188 ),
            new TrophyInfo( typeof( GiantKoi ),            3,       1116122, 1116183 ),
            new TrophyInfo( typeof( LavaFish ),            4,       1116130, 1116191 ),
            new TrophyInfo( typeof( SummerDragonfish ),    5,       1116124, 1116186 ),
            new TrophyInfo( typeof( UnicornFish ),         6,       1116120, 1116181 ),
            new TrophyInfo( typeof( AbyssalDragonfish ),   7,       1116140, 1116201 ),
            new TrophyInfo( typeof( BlackMarlin ),         8,       1116133, 1116194 ),
            new TrophyInfo( typeof( BlueMarlin ),          9,       1116131, 1116192 ),
            new TrophyInfo( typeof( GiantSamuraiFish ),    10,      1116138, 1116199 ),
            new TrophyInfo( typeof( Kingfish ),            11,      1116119, 1116180 ),
            new TrophyInfo( typeof( LanternFish ),         12,      1116142, 1116203 ),
            new TrophyInfo( typeof( SeekerFish ),          13,      1116145, 1116206 ),
            new TrophyInfo( typeof( SpringDragonfish ),    14,      1116139, 1116200 ),
            new TrophyInfo( typeof( StoneFish),            15,      1116135, 1116196 ),
            new TrophyInfo( typeof( WinterDragonfish),     16,      1116141, 1116202 ),
            new TrophyInfo( typeof( BlueLobster),          17,      1149812, 1149804 ),
            new TrophyInfo( typeof( BloodLobster),         18,      1149816, 1149808 ),
            new TrophyInfo( typeof( DreadLobster),         19,      1149817, 1149809 ),
            new TrophyInfo( typeof( VoidLobster),          20,      1149815, 1149807 ),
            new TrophyInfo( typeof( StoneCrab),            21,      1149811, 1149803 ),
            new TrophyInfo( typeof( SpiderCrab),           22,      1149813, 1149805 ),
            new TrophyInfo( typeof( TunnelCrab),           23,      1149818, 1149810 ),
            new TrophyInfo( typeof( VoidCrab ),            24,      1149814, 1149806 ),

            new TrophyInfo( typeof( CrystalFish ),         25,      1116126, 1116187 ),
            new TrophyInfo( typeof( FairySalmon ),         26,      1116123, 1116184 ),
            new TrophyInfo( typeof( GreatBarracuda ),      27,      1116134, 1116195 ),
            new TrophyInfo( typeof( HolyMackerel ),        28,      1116121, 1116182 ),
            new TrophyInfo( typeof( ReaperFish ),          29,      1116128, 1116189 ),
            new TrophyInfo( typeof( YellowtailBarracuda ), 30,      1116132, 1116193 ),
            new TrophyInfo( typeof( DungeonPike ),         31,      1116143, 1116204 ),
            new TrophyInfo( typeof( GoldenTuna ),          32,      1116137, 1116198 ),
            new TrophyInfo( typeof( RainbowFish ),         33,      1116144, 1116205 ),
            new TrophyInfo( typeof( ZombieFish ),          34,      1116136, 1116197 ),
            
            // Nelderimowe
            
            			new TrophyInfo( typeof( GreyWolf ),		0x20EA,	-1, 0x03E5 ),	// (szary)
			new TrophyInfo( typeof( TimberWolf ),	0x20EA,	-1, 0     ),	// (lesny)
			new TrophyInfo( typeof( DireWolf ),		0x20EA,	-1, 0x0455 ),	// (wsciekly)
			new TrophyInfo( typeof( WhiteWolf ),	0x20EA,	-1, 0x0385 ),	// (bialy)
			new TrophyInfo( typeof( GreatHart ),	0x1E68,	0x1E61, 0 ),
			//new TrophyInfo( typeof( Pig ),			0x1E8F,	0x1E8E, 0 ),	// za nisko sciany wisi
			new TrophyInfo( typeof( Hind ),			0x20D4,	-1, -99 ),
			new TrophyInfo( typeof( Gorilla ),		0x20C9,	-1, -99 ),
			new TrophyInfo( typeof( GiantRat ),		0x20D0,	-1, -99 ),
			new TrophyInfo( typeof( Rabbit ),		0x2125,	-1,	-99 ),
			new TrophyInfo( typeof( Dog ),			0x20D5,	-1, -99 ),
			new TrophyInfo( typeof( Bird ),			0x211A,	-1, -99 ),
			new TrophyInfo( typeof( Bird ),			0x20EE,	-1, -99 ),
			new TrophyInfo( typeof( Eagle ),		0x211D,	-1, -99 ),
			new TrophyInfo( typeof( Eagle ),		0x20F2,	-1, -99 ),
			new TrophyInfo( typeof( Walrus ),		0x20FF,	-1, -99 ),
			new TrophyInfo( typeof( Wisp ),			0x2100,	-1, -99 ),
			new TrophyInfo( typeof( Panther ),		0x2102,	-1, 0x901 ), 	// 0x2119-mniejsza (big cat,kot)
			new TrophyInfo( typeof( Cougar ),		0x2102,	-1, -99 ),
			new TrophyInfo( typeof( Cat ),			0x211B,	-1, -99 ),
			new TrophyInfo( typeof( Gazer ),		0x20F4, -1, -99 ),
			new TrophyInfo( typeof( ElderGazer ),	0x20F4,	-1, -99 ),
			new TrophyInfo( typeof( Snake ),		0x20FE,	-1, -99 ),
			new TrophyInfo( typeof( Crane ),		0x2764,	-1, -99 ),
			new TrophyInfo( typeof( SummonedDaemon ),0x2104, -1, -99 ),			
			new TrophyInfo( typeof( Dragon ),		0x2235,	0x2234, -99 ),
			new TrophyInfo( typeof( Pixie ),		0x2A72,	0x2A71, 0 ),
			new TrophyInfo( typeof( Pixie ),		0x2A74,	0x2A73, 0 ),
			new TrophyInfo( typeof( Pixie ),		0x2A76,	0x2A75, 0 ),
			new TrophyInfo( typeof( Pixie ),		0x2A78,	0x2A77, 0 ),
			new TrophyInfo( typeof( Pixie ),		0x2A7A,	0x2A79, 0 ),
			new TrophyInfo( typeof( Pixie ),		0x2D8A,	-1, -99 ),
			new TrophyInfo( typeof( Unicorn ),		0x3158,	0x3159, 0 ),
			new TrophyInfo( typeof( Lizardman ),	0x20CA,	-1, 0 )
			
			/*
			//new TrophyInfo( typeof( PolarBear ),		0x20E1,	-1, 0 ),	
			//new TrophyInfo( typeof( GrizzlyBear ),	0x20DB,	-1, 0 ), // 0x20DB-za maly 0x211E-za maly
			//new TrophyInfo( typeof( Rat ),			0x2123,	-1, 0 ),	// troche za maly
			//new TrophyInfo( typeof( Sheep ),			0x20EB,	-1, 0 ),	// lipne:0x20EB
			//new TrophyInfo( typeof( Alligator ),		0x20DA,	-1, 0 ),	// za maly
			//new TrophyInfo( typeof( Chicken ),		0x20D1,	-1, 0 ),	// troche za duzy
			//new TrophyInfo( typeof( Bull ),			0x20EF,	-1, 0 ),	// ZaMale: 0x20EF-nielaciaty 0x20F0-lacaity						
			//new TrophyInfo( typeof( Dolphin ),		0x20F1,	-1, 0 ),	// dziwny
			//new TrophyInfo( typeof( Pig ),			0x2101,	-1, 0 ),	// wieksza	
			//new TrophyInfo( typeof( Cow ),			0x2103,	-1, 0 ),	// za mala
			//new TrophyInfo( typeof( MountainGoat ),	0x2108,	-1, 0 ),	// zaMala
			//new TrophyInfo( typeof( Goat ),			0x2580,	-1, 0 ),	// ladna, za zala, niepodobna	
			
			//new TrophyInfo( typeof( Llama ),			0x20F6,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Horse ),			0x211F,	-1, 0 ),	// za male: 0x2120-jasny  0x2121-ciemny  0x211F-bialy  0x2124-jasnyLepszy?
			//new TrophyInfo( typeof( Ridgeback ),		0x2615,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( PackHorse ),		0x2126,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( PackLlama ),		0x2127,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( DesertOstard ),	0x2135,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( FrenziedOstard ),	0x2136,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( ForestOstard ),	0x2137,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( SilverSteed ),	0x259D,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( FireSteed ),		0x21F1,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Beetle ),			0x260F,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Nightmare ),		0x259C,	-1, 0 ),	// za male
		
			//new TrophyInfo( typeof( Cyclops ),	0x212D,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Titan ),		0x25CD,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Troll ),		0x20E9,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( TrollLord ),	0x1E63,	0x1E6A, 0 ),	// za male
			//new TrophyInfo( typeof( Ettin ),		0x20C8,	-1, 0 ),	// za male		
			//new TrophyInfo( typeof( Ogre ),		0x20DF,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( OgreLord ),	0x20CB,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Orc ),		0x20E0,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( OrcCaptain ),	0x25AF,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( OrcishLord ),	0x25B0,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( OrcishMage ),	0x25B1,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Ratman ),		0x20E3,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( RatmanArcher ),	0x20E3,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( RatmanMage ),	0x20E3,	-1, 0 ),	// za male			
			//new TrophyInfo( typeof( Harpy ),		0x20DC,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( StoneHarpy ),	0x2594,	-1, 0 ),	// za male
			
			//new TrophyInfo( typeof( Scorpion ),	0x20E4,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Mongbat ),	0x20F9,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Reaper ),		0x20FA,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( SeaSerpent ),	0x20FB,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( GiantSerpent ),	0x20FC,	-1, 0 ),	// za male		
		
			//new TrophyInfo( typeof( GiantSpider ),		0x20FD,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( GiantBlackWidow ),	0x25C3,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( DreadSpider ),		0x25C4,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( FrostSpider ),		0x25C5,	-1, 0 ),	// za male			
		
			//new TrophyInfo( typeof( TerathanDrone ),		0x212B,	-1, 0 ),	// za male // 0x25C9-inny
			//new TrophyInfo( typeof( TerathanWarrior ),	0x212A,	-1, 0 ),	// za male // 0x25CC-inny		
			//new TrophyInfo( typeof( TerathanMatriarch ),	0x212C,	-1, 0 ),	// za male // 0x25CB-queen
			//new TrophyInfo( typeof( TerathanAvenger ),	0x25CA,	-1, 0 ),	// za male
			
			//new TrophyInfo( typeof( OphidianMage ),			0x2132,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( OphidianArchmage ),		0x25AC,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( OphidianWarrior ),		0x2133,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( OphidianKnight ),		0x25AA,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( OphidianMatriarch ),	0x2134,	-1, 0 ),	// za male // 0x25AC-bok
			
			//new TrophyInfo( typeof( Gorgoyle ),			0x20D9,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( GargoyleDestroyer ),	0x258D,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( StoneGargoyle ),		0x258E,	-1, 0 ),	// za male
			
			//new TrophyInfo( typeof( Zombie ),			0x20EC,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Lich ),			0x20F8,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( LichLord ),		0x25A5,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Ghoul ),			0x2109,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( HeadlessOne ),	0x210A,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Mummy ),			0x25A7,	-1, 0 ),	// za male
			// //new TrophyInfo( typeof( Skeleton ),	0x20E7,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Skeleton ),		0x25BC,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( SkeletalKnight ),	0x25BD,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( SkeletalMage ),	0x25BE,	-1, 0 ),	// za male			
			
			//new TrophyInfo( typeof( Slime ),		0x20E8,	-1, 0 ),	// za duze
			//new TrophyInfo( typeof( GiantToad ),	0x212F,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( BullFrog ),	0x2130,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( LavaLizard ),	0x2131,	-1, 0 ),	// za male
			
			//new TrophyInfo( typeof( Wyvern ),		0x25D4,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Dragon ),		0x20D6,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Kraken ),		0x25A2,	-1, 0 ),	// za male			
			
			//new TrophyInfo( typeof( RedSolenQueen ),	0x2602,	-1, 0 ),	// za male //red
			//new TrophyInfo( typeof( RedSolenWarrior ),	0x2603,	-1, 0 ),	// za male //red
			//new TrophyInfo( typeof( RedSolenWorker ),	0x2604,	-1, 0 ),	// za male //red			
			
			//new TrophyInfo( typeof( bagienny potwor ),	0x2608,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Quagmire ),		0x2614,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Corpser ),			0x20D2,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( WhippingVine ),		0x20D2,	-1, 0 ),	// za male			

			//new TrophyInfo( typeof( FireDaemon ),		0x20D3,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( ChaosDemon ),		0x2609,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( ArcaneDaemon ),	0x2605,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( HordeDaemon ),	0x260E,	-1, 0 ),	// za male //niebieski czteroreki
			//new TrophyInfo( typeof( HordeMinion ),	0x2611,	-1, 0 ),	// za male		
			
			//new TrophyInfo( typeof( EvilMage ),			0x258A,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( MeerMage ),			0x261C,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( MeerWarrior ),		0x261D,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( JukaMage ),			0x261E,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( JukaWarrior ),		0x261F,	-1, 0 ),	// za male			
			
			//new TrophyInfo( typeof( YumotsuElder ),	0x2772,	-1, 0 ),	// za male	
			//new TrophyInfo( typeof( YumotsuPriest ),	0x2773,	-1, 0 ),	// za male	
			
			//new TrophyInfo( typeof( AgapiteElemental ),		0x20D7,	-1, 0x1B9 ),	// za male	
			//new TrophyInfo( typeof( BronzeElemental ),		0x20D7,	-1, 0x156 ),	// za male	
			//new TrophyInfo( typeof( CopperElemental ),		0x20D7,	-1, 0xF9 ),	// za male	
			//new TrophyInfo( typeof( DullCopperElemental ),	0x20D7,	-1, 0x3A7 ),	// za male	
			//new TrophyInfo( typeof( GoldenElemental ),		0x20D7,	-1, 0x35 ),	// za male	
			//new TrophyInfo( typeof( ShadowIronElemental ),	0x20D7,	-1, 0x395 ),	// za male	
			//new TrophyInfo( typeof( ValoriteElemental ),	0x20D7,	-1, 0x5 ),	// za male	
			//new TrophyInfo( typeof( VeriteElemental ),		0x20D7,	-1, 0x41 ),	// za male	
			//new TrophyInfo( typeof( EarthElemental ),	0x20D7,	-1, 0 ),	// za male	
			//new TrophyInfo( typeof( AirElemental ),		0x20ED,	-1, 0 ),	// za male	
			//new TrophyInfo( typeof( FireElemental ),	0x20F3,	-1, 0 ),	// za male	
			//new TrophyInfo( typeof( WaterElemental ),	0x210B,	-1, 0 ),	// za male	
			//new TrophyInfo( typeof( CrystalElemental ),	0x2620,	-1, 0 ),	// za male	
			
			//new TrophyInfo( typeof( Golem ),			0x2610,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( FleshGolem ),		0x2624,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( GoreFiend ),		0x2625,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Imp ),			0x259F,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Gibberling ),		0x2627,	-1, 0 ),	// za male
			//new TrophyInfo( typeof(  devourer  ),		0x2623,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Doppleganger ),	0x260D,	-1, 0 ),	// za male
			
			//new TrophyInfo( typeof( TreeFellow ),		0x2621,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( RaiJu ),			0x2766,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( SnowLady ),		0x276C,	-1, 0 ), 	// za male
			//new TrophyInfo( typeof( Centaur ),			0x2581,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( EtherealWarrior ),	0x2589,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Pixie ),			0x25B6,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Kirin ),			0x25A0,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Unicorn ),			0x25CE,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( BakeKitsune ),		0x2763,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Gaman ),			0x2768,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Hiryu ),			0x276A,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Kappa ),			0x276B,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Oni ),				0x276D,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( RevenantLion ),		0x276E,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( RuneBeetle ),		0x276F,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( TsukiWolf ),		0x2770,	-1, 0 ),	// za male
			//new TrophyInfo( typeof( Yamandon ),			0x2771,	-1, 0 )	// za male
			*/
        };

        public class TrophyInfo
        {
            public TrophyInfo(Type type, int id, int deedNum, int addonNum)
            {
                m_CreatureType = type;
                m_NorthID = id;
                m_DeedNumber = deedNum;
                m_AddonNumber = addonNum;
            }

            private readonly Type m_CreatureType;
            private readonly int m_NorthID;
            private readonly int m_DeedNumber;
            private readonly int m_AddonNumber;

            public Type CreatureType => m_CreatureType;
            public int NorthID => m_NorthID;
            public int DeedNumber => m_DeedNumber;
            public int AddonNumber => m_AddonNumber;
        }

        private class CorpseTarget : Target
        {
            private readonly TaxidermyKit m_Kit;

            public CorpseTarget(TaxidermyKit kit) : base(3, false, TargetFlags.None)
            {
                m_Kit = kit;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Kit.Deleted)
                    return;

                if (!(targeted is Corpse) && !(targeted is BigFish) && !(targeted is BaseHighseasFish) && !(targeted is HuntingPermit))
                {
                    from.SendLocalizedMessage(1042600); // That is not a corpse!
                }
                else if (targeted is Corpse && ((Corpse)targeted).VisitedByTaxidermist)
                {
                    from.SendLocalizedMessage(1042596); // That corpse seems to have been visited by a taxidermist already.
                }
                else if (!m_Kit.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                }
                else if (from.Skills[SkillName.Carpentry].Base < 90.0)
                {
                    from.SendLocalizedMessage(1042603); // You would not understand how to use the kit.
                }
                #region Huntmasters Challenge
                else if (targeted is HuntingPermit)
                {
                    HuntingPermit lic = targeted as HuntingPermit;

                    if (from.Backpack == null || !lic.IsChildOf(from.Backpack))
                        from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    else if (!lic.CanUseTaxidermyOn)
                    {
                        //TODO: Message?
                    }
                    else if (from.Backpack != null && from.Backpack.ConsumeTotal(typeof(Board), 10))
                    {
                        int index = lic.KillEntry.KillIndex;

                        if (index >= 0 && index < Engines.HuntsmasterChallenge.HuntingTrophyInfo.Infos.Count)
                        {
                            Engines.HuntsmasterChallenge.HuntingTrophyInfo info = Engines.HuntsmasterChallenge.HuntingTrophyInfo.Infos[index];

                            if (info != null)
                            {
                                string name = lic.KillEntry.Owner != null ? lic.KillEntry.Owner.Name : from.Name;

                                if (info.Complex)
                                    from.AddToBackpack(new HuntTrophyAddonDeed(name, index, lic.KillEntry.Measurement, lic.KillEntry.DateKilled.ToShortDateString(), lic.KillEntry.Location));
                                else
                                    from.AddToBackpack(new HuntTrophy(name, index, lic.KillEntry.Measurement, lic.KillEntry.DateKilled.ToShortDateString(), lic.KillEntry.Location));

                                lic.ProducedTrophy = true;
                                m_Kit.Delete();
                            }
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(1042598); // You do not have enough boards.
                        return;
                    }
                }
                #endregion
                else
                {
                    object obj = targeted;

                    if (obj is Corpse)
                        obj = ((Corpse)obj).Owner;

                    if (obj != null)
                    {
                        for (int i = 0; i < m_Table.Length; i++)
                        {
                            if (m_Table[i].CreatureType == obj.GetType())
                            {
                                Container pack = from.Backpack;

                                if (pack != null && pack.ConsumeTotal(typeof(Board), 10))
                                {
                                    from.SendLocalizedMessage(1042278); // You review the corpse and find it worthy of a trophy.
                                    from.SendLocalizedMessage(1042602); // You use your kit up making the trophy.

                                    Mobile hunter = null;
                                    int weight = 0;
                                    DateTime dateCaught = DateTime.MinValue;

                                    if (targeted is BigFish)
                                    {
                                        BigFish fish = targeted as BigFish;

                                        hunter = fish.Fisher;
                                        weight = (int)fish.Weight;
                                        dateCaught = fish.DateCaught;

                                        fish.Consume();
                                    }
                                    #region High Seas
                                    else if (targeted is RareFish)
                                    {
                                        RareFish fish = targeted as RareFish;

                                        hunter = fish.Fisher;
                                        weight = (int)fish.Weight;
                                        dateCaught = fish.DateCaught;

                                        from.AddToBackpack(new FishTrophyDeed(weight, hunter, dateCaught, m_Table[i].DeedNumber, m_Table[i].AddonNumber, m_Table[i].NorthID));

                                        fish.Delete();
                                        m_Kit.Delete();
                                        return;
                                    }

                                    else if (targeted is RareCrabAndLobster)
                                    {
                                        RareCrabAndLobster fish = targeted as RareCrabAndLobster;

                                        hunter = fish.Fisher;
                                        weight = (int)fish.Weight;
                                        dateCaught = fish.DateCaught;

                                        from.AddToBackpack(new FishTrophyDeed(weight, hunter, dateCaught, m_Table[i].DeedNumber, m_Table[i].AddonNumber, m_Table[i].NorthID));

                                        fish.Delete();
                                        m_Kit.Delete();
                                        return;
                                    }
                                    #endregion
                                    TrophyDeed deed = new TrophyDeed(m_Table[i], hunter, weight);

                                    if (dateCaught != DateTime.MinValue)
                                    {
                                        deed.DateCaught = dateCaught;
                                    }

                                    from.AddToBackpack(new TrophyDeed(m_Table[i], hunter, weight));

                                    if (targeted is Corpse)
                                        ((Corpse)targeted).VisitedByTaxidermist = true;

                                    m_Kit.Delete();
                                    return;
                                }
                                else
                                {
                                    from.SendLocalizedMessage(1042598); // You do not have enough boards.
                                    return;
                                }
                            }
                        }
                    }

                    from.SendLocalizedMessage(1042599); // That does not look like something you want hanging on a wall.
                }
            }
        }
    }

    public partial class TrophyAddon : Item, IAddon
    {
        public override bool ForceShowProperties => true;

        private int m_WestID;
        private int m_NorthID;
        private int m_DeedNumber;
        private int m_AddonNumber;

        private Mobile m_Hunter;
        private int m_AnimalWeight;

        [CommandProperty(AccessLevel.GameMaster)]
        public int WestID { get { return m_WestID; } set { m_WestID = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int NorthID { get { return m_NorthID; } set { m_NorthID = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DeedNumber { get { return m_DeedNumber; } set { m_DeedNumber = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AddonNumber { get { return m_AddonNumber; } set { m_AddonNumber = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Hunter { get { return m_Hunter; } set { m_Hunter = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AnimalWeight { get { return m_AnimalWeight; } set { m_AnimalWeight = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime DateCaught { get; set; }

        public override int LabelNumber => m_AddonNumber;

        [Constructable]
        public TrophyAddon(Mobile from, int itemID, int westID, int northID, int deedNumber, int addonNumber) : this(from, itemID, westID, northID, deedNumber, addonNumber, null, 0, DateTime.MinValue)
        {
        }

        public TrophyAddon(Mobile from, int itemID, int westID, int northID, int deedNumber, int addonNumber, Mobile hunter, int animalWeight, DateTime dateCaught) : base(itemID)
        {
            m_WestID = westID;
            m_NorthID = northID;
            m_DeedNumber = deedNumber;
            m_AddonNumber = addonNumber;

            m_Hunter = hunter;
            m_AnimalWeight = animalWeight;
            DateCaught = dateCaught;

            Movable = false;

            MoveToWorld(from.Location, from.Map);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_AnimalWeight >= 20)
            {
                if (m_Hunter != null)
                    list.Add(1070857, m_Hunter.Name); // Caught by ~1_fisherman~

                list.Add(1070858, m_AnimalWeight.ToString()); // ~1_weight~ stones
            }

            if (DateCaught != DateTime.MinValue)
            {
                list.Add(string.Format("[{0}]", DateCaught.ToShortDateString()));
            }
        }

        public TrophyAddon(Serial serial) : base(serial)
        {
        }

        public bool CouldFit(IPoint3D p, Map map)
        {
            if (!map.CanFit(p.X, p.Y, p.Z, ItemData.Height))
                return false;

            if (ItemID == m_NorthID)
                return BaseAddon.IsWall(p.X, p.Y - 1, p.Z, map); // North wall
            else
                return BaseAddon.IsWall(p.X - 1, p.Y, p.Z, map); // West wall
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(2); // version

            writer.Write(DateCaught);

            writer.Write(m_Hunter);
            writer.Write(m_AnimalWeight);

            writer.Write(m_WestID);
            writer.Write(m_NorthID);
            writer.Write(m_DeedNumber);
            writer.Write(m_AddonNumber);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        DateCaught = reader.ReadDateTime();
                        goto case 1;
                    }
                case 1:
                    {
                        m_Hunter = reader.ReadMobile();
                        m_AnimalWeight = reader.ReadInt();
                        goto case 0;
                    }
                case 0:
                    {
                        m_WestID = reader.ReadInt();
                        m_NorthID = reader.ReadInt();
                        m_DeedNumber = reader.ReadInt();
                        m_AddonNumber = reader.ReadInt();
                        break;
                    }
            }

            Timer.DelayCall(TimeSpan.Zero, FixMovingCrate);
        }

        private void FixMovingCrate()
        {
            if (Deleted)
                return;

            if (Movable || IsLockedDown)
            {
                Item deed = Deed;

                if (Parent is Item)
                {
                    ((Item)Parent).AddItem(deed);
                    deed.Location = Location;
                }
                else
                {
                    deed.MoveToWorld(Location, Map);
                }

                Delete();
            }
        }

        public Item Deed => new TrophyDeed(m_WestID, m_NorthID, m_DeedNumber, m_AddonNumber, m_Hunter, m_AnimalWeight, DateCaught);

        void IChopable.OnChop(Mobile user)
        {
            OnDoubleClick(user);
        }

        public override void OnDoubleClick(Mobile from)
        {
            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house != null && house.IsCoOwner(from))
            {
                if (from.InRange(GetWorldLocation(), 1))
                {
                    from.AddToBackpack(Deed);
                    Delete();
                }
                else
                {
                    from.SendLocalizedMessage(500295); // You are too far away to do that.
                }
            }
        }
    }

    [Flipable(0x14F0, 0x14EF)]
    public partial class TrophyDeed : Item
    {
        private int m_WestID;
        private int m_NorthID;
        private int m_DeedNumber;
        private int m_AddonNumber;

        private Mobile m_Hunter;
        private int m_AnimalWeight;

        [CommandProperty(AccessLevel.GameMaster)]
        public int WestID { get { return m_WestID; } set { m_WestID = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int NorthID { get { return m_NorthID; } set { m_NorthID = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DeedNumber { get { return m_DeedNumber; } set { m_DeedNumber = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AddonNumber { get { return m_AddonNumber; } set { m_AddonNumber = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Hunter { get { return m_Hunter; } set { m_Hunter = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AnimalWeight { get { return m_AnimalWeight; } set { m_AnimalWeight = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime DateCaught { get; set; }

        public override int LabelNumber => m_DeedNumber;

        [Constructable]
        public TrophyDeed(int westID, int northID, int deedNumber, int addonNumber)
            : this(westID, northID, deedNumber, addonNumber, null, 0, DateTime.MinValue)
        {
        }

        public TrophyDeed(int westID, int northID, int deedNumber, int addonNumber, Mobile hunter, int animalWeight, DateTime dateCaught) : base(0x14F0)
        {
            m_WestID = westID;
            m_NorthID = northID;
            m_DeedNumber = deedNumber;
            m_AddonNumber = addonNumber;
            m_Hunter = hunter;
            m_AnimalWeight = animalWeight;
            DateCaught = dateCaught;
        }

        public TrophyDeed(TaxidermyKit.TrophyInfo info, Mobile hunter, int animalWeight)
            : this(info.NorthID + 7, info.NorthID, info.DeedNumber, info.AddonNumber, hunter, animalWeight, DateTime.MinValue)
        {
        }

        public TrophyDeed(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_AnimalWeight >= 20)
            {
                if (m_Hunter != null)
                    list.Add(1070857, m_Hunter.Name); // Caught by ~1_fisherman~

                list.Add(1070858, m_AnimalWeight.ToString()); // ~1_weight~ stones
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(2); // version

            writer.Write(DateCaught);

            writer.Write(m_Hunter);
            writer.Write(m_AnimalWeight);

            writer.Write(m_WestID);
            writer.Write(m_NorthID);
            writer.Write(m_DeedNumber);
            writer.Write(m_AddonNumber);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
				case 3: //TODO remove case3 after migration
                case 2:
                    {
                        DateCaught = reader.ReadDateTime();
                        goto case 1;
                    }
                case 1:
                    {
                        m_Hunter = reader.ReadMobile();
                        m_AnimalWeight = reader.ReadInt();
                        goto case 0;
                    }
                case 0:
                    {
                        m_WestID = reader.ReadInt();
                        m_NorthID = reader.ReadInt();
                        m_DeedNumber = reader.ReadInt();
                        m_AddonNumber = reader.ReadInt();
                        break;
                    }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                BaseHouse house = BaseHouse.FindHouseAt(from);

                if (house != null && house.IsCoOwner(from))
                {
                    bool northWall = BaseAddon.IsWall(from.X, from.Y - 1, from.Z, from.Map);
                    bool westWall = BaseAddon.IsWall(from.X - 1, from.Y, from.Z, from.Map);

                    if (northWall && westWall)
                    {
                        switch (from.Direction & Direction.Mask)
                        {
                            case Direction.North:
                            case Direction.South: northWall = true; westWall = false; break;

                            case Direction.East:
                            case Direction.West: northWall = false; westWall = true; break;

                            default: from.SendMessage("Turn to face the wall on which to hang this trophy."); return;
                        }
                    }

                    int itemID = 0;

                    if (northWall)
                        itemID = m_NorthID;
                    else if (westWall)
                        itemID = m_WestID;
                    else
                        from.SendLocalizedMessage(1042626); // The trophy must be placed next to a wall.

                    if (itemID > 0)
                    {
                        Item trophy = new TrophyAddon(from, itemID, m_WestID, m_NorthID, m_DeedNumber, m_AddonNumber, m_Hunter, m_AnimalWeight, DateCaught);

                        if (m_DeedNumber == 1113567)
                            trophy.Hue = 1645;
                        else if (m_DeedNumber == 1113568)
                            trophy.Hue = 1032;

                        house.Addons[trophy] = from;
                        Delete();
                    }
                }
                else
                {
                    from.SendLocalizedMessage(502092); // You must be in your house to do this.
                }
            }
            else
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        }
    }
}
