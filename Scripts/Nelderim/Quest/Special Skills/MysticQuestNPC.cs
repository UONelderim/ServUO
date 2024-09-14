using Server.Items;
using System;

namespace Server.Engines.Quests
{
	public class Matheo : MondainQuester
	{
		[Constructable]
		public Matheo()
			: base("Matheo", "- Mistyk")
		{
		}

		public Matheo(Serial serial)
			: base(serial)
		{
		}

		public override Type[] Quests => new Type[] { typeof(MysticQuest) };

		public override void InitBody()
		{
			InitStats(100, 100, 25);

			Female = false;
			Race = Race.NTamael;
		}

		public override void InitOutfit()
		{
			SetWearable(new Boots(), 0x901, 1);
			SetWearable(new Robe(), dropChance: 1);
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
