namespace Server.Items
{
	[FlipableAttribute(0xA5DC)]
	public class KostkaAncientQuestItem : Item
	{
		[Constructable]
		public KostkaAncientQuestItem() : this(0)
		{
		}

		[Constructable]
		public KostkaAncientQuestItem(int hue) : base(0xA5DB)
		{
			Weight = 20.0;
			Name = "Pierwsza Czesc Starozytnej Kosci";
			Label1 = "*przedmiot zadania Starozytnej Magii*";
		}

		public KostkaAncientQuestItem(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
	
	[FlipableAttribute(0xA5DC)]
	public class KostkaAncientQuestItem2 : Item
	{
		[Constructable]
		public KostkaAncientQuestItem2() : this(0)
		{
		}

		[Constructable]
		public KostkaAncientQuestItem2(int hue) : base(0xA5DC)
		{
			Weight = 20.0;
			Name = "Druga Czesc Starozytnej Kosci";
			Label1 = "*przedmiot zadania Starozytnej Magii*";
		}

		public KostkaAncientQuestItem2(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
