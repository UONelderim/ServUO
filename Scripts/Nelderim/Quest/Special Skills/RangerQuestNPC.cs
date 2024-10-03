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
			SetWearable(new Boots(), 0x901, 1);
			SetWearable(new ElvenShirt(), 2129, dropChance: 1);
			SetWearable(new ElvenPants(), 1153, dropChance: 1);
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
