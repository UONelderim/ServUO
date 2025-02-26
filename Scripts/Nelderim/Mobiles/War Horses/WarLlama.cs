#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki lamy bojowej")]
	public class WarLlama : BaseMount
	{
		[Constructable]
		public WarLlama() : this("lama bojowa")
		{
		}
		
		public WarLlama(string name) : base(name, 0xDC, 0x3EA6, AIType.AI_Melee,
			FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			BaseSoundID = 0xA8;

			SetStr(400);
			SetDex(120);
			SetInt(50);

			SetHits(250);
			SetMana(0);

			SetDamage(6, 8);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Fire, 25, 35);
			SetResistance(ResistanceType.Cold, 25, 35);
			SetResistance(ResistanceType.Poison, 25, 35);
			SetResistance(ResistanceType.Energy, 25, 35);

			SetSkill(SkillName.MagicResist, 30.0, 40.0);
			SetSkill(SkillName.Tactics, 40.0, 50.0);
			SetSkill(SkillName.Wrestling, 40.0, 50.0);

			Fame = 500;
			Karma = 500;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 29.1;

			switch (Utility.Random( 4 ))
			{
				case 0: 
					SetResistance( ResistanceType.Energy, 55, 65 );
					break;
				case 1:
					SetResistance( ResistanceType.Cold, 55, 65 );
					break;
				case 2:
					SetResistance( ResistanceType.Fire, 55, 65 );
					break;
				case 3:
					SetResistance( ResistanceType.Poison, 45, 55 );
					break;
			}
		}
		public override FoodType FavoriteFood => FoodType.FruitsAndVegies | FoodType.GrainsAndHay;
		
		public override PackInstinct PackInstinct => PackInstinct.Equine;

		public override bool BardImmune => false;

		public override double GetControlChance(Mobile m, bool useBaseSkill)
		{
			AbilityProfile profile = PetTrainingHelper.GetAbilityProfile(this);

			if (profile != null && profile.HasCustomized())
			{
				return base.GetControlChance(m, useBaseSkill);
			}
			return 1.0;
		}
		
		public WarLlama(Serial serial) : base(serial)
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

			if (BaseSoundID == -1)
				BaseSoundID = 0xA8;
		}
	}
}
