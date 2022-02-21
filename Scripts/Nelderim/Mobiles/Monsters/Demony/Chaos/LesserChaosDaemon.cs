#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki mniejszego demona chaosu")]
	public class LesserChaosDaemon : BaseCreature
	{
		[Constructable]
		public LesserChaosDaemon() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "mniejszy demon chaosu";
			Body = 304;
			Hue = 1572;
			BaseSoundID = 0x3E9;

			SetStr(266, 330);
			SetDex(171, 200);
			SetInt(56, 80);

			SetHits(291, 310);

			SetDamage(22, 27);

			SetDamageType(ResistanceType.Physical, 85);
			SetDamageType(ResistanceType.Fire, 15);

			SetResistance(ResistanceType.Physical, 60, 65);
			SetResistance(ResistanceType.Fire, 60, 75);
			SetResistance(ResistanceType.Cold, 40, 50);
			SetResistance(ResistanceType.Poison, 20, 30);
			SetResistance(ResistanceType.Energy, 20, 30);

			SetSkill(SkillName.MagicResist, 85.1, 95.0);
			SetSkill(SkillName.Tactics, 80.1, 90.0);
			SetSkill(SkillName.Wrestling, 95.1, 110.0);

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 45;

			SetWeaponAbility(WeaponAbility.CrushingBlow);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.04)
					corpse.DropItem(new Bloodspawn());
				if (Utility.RandomDouble() < 0.10)
					corpse.DropItem(new DaemonBone());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich);
		}

		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override bool BardImmune { get { return false; } }

		public LesserChaosDaemon(Serial serial) : base(serial)
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
