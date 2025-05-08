// -------------------------------
// File: PostBox.cs (updated)
// -------------------------------
using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Mail;

namespace Server.Engines.MailSystem
{
    public class PostBox : Item, IMailDestination
    {
        [Constructable]
        public PostBox() : base(0xE3B) // example itemID
        {
            Name = "skrzyneczka pocztowa";
            Movable = false;
        }

        public PostBox(Serial serial) : base(serial) { }

        public Item ContainerItem => this;

        public bool CanAccept(MailItem mail, Mobile sender)
        {
            // Bank boxes accept all
            return true;
        }

        public void Accept(MailItem mail, Mobile sender)
        {
            // Deliver to recipient's home mailbox or bank
            mail.Recipient.SendMessage($"Otrzymałeś przesyłkę od {mail.Sender.Name}.");
            mail.Recipient.AddToBackpack(mail);
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is MailItem mail)
            {
                // Ensure the dropper is the sender
                if (mail.Sender != from)
                {
                    from.SendMessage("Nie możesz wysłać czyjejś przesyłki.");
                    return false;
                }

                // Find best destination
                IMailDestination dest = MailUtility.FindBestDestination(mail.Recipient, from);

                // Assign and process
                mail.Destination = dest;
                mail.ProcessSend();

                return true;
            }

            return base.OnDragDrop(from, dropped);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
