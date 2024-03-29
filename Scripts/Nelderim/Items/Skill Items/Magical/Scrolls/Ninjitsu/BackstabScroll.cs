namespace Server.Items
{
	public class BackstabScroll : NinjitsuSpellScroll
	{
		[Constructable]
		public BackstabScroll() : this(1)
		{
		}

		[Constructable]
		public BackstabScroll(int amount) : base(505, 0x1F70, amount)
		{
			Name = "Backstab";
		}

		public BackstabScroll(Serial serial) : base(serial)
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
