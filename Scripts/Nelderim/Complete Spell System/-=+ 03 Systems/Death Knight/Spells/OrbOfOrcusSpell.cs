using System;
using System.Collections;
using Server.Gumps;
using Server.Network;

namespace Server.Spells.DeathKnight
{
    public class OrbOfOrcusSpell : DeathKnightSpell
    {
        private static SpellInfo m_Info = new(
            "Kula Smierci", "Orcus Arma",
            218,
            9031
        );

        public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(1);
        public override int RequiredTithing => 200;
        public override double RequiredSkill => 80.0;
        public override int RequiredMana => 26;

        // Timer to handle updating the counter display
        private static Hashtable m_Timers = new Hashtable();

        public OrbOfOrcusSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            return true;
        }

        private static Hashtable m_Table = new Hashtable();

        public override void OnCast()
        {
            if (Caster.MagicDamageAbsorb > 0)
            {
                Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
            }
            else if (!Caster.CanBeginAction(typeof(OrbOfOrcusSpell)))
            {
                Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
            }
            else if (CheckSequence())
            {
                if (Caster.BeginAction(typeof(OrbOfOrcusSpell)) /*&& CheckFizzle()*/)
                {
                    int value = (int)(GetKarmaPower(Caster) / 4);

                    Caster.MagicDamageAbsorb = value;

                    // Force immediate display of the counter gump 
                    if (Caster.NetState != null)
                    {
                        Caster.SendGump(new MagicDamageAbsorbGump(value));
                        
                        // Store reference in the table for update tracking
                        m_Table[Caster] = value;
                        
                        // Start timer to update the counter
                        StartAbsorbTimer(Caster);
                    }

                    Caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                    Caster.PlaySound(0x1E9);
                }
                else
                {
                    Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
                }

                FinishSequence();
            }
        }

        /// <summary>
        /// Updates the Magic Damage Absorb counter gump for the player
        /// </summary>
        /// <param name="mobile">The player to update the counter for</param>
        /// <param name="value">The current absorb value</param>
        public static void UpdateAbsorbCounter(Mobile mobile, int value)
        {
            if (mobile == null || mobile.NetState == null)
                return;

            mobile.CloseGump(typeof(MagicDamageAbsorbGump));
            mobile.SendGump(new MagicDamageAbsorbGump(value));
        }

        /// <summary>
        /// Starts a timer to periodically update the absorb counter
        /// </summary>
        /// <param name="mobile">The player with the absorb effect</param>
        public static void StartAbsorbTimer(Mobile mobile)
        {
            if (mobile == null)
                return;

            // Stop any existing timer
            StopAbsorbTimer(mobile);

            // Create and start a new timer
            AbsorbTimer timer = new AbsorbTimer(mobile);
            timer.Start();
            m_Timers[mobile] = timer;
        }

        /// <summary>
        /// Stops the absorb counter timer
        /// </summary>
        /// <param name="mobile">The player with the timer</param>
        public static void StopAbsorbTimer(Mobile mobile)
        {
            if (mobile == null)
                return;

            if (m_Timers.Contains(mobile))
            {
                AbsorbTimer timer = (AbsorbTimer)m_Timers[mobile];
                timer.Stop();
                m_Timers.Remove(mobile);
            }

            if (m_Table.Contains(mobile))
            {
                m_Table.Remove(mobile);
            }

            // Close the gump when the effect ends
            mobile.CloseGump(typeof(MagicDamageAbsorbGump));
            
            // End the action to allow recasting
            mobile.EndAction(typeof(OrbOfOrcusSpell));
        }

        /// <summary>
        /// Timer class to update the absorb counter
        /// </summary>
        private class AbsorbTimer : Timer
        {
            private Mobile m_Mobile;
            private int m_LastValue;

            public AbsorbTimer(Mobile mobile) : base(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5))
            {
                m_Mobile = mobile;
                m_LastValue = mobile.MagicDamageAbsorb;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Mobile == null || m_Mobile.Deleted || m_Mobile.NetState == null)
                {
                    Stop();
                    m_Timers.Remove(m_Mobile);
                    return;
                }

                // Check the current absorb value
                int currentValue = m_Mobile.MagicDamageAbsorb;

                // If absorb value has changed, update the counter
                if (m_LastValue != currentValue)
                {
                    m_LastValue = currentValue;
                    
                    if (currentValue > 0)
                    {
                        UpdateAbsorbCounter(m_Mobile, currentValue);
                    }
                    else
                    {
                        // Effect has ended, stop the timer and close the gump
                        StopAbsorbTimer(m_Mobile);
                    }
                }
                else if (currentValue <= 0)
                {
                    // Effect has ended, stop the timer and close the gump
                    StopAbsorbTimer(m_Mobile);
                }
                else
                {
                    // Periodically refresh the gump to ensure it's visible
                    // This helps with any client-side issues
                    if (m_Mobile.HasGump(typeof(MagicDamageAbsorbGump)) == false)
                    {
                        UpdateAbsorbCounter(m_Mobile, currentValue);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gump class to display the Magic Damage Absorb counter
    /// </summary>
    public class MagicDamageAbsorbGump : Gump
    {
        private int m_AbsorbValue;

        public MagicDamageAbsorbGump(int absorbValue) : base(70, 70)
        {
            m_AbsorbValue = absorbValue;

            // Make the gump slightly less transparent and more visible
            Closable = false;  // Don't allow player to close it manually
            Disposable = true;
            Dragable = true;
            Resizable = false;

            // Add the gump background
            AddPage(0);
            AddBackground(0, 0, 150, 60, 9270);
            AddAlphaRegion(10, 10, 130, 40);

            // Add the counter label with brighter colors
            AddLabel(15, 15, 36, "Kula Smierci:");  // Bright blue color
            AddLabel(105, 15, 32, m_AbsorbValue.ToString());  // Bright red color
        }

        // No need for OnResponse as we're not making it closable
    }
}
