using System;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Gumps;

namespace Server.ACC.CSS.Systems.Rogue
{
    public class RogueShadowSpell : RogueSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Cień", " ",
            //SpellCircle.Fourth,
                                                        212,
                                                        9041
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 2; } }
        public override double RequiredSkill { get { return 60; } }
        public override int RequiredMana { get { return 15; } }

        private static Dictionary<Mobile, SkillMod> m_Table = new Dictionary<Mobile, SkillMod>();

        public RogueShadowSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
                                if (this.Scroll != null)
                        Scroll.Consume();
        }

        public static void Remove(Mobile m)
        {
            if (m_Table.ContainsKey(m))
            {
                m.RemoveSkillMod(m_Table[m]);
                m_Table.Remove(m);
            }

            m.EndAction(typeof(RogueShadowSpell));
        }

        public override void OnCast()
        {
            Caster.PlaySound( 22 );
            Caster.Hidden = true;
            if (Caster.CanBeginAction(typeof(RogueShadowSpell)))
            {
                Caster.BeginAction(typeof(RogueShadowSpell));
                DefaultSkillMod mod = new DefaultSkillMod(SkillName.Stealth, true, 5.0);
                m_Table.Add(Caster, mod);
                Caster.AddSkillMod(mod);

                new InternalTimer(Caster, DateTime.Now.AddSeconds(15*(Caster.Skills[DamageSkill].Base / 10))).Start();
            }
            else
                Caster.SendMessage("Już się ukryłeś!");
        }
    
        private class InternalTimer : Timer
        {
            private Mobile m_Owner;
            private DateTime m_ExpiresAt;

            public InternalTimer(Mobile owner, DateTime expiresAt)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(15))
            {
                m_Owner = owner;
                m_ExpiresAt = expiresAt;
            }

            protected override void OnTick()
            {
                if (DateTime.Now >= m_ExpiresAt)
                {
                    RogueShadowSpell.Remove(m_Owner);
                    Stop();
                }
            }
        }
    }
}
