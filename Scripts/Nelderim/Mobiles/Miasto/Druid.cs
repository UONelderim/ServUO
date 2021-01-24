using System;
using Server.Items;

namespace Server.Mobiles
{
    public class Druid : TalkingBaseCreature
    {

        [Constructable]
        public Druid() : this( -1 )
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
                            switch ( Utility.Random( 5 ) )
                            {
                                case 0: Say( "Chwalmy Matkę!" ); break;
                                case 1: Say( "Kochajmy Naturę" ); break;
                                case 2: Say( "Psia krew..." ); break;
                                case 3: Say( "Miłość do lasów i gór..." ); break;
                                case 4: Say( "Miłego dnia!" ); break;


                            }
                        }
                        else if ( Race == Race.NJarling )
                        {
                            switch ( Utility.Random( 5 ) )
                            {
                                case 0: Say( "Chwalmy Matkę!" ); break;
                                case 1: Say( "Kochajmy Naturę" ); break;
                                case 2: Say( "Psia krew..." ); break;
                                case 3: Say( "Miłość do lasów i gór..." ); break;
                                case 4: Say( "Miłego dnia!" ); break;


                            }
                        }
                        else if ( Race == Race.NKrasnolud )
                        {
                            switch ( Utility.Random( 5 ) )
                            {
                                case 0: Say( "Chwalmy Matkę!" ); break;
                                case 1: Say( "Kochajmy Naturę" ); break;
                                case 2: Say( "Psia krew..." ); break;
                                case 3: Say( "Miłość do lasów i gór..." ); break;
                                case 4: Say( "Miłego dnia!" ); break;
                            }
                        }
                        m_Spoken = DateTime.Now;
                    }
                }
            }
        }

        [Constructable]
        public Druid( int gender ) : base( AIType.AI_Melee, FightMode.None, 10, 1, 0.8, 3.0 )
        {
            SetStr( 100, 300 );
            SetDex( 100, 300 );
            SetInt( 100, 300 );

            Fame = 5000;
            Karma = 3000;

            CanHearGhosts = false;

            SpeechHue = Utility.RandomDyedHue();

            Hue = Utility.RandomSkinHue();

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
                Title = "- Druidka";
                Item hat = null;
                switch ( Utility.Random( 6 ) )//4 hats, one empty, for no hat
                {
                    case 0: hat = new FeatheredHat( GetRandomHue() ); break;
                    case 1: hat = new Bonnet( GetRandomHue() ); break;
                    case 2: hat = new Cap( GetRandomHue() ); break;
                    case 3: hat = new WideBrimHat( GetRandomHue() ); break;
                    case 4: hat = new FloppyHat( GetRandomHue() ); break;
                    case 5: hat = null; break;

                }
                AddItem( hat );

                Item pants = null;
                switch ( Utility.Random( 4 ) )
                {
                    case 0: pants = new ShortPants( GetRandomHue() ); break;
                    case 1: pants = new LongPants( GetRandomHue() ); break;
                    case 2: pants = new Skirt( GetRandomHue() ); break;
                    case 3: pants = new ElvenPants( GetRandomHue() ); break;
                }
                AddItem( pants );

                Item shirt = null;
                switch ( Utility.Random( 1 ) )
                {
                    case 0: shirt = new Robe( Utility.RandomGreenHue() ); break;

                }
                AddItem( shirt );
            }
            else
            {
                this.Body = 0x190;
                this.Name = NameList.RandomName( "male" );
                Title = "- Druid";
                Item hat = null;
                switch ( Utility.Random( 0 ) ) //6 hats, one empty, for no hat
                {
                    case 0: hat = new FeatheredHat( GetRandomHue() ); break;
                    case 1: hat = new Bonnet( GetRandomHue() ); break;
                    case 2: hat = new Cap( GetRandomHue() ); break;
                    case 3: hat = new WideBrimHat( GetRandomHue() ); break;
                    case 4: hat = new FloppyHat( GetRandomHue() ); break;
                    case 5: hat = null; break;
                }
                AddItem( hat );

                Item shirt = null;
                switch ( Utility.Random( 0 ) )
                {
                    case 0: shirt = new Robe( Utility.RandomGreenHue() ); break;


                }
                AddItem( shirt );

                if ( Utility.RandomBool() )
                {
                    AddItem( new Cloak( GetRandomHue() ) );
                }
            }

            Item feet = null;
            switch ( Utility.Random( 3 ) )
            {
                case 0: feet = new Boots( Utility.RandomNeutralHue() ); break;
                case 1: feet = new Shoes( Utility.RandomNeutralHue() ); break;
                case 2: feet = new Sandals( Utility.RandomNeutralHue() ); break;
                case 3: feet = new ThighBoots( Utility.RandomNeutralHue() ); break;

            }
            AddItem( feet );
            Container pack = new Backpack();

            pack.Movable = false;

            AddItem( pack );
        }

        public override void GenerateLoot()
        {
            AddLoot( LootPack.Poor );
        }

        public Druid( Serial serial ) : base( serial )
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
