using System;
using Server;
using Server.Network;

namespace Server.Gumps
{
    public class KarmaDeedGump : Gump
    {
        public KarmaDeedGump( Mobile from ) : base( 0, 0 )
        {
            Closable = true;
            Disposable = true;
            Dragable = true;

            AddPage(0);
            AddImage(202, 140, 2500); 
            AddButton(318, 189, 238, 240, 0, GumpButtonType.Reply, 0); 
            AddButton(319, 219, 238, 240, 1, GumpButtonType.Reply, 0); 
            AddLabel(281, 167, 195, @"KARMA"); 
            AddLabel(247, 191, 1271, @"Good"); 
            AddLabel(250, 215, 2455, @"Bad"); 
		}

        public override void OnResponse( NetState sender, RelayInfo info )
        {
			Mobile from = sender.Mobile;

			int button = info.ButtonID;

            if ( button == 0 )
			{
				from.PlaySound( 0x284 );
				from.Karma = 15000;
			}
			else if ( button == 1 )
			{
				from.PlaySound( 0x284 );
				from.Karma = -15000;
			}
			else
			{
				from.CloseGump( typeof( KarmaDeedGump ) );
			}

			if ( button == 0 || button == 1 )
			{
				from.SendMessage(0x127, "You have now: {0}", from.Karma);
			}
        }
    }
}
