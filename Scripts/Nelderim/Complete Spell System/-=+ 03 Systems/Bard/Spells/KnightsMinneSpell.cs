#region References

using System;
using System.Collections;
using Server.Mobiles;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardKnightsMinneSpell : BardSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Wzmacniający Okrzyk", "Resistus",
			//SpellCircle.First,
			212, 9041
		);

		public override SpellCircle Circle => SpellCircle.First;

		public override double CastDelay => 3;
		public override double RequiredSkill => 45.0;
		public override int RequiredMana => 12;

		public BardKnightsMinneSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				ArrayList targets = new ArrayList();

				foreach (Mobile m in Caster.GetMobilesInRange(3))
				{
					if (Caster.CanBeBeneficial(m, false, true) && !(m is Golem))
						targets.Add(m);
				}

				for (int i = 0; i < targets.Count; ++i)
				{
					Mobile m = (Mobile)targets[i];
					if (m.BeginAction(typeof(BardKnightsMinneSpell)))
					{
						TimeSpan duration = TimeSpan.FromSeconds(Caster.Skills[SkillName.Provocation].Value * 0.3);
						int amount = (int)(Caster.Skills[SkillName.Musicianship].Value * .125);

						m.SendMessage("Twoja odporność fizyczna wzrasta.");
						ResistanceMod mod1 = new ResistanceMod(ResistanceType.Physical, +amount);

						m.AddResistanceMod(mod1);

						m.FixedParticles(0x373A, 10, 15, 5012, 0x450, 3, EffectLayer.Waist);

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
				m_Mobile.EndAction(typeof(BardKnightsMinneSpell));

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
