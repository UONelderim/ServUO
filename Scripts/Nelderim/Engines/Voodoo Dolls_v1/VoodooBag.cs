//Edits by Tru 9/28/2012
//Edits by Raist/Tass23 2/2/2017
using System; 
using Server; 
using Server.Items; 

namespace Server.Items 
{ 
	public class VooDooBag : Bag
	{ 
		[Constructable] 
		public VooDooBag() : this( 1 )
		{ 
			Movable = true; 
			//Hue = 0x386;
			Name = "worek voodoo";
		}
		
		[Constructable] 
		public VooDooBag( int amount )
		{ 
			DropItem( new VoodooPin() );
			DropItem( new VoodooPin() );
			DropItem( new VoodooPin() );
			DropItem( new Doll() );
		} 

		public VooDooBag( Serial serial ) : base( serial )
		{ 
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
