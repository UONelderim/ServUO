// ID 0000067 

///////////////////////////////////////////
// C# Exporter Generated: 2007-05-18 11:01:35
//
// Designed by Ravenal of OrBSydia
// Version: 2.0
//
// Script: MountChangeGump
///////////////////////////////////////////

using System;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Gumps
{
    public class MountChangeGump : Gump
    {
        public Mobile Player;
        public int id;

        public MountChangeGump( Mobile from ) : base( 0, 0 )
        {
            Closable = true;
            Disposable = true;
            Dragable = true;
            Player = from;

            AddPage( 0 );
            AddBackground( 20, 49, 388, 338, 9200 );
            AddLabel( 86, 68, 0, @"Please specify your mount's appearance." );
            AddLabel( 240, 336, 0, @"Reptalon" );
            AddLabel( 239, 313, 0, @"Cu Sidhe" );
            AddLabel( 241, 280, 0, @"Hiryu" );
            AddLabel( 46, 138, 0, @"Giant Beetle" );
            AddLabel( 49, 181, 0, @"Swamp Dragon" );
            AddLabel( 54, 215, 0, @"Ridgeback" );
            AddLabel( 51, 249, 0, @"Unicorn" );
            AddLabel( 235, 235, 0, @"Ki-rin" );
            AddLabel( 230, 207, 0, @"Fire Steed" );
            AddLabel( 51, 310, 0, @"Horse" );
            AddLabel( 230, 137, 0, @"Ostard" );
            AddLabel( 228, 107, 0, @"Frenzied Ostard" );
            AddLabel( 53, 350, 0, @"Llama" );
            AddLabel( 53, 279, 0, @"Nightmare" );
            AddLabel( 232, 171, 0, @"Silver Steed" );
            AddButton( 101, 310, 4005, 248, 16031, GumpButtonType.Reply, 0 );
            AddButton( 138, 138, 4005, 248, 16021, GumpButtonType.Reply, 0 );
            AddButton( 136, 279, 4005, 248, 16039, GumpButtonType.Reply, 0 );
            AddButton( 148, 185, 4005, 248, 16024, GumpButtonType.Reply, 0 );
            AddButton( 126, 215, 4005, 248, 16026, GumpButtonType.Reply, 0 );
            AddButton( 113, 249, 4005, 248, 16027, GumpButtonType.Reply, 0 );
            AddButton( 287, 234, 4005, 248, 16028, GumpButtonType.Reply, 0 );
            AddButton( 105, 349, 4005, 248, 16038, GumpButtonType.Reply, 0 );
            AddButton( 312, 207, 4005, 248, 16030, GumpButtonType.Reply, 0 );
            AddButton( 323, 174, 4005, 248, 16040, GumpButtonType.Reply, 0 );
            AddButton( 290, 136, 4005, 248, 16035, GumpButtonType.Reply, 0 );
            AddButton( 341, 109, 4005, 248, 16036, GumpButtonType.Reply, 0 );
            AddButton( 313, 335, 4005, 248, 16016, GumpButtonType.Reply, 0 );
            AddButton( 310, 305, 4005, 248, 16017, GumpButtonType.Reply, 0 );
            AddButton( 289, 279, 4005, 248, 16020, GumpButtonType.Reply, 0 );
            AddLabel( 50, 105, 0, @"Invisibile" );
            AddButton( 140, 107, 4005, 248, 16000, GumpButtonType.Reply, 0 );
        }

        public override void OnResponse( NetState sender, RelayInfo info )
        {
            int button = info.ButtonID;
            id = button;

            Player.SendMessage( "Select a mount you want to change." );
            Player.BeginTarget( 6, false, TargetFlags.None, new TargetCallback( OnTarget ) );
        }

        public virtual void OnTarget( Mobile Player, object targeted )
        {
            if ( !( ( targeted is BaseMount ) || ( targeted is EtherealMount ) ) )
            {
                Player.SendMessage( "This is not a mount!" );
                return;
            }


            if ( targeted is BaseMount )
            {
                BaseMount mount = (BaseMount)targeted;

                if ( mount.ControlMaster == Player )
                {
                    mount.ItemID = id;
                    // ID 0000067
                    Player.SendMessage( "Success!" );
                }
                else
                {
                    Player.SendMessage( "This is not your mount!" );
                }
            }

            if ( targeted is EtherealMount )
            {
                Item item = (Item)targeted;
                EtherealMount mount = (EtherealMount)targeted;

                if ( item.IsChildOf( Player.Backpack ) )
                {
                    mount.TransparentMountedID = id;
                    // ID 0000067
                    Player.SendMessage( "Success!" );
                }
                else
                {
                    Player.SendMessage( "Selected ethereal must be in your backpack." );
                }
            }

        }
    }
}
