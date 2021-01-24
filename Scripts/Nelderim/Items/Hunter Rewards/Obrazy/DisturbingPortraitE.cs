using System;
using Server.Network;

namespace Server.Items
{
	
	public class DisturbingPortraitE : Item
	{
		[Constructable]
		public DisturbingPortraitE() : base( 10849 )
		{
		  	Name = "Portret Niepokoju";
			Weight = 10.0;
			Movable = true;
		}

		public DisturbingPortraitE( Serial serial ) : base( serial )
		{
		}

    public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this, 3 ) ) 
			{
				switch ( ItemID ) 
				{ 
					//do swap or animation here 
					case 10849: //1
						this.ItemID=10850; 
						break;
					case 10850: //2
						this.ItemID=10851; 
						break;
					case 10851: //3
						this.ItemID=10852; 
						break;
					case 10852: //4
						this.ItemID=10849; 
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
