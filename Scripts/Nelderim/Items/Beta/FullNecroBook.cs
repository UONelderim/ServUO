using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class FullNecrobook : NecromancerSpellbook 
    {
        [Constructable]
        public FullNecrobook() : base( (ulong) 131071 )
        {
        }
        
        public FullNecrobook( Serial serial ) : base( serial )
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