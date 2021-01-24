using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Spellbook1 : Spellbook
    {
        [Constructable]
        public Spellbook1() : base( UInt64.MaxValue )
        {
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.SpellDamage = 9;
        }
        
        public Spellbook1( Serial serial ) : base( serial )
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