using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Spellbook2 : Spellbook
    {
        [Constructable]
        public Spellbook2() : base( UInt64.MaxValue )
        {
            Attributes.SpellDamage = 9;
            Attributes.LowerManaCost = 6;
            Attributes.CastSpeed = 1;
            
        }
        
        public Spellbook2( Serial serial ) : base( serial )
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