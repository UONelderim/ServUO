using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Spellbook5 : Spellbook
    {
        [Constructable]
        public Spellbook5() : base( UInt64.MaxValue )
        {
            Attributes.RegenMana = 1;
            Attributes.BonusInt = 8;
		        Attributes.LowerManaCost = 6;
        }
        
        public Spellbook5( Serial serial ) : base( serial )
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