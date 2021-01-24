using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Spellbook6 : Spellbook
    {
        [Constructable]
        public Spellbook6() : base( UInt64.MaxValue )
        {
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
		        Attributes.LowerManaCost = 6;
        }
        
        public Spellbook6( Serial serial ) : base( serial )
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