using System; 
using Server; 

namespace Server.Items 
{ 
	public class ShoeShine : Item
	{ 
		
		[Constructable]
		public ShoeShine() : base( 0xE7A ) 
		{
			Weight = 1.0; 
			Name = "czuscidlo do butow"; 
		} 

		public ShoeShine( Serial serial ) : base( serial ) 
		{ 
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

		public override void OnDoubleClick( Mobile from ) 
		{ 
			if ( IsChildOf( from.Backpack ) )
			{               
			Item feet = from.FindItemOnLayer( Layer.Shoes );
			if (feet == null)
				from.SendMessage( "Twoje buty nie potrzebuja czyszczenia!" );
			else
			{
				   	  		
				feet.Hue = 0;
				feet.Name = null;
				from.SendMessage( "Wyczysciles buty" );
				from.SendMessage( "The cleaner was a bit harsh, you may need to re-dye them." );
			         from.PlaySound( 995 );
			    Consume();
			}
		} 
			else 
			{ 
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack to use. 
			} 
		}
	} 
}

