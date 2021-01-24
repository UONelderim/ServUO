using System; 
using Server.Items;

namespace Server.Items
{ 
   public class BandageDeed : Item 
   { 
      [Constructable] 
      public BandageDeed() : base( 0xED4 ) 
      { 
         Movable = true; 
         Hue = 1266; 
         Name = "zwoj bandazy";
         ItemID = 5360; 
      } 

      public override void OnDoubleClick( Mobile from ) 
      { 
         Bandage bandage = new Bandage( 250 ); 

         if ( !from.AddToBackpack( bandage ) ) 
            bandage.Delete(); 
      } 

      public BandageDeed( Serial serial ) : base( serial ) 
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