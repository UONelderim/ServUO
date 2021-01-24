using System;
using Server.Network;

namespace Server.Items
{
	
	public class CreepyPortraitS : Item
	{
		[Constructable]
		public CreepyPortraitS() : base( 10857 )
		{
		  	Name = "Portret Strachu";
			Weight = 10.0;
			Movable = true;
		}

		public CreepyPortraitS( Serial serial ) : base( serial )
		{
		}

    public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this, 3 ) ) 
			{
				switch ( ItemID ) 
				{ 
					//do swap or animation here 
					case 10857: //1
						this.ItemID=10858; 
						break;
					case 10858: //2
						this.ItemID=10859; 
						break;
					case 10859: //3
						this.ItemID=10860; 
						break;
					case 10860: //4
						this.ItemID=10857; 
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
