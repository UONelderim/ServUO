using System;
using Server.Items;

namespace Server.Mobiles
{
    public class XmlQuestNPCGornik : TalkingBaseCreature
    {

        [Constructable]
        public XmlQuestNPCGornik() : this( -1 )
        {
        }
        private DateTime m_Spoken;
        public override void OnMovement( Mobile m, Point3D oldLocation )
        {
            if ( m.Alive && m is PlayerMobile )
            {
                PlayerMobile pm = (PlayerMobile)m;

                int range = 2;

                if ( Utility.RandomDouble() < 0.20 )
                {
                    if ( range >= 0 && InRange( m, range ) && !InRange( oldLocation, range ) && DateTime.Now >= m_Spoken + TimeSpan.FromSeconds( 10 ) )
                    {
                        if ( Race == Race.NTamael )
                        {
                            switch ( Utility.Random( 19 ) )
                            {
                                case 0: Emote( "*Ociera pot z czoła*" ); break;
                                case 1: Emote( "*Przeciera ubrudzoną twarz*" ); break;
                                case 2: Emote( "*Klnie pod nosem*" ); break;
                                case 3: Say( "Kurwa... Tyle roboty" ); Emote( "*Wzdycha ciężko*" ); break;
                                case 4: Emote( "*Odkłada kilof na ziemię po czym wciąga coś do nosa*" ); Say( "*O matulo...*" ); Emote( "*Pociera nos krzywiąc się*" ); break;
                                case 5: Say( "Jak wrócę do domu to zjem konia z kopytami" ); break;
                                case 6: Say( "Namacham sie kilofem a zarobek z tego marny..." ); break;
                                case 7: Say( "Chyba mi coś łupnęło w krzyżu..." ); Emote( "*Łapie sie za plecy*" ); break;
                                case 8: Say( "Ostatnio ponoć zawalił sie strop na paru górników..." ); break;
                                case 9: Say( "Dobra... Pora na przerwę" ); break;
                                case 10: Say( "Praca, praca..." ); break;
                                case 11: Say( "Siedziała na rynku, sprzedawała buty, jak nie miała reszty to dawała dupy!" ); Emote( "*Przyśpiewuje podczas pracy*" ); break;
                                case 12: Say( "Ciemno jak w dupie..." ); break;
                                case 13: Say( "Co to za dziewucha w tej różowej halce, chciałem ją posmyrać, obszczała mi palce! " ); Emote( "*Przyśpiewuje podczas pracy*" ); break;
                                case 14: Emote( "*Zacharczał wypluwając gęstą wydzieline*" ); break;
                                case 15: Say( "Kto to widział kurwa..." ); break;
                                case 16: Emote( "*Spluwa na ziemię*" ); break;
                                case 17: Say( "Potężne złoże tu wyczuwam..." ); break;
                                case 18: Say( "Kiedyś się dorobię i sam sobie pałac zbuduje... kiedyś" ); break;
                            }
                        }
                        else if ( Race == Race.NJarling )
                        {
                            switch ( Utility.Random( 19 ) )
                            {
                                case 0: Emote( "*Ociera pot z czoła*" ); break;
                                case 1: Emote( "*Przeciera ubrudzoną twarz*" ); break;
                                case 2: Emote( "*Klnie pod nosem*" ); break;
                                case 3: Say( "Kurwa... Tyle roboty" ); Emote( "*Wzdycha ciężko*" ); break;
                                case 4: Emote( "*Odkłada kilof na ziemię po czym wciąga coś do nosa*" ); Say( "*O matulo...*" ); Emote( "*Pociera nos krzywiąc się*" ); break;
                                case 5: Say( "Jak wrócę do domu to zjem konia z kopytami" ); break;
                                case 6: Say( "Namacham sie kilofem a zarobek z tego marny..." ); break;
                                case 7: Say( "Chyba mi coś łupnęło w krzyżu..." ); Emote( "*Łapie sie za plecy*" ); break;
                                case 8: Say( "Ostatnio ponoć zawalił sie strop na paru górników..." ); break;
                                case 9: Say( "Dobra... Pora na przerwę" ); break;
                                case 10: Say( "Praca, praca..." ); break;
                                case 11: Say( "Siedziała na rynku, sprzedawała buty, jak nie miała reszty to dawała dupy!" ); Emote( "*Przyśpiewuje podczas pracy*" ); break;
                                case 12: Say( "Ciemno jak w dupie..." ); break;
                                case 13: Say( "Co to za dziewucha w tej różowej halce, chciałem ją posmyrać, obszczała mi palce! " ); Emote( "*Przyśpiewuje podczas pracy*" ); break;
                                case 14: Emote( "*Zacharczał wypluwając gęstą wydzieline*" ); break;
                                case 15: Say( "Kto to widział kurwa..." ); break;
                                case 16: Emote( "*Spluwa na ziemię*" ); break;
                                case 17: Say( "Potężne złoże tu wyczuwam..." ); break;
                                case 18: Say( "Kiedyś się dorobię i sam sobie pałac zbuduje... kiedyś" ); break;
                            }
                        }
                        else if ( Race == Race.NKrasnolud )
                        {
                            switch ( Utility.Random( 20 ) )
                            {
                                case 0: Emote( "*Ociera pot z czoła*" ); break;
                                case 1: Emote( "*Przeciera ubrudzoną twarz*" ); break;
                                case 2: Emote( "*Klnie pod nosem*" ); break;
                                case 3: Say( "Kurwa... Tyle roboty" ); Emote( "*Wzdycha ciężko*" ); break;
                                case 4: Emote( "*Odkłada kilof na ziemię po czym wciąga coś do nosa*" ); Say( "*O matulo...*" ); Emote( "*Pociera nos krzywiąc się*" ); break;
                                case 5: Say( "Jak wrócę do domu to zjem konia z kopytami" ); break;
                                case 6: Say( "Namacham sie kilofem a zarobek z tego marny..." ); break;
                                case 7: Say( "Chyba mi coś łupnęło w krzyżu..." ); Emote( "*Łapie sie za plecy*" ); break;
                                case 8: Say( "Ostatnio ponoć zawalił sie strop na paru górników..." ); break;
                                case 9: Say( "Dobra... Pora na przerwę" ); break;
                                case 10: Say( "Praca, praca..." ); break;
                                case 11: Say( "Siedziała na rynku, sprzedawała buty, jak nie miała reszty to dawała dupy!" ); Emote( "*Przyśpiewuje podczas pracy*" ); break;
                                case 12: Say( "Ciemno jak w dupie..." ); break;
                                case 13: Say( "Co to za dziewucha w tej różowej halce, chciałem ją posmyrać, obszczała mi palce! " ); Emote( "*Przyśpiewuje podczas pracy*" ); break;
                                case 14: Emote( "*Zacharczał wypluwając gęstą wydzieline*" ); break;
                                case 15: Say( "Kto to widział kurwa..." ); break;
                                case 16: Emote( "*Spluwa na ziemię*" ); break;
                                case 17: Say( "Potężne złoże tu wyczuwam..." ); break;
                                case 18: Say( "Kiedyś się dorobię i sam sobie pałac zbuduje... kiedyś" ); break;
                                case 19: Say( "Bez smoków, tak to można pracować..." ); break;
                            }
                        }
                        m_Spoken = DateTime.Now;
                    }
                }
            }
        }

