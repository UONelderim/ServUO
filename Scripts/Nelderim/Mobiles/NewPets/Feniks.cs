#region References

using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zgliszcza feniksa")]
	public class Feniks : BaseCreature
	{
		public override double DifficultyScalar { get { return 1.15; } }
		private Timer m_aoeTimer;

		[Constructable]
		public Feniks() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.3, 0.4)
		{
			Name = "feniks";
			Body = 0x5;
			Hue = 0x489;

			SetStr(605, 611);
			SetDex(391, 519);
			SetInt(669, 818);

			SetHits(670, 850);

			SetDamage(15, 25);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Fire, 50);

			SetResistance(ResistanceType.Physical, 65);
			SetResistance(ResistanceType.Fire, 72, 74);
			SetResistance(ResistanceType.Poison, 36, 41);
			SetResistance(ResistanceType.Energy, 50, 51);

			SetSkill(SkillName.Wrestling, 121.9, 130.6);
			SetSkill(SkillName.Tactics, 114.9, 117.4);
			SetSkill(SkillName.MagicResist, 147.7, 153.0);
			SetSkill(SkillName.Poisoning, 122.8, 124.0);
			SetSkill(SkillName.Magery, 121.8, 127.8);
			SetSkill(SkillName.EvalInt, 103.6, 117.0);

			Tamable = true;
			ControlSlots = 5;
			MinTameSkill = 115.0;

			m_aoeTimer = new AoeTimer(this);

			SetWeaponAbility(WeaponAbility.ParalyzingBlow);
			SetWeaponAbility(WeaponAbility.BleedAttack);
			SetSpecialAbility(SpecialAbility.DragonBreath);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.08)
					corpse.DropItem(new DragonsHeart());
				if (Utility.RandomDouble() < 0.20)
					corpse.DropItem(new DragonsBlood());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.UltraRich, 4);
		}

		public override bool AutoDispel { get { return !Controlled; } }

		public override int TreasureMapLevel { get { return 5; } }
		public override int Feathers { get { return 36; } }
		public override int GetIdleSound() { return 0x2EF; }
		public override int GetAttackSound() { return 0x2EE; }
		public override int GetAngerSound() { return 0x2EF; }
		public override int GetHurtSound() { return 0x2F1; }
		public override int GetDeathSound() { return 0x2F2; }

		public Feniks(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_aoeTimer = new AoeTimer(this);
		}

		private class AoeTimer : Timer
		{
			readonly Feniks m_from;

			public AoeTimer(Feniks from) : base(TimeSpan.Zero, TimeSpan.FromSeconds(2))
			{
				m_from = from;
				Start();
			}

			protected override void OnTick()
			{
				if (m_from == null || m_from.Deleted)
					Stop();

				List<Mobile> targets = new List<Mobile>();

				if (m_from.Map != null && m_from.Map != Map.Internal)
				{
					var eable = m_from.GetMobilesInRange(2);
					foreach (Mobile m in eable)
					{
						if (m_from != m && SpellHelper.ValidIndirectTarget(m_from, m) &&
						    m_from.CanBeHarmful(m, false) && !m_from.InLOS(m))
							if (m_from.Controlled || m.Player || m is BaseCreature && ((BaseCreature)m).Controlled)
								targets.Add(m);
					}
					eable.Free();
				}

				for (int i = 0; i < targets.Count; ++i)
				{
					Mobile m = targets[i];

					int firedmg = Utility.RandomMinMax(5, 15);
					AOS.Damage(m, m_from, firedmg, true, 0, 100, 0, 0, 0);

					if (m.Player)
						m.SendLocalizedMessage(1008112, m_from.Name); // : The intense heat is damaging you!
				}
			}
		}
	}
}
