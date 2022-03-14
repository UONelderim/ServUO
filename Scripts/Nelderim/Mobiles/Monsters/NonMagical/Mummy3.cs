#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("resztki mumii")]
	public class Mummy3 : BaseCreature
	{
		public override bool BleedImmune { get { return true; } }

		[Constructable]
		public Mummy3() : base(AIType.AI_Melee, FightMode.Weakest, 9, 1, 0.4, 0.8)
		{
			Name = "mumia";
			Body = 154;
			BaseSoundID = 471;
			Hue = 2102;

			SetStr(546, 570);
			SetDex(120, 140);
			SetInt(40, 60);

			SetHits(700, 800);

			SetDamage(28, 34);

			SetDamageType(ResistanceType.Physical, 40);
			SetDamageType(ResistanceType.Cold, 60);

			SetResistance(ResistanceType.Physical, 45, 55);
			SetResistance(ResistanceType.Fire, 10, 20);
			SetResistance(ResistanceType.Cold, 50, 60);
			SetResistance(ResistanceType.Poison, 120, 130);
			SetResistance(ResistanceType.Energy, 20, 30);

			SetSkill(SkillName.MagicResist, 15.1, 40.0);
			SetSkill(SkillName.Tactics, 35.1, 50.0);
			SetSkill(SkillName.Wrestling, 35.1, 50.0);

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 50;

			PackItem(new Garlic(10));
			PackItem(new Bandage(10));
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich);
			AddLoot(LootPack.Potions);
			AddLoot(LootPack.NecroRegs, 10);
		}

		public override Poison PoisonImmune { get { return Poison.Deadly; } }

		public Mummy3(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
