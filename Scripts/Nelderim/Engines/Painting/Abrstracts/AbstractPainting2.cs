using System;

namespace Server.Items
{
	[FlipableAttribute(0x2415, 0x2416)]
	public class AbstractPainting2 : Item
	{
		[Constructable]
		public AbstractPainting2(string artistName, string subject) : base(0x2415)
		{
			Name = String.Format("Abstrakcyjne malowidlo zatytulowane {0} stworzone przez {1}", subject, artistName);
			Weight = 3.0;
			Hue = 0;
		}

		public AbstractPainting2(Serial serial) : base(serial)
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
