using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Nelderim.Engines.MailSystem
{
	public class SendMailGump : Gump
	{
		IMailItem _Mail;
		bool _Priority;
		
		public SendMailGump(IMailItem mail, bool priority = false) : base(100, 100)
		{
			_Mail = mail;
			_Priority = priority;
			var bgHeight = 200;
			
			AddPage(0);
			AddBackground(0,0, 300, bgHeight, 302);
			AddLabel(20, 20, 0, $"Nadawca: {mail.Sender}");
			AddLabel(20, 40, 0, $"Odbiora: {mail.Recipient}");
			AddLabel(20, 60, 0, $"Koszt wysyłki: {PostSystemControl.GetMailCost(mail, priority)}");
			AddLabel(20, 80, 0, $"Czas dostawy: {PostSystemControl.GetDeliveryDuration(mail, priority)}");
			AddLabel(20, 105, 0, "Priorytet:");
			
			var checkBoxIdNormal = priority ? 2153 : 2151;
			var checkBoxIdPressed = priority ? 2154 : 2152;
			AddButton(80, 100, checkBoxIdNormal, checkBoxIdPressed, 1, GumpButtonType.Reply, 0);
			
			int buttonsY = bgHeight - 50;
			AddButton(20, buttonsY, 5824, 5825, 2, GumpButtonType.Reply, 0);
			AddHtmlLocalized(55, buttonsY + 5, 75, 20, 3000164, false, false); // Wyslij

			AddButton(135, buttonsY, 5830, 5832, 0, GumpButtonType.Reply, 0);
			AddHtmlLocalized(170, buttonsY + 5, 75, 20, 1006045, false, false); // Anuluj
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			switch (info.ButtonID)
			{
				case 0:
					sender.Mobile.AddToBackpack(_Mail as Item);
					break;
				case 1: 
					sender.Mobile.SendGump(new SendMailGump(_Mail, !_Priority)); 
					break;
				case 2:
					PostSystemControl.Schedule(sender.Mobile, _Mail, _Priority);
					break;
			}
		}
	}
}
