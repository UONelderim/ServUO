namespace Server.Mobiles
{
	[CorpseName("zwloki szkarlatnego rumaka")]
	public class MorrlokWarHorse : BaseMount
	{
		[Constructable]
		public MorrlokWarHorse() : base("szkarlatny rumak", 0x78, 0x3EAF, AIType.AI_Melee, FightMode.Closest, 10, 1,
			0.2, 0.4)
		{
			BaseSoundID = 0xA8;

			InitStats(Utility.Random(300, 100), 125, 60);

			SetHits(240);
			SetMana(0);

			SetDamage(5, 8);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Fire, 30, 40);
			SetResistance(ResistanceType.Cold, 30, 40);
			SetResistance(ResistanceType.Poison, 30, 40);
			SetResistance(ResistanceType.Energy, 30, 40);

			SetSkill(SkillName.MagicResist, 25.1, 30.0);
			SetSkill(SkillName.Tactics, 29.3, 44.0);
			SetSkill(SkillName.Wrestling, 29.3, 44.0);

			Fame = 300;
			Karma = 300;
		}

		public MorrlokWarHorse(Serial serial) : base(serial)
		{
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

		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public override void OnDoubleClick(Mobile from)
		{
			if (from.AccessLevel > AccessLevel.Player)
				base.OnDoubleClick(from);
			else
				OnDisallowedRider(from);
		}
	}
}
