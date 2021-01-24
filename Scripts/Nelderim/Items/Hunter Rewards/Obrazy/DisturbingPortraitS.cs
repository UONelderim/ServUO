using System;
using Server.Network;

namespace Server.Items
{
	
	public class DisturbingPortraitS : Item
	{
		[Constructable]
		public DisturbingPortraitS() : base( 10845 )
		{
		  	Name = "Portret Niepokoju";
			Weight = 10.0;
			Movable = true;
		}

		public DisturbingPortraitS( Serial serial ) : base( serial )
		{
		}

    public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this, 3 ) ) 
			{
				switch ( ItemID ) 
				{ 
					//do swap or animation here 
					case 10845: //1
						this.ItemID=10846; 
						break;
					case 10846: //2
						this.ItemID=10847; 
						break;
					case 10847: //3
						this.ItemID=10848; 
						break;
					case 10848: //4
						this.ItemID=10845; 
//						new InternalTimer( this, m ).Start();
						break; 
					default: break; 
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
