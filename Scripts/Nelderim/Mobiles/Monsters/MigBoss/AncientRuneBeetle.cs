using System;
using System.Collections;
using System.Collections.Generic;
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
			Hue = 2874;

			SetStr(898, 1030);
			SetDex(130, 150);
			SetInt(488, 620);

			SetHits(15000);

			SetDamage(29, 35);

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
				PackItem(Engines.Plants.Seed.RandomBonsaiSeed());

			switch (Utility.Random(10))
			{
				case 0:
					PackItem(new LeftArm());
					break;
				case 1:
					PackItem(new RightArm());
					break;
				case 2:
					PackItem(new Torso());
					break;
				case 3:
					PackItem(new Bone());
					break;
				case 4:
					PackItem(new RibCage());
					break;
				case 5:
					PackItem(new RibCage());
					break;
				case 6:
					PackItem(new BonePile());
					break;
				case 7:
					PackItem(new BonePile());
					break;
				case 8:
					PackItem(new BonePile());
					break;
				case 9:
					PackItem(new BonePile());
					break;
			}

			Tamable = false;
			ControlSlots = 3;
			MinTameSkill = 93.9;

			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public override int GetAngerSound()
		{
			return 0x4E8;
		}

		public override int GetIdleSound()
		{
			return 0x4E7;
		}

		public override int GetAttackSound()
		{
			return 0x4E6;
		}

		public override int GetHurtSound()
		{
			return 0x4E9;
		}

		public override int GetDeathSound()
		{
			return 0x4E5;
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 2);
		}

		public override Poison PoisonImmune => Poison.Greater;
		public override Poison HitPoison => Poison.Greater;
		public override FoodType FavoriteFood => FoodType.FruitsAndVegies | FoodType.GrainsAndHay;
		public override bool BardImmune => true;
		public override double AttackMasterChance => 0.15;
		public override double SwitchTargetChance => 0.15;

		public override double DispelDifficulty => 135.0;
		public override double DispelFocus => 45.0;

		public override double WeaponAbilityChance => 0.4;

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			if (0.20 > Utility.RandomDouble())
			{
				/* Rune Corruption
				 * Start cliloc: 1070846 "The creature magically corrupts your armor!"
				 * Effect: All resistances -70 (lowest 0) for 5 seconds
				 * End ASCII: "The corruption of your armor has worn off"
				 */

				ExpireTimer timer = (ExpireTimer)m_Table[defender];

				if (timer != null)
				{
					timer.DoExpire();
					defender.SendLocalizedMessage(1070845); // The creature continues to corrupt your armor!
				}
				else
					defender.SendLocalizedMessage(1070846); // The creature magically corrupts your armor!

				List<ResistanceMod> mods = new List<ResistanceMod>();

				if (defender.PhysicalResistance > 0)
					mods.Add(new ResistanceMod(ResistanceType.Physical, -(defender.PhysicalResistance / 2)));

				if (defender.FireResistance > 0)
					mods.Add(new ResistanceMod(ResistanceType.Fire, -(defender.FireResistance / 2)));

				if (defender.ColdResistance > 0)
					mods.Add(new ResistanceMod(ResistanceType.Cold, -(defender.ColdResistance / 2)));

				if (defender.PoisonResistance > 0)
					mods.Add(new ResistanceMod(ResistanceType.Poison, -(defender.PoisonResistance / 2)));

				if (defender.EnergyResistance > 0)
					mods.Add(new ResistanceMod(ResistanceType.Energy, -(defender.EnergyResistance / 2)));

				for (int i = 0; i < mods.Count; ++i)
					defender.AddResistanceMod(mods[i]);

				defender.FixedEffect(0x37B9, 10, 5);

				timer = new ExpireTimer(defender, mods, TimeSpan.FromSeconds(5.0));
				timer.Start();
				m_Table[defender] = timer;
			}
		}

		private static Hashtable m_Table = new Hashtable();

		private class ExpireTimer : Timer
		{
			private Mobile m_Mobile;
			private List<ResistanceMod> m_Mods;

			public ExpireTimer(Mobile m, List<ResistanceMod> mods, TimeSpan delay) : base(delay)
			{
				m_Mobile = m;
				m_Mods = mods;
				Priority = TimerPriority.TwoFiftyMS;
			}

			public void DoExpire()
			{
				for (int i = 0; i < m_Mods.Count; ++i)
					m_Mobile.RemoveResistanceMod(m_Mods[i]);

				Stop();
				m_Table.Remove(m_Mobile);
			}

			protected override void OnTick()
			{
				m_Mobile.SendMessage("The corruption of your armor has worn off");
				DoExpire();
			}
		}

		public AncientRuneBeetle(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1);
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
