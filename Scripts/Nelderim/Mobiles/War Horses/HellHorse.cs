// 09.06.28 :: juri :: utworzenie

namespace Server.Mobiles
{
	[CorpseName("zwloki piekielnego rumaka")]
	public class HellHorse : BaseMount
	{
		[Constructable]
		public HellHorse() : this("piekielny rumak")
		{
		}

		[Constructable]
		public HellHorse(string name) : base(name, 0x319, 0x3EBB, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			BaseSoundID = 0xA8;

			SetStr(250);
			SetDex(90);
			SetInt(50);

			SetHits(180);
			SetMana(0);


			SetDamage(5, 7);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 35, 45);
			SetResistance(ResistanceType.Fire, 30, 40);
			SetResistance(ResistanceType.Cold, 30, 40);
			SetResistance(ResistanceType.Poison, 30, 40);
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

		public override bool BleedImmune { get { return true; } }

		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public override bool BardImmune { get { return false; } }

		public override double GetControlChance(Mobile m, bool useBaseSkill)
		{
			AbilityProfile profile = PetTrainingHelper.GetAbilityProfile(this);

			if (profile != null && profile.HasCustomized())
			{
				return base.GetControlChance(m, useBaseSkill);
			}
			
			double skill = (useBaseSkill ? m.Skills.Necromancy.Base : m.Skills.Necromancy.Value);

			if (skill >= 90.0)
				return 1.0;

			return base.GetControlChance(m, useBaseSkill);
		}

		public HellHorse(Serial serial) : base(serial)
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
