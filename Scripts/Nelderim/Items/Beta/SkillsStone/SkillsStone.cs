// ID 0000042
using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
    public class SkillsStone : Item
    {
        [Constructable]
        public SkillsStone()
            : base( 0xEDC )
        {
            Movable = false;
            Name = "kamien umiejetnosci";
            Hue = 2451;
        }

        public SkillsStone( Serial serial ) : base( serial )
        {
        }

        public override void OnDoubleClick( Mobile from )
        {
            if ( from is PlayerMobile )
            {
                from.SendGump( new PvPSkillsGump( from, (Mobile) from ) );
            }

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