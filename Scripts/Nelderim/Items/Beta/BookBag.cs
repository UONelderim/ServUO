using System; 
using Server; 
using Server.Items;

namespace Server.Items
{ 
   public class BookBag : Bag 
   { 
      [Constructable] 
      public BookBag() : this( 1 ) 
      { 
		  Movable = true; 
		  Hue = 0x489; 
		  Name = "worek z ksiegami";
      } 
	   [Constructable]
	   public BookBag( int amount )
	   {
		   DropItem( new Spellbook( UInt64.MaxValue ) );
		   DropItem( new NecromancerSpellbook( (UInt64)0xFFFF ) );
		   DropItem( new BookOfChivalry( (UInt64)0x3FF ) );
	   }
		

      public BookBag( Serial serial ) : base( serial ) 
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
