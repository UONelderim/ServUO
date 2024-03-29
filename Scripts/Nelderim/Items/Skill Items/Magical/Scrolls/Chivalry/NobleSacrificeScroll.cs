namespace Server.Items
{
	public class NobleSacrificeScroll : ChivalrySpellScroll
	{
		[Constructable]
		public NobleSacrificeScroll() : this(1)
		{
		}

		[Constructable]
		public NobleSacrificeScroll(int amount) : base(207, 0x1F6D, amount)
		{
			Name = "Ostateczny wysilek";
		}

		public NobleSacrificeScroll(Serial serial) : base(serial)
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
