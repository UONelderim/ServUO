#region References

using System;

#endregion

namespace Server.Items
{
	[FlipableAttribute(0x2413, 0x2414)]
	public class StillLifeLarge2 : Item
	{
		[Constructable]
		public StillLifeLarge2() : base(0x2411)
		{
			Weight = 3.0;
			Hue = 0;
		}

		[Constructable]
		public StillLifeLarge2(string artistName, string subject) : base(0x2413)
		{
			Name = String.Format("Malowidlo {0} stworzone przez {1}", subject, artistName);
			Weight = 3.0;
			Hue = 0;
		}

		public StillLifeLarge2(Serial serial) : base(serial)
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
