#region References

using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

#endregion

namespace Server.Mobiles.Swiateczne
{
	public class MikolajBoss : BaseCreature
	{
		private readonly int m_MaxAbilityInterval = 3; //seconds
		private readonly int m_MinAbilityInterval = 1; //seconds
		private DateTime m_NextAbilityTime;

		[Constructable]
		public MikolajBoss() : base(AIType.AI_Boss, FightMode.Closest, 12, 1, 0.25, 0.5)
		{
			Name = "Jołakim";

			Body = 400;
			Hue = Race.RandomSkinHue();

			SetStr(900, 1000);
			SetDex(30, 60);
			SetInt(400, 550);
			SetHits(10000);
			SetStam(205, 300);

			SetDamage(19, 25);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Cold, 50);

			SetResistance(ResistanceType.Physical, 55);
			SetResistance(ResistanceType.Fire, 55);
			SetResistance(ResistanceType.Cold, 100);
			SetResistance(ResistanceType.Poison, 75);
			SetResistance(ResistanceType.Energy, 75);

			SetSkill(SkillName.MagicResist, 120.0, 120.0);
			SetSkill(SkillName.Tactics, 120.0, 120.0);
			SetSkill(SkillName.Wrestling, 120.0, 120.0);
			SetSkill(SkillName.Anatomy, 120.0, 120.0);
			SetSkill(SkillName.EvalInt, 80.0, 100.0);
			SetSkill(SkillName.Magery, 80.0, 100.0);

			Fame = 22500;
			Karma = 22500;

			VirtualArmor = 80;
			AddItem(new LightSource());

			AddItem(new FancyShirt(0x20));
			AddItem(new Surcoat(0x20));
			AddItem(new LongPants(0x20));
			AddItem(new FurCape(0x497));
			AddItem(new ElvenBoots());

			HairItemID = 0x203C;
			HairHue = 0x47F;

			FacialHairItemID = 0x204B;
			FacialHairHue = 0x47F;
		}
		// public override Poison HitPoison { get { return Poison.Lethal; } }

		public MikolajBoss(Serial serial) : base(serial)
		{
		}

		public override bool BardImmune
		{
			get { return true; }
		}

		public override double AttackMasterChance
		{
			get { return 0.15; }
		}

		public override double SwitchTargetChance
		{
			get { return 0.15; }
		}

		public override double DispelDifficulty
		{
			get { return 135.0; }
		}

		public override Poison PoisonImmune
		{
			get { return Poison.Lethal; }
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
			var targets = new HashSet<Mobile>();
			foreach (var aggressorInfo in Aggressors) targets.Add(aggressorInfo.Attacker);
			foreach (var aggressorInfo in Aggressed) targets.Add(aggressorInfo.Defender);

			if (targets.Count > 0)
			{
				PublicOverheadMessage(MessageType.Regular, 0, 1007149); //Ho ho ho!
				foreach (var target in targets)
					if (Utility.RandomDouble() > 0.8)
					{
						PlaySound(0x145);
						Animate(9, 1, 1, true, false, 0);

						target.SendLocalizedMessage(1010572); // You have just been hit by a snowball!
						Effects.SendMovingEffect(this, target, 0x36E4, 7, 0, false, true, 0x480, 0);
						AOS.Damage(target, Utility.RandomMinMax(2, 8), 0, 0, 100, 0, 0);
					}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();
		}
	}
}
