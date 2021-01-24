using System; 
using Server.Items;

namespace Server.Items
{ 
   public class czek : Item 
   { 
      [Constructable] 
      public czek() : base( 0xED4 ) 
      { 
         Movable = true; 
         Hue = 1151; 
         Name = "czek";
         ItemID = 5360; 
      } 

      public override void OnDoubleClick( Mobile from ) 
      { 
         BankCheck bankcheck = new BankCheck( 50000 ); 

         if ( !from.AddToBackpack( bankcheck ) ) 
            bankcheck.Delete(); 
      } 

      public czek( Serial serial ) : base( serial ) 
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