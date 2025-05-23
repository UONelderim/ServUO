using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
    public class VoodooSpellGump : Gump
    {
        private readonly Mobile _from;
        private readonly VoodooDoll _doll;

        // Button IDs: spells and connect option
        private enum Buttons
        {
            Stab    = (int)VoodooSpellType.Stab,    // Ukłucie
            Curse   = (int)VoodooSpellType.Curse,   // Klątwa Voodoo
            Disease = (int)VoodooSpellType.Disease, // Przeniesienie Choroby
            Connect = 1000                           // Nawiąż połączenie
        }

        public VoodooSpellGump(Mobile from, VoodooDoll doll) : base(100, 100)
        {
            _from = from;
            _doll = doll;

            Closable   = true;   // pozwala na zamknięcie gumpa kliknięciem X
            Disposable = true;   // gump usuwa się z listy po zamknięciu

            AddPage(0);
            AddBackground(0, 0, 220, 200, 9270);
            AddLabel(20, 20, 1152, "Wybierz zaklęcie Guślarstwa:");

            int yOffset = 50;

            // Ukłucie
            AddButton(20, yOffset, 4005, 4007, (int)Buttons.Stab, GumpButtonType.Reply, 0);
            AddLabel(55, yOffset, 1152, "Ukłucie");
            yOffset += 30;

            // Klątwa Voodoo
            AddButton(20, yOffset, 4005, 4007, (int)Buttons.Curse, GumpButtonType.Reply, 0);
            AddLabel(55, yOffset, 1152, "Klątwa Słabości");
            yOffset += 30;

            // Przeniesienie Choroby
            AddButton(20, yOffset, 4005, 4007, (int)Buttons.Disease, GumpButtonType.Reply, 0);
            AddLabel(55, yOffset, 1152, "Przeniesienie Choroby");
            yOffset += 30;

            // Separator
            yOffset += 10;

            // Nawiąż połączenie
            AddButton(20, yOffset, 4005, 4007, (int)Buttons.Connect, GumpButtonType.Reply, 0);
            AddLabel(55, yOffset, 1152, "Nawiąż połączenie");
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            // Jeśli ButtonID == 0 użytkownik zamknął gump (klik X lub ESC) – przerywamy
            if (info.ButtonID == 0)
                return;

            switch (info.ButtonID)
            {
                case (int)Buttons.Connect:
                    _doll.TryLink(_from);
                    break;

                case (int)Buttons.Stab:
                case (int)Buttons.Curse:
                case (int)Buttons.Disease:
                    _doll.CastVoodooSpell(_from, (VoodooSpellType)info.ButtonID);
                    break;

                default:
                    // nieobsługiwane ID – nic nie robimy
                    return;
            }

            // Po wykonaniu akcji prezentujemy gump ponownie
            _from.SendGump(new VoodooSpellGump(_from, _doll));
        }
    }
}
