using Server.Items;
using Server.Spells;
using System;
using System.Collections.Generic;
using Server.Nelderim;

namespace Server.Mobiles
{
	public partial class BaseCreature
	{
		public void AnnounceRandomRumor( PriorityLevel level )
        {
            try
            {
                List<RumorRecord> RumorsList = RumorsSystem.GetRumors( this, level );

                if ( RumorsList == null || RumorsList.Count == 0 )
                    return;
                
                int sum = 0;

                foreach ( RumorRecord r in RumorsList )
                    sum += (int)r.Priority;

                int index = Utility.Random( sum );
                double chance = ( (double)sum ) / ( 4.0 * ( (int)level ) );
                
                sum = 0;
                RumorRecord rumor = null;

                foreach ( RumorRecord r in RumorsList )
                {
                    sum += (int)r.Priority;

                    if ( sum > index )
                    {
                        rumor = r;
                        break;
                    }
                }

                if ( Utility.RandomDouble() < chance )
                    Say( rumor.Coppice );
            }
            catch ( Exception exc )
            {
                Console.WriteLine( exc.ToString() );
            }

        }

        public double GetRumorsActionPropability()
        {
	        try
            {
                List<RumorRecord> RumorsList = RumorsSystem.GetRumors( this, PriorityLevel.Low );

                if ( RumorsList == null || RumorsList.Count == 0 )
                    return 0;

                int sum = 0;

                foreach ( RumorRecord r in RumorsList )
                    sum += (int)r.Priority;

                double chance = sum / 320.0;

                return Math.Max(1.0, chance);

            }
            catch ( Exception exc )
            {
                Console.WriteLine( exc.ToString() );
            }

            return 0.00;
        }
		
		public bool Activation( Mobile target )
		{
			return ( Utility.RandomDouble() < Math.Pow( this.GetDistanceToSqrt( target ), -2 ) );
		}
		
		[CommandProperty(AccessLevel.Counselor)]
		public virtual double AttackMasterChance => 0.05;

		[CommandProperty(AccessLevel.Counselor)]
		public virtual double SwitchTargetChance => 0.05;

		private double? m_Difficulty;

		public double GetPoisonBonus(Poison p)
		{
			if (p == Poison.Lethal) return 1;
			if (p == Poison.Deadly) return 0.92;
			if (p == Poison.Greater) return 0.70;
			if (p == Poison.Regular) return 0.40;
			if (p == Poison.Lesser) return 0.30;
			return 0;
		}

		public double MeleeDPS
		{
			get
			{
				BaseWeapon bw = (BaseWeapon)Weapon;
				int min, max;

				bw.GetBaseDamageRange(this, out min, out max);
				int avgDamage = (int)(min + max) / 2;
				double damage = (double)bw.ScaleDamageAOS((Mobile)this, avgDamage, false);

				return (damage / bw.GetDelay((Mobile)this).TotalSeconds) * MeeleeSkillFactor;
			}
		}

		public double MagicDPS //TODO
		{
			get
			{
				double spellDamage = 0;
				double castDelay = 0;

				// if (AIObject is SpellCasterAI)
				// {
				// 	SpellCasterAI ai = (SpellCasterAI)AIObject;
				// 	int maxCircle = ai.GetMaxCircle();
				// 	Spell s = ai.GetRandomDamageSpell();
				//
				// 	if (s == null)
				// 		return 0;
				//
				// 	int[] circleDmg = new int[] {1, 8, 11, 11, 20, 30, 38};
				// 	spellDamage = (double)s.GetNewAosDamage(circleDmg[maxCircle - 1], 1, 5, false, null);
				// 	castDelay = 0.25 + (maxCircle * 0.25);
				// 	return spellDamage / castDelay;
				// }
				// else
				// 	return 0;
				return 0;
			}
		}

		public double DPS
		{
			get
			{
				double dps = Math.Max(MeleeDPS, MagicDPS);

				if (HitPoisonBonus > 0)
					dps += dps * HitPoisonBonus;

				if (WeaponAbilitiesBonus > 0)
					dps += dps * WeaponAbilitiesBonus;

				return dps;
			}
		}

		public double Life => HitsMax * AvgResFactor * MeeleeSkillFactor / 100;