        [Constructable]
        public XmlQuestNPCGornik( int gender ) : base( AIType.AI_Melee, FightMode.None, 10, 1, 0.8, 3.0 )
        {
            SetStr( 100, 300 );
            SetDex( 100, 300 );
            SetInt( 100, 300 );

            Fame = 5000;
            Karma = 3000;

            CanHearGhosts = false;

            SpeechHue = Utility.RandomDyedHue();

            Hue = Race.RandomSkinHue();

            switch ( gender )
            {
                case -1: this.Female = Utility.RandomBool(); break;
                case 0: this.Female = false; break;
                case 1: this.Female = true; break;
            }

            if ( this.Female )
            {
                this.Body = 0x191;
                this.Name = NameList.RandomName( "female" );
                Title = "- Górnik";
                Item hat = null;
                switch ( Utility.Random( 3 ) )
                {
                    case 0: hat = new Bandana( GetRandomHue() ); break;
                    case 1: hat = new SkullCap( GetRandomHue() ); break;
                    case 2: hat = null; break;

                }
                AddItem( hat );
                Item pants = null;
                switch ( Utility.Random( 3 ) )
                {
                    case 0: pants = new ShortPants( GetRandomHue() ); break;
                    case 1: pants = new LongPants( GetRandomHue() ); break;
                    case 2: pants = new Skirt( GetRandomHue() ); break;
                }
                AddItem( pants );
                Item shirt = null;
                switch ( Utility.Random( 5 ) )
                {
                    case 0: shirt = new Doublet( GetRandomHue() ); break;
                    case 1: shirt = new Surcoat( GetRandomHue() ); break;
                    case 2: shirt = new Robe( GetRandomHue() ); break;
                    case 3: shirt = new FancyShirt( GetRandomHue() ); break;
                    case 4: shirt = new Shirt( GetRandomHue() ); break;
                }
                AddItem( shirt );
            }
            else
            {
                this.Body = 0x190;
                this.Name = NameList.RandomName( "male" );
                Title = "- Górnik";
                Item hat = null;
                switch ( Utility.Random( 2 ) )
                {
                    case 0: hat = new SkullCap( GetRandomHue() ); break;
                    case 1: hat = new Bandana( GetRandomHue() ); break;
                }
                AddItem( hat );

                Item pants = null;
                switch ( Utility.Random( 2 ) )
                {
                    case 0: pants = new ShortPants( GetRandomHue() ); break;
                    case 1: pants = new LongPants( GetRandomHue() ); break;
                }
                AddItem( pants );

                Item shirt = null;
                switch ( Utility.Random( 5 ) )
                {
                    case 0: shirt = new Doublet( GetRandomHue() ); break;
                    case 1: shirt = new Surcoat( GetRandomHue() ); break;
                    case 2: shirt = new Tunic( GetRandomHue() ); break;
                    case 3: shirt = new FancyShirt( GetRandomHue() ); break;
                    case 4: shirt = new Shirt( GetRandomHue() ); break;
                }
                AddItem( shirt );

                //hand
                AddItem( new Pickaxe() );

                //hand2
                if ( Utility.RandomBool() )
                {
                    Lantern lantern = new Lantern();
                    lantern.Ignite();
                    AddItem( lantern );
                }
                Item torso = null;
                switch ( Utility.Random( 3 ) )
                {
                    case 0: torso = new HalfApron( GetRandomHue() ); break;
                    case 1: torso = new FullApron( GetRandomHue() ); break;
                    case 2: torso = null; break;
                }
                AddItem( torso );

            }

            Item feet = null;
            switch ( Utility.Random( 3 ) )
            {
                case 0: feet = new Boots( Utility.RandomNeutralHue() ); break;
                case 1: feet = new Shoes( Utility.RandomNeutralHue() ); break;
                case 2: feet = new Sandals( Utility.RandomNeutralHue() ); break;
            }
            AddItem( feet );
            Container pack = new Backpack();

            pack.Movable = false;

            AddItem( pack );
        }

        public XmlQuestNPCGornik( Serial serial ) : base( serial )
        {
        }
        private static int GetRandomHue()
        {
            switch ( Utility.Random( 6 ) )
            {
                default:
                case 0: return 0;
                case 1: return Utility.RandomBlueHue();
                case 2: return Utility.RandomGreenHue();
                case 3: return Utility.RandomRedHue();
                case 4: return Utility.RandomYellowHue();
                case 5: return Utility.RandomNeutralHue();
            }
        }


        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 ); // version

        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

        }
    }
}
