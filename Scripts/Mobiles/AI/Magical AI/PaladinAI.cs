#region References
using Server.Spells;
using Server.Spells.Chivalry;
using System;
#endregion

namespace Server.Mobiles
{
    public class PaladinAI : MageAI
    {
        private DateTime m_NextDispelTime = DateTime.UtcNow;
        private readonly TimeSpan m_DispelCooldown = TimeSpan.FromSeconds(30); // 30 second cooldown

        public override SkillName CastSkill => SkillName.Chivalry;
        public override bool UsesMagery => false;
        public override double HealChance => .1;

        public PaladinAI(BaseCreature m)
            : base(m)
        { }

        public override Spell GetRandomDamageSpell()
        {
            if (m_Mobile.Mana > 10 && 0.1 > Utility.RandomDouble())
                return new HolyLightSpell(m_Mobile, null);

            return null;
        }

        public override Spell GetRandomCurseSpell()
        {
            // Ensure we have enough mana to cast (DispelEvil costs 10 mana)
            if (m_Mobile.Mana < 10)
                return null;
                
            // If cooldown hasn't expired, don't cast
            if (DateTime.UtcNow < m_NextDispelTime)
                return null;
                
            // Ensure we have a valid target
            if (m_Mobile.Combatant is not BaseCreature bc)
                return null;
            
            var dispellable = bc.Summoned && !bc.IsAnimatedDead;
            var evil = !bc.Controlled && bc.Karma < -5000; // Require significantly negative karma
            var isRevenant = bc is Revenant; // Check if target is a Revenant
            
            // Add randomization factor (15% chance to cast when conditions are met)
            if ((dispellable || evil || isRevenant) && Utility.RandomDouble() < 0.15)
            {
                // Set the cooldown timer
                m_NextDispelTime = DateTime.UtcNow + m_DispelCooldown;
                return new DispelEvilSpell(m_Mobile, null);
            }

            return null;
        }

        public override Spell GetRandomBuffSpell()
        {
            int mana = m_Mobile.Mana;
            int select = 1;

            if (mana >= 15)
                select = 3;

            if (mana >= 20 && !EnemyOfOneSpell.UnderEffect(m_Mobile))
                select = 4;

            switch (Utility.Random(select))
            {
                case 0:
                    return new RemoveCurseSpell(m_Mobile, null);
                case 1:
                    return new DivineFurySpell(m_Mobile, null);
                case 2:
                    return new ConsecrateWeaponSpell(m_Mobile, null);
                case 3:
                    return new EnemyOfOneSpell(m_Mobile, null);
            }

            return new ConsecrateWeaponSpell(m_Mobile, null);
        }

        public override Spell GetHealSpell()
        {
            if (m_Mobile.Hits > m_Mobile.HitsMax * 0.5) 
               return null;
            if (m_Mobile.Mana <= 10)
               return null;
            if (m_Mobile.Combatant != null && m_Mobile.Combatant.Hits > m_Mobile.Hits)
               return new CloseWoundsSpell(m_Mobile, null);
            
            return null;
        }

        public override Spell GetCureSpell()
        {
            if (m_Mobile.Mana > 10)
                return new CleanseByFireSpell(m_Mobile, null);

            return null;
        }

        protected override bool ProcessTarget()
        {
            if (m_Mobile.Target == null)
                return false;

            m_Mobile.Target.Invoke(m_Mobile, m_Mobile);
            return true;
        }
    }
}
