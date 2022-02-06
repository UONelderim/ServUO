namespace Server.Items
{
	public class HumanSkull : Item
	{
		[Constructable]
		public HumanSkull()
			: base(Utility.Random(0x1AE2, 3))
		{
			Name = "ludzka czaszka";
			Weight = 1.0;
		}

		public HumanSkull(Serial serial)
			: base(serial)
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

	public class HumanBones : Item, IChopable
	{
		[Constructable]
		public HumanBones()
			: base(Utility.Random(0x1B13, 6))
		{
			Weight = 1.0;
		}

		public HumanBones(Serial serial)
			: base(serial)
		{
		}

		public void OnChop(Mobile from)
		{
			from.AddToBackpack(new Bone(Utility.RandomMinMax(1, 3)));
			from.PlaySound(0x48E);
			Delete();
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
