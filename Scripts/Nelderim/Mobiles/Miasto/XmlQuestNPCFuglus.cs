using System;
using Server.Items;

namespace Server.Mobiles
{
	public class XmlQuestNPCFuglus : TalkingBaseCreature
	{

        [Constructable]
        public XmlQuestNPCFuglus() : this(-1)
        {
        }
		
		private DateTime m_Spoken;
		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( m.Alive && m is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile)m;
					
				int range = 2;
				
				if ( range >= 0 && InRange( m, range ) && !InRange( oldLocation, range ) && DateTime.Now >= m_Spoken + TimeSpan.FromSeconds( 10 ) )
				{
                    switch ( Utility.Random( 18 ) )
				{
                                case 0: Say( "O ja biedny nieszczęśliwy!" ); Emote("*Szlocha...*"); break;
					            case 1: Say( "Znowu mnie wyruchali bez mydła!" ); break;
					            case 2: Say( "Płaczę bo lubię!" ); Emote("*Szlocha...*"); break;
					            case 3: Say( "Dlaczego wojownik musi mieć pod górkę?!" ); break;
					            case 4: Say( "Nie umiem, nie umiem" ); Emote("*Szlocha...*"); break;
					            case 5: Say( "Jak się wkurwię wpadnę w furię!" ); break;
					            case 6: Say( "Do czego by się tu dzisiaj przypierdolić..."); break;
					            case 7: Say( "O co ci kurwa chodzi co?!"); break;
					            case 8: Say( "Czemu mnie drażnisz?" ); break;
					            case 9: Say( "Wszyscy przeciwko mnie?! Dlaczego!!!" ); break;
					            case 10: Say( "Do czego by się tu dzisiaj przypierdolić..." ); break;
					            case 11: Say( "Dobra nie mam siły do ciebie." ); break;
					            case 12: Say( "Chciałem być kowalem, a zostałem furiatem!" ); break;
								case 13: Say( "Czysty wojownik nie ma szans w tym świecie... dlatego postanowiłem, że przestaje sie myć!" ); break;
								case 14: Say( "Od poł godziny probuje ustalić co ci nie pasuje..." ); break;
								case 15: Say( "Nie pyskuj gnoju!" ); break;
								case 16: Say( "To wszystko wina świata!" ); Emote("*Szlocha...*"); break;
								case 17: Say( "Czy słyszałeś już, że krasnoludy dużo pierdzą po grochówce? Chcesz bym ci o tym opowiedział?" ); break;
								
				}			
					m_Spoken = DateTime.Now;
				}
			}
		}				
		
        [Constructable]
        public XmlQuestNPCFuglus(int gender) : base( AIType.AI_Melee, FightMode.None, 10, 1, 0.8, 3.0 )
        {
            SetStr( 100, 300 );
            SetDex( 100, 300 );
            SetInt( 100, 300 );

            Fame = 100;
            Karma = -500;

            CanHearGhosts = false;

            SpeechHue = Utility.RandomDyedHue();
            
            Hue = Utility.RandomSkinHue();
            
            switch(gender)
            {
                case -1: this.Female = Utility.RandomBool(); break;
                case 0: this.Female = false; break;
                case 1: this.Female = true; break;
            }

            if ( this.Female)
            {
                this.Body = 0x191;
                Name = "Fugiella";
				Title = "- Furiatka";
                Item hat = null;
                switch ( Utility.Random( 5 ) )//4 hats, one empty, for no hat
                {
                    case 0: hat = new FeatheredHat( GetRandomHue() );	    break;
                    case 1: hat = new Bandana(GetRandomHue());			break;
                    case 2: hat = new SkullCap(GetRandomHue());			    break;
					case 3: hat = new Cap(GetRandomHue());     break;
					//case 4: hat = new TricorneHat( GetRandomHue() );					break;
					case 4: hat = null; break;
					
                }
                AddItem( hat );

                Item pants = null;
                switch ( Utility.Random( 4 ) )
                {
                    case 0: pants = new ShortPants( GetRandomHue() );	break;
                    case 1: pants = new LongPants( GetRandomHue() );	break;
                    case 2: pants = new Skirt( GetRandomHue() );		break;
					case 3: pants = new Kilt( GetRandomHue() );		break;
                }
                AddItem( pants );

                Item shirt = null;
                switch ( Utility.Random( 3 ) )
                {
                    case 0: shirt = new Shirt( GetRandomHue() );		break;
                    case 1: shirt = new FancyShirt( GetRandomHue() );		break;
                    case 2: shirt = new Robe( GetRandomHue() );		break;
                    /*case 3: shirt = new FancyShirt( GetRandomHue() );	break;
                    case 4: shirt = new ElvenDarkShirt( GetRandomHue() );		break;
					case 5: shirt = new ElvenShirt( GetRandomHue() );		break;*/
                }
                AddItem( shirt );
            }
            else
            {
                this.Body = 0x190;
                Name = "Fuglus";
				Title = "- Furiat";
                Item hat = null;
                switch ( Utility.Random( 7 ) ) //6 hats, one empty, for no hat
                {
                    case 0: hat = new SkullCap( GetRandomHue() );					break;
                    case 1: hat = new FeatheredHat( GetRandomHue() );	    break;
                    case 2: hat = new Bonnet(GetRandomHue());			break;
                    case 3: hat = new Cap(GetRandomHue());			    break;
					case 4: hat = new Bandana(GetRandomHue());     break;
					case 5: hat = new FloppyHat( GetRandomHue() );					break;
					case 6: hat = null; break;
                }
			
                AddItem( hat );
                Item pants = null;
                switch ( Utility.Random( 2 ) )
                {
                    case 0: pants = new ShortPants( GetRandomHue() );	break;
                    case 1: pants = new LongPants( GetRandomHue() );	break;
					//case 2: pants = new ElvenPants( GetRandomHue() );		break;
                }

                AddItem( pants );

                Item shirt = null;
                switch ( Utility.Random( 5 ) )
                {
                    case 0: shirt = new Doublet( GetRandomHue() );		break;
                   // case 1: shirt = new Surcoat( GetRandomHue() );		break;
                   // case 2: shirt = new Tunic( GetRandomHue() );		break;
                    case 1: shirt = new FancyShirt( GetRandomHue() );	break;
                    case 2: shirt = new Shirt( GetRandomHue() );		break;
					case 3: shirt = new Robe( GetRandomHue() );		break;
					case 4: shirt = null; break;
					//case 5: shirt = new ElvenDarkShirt( GetRandomHue() );		break;
					
                }
                AddItem( shirt );
				
                /*Item hand = null;
                switch ( Utility.Random( 4 ) )
                {
                    case 0: hand = new Dagger( Utility.RandomNeutralHue() );	    break;
                    case 1: hand = new Club( Utility.RandomNeutralHue() );	break;
					case 2: hand = new ButcherKnife( Utility.RandomNeutralHue() );	break;
					case 3: hand = new AssassinSpike( Utility.RandomNeutralHue() );	break;
                }
				AddItem( hand );*/
            }

            Item feet = null;
            switch ( Utility.Random( 3 ) )
            {
                case 0: feet = new Sandals( Utility.RandomNeutralHue() );	break;
                case 1: feet = new Shoes( Utility.RandomNeutralHue() );	break;
				case 2: feet = null; break;
                //case 2: feet = new Sandals( Utility.RandomNeutralHue() );		break;
				//case 3: feet = new ThighBoots( Utility.RandomNeutralHue() );		break;
				
            }
            AddItem( feet ); 
            Container pack = new Backpack();

            pack.Movable = false;

            AddItem( pack );
        }

        public XmlQuestNPCFuglus( Serial serial ) : base( serial )
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

            writer.Write( (int) 0 ); // version

        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

        }
    }
}
