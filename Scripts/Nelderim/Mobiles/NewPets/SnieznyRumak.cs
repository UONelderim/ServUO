namespace Server.Mobiles
{
	[CorpseName("zwloki Śnieżnego rumaka")]
	public class SnieznyRumak : BaseMount
	{
		[Constructable]
		public SnieznyRumak() : this("śnieżny rumak")
		{
		}

		[Constructable]
		public SnieznyRumak(string name) : base(name, 0xE4, 0x3EA1, AIType.AI_Melee, FightMode.Aggressor, 12, 1, 0.2,
			0.4)
		{
			SetStr(94, 170);
			SetDex(96, 115);
			SetInt(6, 10);

			SetHits(71, 110);
			SetMana(0);

			SetDamage(11, 17);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 25, 30);
			SetResistance(ResistanceType.Cold, 70, 75);
			SetResistance(ResistanceType.Poison, 20, 25);
			SetResistance(ResistanceType.Energy, 20, 25);

			SetSkill(SkillName.MagicResist, 75.1, 80.0);
			SetSkill(SkillName.Tactics, 79.3, 94.0);
			SetSkill(SkillName.Wrestling, 79.3, 94.0);

			Fame = 1500;
			Karma = -1500;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 29.1;
			Hue = 1150;
		}

		public SnieznyRumak(Serial serial) : base(serial)
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
	}
}
