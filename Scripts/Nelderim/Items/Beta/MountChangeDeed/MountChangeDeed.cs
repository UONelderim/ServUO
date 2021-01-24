// ID 0000066
using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Targeting;

namespace Server.Items
{
    public class MountChangeDeed : Item
    {
        [Constructable]
        public MountChangeDeed() : base( 0xED4 )
        {
            Movable = true;
            Hue = 1000;
            Name = "Mount change deed";
            ItemID = 5360;
        }

        public override void OnDoubleClick( Mobile player )
        {
            if ( IsChildOf( player.Backpack ) )
            {
                player.SendGump( new MountChangeGump( (Mobile)player ) );
            }
            else
            {
                player.SendLocalizedMessage( 1042001 );
            }
        }

        public MountChangeDeed( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }
}