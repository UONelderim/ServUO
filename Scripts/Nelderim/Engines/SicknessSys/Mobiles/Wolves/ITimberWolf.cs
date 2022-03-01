#region References

using Server.Mobiles;

#endregion

namespace Server.SicknessSys.Mobiles
{
	[CorpseName("zwloki zarazonego lesnego wilka")]
	public class ITimberWolf : InfectedWolf
	{
		[Constructable]
		public ITimberWolf() : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			Name = "zarazony lesny wilk";
			Body = 225;
			BaseSoundID = 0xE5;

			SetStr(56, 80);
			SetDex(56, 75);
			SetInt(11, 25);

			SetHits(34, 48);
			SetMana(0);

			SetDamage(5, 9);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 15, 20);
			SetResistance(ResistanceType.Fire, 5, 10);
			SetResistance(ResistanceType.Cold, 10, 15);
			SetResistance(ResistanceType.Poison, 5, 10);
			SetResistance(ResistanceType.Energy, 5, 10);

			SetSkill(SkillName.MagicResist, 27.6, 45.0);
			SetSkill(SkillName.Tactics, 30.1, 50.0);
			SetSkill(SkillName.Wrestling, 40.1, 60.0);

			Fame = 450;
			Karma = 0;

			VirtualArmor = 16;

			Tamable = false;
		}

		public ITimberWolf(Serial serial) : base(serial)
		{
		}

		public override int Meat
		{
			get
			{
				return 1;
			}
		}

		public override int Hides
		{
			get
			{
				return 5;
			}
		}

		public override FoodType FavoriteFood
		{
			get
			{
				return FoodType.Meat;
			}
		}

		public override PackInstinct PackInstinct
		{
			get
			{
				return PackInstinct.Canine;
			}
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
