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
			AddBackground(0, 0, 320, 140, 302);
			AddLabel(20,20,0, "Nadawca:");
			AddTextEntry(20, 40, 280, 20, 0, 0, mail.Sender, 40);
			AddLabel(20,80,0, "Adresat:");
			AddTextEntry(20, 100, 280, 20, 0, 1, mail.Recipient, 40);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			_Mail.Sender = info.GetTextEntry(0).Text;
			_Mail.Recipient = info.GetTextEntry(1).Text;
		}
	}
}
