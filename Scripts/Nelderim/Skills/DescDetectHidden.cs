//////////////////////////////////////////////////////////////////////
// Custom DetectHidden by ViWinfii 
// Version 3.1415
// Date:  3-14-15
//////////////////////////////////////////////////////////////////////

using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Gumps
{
    public class DescDetectHidden : Gump
    {
        public DescDetectHidden()
            : base(75, 75)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(10, 7, 206, 276, 9200);
            AddLabel(39, 15, 0, @"Opis wykrywania");
            AddLabel(30, 35, 0, @"oraz wyczucia niewidzialnych");
            AddHtml(15, 56, 196, 223, @"Wyczucie: Uzyj zmyslow by wyczuc ukryte postaci wokol ciebie bez odkrywania ich obecnosci.

Wykrywanie: Uzyj swoich zmyslow by odkryc niewidzialne postaci i obiekty we wskazanym miejscu.", (bool)true, (bool)false);
            AddButton(184, 13, 5052, 5053, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            from.CloseGump(typeof(DescDetectHidden));
        }
    }
}