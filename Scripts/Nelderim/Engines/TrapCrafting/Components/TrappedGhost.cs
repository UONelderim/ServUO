namespace Server.Items
{
	[Flipable(0x1053, 0x1054)]
	public class TrappedGhost : Item
	{
		[Constructable]
		public TrappedGhost() : this(1)
		{
		}

		[Constructable]
		public TrappedGhost(int amount) : base(0xF0E)
		{
			Stackable = true;
			Amount = amount;
			Weight = 2.0;
			Name = "uwieziony duch";
		}

		public TrappedGhost(Serial serial) : base(serial)
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
