#region References

using System;
using System.Collections;
using Server.Mobiles;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardPoisonCarolSpell : BardSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Wężowa Pieśń", "Antidotus",
			//SpellCircle.First,
			212, 9041
		);

		public override SpellCircle Circle => SpellCircle.First;

		public override double CastDelay => 3;
		public override double RequiredSkill => 30.0;
		public override int RequiredMana => 12;

		public BardPoisonCarolSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				ArrayList targets = new ArrayList();

				var eable = Caster.GetMobilesInRange(3);
				foreach (Mobile m in eable)
				{
					if (Caster.CanBeBeneficial(m, false, true) && !(m is Golem))
						targets.Add(m);
				}
				eable.Free();

				for (int i = 0; i < targets.Count; ++i)
				{
					Mobile m = (Mobile)targets[i];
					if (m.BeginAction(typeof(BardPoisonCarolSpell)))
					{
						TimeSpan duration = TimeSpan.FromSeconds(Caster.Skills[CastSkill].Value * 5.0);
						int amount = (int)(Caster.Skills[SkillName.Musicianship].Value * .125);

						m.SendMessage("Twoja odporność na trucizny rośnie.");
						ResistanceMod mod1 = new ResistanceMod(ResistanceType.Poison, +amount);

						m.AddResistanceMod(mod1);

						if (m.Hidden == false)
						{
							m.FixedParticles(0x373A, 10, 15, 5012, 0x238, 3, EffectLayer.Waist);
						}

						new ExpireTimer(m, mod1, duration).Start();
					}
				}

				FinishSequence();
			}
		}

		private class ExpireTimer : Timer
		{
			private readonly Mobile m_Mobile;
			private readonly ResistanceMod m_Mods;

			public ExpireTimer(Mobile m, ResistanceMod mod, TimeSpan delay) : base(delay)
			{
				m_Mobile = m;
				m_Mods = mod;
			}

			public void DoExpire()
			{
				m_Mobile.RemoveResistanceMod(m_Mods);
				m_Mobile.EndAction(typeof(BardPoisonCarolSpell));

				Stop();
			}

			protected override void OnTick()
			{
				if (m_Mobile != null)
				{
					m_Mobile.SendMessage("Efekt pieśni wygasa.");
					DoExpire();
				}
			}
		}
	}
}
