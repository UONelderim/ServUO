using Server.Mail;
using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Items
{
    [Flipable(0x4142, 0x4143)]
    public class BankMailBox : Item, IMailDestination
    {
		[Constructable]
        public BankMailBox() : base(0x4142)
        {
            Name = "Skrzynka pocztowa bankowa";
            Movable = false;
        }

        public Item ContainerItem => this;

        public bool CanAccept(MailItem mail, Mobile sender) => true;

        public void Accept(MailItem mail, Mobile sender)
        {
            mail.Recipient.AddToBackpack(mail);
            mail.Recipient.SendMessage($"Przesyłka z banku: od {mail.Sender.Name}");
        }
    }
}
