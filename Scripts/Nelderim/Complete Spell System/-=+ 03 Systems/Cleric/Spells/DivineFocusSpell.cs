#region References

using System;
using System.Collections;
using Server.Spells;

#endregion

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericDivineFocusSpell : ClericSpell
	{
		private static readonly SpellInfo m_Info = new SpellInfo(
			"Boskie Skupienie", "Divinium Cogitatus",
			//SpellCircle.First,
			212,
			9041
		);

		public override SpellCircle Circle => SpellCircle.First;

		public override int RequiredTithing => 15;
		public override double RequiredSkill => 35.0;

		public override int RequiredMana => 4;

		private static readonly Hashtable m_Table = new Hashtable();

		public ClericDivineFocusSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
			if (this.Scroll != null)
				Scroll.Consume();
		}

		public static double GetScalar(Mobile m)
		{
			double val = 1.0;

			if (!m.CanBeginAction(typeof(ClericDivineFocusSpell)))
				val = 1.5;

			return val;
		}

		public override bool CheckCast()
		{
			if (!base.CheckCast())
			{
				return false;
			}

			if (!Caster.CanBeginAction(typeof(ClericDivineFocusSpell)))
			{
				Caster.SendMessage("Ten czar już działa!");
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			if (!Caster.CanBeginAction(typeof(ClericDivineFocusSpell)))
			{
				Caster.SendMessage("Ten czar już działa!");
				return;
			}

			if (CheckSequence())
			{
				Caster.BeginAction(typeof(ClericDivineFocusSpell));

				Timer t = new InternalTimer(Caster);
				m_Table[Caster] = t;
				t.Start();

				Caster.FixedParticles(0x375A, 1, 15, 0x480, 1, 4, EffectLayer.Waist);
			}
		}


		private class InternalTimer : Timer
		{
			private readonly Mobile m_Owner;

			public InternalTimer(Mobile owner) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.5))
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if (!m_Owner.CheckAlive() || m_Owner.Mana < 3)
				{
					m_Owner.EndAction(typeof(ClericDivineFocusSpell));
					m_Table.Remove(m_Owner);
					m_Owner.SendMessage("Twój umysł słabnie! Nie jesteś w stanie pozostać skupionym.");
					Stop();
				}
				else
				{
					m_Owner.Mana -= 3;
				}
			}
		}
	}
}
