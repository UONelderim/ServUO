using System;
using Server.Network;

namespace Server.Items
{
	
	public class CreepyPortraitE : Item
	{
		[Constructable]
		public CreepyPortraitE() : base( 10861 )
		{
		  	Name = "Portret Strachu";
			Weight = 10.0;
			Movable = true;
		}

		public CreepyPortraitE( Serial serial ) : base( serial )
		{
		}

    public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this, 3 ) ) 
			{
				switch ( ItemID ) 
				{ 
					//do swap or animation here 
					case 10861: //1
						this.ItemID=10862; 
						break;
					case 10862: //2
						this.ItemID=10863; 
						break;
					case 10863: //3
						this.ItemID=10864; 
						break;
					case 10864: //4
						this.ItemID=10861; 
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
