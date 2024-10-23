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
			SetWearable(new DragonTurtleHideArms(), 2680, dropChance: 1);
			SetWearable(new DaggerBelt(), 2680, dropChance: 1);
			SetWearable(new LeatherNinjaJacket(), 1672, dropChance: 1);
			SetWearable(new LeatherNinjaPants(), 1672, dropChance: 1);
			SetWearable(new Boots(), 2680, dropChance: 1);
			SetWearable(new Cloak(), 2680, dropChance: 1);
			SetWearable(new MagesHood(), 2680, dropChance: 1);
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
