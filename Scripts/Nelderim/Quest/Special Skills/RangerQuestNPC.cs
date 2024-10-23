using Server.Items;
using System;

namespace Server.Engines.Quests
{
	public class Tarak : MondainQuester
	{
		[Constructable]
		public Tarak()
			: base("Tarak", "- Straznik Lesny")
		{
		}

		public Tarak(Serial serial)
			: base(serial)
		{
		}

		public override Type[] Quests => new Type[] { typeof(RangerQuest) };

		public override void InitBody()
		{
			InitStats(100, 100, 25);

			Female = true;
			Race = Race.NTamael;
		}

		public override void InitOutfit()
		{
			SetWearable(new CompositeBow(), 1421, dropChance: 1);
			SetWearable(new Epaulette(), 1421, dropChance: 1);
			SetWearable(new QuiverOfInfinity(), 0, dropChance: 1);
			SetWearable(new LeafChest(), 1190, dropChance: 1);
			SetWearable(new Boots(), 1421, dropChance: 1);
			SetWearable(new DragonTurtleHideArms(), 1190, dropChance: 1);
			SetWearable(new DragonTurtleHideLegs(), 0, dropChance: 1);
			SetWearable(new LeafGloves(), 1421, dropChance: 1);
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
