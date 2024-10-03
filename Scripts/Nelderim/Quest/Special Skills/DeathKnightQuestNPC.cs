using Server.Items;
using System;

namespace Server.Engines.Quests
{
	public class EroganDrath : MondainQuester
	{
		[Constructable]
		public EroganDrath()
			: base("Erogan Drath", "- Mroczny Rycerz")
		{
		}

		public EroganDrath(Serial serial)
			: base(serial)
		{
		}

		public override Type[] Quests => new Type[] { typeof(DeathKnightQuest) };

		public override void InitBody()
		{
			InitStats(100, 100, 25);

			Female = false;
			Race = Race.NTamael;

			HairHue = 1150;
		}

		public override void InitOutfit()
		{
			SetWearable(new Boots(), 0x901, 1);
			SetWearable(new NorseHelm(),1151, dropChance: 1);
			SetWearable(new PlateArms(),1151, dropChance: 1);
			SetWearable(new PlateChest(), 1151, 1);
			SetWearable(new PlateLegs(), 1151, 1);
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
