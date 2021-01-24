using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
    public class SkillsResetStone : Item
    {
        [Constructable]
        public SkillsResetStone()
            : base( 0xEDC )
        {
            Movable = false;
            Name = "kamien resetowania umiejetnosci";
            Hue = 2451;
        }

        public SkillsResetStone( Serial serial )
            : base( serial )
        {
        }

        public override void OnDoubleClick( Mobile from )
        {
            if ( from != null && from is PlayerMobile )
            {
                PlayerMobile f = (PlayerMobile) from;

                f.SendGump( new ResetGump( f, ResetGump.Action.Skill ));
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