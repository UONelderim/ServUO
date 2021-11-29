using System;
using System.Data;
using System.IO;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Gumps;
using Server.Targeting;
using System.Reflection;
using Server.Commands;
using CPA = Server.CommandPropertyAttribute;
using System.Xml;
using Server.Spells;
using System.Text;
using Server.Accounting;
using System.Diagnostics;



namespace Server.Mobiles
{
    public class XmlQuestNPCChlop : TalkingBaseCreature
    {

        [Constructable]
        public XmlQuestNPCChlop() : this( -1 )
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
                            switch ( Utility.Random( 14 ) )
                            {
                                case 0: Say( "Mogłoby troche popadać..." ); break;
                                case 1: Say( "Oby nie było suszy." ); break;
                                case 2: Say( "Cholerni Jarlingowie, ciągle nas wyzyskują..." ); break;
                                case 3: Say( "Ah... Mam już dość." ); break;
                                case 4: Say( "Odejdź... Mam dużo pracy." ); Emote( "*Odgania ręką*" ); break;
                                case 5: Say( "Zjeżdżaj stąd..." ); break;
                                case 6: Emote( "*Szura powoli nogą po ziemi rozglądając się w koło*" ); break;
                                case 7: Say( "Oby Matka zesłała urodzaj..." ); break;
                                case 8: Say( "Trza by załatać dach... znów cieknie." ); Emote( "*Wzdycha ponuro*" ); break;
                                case 9: Say( "Kończą sie zapasy, trzaby sie wybrać do Miasta." ); break;
                                case 10: Say( "Muszę jutro zajść do młyna." ); break;
                                case 11: Say( "Cholera, trzeba nakarmić kury." ); break;
                                case 12: Say( "Sprawię sobie chyba nowe buty... Albo chociaż załatam stare." ); break;
                                case 13: Say( "Oni tak zawsze obiecują... A potem nic z tego nie ma" ); break;
                                case 14: Emote( "*Rozrzuca coś na ziemi*" ); break;
                                case 15: Emote( "*Rozgląda się leniwym spojrzeniem*" ); break;
                                case 16: Emote( "*Przeciąga się*" ); break;
                                case 17: Emote( "*Nuci pod nosem*" ); break;
                                case 18: Emote( "*Popija świeże mleko*" ); break;
                                case 19: Say( "Sprowadzają się tu... I jeszcze chcą nam ziemię zabrać" ); Emote( "*Prycha*" ); break;
                                case 20: Emote( "*Klnie siarczyście pod nosem*" ); break;
                                case 21: Emote( "O kurwa..." ); Emote( "*Zamyśla się*" ); break;

                            }
                        }
                        else if ( Race == Race.NJarling )
                        {
                            switch ( Utility.Random( 2 ) )
                            {
                                case 0: Say( "Mogłoby troche popadać..." ); break;
                                case 1: Say( "Oby nie było suszy." ); break;
                                case 2: Say( "Mam juz dość tych śmierdzących Tamaeli..." ); break;
                                case 3: Say( "Ah... Mam już dość." ); break;
                                case 4: Say( "Odejdź... Mam dużo pracy." ); Emote( "*Odgania ręką*" ); break;
                                case 5: Say( "Zjeżdżaj stąd..." ); break;
                                case 6: Emote( "*Szura powoli nogą po ziemi rozglądając się w koło*" ); break;
                                case 7: Say( "Oby Matka zesłała urodzaj..." ); break;
                                case 8: Say( "Trza by załatać dach... znów cieknie." ); Emote( "*Wzdycha ponuro*" ); break;
                                case 9: Say( "Kończą sie zapasy, trzaby sie wybrać do Miasta." ); break;
                                case 10: Say( "Muszę jutro zajść do młyna." ); break;
                                case 11: Say( "Cholera, trzeba nakarmić kury." ); break;
                                case 12: Say( "Sprawię sobie chyba nowe buty... Albo chociaż załatam stare." ); break;
                                case 13: Say( "Oni tak zawsze obiecują... A potem nic z tego nie ma" ); break;
                                case 14: Emote( "*Rozrzuca coś na ziemi*" ); break;
                                case 15: Emote( "*Rozgląda się leniwym spojrzeniem*" ); break;
                                case 16: Emote( "*Przeciąga się*" ); break;
                                case 17: Emote( "*Nuci pod nosem*" ); break;
                                case 18: Emote( "*Popija świeże mleko*" ); break;
                                case 19: Say( "Tą ziemię przynajmniej da się uprawiać... Nie to co na północy." ); Emote( "*Wzdycha*" ); break;
                                case 20: Emote( "*Klnie siarczyście pod nosem*" ); break;
                                case 21: Emote( "O kurwa..." ); Emote( "*Zamyśla się*" ); break;
                            }
                        }
                        else if ( Race == Race.NKrasnolud )
                        {
                            switch ( Utility.Random( 2 ) )
                            {
                                case 0: Say( "Mogłoby troche popadać..." ); break;
                                case 1: Say( "Oby nie było suszy." ); break;
                                case 2: Say( "Cholerni Jarlingowie, ciągle nas wyzyskują..." ); break;
                                case 3: Say( "Ah... Mam już dość." ); break;
                                case 4: Say( "Odejdź... Mam dużo pracy." ); Emote( "*Odgania ręką*" ); break;
                                case 5: Say( "Zjeżdżaj stąd..." ); break;
                                case 6: Emote( "*Szura powoli nogą po ziemi rozglądając się w koło*" ); break;
                                case 7: Say( "Oby Matka zesłała urodzaj..." ); break;
                                case 8: Say( "Trza by załatać dach... znów cieknie." ); Emote( "*Wzdycha ponuro*" ); break;
                                case 9: Say( "Kończą sie zapasy, trzaby sie wybrać do Miasta." ); break;
                                case 10: Say( "Muszę jutro zajść do młyna." ); break;
                                case 11: Say( "Cholera, trzeba nakarmić kury." ); break;
                                case 12: Say( "Sprawię sobie chyba nowe buty... Albo chociaż załatam stare." ); break;
                                case 13: Say( "Oni tak zawsze obiecują... A potem nic z tego nie ma" ); break;
                                case 14: Emote( "*Rozrzuca coś na ziemi*" ); break;
                                case 15: Emote( "*Rozgląda się leniwym spojrzeniem*" ); break;
                                case 16: Emote( "*Przeciąga się*" ); break;
                                case 17: Emote( "*Nuci pod nosem*" ); break;
                                case 18: Emote( "*Popija świeże mleko*" ); break;
                                case 19: Say( "Co za buc..." ); Emote( "*Prycha*" ); break;
                                case 20: Emote( "*Klnie siarczyście pod nosem*" ); break;
                                case 21: Emote( "O kurwa..." ); Emote( "*Zamyśla się*" ); break;
                            }
                        }
                        m_Spoken = DateTime.Now;
                    }
                }
            }
        }

        [Constructable]
        public XmlQuestNPCChlop( int gender ) : base( AIType.AI_Melee, FightMode.None, 10, 1, 0.8, 3.0 )
        {
            SetStr( 100, 300 );
            SetDex( 100, 300 );
            SetInt( 100, 300 );

            Fame = 10;
            Karma = 10;


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
                Title = "- Chłopka";
                Item hat = null;
                switch ( Utility.Random( 7 ) )
                {
                    case 0: hat = new StrawHat( GetRandomHue() ); break;
                    case 1: hat = new TallStrawHat( GetRandomHue() ); break;
                    case 2: hat = new FloppyHat( GetRandomHue() ); break;
                    case 3: hat = new Bandana( GetRandomHue() ); break;
                    case 4: hat = new Kasa( GetRandomHue() ); break;
                    case 5: hat = new SkullCap( GetRandomHue() ); break;
                    case 6: hat = null; break;

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
                switch ( Utility.Random( 4 ) )
                {
                    case 0: shirt = new Shirt( GetRandomHue() ); break;
                    case 1: shirt = new FancyShirt( GetRandomHue() ); break;
                    case 2: shirt = new Tunic( GetRandomHue() ); break;
                    case 3: shirt = new PlainDress( GetRandomHue() ); break;

                }
                AddItem( shirt );
            }
            else
            {
                this.Body = 0x190;
                this.Name = NameList.RandomName( "male" );
                Title = "- Chłop";
                Item hat = null;
                switch ( Utility.Random( 7 ) )
                {
                    case 0: hat = new StrawHat( GetRandomHue() ); break;
                    case 1: hat = new TallStrawHat( GetRandomHue() ); break;
                    case 2: hat = new FloppyHat( GetRandomHue() ); break;
                    case 3: hat = new Bandana( GetRandomHue() ); break;
                    case 4: hat = new Kasa( GetRandomHue() ); break;
                    case 5: hat = new SkullCap( GetRandomHue() ); break;
                    case 6: hat = null; break;
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
                switch ( Utility.Random( 7 ) )
                {
                    case 0: shirt = new Doublet( GetRandomHue() ); break;
                    case 1: shirt = new Surcoat( GetRandomHue() ); break;
                    case 2: shirt = new Tunic( GetRandomHue() ); break;
                    case 3: shirt = new FancyShirt( GetRandomHue() ); break;
                    case 4: shirt = new Shirt( GetRandomHue() ); break;
                    case 5: shirt = new Robe( GetRandomHue() ); break;
                    case 6: shirt = null; break;
                }
                AddItem( shirt );

                Item hand = null;
                switch ( Utility.Random( 3 ) )
                {
                    case 0: hand = new Pitchfork(); break;
                    case 1: hand = null; break;
                    case 2: hand = null; break;
                }
                AddItem( hand );
            }

            Item feet = null;
            switch ( Utility.Random( 4 ) )
            {
                case 0: feet = new Boots( Utility.RandomNeutralHue() ); break;
                case 1: feet = new Shoes( Utility.RandomNeutralHue() ); break;
                case 2: feet = new Sandals( Utility.RandomNeutralHue() ); break;
                case 3: feet = null; break;
            }
            AddItem( feet );
            Container pack = new Backpack();

            pack.Movable = false;

            AddItem( pack );
        }

        public XmlQuestNPCChlop( Serial serial ) : base( serial )
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
