using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardArmysPaeonSpell : BardSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
			"Śpiew Armii",
			"Paeonus",
			//SpellCircle.First,
			212,
			9041
		);

		public override SpellCircle Circle => SpellCircle.First;
		public override double CastDelay => 3;
		public override double RequiredSkill => 60.0;
		public override int RequiredMana => 15;

		public BardArmysPaeonSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			Scroll?.Consume();
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				var targets = new List<Mobile>();

				IPooledEnumerable eable = Caster.GetMobilesInRange(3);
				foreach (Mobile m in eable)
				{
					if (Caster.CanBeBeneficial(m, false, true) && !(m is Golem))
						targets.Add(m);
				}

				eable.Free();

				foreach (var m in targets)
				{
					int rounds = (int)(Caster.Skills[SkillName.Musicianship].Value * 0.5);

					new ExpireTimer(m, rounds, TimeSpan.FromSeconds(5)).Start();

					if (m.Hidden == false)
					{
						m.FixedParticles(0x376A, 9, 32, 5030, 1153, 3, EffectLayer.Waist);
					}
				}
			}

			FinishSequence();
		}

		private class ExpireTimer : Timer
		{
			private Mobile m_Mobile;
			private int m_Round;
			private int m_Totalrounds;

			public ExpireTimer(Mobile m, int totalrounds, TimeSpan delay) : base(delay, delay)
			{
				m_Mobile = m;
				m_Round = 0;
				m_Totalrounds = totalrounds;
			}

			protected override void OnTick()
			{
				if (m_Mobile == null || m_Mobile.Deleted)
				{
					Stop();
					return;
				}

				m_Mobile.Hits += 5;

				if (m_Round >= m_Totalrounds)
				{
					m_Mobile.SendMessage("The effect of Army's Paeon wears off.");
					Stop();
				}
				else
				{
					m_Round += 1;
				}
			}
		}
	}
}
