#region References

using System;
using System.Collections;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Ancient
{
	public class AncientWeatherSpell : AncientSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Zmiana pogody", "Rel Hur",
			236,
			9011,
			Reagent.SulfurousAsh
		);

		public override SpellCircle Circle => SpellCircle.First;

		public override double CastDelay => 0.5;
		public override double RequiredSkill => 0.0;
		public override int RequiredMana => 7;

		public AncientWeatherSpell(Mobile caster, Item scroll)
			: base(caster, scroll, m_Info)
		{
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				if (this.Scroll != null)
					Scroll.Consume();

				Timer t = (Timer)m_UnderEffect[Caster];

				if (Caster != null && t == null)
				{
					TimeSpan duration = SpellHelper.GetDuration(Caster, Caster);
					m_UnderEffect[Caster] = t = Timer.DelayCall(duration, new TimerStateCallback(RemoveEffect), Caster);
					Caster.SendMessage("Chmury nad waszymi głowami zaczynają wirować!");
					Caster.FixedParticles(0xC4A, 10, 15, 5028, EffectLayer.Head);
					Caster.PlaySound(0x16);
				}
			}

			FinishSequence();
		}

		private static readonly Hashtable m_UnderEffect = new Hashtable();

		private static void RemoveEffect(object state)
		{
			Mobile m = (Mobile)state;

			m_UnderEffect.Remove(m);

			m.SendMessage("Magiczna pogoda zaczyna się rozpraszać.");
		}

		public static bool UnderEffect(Mobile m)
		{
			return m_UnderEffect.Contains(m);
		}
	}
}
