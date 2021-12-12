using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles.Swiateczne
{
	public class MikolajBoss : BaseCreature
	{
		private DateTime m_NextAbilityTime;
		private int m_MinAbilityInterval = 1; //seconds
		private int m_MaxAbilityInterval = 3; //seconds

		[Constructable]
		public MikolajBoss() : base(AIType.AI_Boss, FightMode.Closest, 12, 1, 0.25, 0.5)
		{
			Name = "Jołakim";

			Body = 400;
			Hue = Tamael.Instance.RandomSkinHue();

			SetStr(1100, 1200);
			SetDex(80, 90);
			SetInt(600, 650);
			SetHits(15000);
			SetStam(205, 300);

			SetDamage(19, 25);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Cold, 50);

			SetResistance(ResistanceType.Physical, 55);
			SetResistance(ResistanceType.Fire, 55);
			SetResistance(ResistanceType.Cold, 100);
			SetResistance(ResistanceType.Poison, 75);
			SetResistance(ResistanceType.Energy, 75);

			SetSkill(SkillName.MagicResist, 110.0, 110.0);
			SetSkill(SkillName.Tactics, 100.0, 100.0);
			SetSkill(SkillName.Wrestling, 100.0, 100.0);
			SetSkill(SkillName.EvalInt, 120.0, 120.0);
			SetSkill(SkillName.Magery, 120.0, 120.0);

			Fame = 22500;
			Karma = 22500;

			VirtualArmor = 80;
			AddItem(new LightSource());
		}

		public override void OnThink()
		{
			if (DateTime.Now >= m_NextAbilityTime)
			{
				ThrowSnowball();
				m_NextAbilityTime = DateTime.Now +
				                    TimeSpan.FromSeconds(Utility.RandomMinMax(m_MinAbilityInterval,
					                    m_MaxAbilityInterval));
			}

			base.OnThink();
		}

		private void ThrowSnowball()
		{
			HashSet<Mobile> targets = new HashSet<Mobile>();
			foreach (AggressorInfo aggressorInfo in Aggressors)
			{
				targets.Add(aggressorInfo.Attacker);
			}

			foreach (AggressorInfo aggressorInfo in Aggressed)
			{
				targets.Add(aggressorInfo.Defender);
			}

			if (targets.Count > 0)
			{
				PublicOverheadMessage(MessageType.Regular, 0, 1007149); //Ho ho ho!
				foreach (Mobile target in targets)
				{
					if (Utility.RandomDouble() > 0.8)
					{
						PlaySound(0x145);
						Animate(9, 1, 1, true, false, 0);

						target.SendLocalizedMessage(1010572); // You have just been hit by a snowball!
						Effects.SendMovingEffect(this, target, 0x36E4, 7, 0, false, true, 0x480, 0);
						AOS.Damage(target, Utility.RandomMinMax(5, 10), 0, 0, 100, 0, 0);
					}
				}
			}
		}

		public override bool BardImmune { get { return true; } }
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }
		public override double DispelDifficulty { get { return 135.0; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }

		public MikolajBoss(Serial serial) : base(serial)
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
