using Server.Items;
using System;

namespace Server.Engines.Quests
{
	public class Lillianna : MondainQuester
	{
		[Constructable]
		public Lillianna()
			: base("Lillianna", "- Nauczycielka Magii Natury")
		{
		}

		public Lillianna(Serial serial)
			: base(serial)
		{
		}

		public override Type[] Quests => new Type[] { typeof(NatureQuest) };

		public override void InitBody()
		{
			InitStats(100, 100, 25);

			Female = true;
			Race = Race.NElf;
		}

		public override void InitOutfit()
		{
			SetWearable(new Cloak(), 2527, dropChance: 1);
			SetWearable(new WildStaff(), 2537, dropChance: 1);
			SetWearable(new GargishNecklace(), 0, dropChance: 1);
			SetWearable(new HawkwindsRobe(), 2538, dropChance: 1);
			SetWearable(new Boots(), 0, dropChance: 1);
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
