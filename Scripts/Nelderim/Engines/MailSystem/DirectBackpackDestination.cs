namespace Server.Mail
{
	public class DirectBackpackDestination : IMailDestination
	{
		private readonly Mobile _recipient;

		public DirectBackpackDestination(Mobile recipient)
		{
			_recipient = recipient;
		}

		public Item ContainerItem => _recipient.Backpack;

		public bool CanAccept(MailItem mail, Mobile sender) => true;

		public void Accept(MailItem mail, Mobile sender)
		{
			_recipient.AddToBackpack(mail);
			_recipient.SendMessage("Brak skrzynki – przesyłka została doręczona do Twojego ekwipunku.");
		}
	}
}
