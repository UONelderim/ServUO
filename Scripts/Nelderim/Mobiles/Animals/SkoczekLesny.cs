namespace Server.Mobiles
{
	[CorpseName("zwloki Skoczka Leśnego")]
	public class SkoczekLesny : BaseMount
	{
		[Constructable]
		public SkoczekLesny() : this("Skoczek Leśny")
		{
		}

		[Constructable]
		public SkoczekLesny(string name) : base(name, 0xDB, 0x3EA5, AIType.AI_Animal, FightMode.Aggressor, 12, 1, 0.2, 0.4)
		{
			Hue = Utility.RandomSlimeHue() | 0x8000;

			BaseSoundID = 0x270;

			SetStr(376, 400);
			SetDex(91, 120);
			SetInt(291, 300);

			SetHits(226, 240);

			SetDamage(11, 30);

			SetDamageType(ResistanceType.Physical, 80);
			SetDamageType(ResistanceType.Energy, 10);
			SetDamageType(ResistanceType.Cold, 10);

			SetResistance(ResistanceType.Physical, 30, 40);
			SetResistance(ResistanceType.Fire, 70, 80);
			SetResistance(ResistanceType.Cold, 20, 30);
			SetResistance(ResistanceType.Poison, 30, 40);
			SetResistance(ResistanceType.Energy, 30, 40);

			SetSkill(SkillName.MagicResist, 80.0, 110.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Wrestling, 100.0);
			SetSkill(SkillName.Chivalry, 80.0);


			Fame = 4500;
			Karma = 5000;
			Hue = 551;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 110.0;
		}

		public override int Meat { get { return 2; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Ostard; } }

		public SkoczekLesny(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}