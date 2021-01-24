using System; 
using Server.Items;

namespace Server.Items
{ 
   public class ArtifactStone : Item 
   { 
      [Constructable] 
      public ArtifactStone() : base( 0xED4 ) 
      { 
         Movable = false; 
         Hue = 2991; 
         Name = "kamien artefaktow"; 
      } 

      public override void OnDoubleClick( Mobile from ) 
      { 
         ArtiBag bagarti = new ArtiBag( 1 ); 

         if ( !from.AddToBackpack( bagarti ) ) 
            bagarti.Delete(); 
      } 

      public ArtifactStone( Serial serial ) : base( serial ) 
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