#region References

using System;
using System.Collections;
using Server.ACC.CSS.Systems.Ancient;
using Server.Items;
using Server.Spells;
using Server.Spells.Fifth;

#endregion

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadSeanceSpell : UndeadSpell
	{
		public override TimeSpan CastDelayBase => TimeSpan.FromSeconds( 3.5 );
		public override double RequiredSkill => 80.0;
		public override int RequiredMana => 32;

		private static readonly SpellInfo m_Info = new SpellInfo(
			"Seans", "Kal Wis Corp",
			239,
			9031,
			Reagent.Bloodmoss,
			Reagent.SpidersSilk,
			Reagent.MandrakeRoot,
			Reagent.Nightshade,
			Reagent.SulfurousAsh
		);

		public override SpellCircle Circle => SpellCircle.Fourth;

		public UndeadSeanceSpell(Mobile caster, Item scroll)
			: base(caster, scroll, m_Info)
		{
		}

		public override bool CheckCast()
		{
			if (Caster.Mounted)
			{
				Caster.SendLocalizedMessage(1042561); //Please dismount first.
				return false;
			}

			if (TransformationSpellHelper.UnderTransformation(Caster))
			{
				Caster.SendMessage("Nie możesz wejść do królestwa zmarłych w tej formie.");
				return false;
			}

			if (DisguiseTimers.IsDisguised(Caster))
			{
				Caster.SendMessage("Nie możesz wejść do krainy zmarłych będąc ukrytym.");
				return false;
			}

			if (Caster.BodyMod == 183 || Caster.BodyMod == 184)
			{
				Caster.SendMessage("Nie możesz wejść do krainy umarłych bez usunięcia farby.");
				return false;
			}

			if (!Caster.CanBeginAction(typeof(UndeadSeanceSpell)))
			{
				Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
				return false;
			}

			return true;
		}

		        public override void OnCast()
        {
            if (!CheckSequence())
            {
                return;
            }
            else if (!Caster.CanBeginAction(typeof(UndeadSeanceSpell)))
            {
                Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
            }
            else if (TransformationSpellHelper.UnderTransformation(Caster))
            {
                Caster.SendMessage("Nie możesz wejść do królestwa zmarłych w tej formie.");
            }
            else if (DisguiseTimers.IsDisguised(Caster))
            {
                Caster.SendMessage("Nie możesz wejść do krainy zmarłych będąc ukrytym.");
            }
            else if (Caster.BodyMod == 183 || Caster.BodyMod == 184)
            {
                Caster.SendMessage("Nie możesz wejść do krainy umarłych bez usunięcia farby.");
            }
            else if (!Caster.CanBeginAction(typeof(Server.Spells.Fifth.IncognitoSpell)) || Caster.IsBodyMod)
            {
                DoFizzle();
            }
            else if (CheckSequence())
            {
                if (Caster.BeginAction(typeof(UndeadSeanceSpell)))
                {
                    if (this.Scroll != null)
                        Scroll.Consume();
                    Caster.PlaySound(0x379);

                    AncientSeanceSpell.SeanceSpellExt.Get(Caster).OldBody = Caster.BodyValue;
                    Caster.BodyValue = Caster.Female ? 403 : 402;

                    Caster.SendMessage("Wkraczasz do królestwa zmarłych.");
                    BaseArmor.ValidateMobile(Caster);

                    StopTimer(Caster);

                    Timer t = new InternalTimer(Caster);
                    m_Timers[Caster] = t;
                    t.Start();

                    // Set a flag indicating that the spell cannot be cast again until the timer expires
                    Caster.BeginAction(typeof(UndeadSeanceSpellCooldown));
                }
                else
                {
                    Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
                }
            }
            FinishSequence();
        }
        
        public class UndeadSeanceSpellCooldown : Timer
        {
            private Mobile m_Caster;

            public UndeadSeanceSpellCooldown(Mobile caster)
                : base(TimeSpan.FromMinutes(5))
            {
                m_Caster = caster;
            }

            protected override void OnTick()
            {
                m_Caster.EndAction(typeof(UndeadSeanceSpellCooldown));
               
            }
        }

		private static readonly Hashtable m_Timers = new Hashtable();

		public static bool StopTimer(Mobile m)
		{
			Timer t = (Timer)m_Timers[m];

			if (t != null)
			{
				t.Stop();
				m_Timers.Remove(m);
			}

			return (t != null);
		}

		private class InternalTimer : Timer
		{
			private readonly Mobile m_Owner;

			public InternalTimer(Mobile owner)
				: base(TimeSpan.FromSeconds(0))
			{
				m_Owner = owner;

				int val = (int)owner.Skills[SkillName.Necromancy].Value;

				if (val > 50)
					val = 50;

				Delay = TimeSpan.FromSeconds(val);
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if (!m_Owner.CanBeginAction(typeof(UndeadSeanceSpell)))
				{
					m_Owner.BodyValue = AncientSeanceSpell.SeanceSpellExt.Get(m_Owner).OldBody;
					AncientSeanceSpell.SeanceSpellExt.Delete(m_Owner);
					m_Owner.SendMessage("Opuszczasz królestwo zmarłych.");
					m_Owner.EndAction(typeof(UndeadSeanceSpell));

					BaseArmor.ValidateMobile(m_Owner);
				}
			}
		}
	}
}
