namespace Server.Items
{
	public interface IGraveDiggingItem
	{
		bool IsGraveDiggingItem { get; set; }
	}

	public class GraveDiggingItem : Item, IDyable, IGraveDiggingItem
	{
		private string m_Desc = "wydobyte z grobu";

		public GraveDiggingItem(int itemID)
			: base(itemID)
		{
			int weight = ItemData.Weight;

			if (weight >= 255 || weight <= 0)
				weight = 1;

			Weight = weight;
		}

		[Constructable]
		public GraveDiggingItem()
			: base(Utility.RandomList(0x1CDD, 0x1CE5,
				0x1CE0, 0x1CE8, 0x1CE1, 0x1CE9, 0x1CE1, 0x1CE9,
				0x1CE2, 0x1CEC, 0x1E20, 0x1E21, 0xC16,
				0x1E24, 0x1E25))
		{
			int weight = ItemData.Weight;

			if (weight >= 255 || weight <= 0)
				weight = 1;

			Weight = weight;
		}

/*
        public override void AddNameProperties(ObjectPropertyList list)
        {
            list.Add("exhumed from a grave");
        }
*/

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public string Desc
		{
			get { return m_Desc; }
			set
			{
				m_Desc = value;
				InvalidateProperties();
			}
		}

		public GraveDiggingItem(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // version
			writer.Write(m_Desc);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_Desc = reader.ReadString();
		}

		public bool Dye(Mobile from, DyeTub sender)
		{
			if (Deleted)
				return false;

			if (ItemID >= 0x13A4 && ItemID <= 0x13AE)
			{
				Hue = sender.DyedHue;
				return true;
			}

			from.SendLocalizedMessage(sender.FailMessage);
			return false;
		}

		public bool IsGraveDiggingItem
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add(Desc);
		}
	}
}
