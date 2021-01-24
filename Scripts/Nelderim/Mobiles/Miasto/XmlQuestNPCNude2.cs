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
	public class XmlQuestNPCNude2 : TalkingBaseCreature
	{

        [Constructable]
        public XmlQuestNPCNude2() : this(-1)
        {
        }

        [Constructable]
        public XmlQuestNPCNude2(int gender) : base( AIType.AI_Melee, FightMode.None, 10, 1, 0.8, 3.0 )
        {
            SetStr( 10, 30 );
            SetDex( 10, 30 );
            SetInt( 10, 30 );

            Fame = 50;
            Karma = 50;

            CanHearGhosts = true;

            SpeechHue = Utility.RandomDyedHue();
            Title = string.Empty;
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
            }
            else
            {
                this.Body = 0x190;
                this.Name = NameList.RandomName( "male" );
            }

            Container pack = new Backpack();

            pack.DropItem( new Gold( 0, 50 ) );

            pack.Movable = false;

            AddItem( pack );
        }

        public XmlQuestNPCNude2( Serial serial ) : base( serial )
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
