using Server.Items;
using Server.Spells;
using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public partial class BaseCreature
    {
        [CommandProperty( AccessLevel.Counselor )]
        public virtual double AttackMasterChance => 0.05;

        [CommandProperty( AccessLevel.Counselor )]
        public virtual double SwitchTargetChance => 0.05;

        private double m_Difficulty;

        public double GetPoisonBonus( Poison p )
        {
            if ( p == Poison.Lethal ) return 1;
            else if ( p == Poison.Deadly ) return 0.92;
            else if ( p == Poison.Greater ) return 0.70;
            else if ( p == Poison.Regular ) return 0.40;
            else if ( p == Poison.Lesser ) return 0.30;
            else return 0;
        }

        public double MeleeDPS
        {
            get
            {
                BaseWeapon bw = (BaseWeapon)Weapon;
                int min, max;

                bw.GetBaseDamageRange( this, out min, out max );
                int avgDamage = (int)(min + max) / 2;
                double damage = (double)bw.ScaleDamageAOS( (Mobile)this, avgDamage, false );

                return (damage / bw.GetDelay( (Mobile)this ).TotalSeconds) * MeeleeSkillFactor;
            }
        }

        public double MagicDPS
        {
            get
            {
                double spellDamage = 0;
                double castDelay = 0;

                if ( AIObject is SpellCasterAI )
                {
                    SpellCasterAI ai = (SpellCasterAI)AIObject;
                    int maxCircle = ai.GetMaxCircle();
                    Spell s = ai.GetRandomDamageSpell();

                    if ( s == null )
                        return 0;

                    int[] circleDmg = new int[] { 1, 8, 11, 11, 20, 30, 38 };
                    spellDamage = (double)s.GetNewAosDamage( circleDmg[maxCircle - 1], 1, 5, false, null );
                    castDelay = 0.25 + (maxCircle * 0.25);
                    return spellDamage / castDelay;
                }
                else
                    return 0;
            }
        }

        public double DPS
        {
            get
            {
                double dps = Math.Max( MeleeDPS, MagicDPS );

                if ( HitPoisonBonus > 0 )
                    dps += dps * HitPoisonBonus;

                if ( WeaponAbilitiesBonus > 0 )
                    dps += dps * WeaponAbilitiesBonus;

                return dps;
            }
        }

        public double Life
        {
            get { return ((double)HitsMax * AvgResFactor * MeeleeSkillFactor) / 100; }
        }

        public Skill MaxMeleeSkill
        {
            get
            {
                SkillName[] meleeSkillNames = new SkillName[]
                {
                    SkillName.Wrestling,
                    SkillName.Macing,
                    SkillName.Fencing,
                    SkillName.Swords,
                    SkillName.Archery,
                };

                Skill skillMax = Skills[meleeSkillNames[0]];

                foreach ( SkillName msn in meleeSkillNames )
                {
                    Skill skill = Skills[msn];

                    if ( skill.Base > skillMax.Base )
                        skillMax = skill;
                }

                return skillMax;
            }
        }

        public double MeeleeSkillFactor
        {
            get { return Math.Max( 0.5, MaxMeleeSkill.Value / 120 ); }
        }

        public double AvgRes
        {
            get { return (PhysicalResistance + FireResistance + ColdResistance + PoisonResistance + EnergyResistance) / 5; }
        }

        public double AvgResFactor
        {
            get { return AvgRes / 100; }
        }

        public double HitPoisonBonus
        {
            get { return GetPoisonBonus( HitPoison ) * HitPoisonChance * MeeleeSkillFactor; }
        }

        public double WeaponAbilitiesBonus
        {
            get
            {
                double sum = 0;
                Dictionary<WeaponAbility, double> abilities = new Dictionary<WeaponAbility, double>();

                abilities[WeaponAbility.ArmorIgnore] = 0.4;
                abilities[WeaponAbility.BleedAttack] = 0.7;
                abilities[WeaponAbility.ConcussionBlow] = 0.4;
                abilities[WeaponAbility.CrushingBlow] = 0.3;
                abilities[WeaponAbility.Disarm] = 0.3;
                abilities[WeaponAbility.Dismount] = 0.3;
                abilities[WeaponAbility.DoubleStrike] = 0.3;
                abilities[WeaponAbility.InfectiousStrike] = 0.8;
                abilities[WeaponAbility.MortalStrike] = 0.7;
                abilities[WeaponAbility.MovingShot] = 0.2;
                abilities[WeaponAbility.ParalyzingBlow] = 0.3;
                abilities[WeaponAbility.ShadowStrike] = 0.2;
                abilities[WeaponAbility.WhirlwindAttack] = 0.3;
                abilities[WeaponAbility.RidingSwipe] = 0.3;
                abilities[WeaponAbility.FrenziedWhirlwind] = 0.3;
                abilities[WeaponAbility.Block] = 0.1;
                abilities[WeaponAbility.DefenseMastery] = 0.1;
                abilities[WeaponAbility.NerveStrike] = 0.3;
                abilities[WeaponAbility.TalonStrike] = 0.2;
                abilities[WeaponAbility.Feint] = 0.1;
                abilities[WeaponAbility.DualWield] = 0.2;
                abilities[WeaponAbility.DoubleShot] = 0.3;
                abilities[WeaponAbility.ArmorPierce] = 0.4;

                if ( _Profile != null && _Profile.WeaponAbilities != null) { 
                    foreach ( WeaponAbility ab in _Profile.WeaponAbilities )
                    {
                        if ( abilities.ContainsKey( ab ) )
                        {
                            double chance = WeaponAbilityChance / _Profile.WeaponAbilities.Length;
                            sum += chance * abilities[ab];
                        }
                    }
                }

                return sum * 0.5;
            }
        }

        public virtual double DifficultyScalar => 1.0;

        public double BaseDifficulty
        {
            get { return DPS * Math.Max( 0.01, Life ); }
        }

        public void GenerateDifficulty()
        {
            double difficulty = BaseDifficulty * DifficultyScalar;

            m_Difficulty = Math.Round( difficulty, 4 ) + 0.001; // So it's never 0.0
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public double Difficulty
        {
            get
            {
                if ( m_Difficulty == 0.0 )
                    GenerateDifficulty();
                return m_Difficulty;
            }
        }
    }
}
