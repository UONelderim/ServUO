using System; 
using Server; 
using Server.Items;

namespace Server.Items
{ 
   public class ToolBag : Bag 
   { 
      [Constructable] 
      public ToolBag() : this( 1 ) 
      { 
		  Movable = true; 
		  Hue = 0x492; 
		  Name = "worek z narzedziami";
      } 
	   [Constructable]
	   public ToolBag( int amount )
	   {
		   DropItem( new TinkerTools( 1000 ) );
		   DropItem( new Scissors() );
		   DropItem( new HousePlacementTool() );
		   DropItem( new DovetailSaw( 1000 ) );
		   DropItem( new MortarPestle( 1000 ) );
		   DropItem( new ScribesPen( 1000 ) );
		   DropItem( new SmithHammer( 1000 ) );
		   DropItem( new TwoHandedAxe() );
		   DropItem( new FletcherTools( 1000 ) );
		   DropItem( new BlackDyeTub() );
		   DropItem( new SewingKit( 1000 ) ) ;
	   }
		

      public ToolBag( Serial serial ) : base( serial ) 
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
