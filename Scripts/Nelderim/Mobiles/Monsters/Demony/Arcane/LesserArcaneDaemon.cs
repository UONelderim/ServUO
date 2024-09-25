#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki mniejszego demona cienia")]
	public class LesserArcaneDaemon : BaseCreature
	{
		[Constructable]
		public LesserArcaneDaemon() : base(AIType.AI_Mage, FightMode.Closest, 12, 2, 0.2, 0.4)
		{
			Name = "mniejszy demon cienia";
			Body = 4;
			Hue = 2106;
			BaseSoundID = 0x47D;

			SetStr(431, 450);
			SetDex(146, 165);
			SetInt(301, 350);

			SetHits(301, 315);

			SetDamage(12, 18);

			SetDamageType(ResistanceType.Physical, 80);
			SetDamageType(ResistanceType.Fire, 20);

			SetResistance(ResistanceType.Physical, 55, 65);
			SetResistance(ResistanceType.Fire, 75, 85);
			SetResistance(ResistanceType.Cold, 15, 25);
			SetResistance(ResistanceType.Poison, 55, 65);
			SetResistance(ResistanceType.Energy, 35, 45);

			SetSkill(SkillName.MagicResist, 105.1, 115.0);
			SetSkill(SkillName.Tactics, 70.1, 80.0);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);
			SetSkill(SkillName.Magery, 100.1, 110.0);
			SetSkill(SkillName.EvalInt, 100.1, 110.0);
			SetSkill(SkillName.Meditation, 70.1, 80.0);

			Fame = 7000;
			Karma = -10000;

			VirtualArmor = 55;

			SetWeaponAbility(WeaponAbility.ConcussionBlow);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.02)
					corpse.DropItem(new Bloodspawn());
				if (Utility.RandomDouble() < 0.10)
					corpse.DropItem(new DaemonBone());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich);
			AddLoot(LootPack.HighScrolls);
		}

		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override double AttackMasterChance { get { return 0.65; } }
		public override double SwitchTargetChance { get { return 0.25; } }
		public override bool BardImmune { get { return false; } }

		public LesserArcaneDaemon(Serial serial) : base(serial)
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
