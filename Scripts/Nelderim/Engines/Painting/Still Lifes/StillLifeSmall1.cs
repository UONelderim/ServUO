using System;

namespace Server.Items
{
	[FlipableAttribute(0x240D, 0x240E)]
	public class StillLifeSmall1 : Item
	{
		[Constructable]
		public StillLifeSmall1() : base(0x240D)
		{
			Weight = 3.0;
			Hue = 0;
		}

		[Constructable]
		public StillLifeSmall1(string artistName, string subject) : base(0x240D)
		{
			Name = String.Format("Malowidlo {0} stworzone przez {1}", subject, artistName);
			Weight = 3.0;
			Hue = 0;
		}

		public StillLifeSmall1(Serial serial) : base(serial)
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
