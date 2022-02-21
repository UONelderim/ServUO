#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki minotaura lorda")]
	public class MinotaurLord : BaseCreature
	{
		[Constructable]
		public MinotaurLord() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "minotaur lord";
			Body = 281;
			BaseSoundID = 367;

			SetStr(436, 465);
			SetDex(116, 125);
			SetInt(31, 85);

			SetHits(582, 599);

			SetDamage(17, 27);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 55, 60);
			SetResistance(ResistanceType.Fire, 45, 65);
			SetResistance(ResistanceType.Cold, 40, 60);
			SetResistance(ResistanceType.Poison, 35, 65);
			SetResistance(ResistanceType.Energy, 35, 65);

			SetSkill(SkillName.MagicResist, 80.1, 95.0);
			SetSkill(SkillName.Tactics, 100.1, 110.0);
			SetSkill(SkillName.Wrestling, 100.1, 110.0);

			Fame = 9000;
			Karma = -9000;

			VirtualArmor = 58;

			SetWeaponAbility(WeaponAbility.CrushingBlow);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 1);
			AddLoot(LootPack.Gems, 4);
		}

		public override double AttackMasterChance { get { return 0.15; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override int TreasureMapLevel { get { return 1; } }
		public override int Meat { get { return 4; } }

		public MinotaurLord(Serial serial) : base(serial)
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
