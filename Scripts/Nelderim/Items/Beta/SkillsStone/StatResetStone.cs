using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
    public class StatResetStone : Item
    {
        [Constructable]
        public StatResetStone()
            : base( 0xEDC )
        {
            Movable = false;
            Name = "kamien resetowania statystyk";
            Hue = 2451;
        }

        public StatResetStone( Serial serial )
            : base( serial )
        {
        }

        public override void OnDoubleClick( Mobile from )
        {
            if ( from != null && from is PlayerMobile )
            {
                PlayerMobile f = (PlayerMobile) from;

                f.SendGump( new ResetGump( f, ResetGump.Action.Stat ) );
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