using Server.Items;
using Server.Network;

namespace Server.Gumps
{
	public class EnterAddressGump : Gump
	{
		private IMailItem _Mail;
		
		public EnterAddressGump(IMailItem mail) : base(100, 100)
		{
			_Mail = mail;

			AddPage(0);
			AddBackground(0, 0, 260, 300, 1579);
			AddLabel(20,20,0, "Nadawca:");
			AddTextEntry(40, 20, 220, 260, 0, 0, mail.Sender, 250);
			AddLabel(120,20,0, "Adresat:");
			AddTextEntry(140, 20, 220, 260, 0, 1, mail.Recipient, 250);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			_Mail.Sender = info.GetTextEntry(0).Text;
			_Mail.Recipient = info.GetTextEntry(1).Text;
		}
	}
}
