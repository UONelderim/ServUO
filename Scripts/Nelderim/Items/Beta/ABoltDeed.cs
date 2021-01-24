//Created By Ryan_2005 [Please do Not remove this]
using System; 
using Server.Items; 

namespace Server.Items 
{ 
   public class BoltDeed : Item 
   { 
      [Constructable] 
      public BoltDeed() : base( 0xED4 ) 
      { 
         Movable = true; 
         Hue = 1156; 
         Name = "zwoj beltow"; 
         ItemID = 5360;
      } 

      public override void OnDoubleClick( Mobile from ) 
      { 
         Bolt Bolt = new Bolt( 250 ); 

         if ( !from.AddToBackpack( Bolt ) ) 
            Bolt.Delete(); 
      } 

      public BoltDeed( Serial serial ) : base( serial ) 
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