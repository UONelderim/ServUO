using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Spellbook4 : Spellbook
    {
        [Constructable]
        public Spellbook4() : base( UInt64.MaxValue )
        {
            Attributes.CastRecovery = 2;
            Attributes.SpellDamage = 9;
		        Attributes.LowerManaCost = 6;
        }
        
        public Spellbook4( Serial serial ) : base( serial )
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