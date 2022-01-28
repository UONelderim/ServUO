using Server.Items;
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
                double chance = sum / ( 4.0 * (int)level );
                
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
				int avgDamage = (min + max) / 2;
				double damage = bw.ScaleDamageAOS(this, avgDamage, false);

				return damage / bw.GetDelay(this).TotalSeconds * MeeleeSkillFactor;
			}
		}

		private static double _mdpsMageryScalar = 0.5;
		private static double _mdpsEvalScalar = 0.003;
		private static double _mdpsMeditScalar = 0.001;
		private static double _mdpsManaScalar = 0.0002;
		private static double _mdpsMaxCircleScalar = 0.05;
		private static double _mdpsManaCap = 500;
		
		public double MagicDPS
		{
			get
			{
				if (AIObject is MageAI ai)
				{
					int maxCircle = ai.GetMaxCircle();
					double magery = Skills[SkillName.Magery].Value;
					double evalInt = Skills[SkillName.EvalInt].Value;
					double meditation = Skills[SkillName.Meditation].Value;

					double mageryValue = magery * _mdpsMageryScalar * (1 + maxCircle * _mdpsMaxCircleScalar);
					
					double evalIntBonus = mageryValue * evalInt * _mdpsEvalScalar;
					double meditBonus = mageryValue * meditation * _mdpsMeditScalar;
					double manaBonus = mageryValue * Math.Min(ManaMax, _mdpsManaCap) * _mdpsManaScalar;

					return mageryValue + evalIntBonus + meditBonus + manaBonus;
				}
				
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

				if (AreaEffectsBonus > 0)
					dps += dps * AreaEffectsBonus;

				return dps;
			}
		}

		public double Life => HitsMax * AvgResFactor * MeeleeSkillFactor / 100;

		public Skill MaxMeleeSkill
		{
			get
			{
				SkillName[] meleeSkillNames =
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

		public double AreaEffectsBonus
		{
			get
			{
				double sum = 0;
				Dictionary<AreaEffect, double> areaEffects = new Dictionary<AreaEffect, double>();
				areaEffects[AreaEffect.AuraOfEnergy] = 0.2;
				areaEffects[AreaEffect.AuraOfNausea] = 0.4;
				areaEffects[AreaEffect.EssenceOfDisease] = 0.2;
				areaEffects[AreaEffect.EssenceOfEarth] = 0.2;
				areaEffects[AreaEffect.ExplosiveGoo] = 0.2;
				areaEffects[AreaEffect.PoisonBreath] = 0.05;
				areaEffects[AreaEffect.AuraDamage] = 0;
				
				if (_Profile != null && _Profile.AreaEffects != null)
				{
					foreach (AreaEffect ae in _Profile.AreaEffects)
					{
						if (areaEffects.ContainsKey(ae))
						{
							double chance = areaEffects[ae] * ae.TriggerChance;
							if (ae is PoisonBreath poisonBreath)
								chance *= poisonBreath.GetPoison(this).Level; // 0.05 for level1, 0.2 for level4
							else if (ae is AuraDamage)
							{
								var aura = AuraDamage.AuraDefinition.GetDefinition(this);
								chance = (aura.Damage / aura.Cooldown.TotalSeconds) * 0.1;
							}
							sum += chance;
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

		// difficulty=1     fame=100
		// difficulty=10    fame=400
		// difficulty=100   fame=1600
		// difficulty=1000  fame=6400
		// difficulty=10000 fame=25600
		public int NelderimFame => (int) (100 * Math.Pow(4, Math.Log(Difficulty)/Math.Log(10)));

		public int NelderimKarma
		{
			get
			{
				switch (Math.Sign(base.Karma))
				{
					case -1: return -NelderimFame;
					default: return NelderimFame;
				}
			}
		}

		public override int Fame => Config.Get("Nelderim.CustomFameKarma", false) ? NelderimFame : base.Fame;
		
		public override int Karma => Config.Get("Nelderim.CustomFameKarma", false) ? NelderimKarma : base.Karma;
	}
}
