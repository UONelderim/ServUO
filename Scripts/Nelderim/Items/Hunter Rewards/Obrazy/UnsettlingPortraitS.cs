using System;
using Server.Network;

namespace Server.Items
{
	
	public class UnsettlingPortraitS : Item
	{
		[Constructable]
		public UnsettlingPortraitS() : base( 10853 )
		{
		  	Name = "Lustrzany Portret";
			Weight = 10.0;
			Movable = true;
		}

		public UnsettlingPortraitS( Serial serial ) : base( serial )
		{
		}

    public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this, 3 ) ) 
			{
				switch ( ItemID ) 
				{ 
					//do swap or animation here 
					case 10853: //1
						this.ItemID=10854; 
						break;
					case 10854: //2
						this.ItemID=10853; 
						break;
					default:
                        break; 
				}
			}
			else
			{
				m.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that
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
