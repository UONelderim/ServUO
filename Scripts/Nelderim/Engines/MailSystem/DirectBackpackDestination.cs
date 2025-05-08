// -------------------------------
// File: DirectBackpackDestination.cs
// -------------------------------
using Server.Items;
using Server.Mobiles;
using Server.Mail;

namespace Server.Mail
{
    /// <summary>
    /// Fallbackowa destynacja: dostarcza przesyłkę bezpośrednio do plecaka odbiorcy,
    /// gdy nie ma dostępnej fizycznej skrzynki.
    /// </summary>
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
