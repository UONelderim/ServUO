namespace Server.Mail
{
	public interface IMailDestination
	{
		bool CanAccept(MailItem mail, Mobile sender);
		void Accept(MailItem mail, Mobile sender);
		Item ContainerItem { get; }
	}
}
