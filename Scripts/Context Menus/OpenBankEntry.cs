
using Server.Gumps;
using Server.Mobiles;

namespace Server.ContextMenus
{
    public class OpenBankEntry : ContextMenuEntry
    {
        private readonly Banker m_Banker;

        public OpenBankEntry(Banker banker)
            : base(6105, 12)
        {
            m_Banker = banker;
        }

        public override void OnClick()
        {
            if (!Owner.From.CheckAlive())
                return;

            if (Owner.From.Criminal)
            {
                m_Banker.Say(500378); // Thou art a criminal and cannot access thy bank box.
            }
            else
            {
                Owner.From.BankBox.Open();

                if (Owner.From is PlayerMobile)
                {
                    Owner.From.CloseGump(typeof(BankerGump));
					BaseGump.SendGump(new BankerGump(m_Banker, (PlayerMobile)Owner.From));
                }
            }
        }
    }
}
