using System;

namespace Server.Items
{
	[FlipableAttribute(0x2887, 0x2886)]
	public class AbstractPainting3 : Item
	{
		[Constructable]
		public AbstractPainting3(string artistName, string subject) : base(0x2887)
		{
			Name = String.Format("Abstrakcyjne malowidlo zatytulowane {0} stworzone przez {1}", subject, artistName);
			Weight = 3.0;
			Hue = 0;
		}

		public AbstractPainting3(Serial serial) : base(serial)
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
