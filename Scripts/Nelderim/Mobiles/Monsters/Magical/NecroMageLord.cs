namespace Server.Mobiles
{
	[CorpseName("zwloki lorda nekromanty")]
	public class NecroMageLord : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.15; } }

		[Constructable]
		public NecroMageLord() : base(AIType.AI_NecroMage, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = NameList.RandomName("demon knight");
			Title = "Nekromag";
			Body = 146;
			BaseSoundID = 357;
			Hue = 0x151;

			SetStr(416, 505);
			SetDex(146, 165);
			SetInt(566, 655);

			SetHits(2000);
			SetMana(2000);

			SetDamage(10, 15);

			SetDamageType(ResistanceType.Physical, 70);
			SetDamageType(ResistanceType.Cold, 15);
			SetDamageType(ResistanceType.Energy, 15);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Fire, 30, 40);
			SetResistance(ResistanceType.Cold, 50, 60);
			SetResistance(ResistanceType.Poison, 50, 60);
			SetResistance(ResistanceType.Energy, 40, 50);

			SetSkill(SkillName.EvalInt, 77.6, 87.5);
			SetSkill(SkillName.Necromancy, 100.6, 120.5);
			SetSkill(SkillName.SpiritSpeak, 110.1, 120.5);
			SetSkill(SkillName.Magery, 90.1, 100.1);
			SetSkill(SkillName.Poisoning, 80.5);
			SetSkill(SkillName.Meditation, 110.0);
			SetSkill(SkillName.MagicResist, 80.1, 85.0);
			SetSkill(SkillName.Parry, 90.1, 95.1);
			SetSkill(SkillName.Tactics, 120.0);
			SetSkill(SkillName.Wrestling, 70.1, 80.0);

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 65;
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 3);
			AddLoot(LootPack.Rich, 4);
			AddLoot(LootPack.NecroRegs, 20, 40);
		}

		public override bool CanRummageCorpses { get { return true; } }
		public override bool AutoDispel => false;
		public override bool AlwaysMurderer { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override bool Uncalmable { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int TreasureMapLevel { get { return 5; } }

		public NecroMageLord(Serial serial) : base(serial)
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
