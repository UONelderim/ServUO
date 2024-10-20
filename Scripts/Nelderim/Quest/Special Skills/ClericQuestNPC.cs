using Server.Items;
using System;

namespace Server.Engines.Quests
{
	public class Orion : MondainQuester
	{
		[Constructable]
		public Orion()
			: base("Orion", "- Herdeista")
		{
		}

		public Orion(Serial serial)
			: base(serial)
		{
		}

		public override Type[] Quests => new Type[] { typeof(ClericQuest) };

		public override void InitBody()
		{
			InitStats(100, 100, 25);

			Female = false;
			Race = Race.NTamael;

			HairHue = 1150;
		}

		public override void InitOutfit()
		{
			SetWearable(new PlateArms(), 0, dropChance: 1);
			SetWearable(new PlateGloves(), 0, dropChance: 1);
			SetWearable(new PlateGorget(), 0, dropChance: 1);
			SetWearable(new PlateLegs(), 0, dropChance: 1);
			SetWearable(new ChainChest(), 0, dropChance: 1);
			SetWearable(new OrderShield(), 0, dropChance: 1);
			SetWearable(new SwordBelt(), 0, dropChance: 1);
			SetWearable(new HumilityCloak(), 2150, dropChance: 1);
			SetWearable(new VikingSword(), 0, dropChance: 1);
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
