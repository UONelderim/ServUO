using System;
using Nelderim;
using Server.Engines.Plants;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("zwloki prasterego zuka runicznego")]
	public class AncientRuneBeetle : BaseCreature
	{
		[Constructable]
		public AncientRuneBeetle() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "prastary zuk runiczny";
			Body = 244;
			Hue = 1455;

			SetStr(898, 1030);
			SetDex(130, 150);
			SetInt(488, 620);

			SetHits(15000);

			SetDamage(19, 25);

			SetDamageType(ResistanceType.Physical, 20);
			SetDamageType(ResistanceType.Poison, 10);
			SetDamageType(ResistanceType.Energy, 70);


			SetResistance(ResistanceType.Physical, 75, 80);
			SetResistance(ResistanceType.Fire, 40, 60);
			SetResistance(ResistanceType.Cold, 40, 60);
			SetResistance(ResistanceType.Poison, 70, 80);
			SetResistance(ResistanceType.Energy, 40, 60);

			SetSkill(SkillName.EvalInt, 100.1, 125.0);
			SetSkill(SkillName.Magery, 100.1, 110.0);
			SetSkill(SkillName.Poisoning, 120.1, 140.0);
			SetSkill(SkillName.MagicResist, 95.1, 110.0);
			SetSkill(SkillName.Tactics, 78.1, 93.0);
			SetSkill(SkillName.Wrestling, 70.1, 77.5);

			Fame = 15000;
			Karma = -15000;


			if (Utility.RandomDouble() < .25)
				PackItem(Seed.RandomBonsaiSeed());

			Tamable = false;
			ControlSlots = 3;
			MinTameSkill = 93.9;

			SetWeaponAbility(WeaponAbility.BleedAttack);
			SetSpecialAbility(SpecialAbility.RuneCorruption);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.BodyPartsAndBones);
			AddLoot(NelderimLoot.DruidScrolls);
		}

		public override int GetAngerSound() => 0x4E8;

		public override int GetIdleSound() => 0x4E7;

		public override int GetAttackSound() => 0x4E6;

		public override int GetHurtSound() => 0x4E9;

		public override int GetDeathSound() => 0x4E5;

		public override Poison PoisonImmune => Poison.Greater;
		public override Poison HitPoison => Poison.Greater;
		public override FoodType FavoriteFood => FoodType.FruitsAndVegies | FoodType.GrainsAndHay;
		public override bool BardImmune => true;
		public override double AttackMasterChance => 0.15;
		public override double SwitchTargetChance => 0.15;
		public override double DispelDifficulty => 135.0;
		public override double DispelFocus => 45.0;
		public override double WeaponAbilityChance => 0.4;

		public AncientRuneBeetle(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(1);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			if (version < 1)
			{
				for (int i = 0; i < Skills.Length; ++i)
				{
					Skills[i].Cap = Math.Max(100.0, Skills[i].Cap * 0.9);

					if (Skills[i].Base > Skills[i].Cap)
					{
						Skills[i].Base = Skills[i].Cap;
					}
				}
			}
		}
	}
}
