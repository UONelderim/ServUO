﻿#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki worga")]
	public class Worg : BaseMount
	{
		[Constructable]
		public Worg() : this("Worg")
		{
		}

		[Constructable]
		public Worg(string name) : base(name, 277, 0x3E91, AIType.AI_Melee, FightMode.Aggressor, 12, 1, 0.2, 0.4)
		{
			double chance = Utility.RandomDouble() * 23301;

			if (chance <= 1)
				Hue = 0x489;
			else if (chance < 50)
				Hue = Utility.RandomList(0x657, 0x515, 0x4B1, 0x481, 0x482, 0x455);
			else if (chance < 500)
				Hue = Utility.RandomList(0x97A, 0x978, 0x901, 0x8AC, 0x5A7, 0x527);

			SetStr(600, 630);
			SetDex(151, 170);
			SetInt(251, 282);

			SetDamage(15, 22);

			SetDamageType(ResistanceType.Physical, 0);
			SetDamageType(ResistanceType.Cold, 50);
			SetDamageType(ResistanceType.Energy, 50);

			SetResistance(ResistanceType.Physical, 55, 65);
			SetResistance(ResistanceType.Fire, 30, 45);
			SetResistance(ResistanceType.Cold, 40, 55);
			SetResistance(ResistanceType.Poison, 30, 50);
			SetResistance(ResistanceType.Energy, 45, 55);

			SetSkill(SkillName.Wrestling, 90.1, 96.8);
			SetSkill(SkillName.Tactics, 90.3, 99.3);
			SetSkill(SkillName.MagicResist, 75.3, 90.0);
			SetSkill(SkillName.Anatomy, 65.5, 69.4);
			//SetSkill( SkillName.Healing, 72.2, 98.9 );

			Fame = 5000; //Guessing here
			Karma = 5000; //Guessing here

			Tamable = true;
			ControlSlots = 4;
			MinTameSkill = 101.1;

			if (Utility.RandomDouble() < 0.2)
				PackItem(new TreasureMap(5, Map.Felucca));

			// TODO 0-2 spellweaving scroll

			if (Utility.RandomDouble() < 0.2)
				PackItem(new ObsidianStone());

			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 5);
		}

		public override double GetControlChance(Mobile m, bool useBaseSkill)
		{
			double skill = (useBaseSkill ? m.Skills.Tactics.Base : m.Skills.Tactics.Value);

			if (skill >= 110.0)
				return 1.0;

			return base.GetControlChance(m, useBaseSkill);
		}

		//public override bool CanHeal{ get{ return true; } }
		//public override bool CanHealOwner{ get{ return true; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override bool CanAngerOnTame { get { return true; } }
		public override bool StatLossAfterTame { get { return true; } }
		public override int Hides { get { return 10; } }
		public override int Meat { get { return 3; } }

		public Worg(Serial serial) : base(serial)
		{
		}

		public override int GetIdleSound() { return 0x577; }
		public override int GetAttackSound() { return 0x576; }
		public override int GetAngerSound() { return 0x578; }
		public override int GetHurtSound() { return 0x576; }
		public override int GetDeathSound() { return 0x579; }

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
