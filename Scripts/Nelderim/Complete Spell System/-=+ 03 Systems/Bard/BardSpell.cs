using System;
using Server;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Bard
{
    public abstract class BardSpell : CSpell
    {
        public abstract SpellCircle Circle { get; }

        public BardSpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
        }

        public override SkillName CastSkill { 
            get {
                double disco = Caster.Skills.Discordance.Base;
                double peace = Caster.Skills.Peacemaking.Base;
                double provo = Caster.Skills.Provocation.Base;
                if ( disco >= peace && disco >= provo )
                    return SkillName.Discordance;
                if ( peace >= provo )
                    return SkillName.Peacemaking;
                else
                    return SkillName.Provocation;
            } 
        }
        public override SkillName DamageSkill { get { return SkillName.Musicianship; } }
        public override bool ClearHandsOnCast { get { return false; } }
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(3 * CastDelaySecondsPerTick); } }
        public override void GetCastSkills(out double min, out double max)
        {
            min = RequiredSkill;
            max = RequiredSkill + 30.0;
        }

        public override int GetMana()
        {
            return RequiredMana;
        }

        public override bool CheckCast()
        {
            Container pack = Caster.Backpack;

            BaseInstrument ints = (BaseInstrument)pack.FindItemByType(typeof(BaseInstrument));

            if (ints == null)
            {
                Caster.SendMessage("Potrzebujesz jakiegoś instrumentu, by wykonać tę pieśń.");
                return false;
            }

            if (ints.UsesRemaining >= 2)
                ints.UsesRemaining -= 1;

            if (ints.UsesRemaining == 1)
                ints.Delete();

            Caster.PlaySound(ints.SuccessSound);
            return true;
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(CastDelay);
        }
        
        public virtual bool CheckResisted(Mobile target)
        {
            double n = GetResistPercent(target);

            n /= 100.0;

            if (n <= 0.0)
                return false;

            if (n >= 1.0)
                return true;

            int maxSkill = (1 + (int)Circle) * 10;
            //int maxSkill = 40;
            maxSkill += (1 + ((int)Circle / 6)) * 25;
            //maxSkill += (1 + (4 / 6)) * 25;

            if (target.Skills[SkillName.MagicResist].Value < maxSkill)
                target.CheckSkill(SkillName.MagicResist, 0.0, 120.0);

            return (n >= Utility.RandomDouble());
        }

        public virtual double GetResistPercent(Mobile target)
        {
            return GetResistPercentForCircle(target, Circle);
            //return GetResistPercentForCircle(target, SpellCircle.Fourth);
        }

        public virtual double GetResistPercentForCircle(Mobile target, SpellCircle circle)
        {
            double firstPercent = target.Skills[SkillName.MagicResist].Value / 5.0;
            double secondPercent = target.Skills[SkillName.MagicResist].Value - (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);

            return (firstPercent > secondPercent ? firstPercent : secondPercent) / 2.0; // Seems should be about half of what stratics says.
        }
    }
}