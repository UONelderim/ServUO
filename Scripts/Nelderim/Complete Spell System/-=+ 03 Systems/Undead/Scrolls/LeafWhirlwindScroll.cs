using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Undead
{
   public class UndeadLeafWhirlwindScroll : CSpellScroll
   {
      [Constructable]
      public UndeadLeafWhirlwindScroll() : this( 1 )
      {
      }

      [Constructable]
      public UndeadLeafWhirlwindScroll( int amount ) : base( typeof( UndeadLeafWhirlwindSpell ), 0xE39 )
      {
         Name = "Piętno";
         Hue = 38;
      }

      public UndeadLeafWhirlwindScroll( Serial serial ) : base( serial )
      {
      		ItemID=0xE39;
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

