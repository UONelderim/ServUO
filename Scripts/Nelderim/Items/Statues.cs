namespace Server.Items
{
	public class GargulecStatua : Item
	{
		[Constructable]
		public GargulecStatua() : base(0x0499)
		{
			Weight = 10;
			Name = "statuetka gargulca";
		}

		public GargulecStatua(Serial serial) : base(serial)
		{
		}

		public override bool ForceShowProperties => true;

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class WojownikStatua : Item
	{
		[Constructable]
		public WojownikStatua() : base(0x05EE)
		{
			Weight = 10;
			Name = "statuetka wojownika";
		}

		public WojownikStatua(Serial serial) : base(serial)
		{
		}

		public override bool ForceShowProperties => true;

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class KamiennaWaza : Item
	{
		[Constructable]
		public KamiennaWaza() : base(0x08AB)
		{
			Weight = 10;
			Name = "Kamienna Waza";
		}

		public KamiennaWaza(Serial serial) : base(serial)
		{
		}

		public override bool ForceShowProperties => true;

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class Nagrobek : Item
	{
		[Constructable]
		public Nagrobek() : base(0x0ED8)
		{
			Weight = 10;
			Name = "Nagrobek";
		}

		public Nagrobek(Serial serial) : base(serial)
		{
		}

		public override bool ForceShowProperties => true;

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
