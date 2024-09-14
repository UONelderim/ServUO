using Server.Items;
using System;

namespace Server.Engines.Quests
{
	public class Annuin : MondainQuester
	{
		[Constructable]
		public Annuin()
			: base("Annuin", "- nauczyciel podstepnych sztuczek")
		{
		}

		public Annuin(Serial serial)
			: base(serial)
		{
		}

		public override Type[] Quests => new Type[] { typeof(RogueQuest) };

		public override void InitBody()
		{
			InitStats(100, 100, 25);

			Female = true;
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
