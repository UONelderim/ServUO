using System.Collections.Generic;
using Server.Gumps;
using Server.Network;
using Server.Mail;
using Server.Mobiles;
using Server.Items;

namespace Server.Gumps
{
    public class DestinationChoiceGump : Gump
    {
        private readonly object _mailItem;
        private readonly List<IMailDestination> _options;

        public DestinationChoiceGump(Mobile from, object mailItem, List<IMailDestination> options) : base(50,50)
        {
            _mailItem = mailItem;
            _options = options;

            AddPage(0);
            AddBackground(0, 0, 280, 200, 2620);
            AddLabel(20, 20, 1152, "Wybierz skrzynkę dostawy:");

            for (int i = 0; i < options.Count; i++)
            {
                string desc = options[i] is DirectBackpackDestination
                    ? "Plecak"
                    : options[i].ContainerItem.Name;
                AddButton(20, 50 + i * 30, 0x845, 0x846, i + 1, GumpButtonType.Reply, 0);
                AddLabel(60, 50 + i * 30, 1152, desc);
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            int idx = info.ButtonID - 1;
            if (idx < 0 || idx >= _options.Count) return;

            var dest = _options[idx];

            switch (_mailItem)
            {
                case PlayerLetter letter:
                    letter.Destination = dest;
                    letter.ProcessSend();
                    break;

                case Parcel parcel:
                    parcel.Destination = dest;
                    parcel.ProcessSend();
                    break;
            }
        }
    }
}
