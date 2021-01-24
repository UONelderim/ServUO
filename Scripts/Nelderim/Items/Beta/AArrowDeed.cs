//Created By Ryan_2005 [Do Not Remove this Please]
using System; 
using Server.Items; 

namespace Server.Items 
{ 
   public class ArrowDeed : Item 
   { 
      [Constructable] 
      public ArrowDeed() : base( 0xED4 ) 
      { 
         Movable = true; 
         Hue = 1154; 
         Name = "zwoj strzal";
         ItemID = 5360; 
      } 

      public override void OnDoubleClick( Mobile from ) 
      { 
         Arrow Arrow = new Arrow( 250 ); 

         if ( !from.AddToBackpack( Arrow ) ) 
            Arrow.Delete(); 
      } 

      public ArrowDeed( Serial serial ) : base( serial ) 
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