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
			SetWearable(new Scythe(), 2888, dropChance: 1);
			SetWearable(new Obi(), 2883, dropChance: 1);
			SetWearable(new HoodedShroudOfShadows(), 2882, dropChance: 1);
			SetWearable(new BoneGloves(), 2883, dropChance: 1);
			SetWearable(new Boots(), 2882, dropChance: 1);
			SetWearable(new Cloak(), 2882, dropChance: 1);
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
