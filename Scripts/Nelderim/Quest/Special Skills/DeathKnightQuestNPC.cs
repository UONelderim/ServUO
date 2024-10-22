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
			SetWearable(new DragonArms(), 2680, dropChance: 1);
			SetWearable(new DragonHelm(), 2680, dropChance: 1);
			SetWearable(new DragonLegs(), 2680, dropChance: 1);
			SetWearable(new DragonGloves(), 2680, dropChance: 1);
			SetWearable(new DragonChest(), 2680, dropChance: 1);
			SetWearable(new LeafGorget(), 2886, dropChance: 1);
			SetWearable(new Cloak(), 2894, dropChance: 1);
			SetWearable(new Boots(), 2894, dropChance: 1);
			SetWearable(new SwordBelt(), 2680, dropChance: 1);
			SetWearable(new Epaulette(), 2680, dropChance: 1);
			SetWearable(new Lantern(), 2980, dropChance: 1);
			SetWearable(new BroadSword(), 2680, dropChance: 1);
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
