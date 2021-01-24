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
	public class XmlQuestNPCBiedak : TalkingBaseCreature
	{

        [Constructable]
        public XmlQuestNPCBiedak() : this(-1)
        {
        }

       public override void OnSpeech(SpeechEventArgs e)
        {
            base.OnSpeech(e);

            Mobile from = e.Mobile;
			
			if ( Utility.RandomDouble() < 0.25 )
			{
            if (from.InRange(this, 3))
            {
                {
                    switch ( Utility.Random( 11 ) )
				{
					case 0: Say( "Daj Panie choć złamanego grosza..." ); break;
					case 1: Say( "...i tak to właśnie pozbyłem się mego dobytku..." ); break;
					case 2: Say( "Psia krew...znów te szczury wygryzły mi koszulę..." ); break;
					case 3: Say( "A z chęcią bym się napił teraz gorzałki..." ); break;
					case 4: Say( "To miasto śmierdzi jeszcze bardziej, po tym jak Krasnoludy tu przylazły." ); break;
					case 5: Say( "A mamusia mówiła... nie handluj z Krasnoludami..." ); break;
					case 6: Say( "Eh.. i po co mi żyć w tej biedocie.." ); break;
					case 7: Say( "Ja chromolę... gdzie moje pieniądze?! ...a... nie mam ich..." ); break;
					case 8: Say( "Daj no chociaż na chleb i kawałek żeberka." ); break;
					case 9: Say( "Ta straż ciągle tu tylko węszy." ); break;
					case 10: Say( "A niech to... znów zabraknie mi nia gorzałkę..." ); break;
				}

                }
            }
			}
        }
		
		
        [Constructable]
        public XmlQuestNPCBiedak(int gender) : base( AIType.AI_Melee, FightMode.None, 10, 1, 0.8, 3.0 )
        {
            SetStr( 100, 300 );
            SetDex( 100, 300 );
            SetInt( 100, 300 );

            Fame = 100;
            Karma = -200;

            CanHearGhosts = false;

            SpeechHue = Utility.RandomDyedHue();
            Title = "- Biedak";
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
                this.Name = NameList.RandomName( "female" );
                Item hair = new Item( Utility.RandomList( 0x203B, 0x203C, 0x203D, 0x2045, 0x204A, 0x2046 , 0x2049 ) );
                hair.Hue = Utility.RandomHairHue();
                hair.Layer = Layer.Hair;
                hair.Movable = false;
                AddItem( hair );
                Item hat = null;
                switch ( Utility.Random( 2 ) )//4 hats, one empty, for no hat
                {
                    case 0: hat = new SkullCap( Utility.RandomNeutralHue() );		break;
                    //case 1: hat = new FeatheredHat( Utility.RandomNeutralHue() );	break;
                    //case 2: hat = new Bonnet();			break;
                    //case 3: hat = new Cap( Utility.RandomNeutralHue() );			break;
                }
                AddItem( hat );
                Item pants = null;
                switch ( Utility.Random( 3 ) )
                {
                    case 0: pants = new ShortPants( GetRandomHue() );	break;
                    case 1: pants = new LongPants( GetRandomHue() );	break;
                    case 2: pants = new Skirt( GetRandomHue() );		break;
                }
                AddItem( pants );
                Item shirt = null;
                switch ( Utility.Random( 5 ) )
                {
                    case 0: shirt = new Doublet( GetRandomHue() );		break;
                    case 1: shirt = new Surcoat( GetRandomHue() );		break;
                    //case 2: shirt = new Robe( GetRandomHue() );		break;
                    case 2: shirt = new FancyDress( GetRandomHue() );	break;
                    case 3: shirt = new PlainDress( GetRandomHue() );	break;
                    //case 5: shirt = new FancyShirt( GetRandomHue() );	break;
                    case 4: shirt = new Shirt( GetRandomHue() );		break;
                }
                AddItem( shirt );
            }
            else
            {
                this.Body = 0x190;
                this.Name = NameList.RandomName( "male" );
                Item hair = new Item( Utility.RandomList( 0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x2047, 0x2048 ) );
                hair.Hue = Utility.RandomHairHue();
                hair.Layer = Layer.Hair;
                hair.Movable = false;
                AddItem( hair );
                Item beard = new Item( Utility.RandomList( 0x0000, 0x203E, 0x203F, 0x2040, 0x2041, 0x2067, 0x2068, 0x2069 ) );
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem( beard );
                Item hat = null;
                switch ( Utility.Random( 6 ) ) //6 hats, one empty, for no hat
                {
                    case 0: hat = new SkullCap( GetRandomHue() );					break;
                    case 1: hat = new FloppyHat( GetRandomHue() );					break;
                    case 2: hat = new WideBrimHat();								break;
                    case 3: hat = new TallStrawHat( Utility.RandomNeutralHue() );	break;
                    case 4: hat = new StrawHat( Utility.RandomNeutralHue() );		break;
                   // case 4: hat = new TricorneHat( Utility.RandomNeutralHue() );	break;
                }
                AddItem( hat );
                Item pants = null;
                switch ( Utility.Random( 2 ) )
                {
                    case 0: pants = new ShortPants( GetRandomHue() );	break;
                    case 1: pants = new LongPants( GetRandomHue() );	break;
                }

                AddItem( pants );
                Item shirt = null;
                switch ( Utility.Random( 5 ) )
                {
                    case 0: shirt = new Doublet( GetRandomHue() );		break;
                    case 1: shirt = new Surcoat( GetRandomHue() );		break;
                    case 2: shirt = new Tunic( GetRandomHue() );		break;
                    case 3: shirt = new FancyShirt( GetRandomHue() );	break;
                    case 4: shirt = new Shirt( GetRandomHue() );		break;
					//case 5: shirt = new Robe( GetRandomHue() );		break;
                }
                AddItem( shirt );
				
                /*Item hand = null;
                switch ( Utility.Random( 4 ) )
                {
                    case 0: hand = new Dagger( GetRandomHue() );	break;
                    case 1: hand = new Broadsword( GetRandomHue() );	break;
					case 2: hand = new GnarledStaff( GetRandomHue() );	break;
                }*/
            }

            Item feet = null;
            switch ( Utility.Random( 2 ) )
            {
               // case 0: feet = new Boots( Utility.RandomNeutralHue() );	break;
               // case 1: feet = new Shoes( Utility.RandomNeutralHue() );	break;
                case 0: feet = new Sandals( Utility.RandomNeutralHue() );		break;
            }
            AddItem( feet );
            Container pack = new Backpack();

            pack.Movable = false;

            AddItem( pack );
        }

        public XmlQuestNPCBiedak( Serial serial ) : base( serial )
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
