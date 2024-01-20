using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardMagesBalladSpell : BardSpell
	{
		private static SpellInfo m_Info = new(
			"Pieśń Do Magów",
			"Mentus",
			//SpellCircle.First,
			212,
			9041
		);

		public override SpellCircle Circle => SpellCircle.First;
		public override double CastDelay => 3;
		public override double RequiredSkill => 85;
		public override int RequiredMana => 30;

		public BardMagesBalladSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			Scroll?.Consume();
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				List<Mobile> targets = new List<Mobile>();

				IPooledEnumerable eable = Caster.GetMobilesInRange(3);
				foreach (Mobile m in eable)
				{
					if (Caster.CanBeBeneficial(m, false, true) && !(m is Golem))
						targets.Add(m);
				}

				eable.Free();

				int ticks = (int)(Caster.Skills[CastSkill].Value * 0.1);
				int manaRegen = Math.Max(1, (int)(Caster.Skills[DamageSkill].Value * 0.08));
				TimeSpan delay = TimeSpan.FromSeconds(2);
				TimeSpan interval = TimeSpan.FromSeconds(2);

				for (int i = 0; i < targets.Count; ++i)
				{
					Mobile m = targets[i];

					new ExpireTimer(m, ticks, manaRegen, delay, interval).Start();

					if (m.Hidden == false)
					{
						m.FixedParticles(0x376A, 9, 32, 5030, 0x256, 3, EffectLayer.Waist);
					}

					m.PlaySound(0x1F2);
				}
			}

			FinishSequence();
		}

		private class ExpireTimer : Timer
		{
			private Mobile m_Mobile;
			private int m_MaxTicks;
			private int m_Ticks;
			private int m_ManaRegen;


			public ExpireTimer(Mobile m, int ticks, int manaRegen, TimeSpan delay, TimeSpan interval) : base(delay,
				interval)
			{
				m_Mobile = m;
				m_MaxTicks = ticks;
				m_Ticks = 0;
				m_ManaRegen = manaRegen;
			}

			protected override void OnTick()
			{
				if (m_Mobile != null)
				{
					m_Mobile.Mana += m_ManaRegen;
					m_Ticks++;

					if (m_Ticks >= m_MaxTicks)
					{
						m_Mobile.SendMessage("Efekt pieśni wygasa");
						Stop();
					}
				}
			}
		}
	}
}
