using Server.Mail;
using Server.Gumps;

namespace Server.Items
{
	public class PlayerLetter : MailItem
	{
		public string BodyText;
		private bool m_Replied;
		private bool m_Read;

		public bool Replied
		{
			get => m_Replied;
			set => m_Replied = value;
		}

		[Constructable]
		public PlayerLetter() : base(0xE35)
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			Movable = true;
			Hue = 1150;
			Name = "zalakowana wiadomosc";
		}

		public PlayerLetter(Serial serial) : base(serial) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (from == Recipient || from == Sender)
			{
				from.SendGump(new LetterGump(from, BodyText, Sender, this));
				m_Read = true;
				Hue = 1102;
				Name = Name;
			}
			else
				from.SendMessage("Ten list jest zalakowany. Nie mozesz go otworzyc!");
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add(1060658, "{0}\t{1}", "Od", Sender.Name);
			list.Add(1060659, "{0}\t{1}", "Do", Recipient.Name);
			list.Add(1060660, "{0}\t{1}", "Wyslano", SentTime.ToString("HH:mm dd/MM/yyyy"));
			list.Add(1060661, "{0}\t{1}", "Odczytano", m_Read);
			list.Add(1060662, "{0}\t{1}", "Odpowiedziano", m_Replied);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
			writer.Write(m_Read);
			writer.Write(m_Replied);
			writer.Write(BodyText);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_Read = reader.ReadBool();
			m_Replied = reader.ReadBool();
			BodyText = reader.ReadString();
		}
	}
}
