//Created by Milva

namespace Server.Items
{
	public class GingerbreadDough : Item
	{
		[Constructable]
		public GingerbreadDough()
		{
			Weight = 1.0;
			Name = "ciasto na pierniki";
			ItemID = 4159;
		}

		public GingerbreadDough(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
