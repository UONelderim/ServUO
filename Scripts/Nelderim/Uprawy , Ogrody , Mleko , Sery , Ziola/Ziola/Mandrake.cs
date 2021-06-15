using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items.Crops
{
	public class MandrakePlant : BaseCrop 
	{ 
		private double mageValue;
		private DateTime lastpicked;

		[Constructable] 
		public MandrakePlant() : base( Utility.RandomList( 0x18DF, 0x18E0 ) ) 
		{ 
			Movable = false; 
			Name = "Kwiat Mandragory"; 
			lastpicked = DateTime.Now;
		} 

		public override void OnDoubleClick(Mobile from) 
		{ 
			if ( from == null || !from.Alive ) return;

			if ( DateTime.Now > lastpicked.AddSeconds(3) ) // 3 seconds between picking
			{
				lastpicked = DateTime.Now;

				if ( from.InRange( this.GetWorldLocation(), 1 ) ) 
				{ 
						from.Direction = from.GetDirectionTo( this );
						from.Animate( 32, 5, 1, true, false, 0 ); // Bow

						from.SendMessage("Wyrwales rosline z korzeniami."); 
						this.Delete(); 

						from.AddToBackpack( new MandrakeUprooted() );
				} 
				else 
				{ 
					from.SendMessage( "Jestes za daleko." ); 
				} 
			}
		} 

		public MandrakePlant( Serial serial ) : base( serial ) 
		{ 
			lastpicked = DateTime.Now;
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	} 
	
	[FlipableAttribute( 0x18DD, 0x18DE )]
	public class MandrakeUprooted : Item, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			int count = Utility.Random( 10 );
			if ( count == 0 ) 
			{
				from.SendMessage( "Znalazles zepsuty korzen." ); 
				this.Consume();
			}
			else
			{
				base.ScissorHelper( from, new MandrakeRoot(), count );
				from.SendMessage( "Obciales {0} korzen{1}.", count, ( count == 1 ? "" : "i" ) ); 
			}

		}

		[Constructable]
		public MandrakeUprooted() : this( 1 )
		{
		}

		[Constructable]
		public MandrakeUprooted( int amount ) : base( Utility.RandomList( 0x18DD, 0x18DE ) )
		{
			Stackable = false;
			Weight = 1.0;
			
			Movable = true; 
			Amount = amount;

			Name = "korzen mandragory";
		}

		public MandrakeUprooted( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
