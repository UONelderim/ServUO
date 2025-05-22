using System;

namespace Server.Mail
{
	public abstract class MailItem : Item
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Sender { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Recipient { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Cost { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan DeliveryDelay { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime SentTime { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public IMailDestination Destination { get; set; }

		protected MailItem(int itemID) : base(itemID) { }
		public MailItem(Serial serial) : base(serial) { }

		public void ProcessSend()
		{
			if (Destination == null)
				throw new InvalidOperationException("MailItem has no Destination set.");

			double distance = MailProcessor.GetDistance(Sender.Location, Destination.ContainerItem.Location);
			double weight = Weight;

			Cost = MailProcessor.CalculateCost(distance, weight);
			DeliveryDelay = MailProcessor.CalculateDelay(distance, weight);
			SentTime = DateTime.Now;

			Sender.SendMessage(
				$"Przesyłka wyceniona na {Cost} szt. i dostarczona za około {DeliveryDelay.TotalSeconds:N0} sek.");
			MailProcessor.ScheduleDelivery(this);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
