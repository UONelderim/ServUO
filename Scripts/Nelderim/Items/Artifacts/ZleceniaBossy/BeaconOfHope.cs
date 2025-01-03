using System;
using Server;

namespace Server.Items
{
    public class BeaconOfHope : MetalShield
    {

		public override int BaseFireResistance{ get{ return 10; } }
        public override int InitMinHits{ get{ return 60; } }
        public override int InitMaxHits{ get{ return 60; } }

        [Constructable]
        public BeaconOfHope()
        {
            Name = "Wskaznik Nadziei";
			ItemID = 2597;
            Hue = 1767;
            StrRequirement = 55;
            Attributes.SpellChanneling = 1;
            Attributes.AttackChance = 12;
            Attributes.DefendChance = 12;
			Attributes.CastSpeed = 1;
			Attributes.BonusMana = 10;
			Attributes.BonusStam = -12;
            Attributes.Luck = -200;

            
        }

        public BeaconOfHope(Serial serial) : base( serial )
        {
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}

			else
			{

				if ( this.ItemID == 2597 )
				{
					this.ItemID = 2594;
				}
				else if ( this.ItemID == 2594 )
				{
					this.ItemID = 2597;
				}
				else if (this.ItemID != 2597 || this.ItemID != 2594 )
				{
					from.SendMessage("There was a problem lighting your lantern. Please contact a staff member");				
				}
				else
				{
					from.SendMessage( "Your lantern is broken. Please contact a staff member to repair it!" );
				}
			}
		}
		
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    } // End Class
} // End Namespace
