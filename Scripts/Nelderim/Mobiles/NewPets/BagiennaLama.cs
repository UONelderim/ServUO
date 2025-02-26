// 09.06.20 :: juri :: utworzenie
// 09.08.31 :: juri :: zmiana basesoundid na dzwiek lamy

namespace Server.Mobiles
{
	[CorpseName("zwloki bagiennej lamy bojowej")]
	public class BagiennaLama : BaseMount
	{
		[Constructable]
		public BagiennaLama() : this("bagienna lama bojowa")
		{
		}

		[Constructable]
		public BagiennaLama(string name) : base(name, 0xDC, 0x3EA6, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2,
			0.4)
		{
			BaseSoundID = 0x3F3;
			Hue = 1388;

			SetStr(400);
			SetDex(120);
			SetInt(50);

			SetHits(250);
			SetMana(0);


			SetDamage(5, 7);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Poison, 50);

			SetResistance(ResistanceType.Physical, 35, 45);
			SetResistance(ResistanceType.Fire, 30, 40);
			SetResistance(ResistanceType.Cold, 30, 40);
			SetResistance(ResistanceType.Poison, 50, 66);
			SetResistance(ResistanceType.Energy, 30, 40);

			SetSkill(SkillName.MagicResist, 30.0, 40.0);
			SetSkill(SkillName.Tactics, 40.0, 50.0);
			SetSkill(SkillName.Wrestling, 40.0, 50.0);

			Fame = 500;
			Karma = 500;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 29.1;
		}

		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public override bool BardImmune { get { return false; } }

		public override double GetControlChance(Mobile m, bool useBaseSkill)
		{
			AbilityProfile profile = PetTrainingHelper.GetAbilityProfile(this);

			if (profile != null && profile.HasCustomized())
			{
				return base.GetControlChance(m, useBaseSkill);
			}
			return 1.0;
		}

		public BagiennaLama(Serial serial) : base(serial)
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
