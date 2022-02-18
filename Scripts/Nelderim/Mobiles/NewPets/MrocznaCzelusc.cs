namespace Server.Mobiles
{
	[CorpseName("zwloki mrocznej czelusci")]
	public class MrocznaCzelusc : BaseCreature
	{
		public override double DifficultyScalar { get { return 1.15; } }

		[Constructable]
		public MrocznaCzelusc() : base(AIType.AI_NecroMage, FightMode.Weakest, 12, 1, 0.2, 0.4)
		{
			Name = "Mroczna Czelusc";
			Body = 0x30C;
			Hue = 2936;
			BaseSoundID = 602;

			SetStr(402, 480);
			SetDex(118, 156);
			SetInt(212, 252);

			SetHits(348, 400);

			SetDamage(13, 21);

			SetDamageType(ResistanceType.Physical, 80);
			SetDamageType(ResistanceType.Cold, 20);

			SetResistance(ResistanceType.Physical, 65, 80);
			SetResistance(ResistanceType.Fire, 45, 70);
			SetResistance(ResistanceType.Cold, 50, 55);
			SetResistance(ResistanceType.Poison, 55, 80);
			SetResistance(ResistanceType.Energy, 55, 62);


			SetSkill(SkillName.Meditation, 95.1, 110.0);
			SetSkill(SkillName.Necromancy, 110.1, 120.0);
			SetSkill(SkillName.MagicResist, 99.1, 100.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 120);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;

			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 99.9;
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 1);
			AddLoot(LootPack.Gems, 5);
			AddLoot(LootPack.MageryRegs, 10, 20);
		}

		public override int TreasureMapLevel { get { return 3; } }
		public override int Meat { get { return 10; } }
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Barbed; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override bool BardImmune { get { return false; } }
		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override Poison HitPoison { get { return Poison.Deadly; } }

		public MrocznaCzelusc(Serial serial) : base(serial)
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
