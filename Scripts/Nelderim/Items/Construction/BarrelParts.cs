using System;

namespace Server.Items
{
    public class BarrelLidRC : ResouceCraftable
	{
		[Constructable]
		public BarrelLidRC() : base(0x1DB8)
		{
			Weight = 2;
		}

		public BarrelLidRC(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	[FlipableAttribute(0x1EB1, 0x1EB2, 0x1EB3, 0x1EB4)]
    public class BarrelStavesRC : ResouceCraftable
	{
		[Constructable]
		public BarrelStavesRC() : base(0x1EB1)
		{
			Weight = 1;
		}

		public BarrelStavesRC(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

}