#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki mnijeszego demona zaglady")]
	public class LesserMoloch : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.15; } }

		[Constructable]
		public LesserMoloch() : base(AIType.AI_Melee, FightMode.Strongest, 11, 1, 0.2, 0.4)
		{
			Name = "mniejszy demon zaglady";
			Body = 31;
			Hue = 2119;
			BaseSoundID = 0x300;

			SetStr(131, 160);
			SetDex(66, 85);
			SetInt(41, 65);

			SetHits(71, 100);

			SetDamage(10, 14);

			SetResistance(ResistanceType.Physical, 60, 70);
			SetResistance(ResistanceType.Fire, 60, 70);
			SetResistance(ResistanceType.Cold, 40, 50);
			SetResistance(ResistanceType.Poison, 20, 30);
			SetResistance(ResistanceType.Energy, 20, 30);

			SetSkill(SkillName.MagicResist, 65.1, 75.0);
			SetSkill(SkillName.Tactics, 75.1, 90.0);
			SetSkill(SkillName.Wrestling, 70.1, 90.0);

			Fame = 7500;
			Karma = -7500;

			VirtualArmor = 32;

			SetWeaponAbility(WeaponAbility.WhirlwindAttack);
		}

		public override bool BardImmune { get { return false; } }

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.05)
					corpse.DropItem(new Bloodspawn());
				if (Utility.RandomDouble() < 0.10)
					corpse.DropItem(new DaemonBone());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich);
		}

		public override Poison PoisonImmune { get { return Poison.Regular; } }

		public LesserMoloch(Serial serial) : base(serial)
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