		public Skill MaxMeleeSkill
		{
			get
			{
				SkillName[] meleeSkillNames = new SkillName[]
				{
					SkillName.Wrestling, SkillName.Macing, SkillName.Fencing, SkillName.Swords, SkillName.Archery, SkillName.Throwing
				};

				Skill skillMax = Skills[meleeSkillNames[0]];

				foreach (SkillName msn in meleeSkillNames)
				{
					Skill skill = Skills[msn];

					if (skill.Base > skillMax.Base)
						skillMax = skill;
				}

				return skillMax;
			}
		}

		public double MeeleeSkillFactor => Math.Max(0.5, MaxMeleeSkill.Value / 120);

		public double AvgRes => (double)(PhysicalResistance + FireResistance + ColdResistance + PoisonResistance + EnergyResistance) / 5;

		public double AvgResFactor => AvgRes / 100;

		public double HitPoisonBonus => GetPoisonBonus(HitPoison) * HitPoisonChance * MeeleeSkillFactor;

		public double WeaponAbilitiesBonus
		{
			get
			{
				double sum = 0;
				Dictionary<WeaponAbility, double> abilities = new Dictionary<WeaponAbility, double>();

				abilities[WeaponAbility.ArmorIgnore] = 0.65;
				abilities[WeaponAbility.BleedAttack] = 0.7;
				abilities[WeaponAbility.ConcussionBlow] = 0.6;
				abilities[WeaponAbility.CrushingBlow] = 0.6;
				abilities[WeaponAbility.Disarm] = 0.3;
				abilities[WeaponAbility.Dismount] = 0.45;
				abilities[WeaponAbility.DoubleStrike] = 0.6;
				abilities[WeaponAbility.InfectiousStrike] = 0.7;
				abilities[WeaponAbility.MortalStrike] = 0.75;
				abilities[WeaponAbility.MovingShot] = 0.2;
				abilities[WeaponAbility.ParalyzingBlow] = 0.45;
				abilities[WeaponAbility.ShadowStrike] = 0.35;
				abilities[WeaponAbility.WhirlwindAttack] = 0.45;
				abilities[WeaponAbility.RidingSwipe] = 0.35;
				abilities[WeaponAbility.FrenziedWhirlwind] = 0.55;
				abilities[WeaponAbility.Block] = 0.4;
				abilities[WeaponAbility.DefenseMastery] = 0.4;
				abilities[WeaponAbility.NerveStrike] = 0.4;
				abilities[WeaponAbility.TalonStrike] = 0.45;
				abilities[WeaponAbility.Feint] = 0.45;
				abilities[WeaponAbility.DualWield] = 0.35;
				abilities[WeaponAbility.DoubleShot] = 0.35;
				abilities[WeaponAbility.ArmorPierce] = 0.65;
				abilities[WeaponAbility.ForceArrow] = 0.2;
				abilities[WeaponAbility.LightningArrow] = 0.25;
				abilities[WeaponAbility.PsychicAttack] = 0.3;
				abilities[WeaponAbility.SerpentArrow] = 0.2;
				abilities[WeaponAbility.ForceOfNature] = 0.55;
				abilities[WeaponAbility.InfusedThrow] = 0.5;
				abilities[WeaponAbility.MysticArc] = 0.45;
				abilities[WeaponAbility.ColdWind] = 0.45;

				if (_Profile != null && _Profile.WeaponAbilities != null)
				{
					foreach (WeaponAbility ab in _Profile.WeaponAbilities)
					{
						if (abilities.ContainsKey(ab))
						{
							double chance = WeaponAbilityChance / _Profile.WeaponAbilities.Length;
							sum += chance * abilities[ab];
						}
					}
				}

				return sum * 0.5;
			}
		}

		public double BaseDifficulty => DPS * Math.Max(0.01, Life);

		public virtual double DifficultyScalar => 1.0;
		
		public double GenerateDifficulty() => Math.Round(BaseDifficulty * DifficultyScalar, 4);

		[CommandProperty(AccessLevel.GameMaster)]
		public double Difficulty
		{
			get
			{
				if (!m_Difficulty.HasValue)
					m_Difficulty = GenerateDifficulty();
				return m_Difficulty.Value;
			}
		}

		public virtual bool IgnoreHonor => false;
	}
}
