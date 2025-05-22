using Server.Mail;
using Server.Multis;

namespace Server.Items
{
	[Flipable(0x4142, 0x4143)]
	public class HouseMailBox : Container, IMailDestination
	{
		[Constructable]
		public HouseMailBox() : base(0x4142)
		{
			Name = "Skrzynka pocztowa domowa";
			Movable = false;
		}

		public HouseMailBox(Serial serial) : base(serial) { }

		public Item ContainerItem => this;

		public bool CanAccept(MailItem mail, Mobile sender)
		{
			var house = BaseHouse.FindHouseAt(this);
			return house != null && house.IsOwner(sender);
		}

		public void Accept(MailItem mail, Mobile sender)
		{
			AddItem(mail);
			mail.Recipient.SendMessage($"Otrzymałeś przesyłkę od {mail.Sender.Name}.");
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
