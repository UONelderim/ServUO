using Server.Items;
using System;

namespace Server.Engines.Quests
{
	public class Laxannuin : MondainQuester
	{
		[Constructable]
		public Laxannuin()
			: base("Laxannuin", "- okultysta")
		{
		}

		public Laxannuin(Serial serial)
			: base(serial)
		{
		}

		public override Type[] Quests => new Type[] { typeof(UndeadQuest) };

		public override void InitBody()
		{
			InitStats(100, 100, 25);

			Female = false;
			Race = Race.NTamael;
		}

		public override void InitOutfit()
		{
			SetWearable(new Boots(), 0x901, 1);
			SetWearable(new Robe(), 2129, dropChance: 1);
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
