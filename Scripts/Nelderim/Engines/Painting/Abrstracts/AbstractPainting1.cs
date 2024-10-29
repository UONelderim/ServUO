using System;

namespace Server.Items
{
	[FlipableAttribute(0x2417, 0x2418)]
	public class AbstractPainting1 : Item
	{
		[Constructable]
		public AbstractPainting1(string artistName, string subject) : base(0x2417)
		{
			Weight = 3.0;
			Hue = 0;
			Name = String.Format("Abstrakcyjne malowidlo zatytulowane {0} stworzone przez {1}", subject, artistName);
		}


		public AbstractPainting1(Serial serial) : base(serial)
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
