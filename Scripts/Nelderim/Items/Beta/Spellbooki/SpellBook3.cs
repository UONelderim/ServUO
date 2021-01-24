using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Spellbook3 : Spellbook
    {
        [Constructable]
        public Spellbook3() : base( UInt64.MaxValue )
        {
            Attributes.RegenMana = 1;
            Attributes.LowerManaCost = 6;
            Attributes.SpellDamage = 9;
        }
        
        public Spellbook3( Serial serial ) : base( serial )
        {
        }
        
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            
            writer.Write( (int) 0 );
        }
        
        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            
            int version = reader.ReadInt();
        }
    }
}