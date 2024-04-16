#region References

using System;
using System.Collections.Generic;
using Nelderim.Configuration;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public partial class BaseCreature
	{
		public double MeleeDPS
		{
			get
			{
				if (Weapon is BaseWeapon bw)
				{
					bw.GetBaseDamageRange(this, out var min, out var max);
					int avgDamage = (min + max) / 2;
					double damage = bw.ScaleDamageAOS(this, avgDamage, false);

					return damage / bw.GetDelay(this).TotalSeconds * MeleeSkillFactor;
				}

				return 0;
			}
				
		}

		public const double MdpsMageryScalar = 0.5;
		public const double MdpsEvalScalar = 0.003;
		public const double MdpsMeditScalar = 0.001;
		public const double MdpsManaScalar = 0.0002;
		public const double MdpsMaxCircleScalar = 0.05;
		public const double MdpsManaCap = 500;
		public const double MdpsNecroScalar = 0.4;
		public const double MdpsSsScalar = 0.001;


		public double MagicDPS
		{
			get
			{
				if (AIObject is MageAI ai)
				{
					var maxCircle = ai.GetMaxCircle();
					var magery = Skills[SkillName.Magery].Value;
					var evalInt = Skills[SkillName.EvalInt].Value;
					var meditation = Skills[SkillName.Meditation].Value;

					var mageryValue = magery * MdpsMageryScalar * (1 + maxCircle * MdpsMaxCircleScalar);

					var evalIntBonus = mageryValue * evalInt * MdpsEvalScalar;
					var meditBonus = mageryValue * meditation * MdpsMeditScalar;
					var manaBonus = mageryValue * Math.Min(ManaMax, MdpsManaCap) * MdpsManaScalar;

					var mageryResult = mageryValue + evalIntBonus + meditBonus + manaBonus;

					if (AIObject is NecroMageAI)
					{
						var necro = Skills[SkillName.Necromancy].Value;
						var spiritSpeak = Skills[SkillName.SpiritSpeak].Value;

						var necroBonus = necro * MdpsNecroScalar;
						var ssBonus = spiritSpeak * MdpsSsScalar;

						mageryResult += necroBonus + ssBonus;
					}
					return mageryResult * 0.4;
				}

				return 0;
			}
		}

		public double DPS
		{
			get
			{
				double dps = MeleeDPS + MagicDPS;

				if (HitPoisonBonus > 0)
					dps += dps * HitPoisonBonus;

				if (WeaponAbilitiesBonus > 0)
					dps += dps * WeaponAbilitiesBonus;

				if (AreaEffectsBonus > 0)
					dps += dps * AreaEffectsBonus;

				if (SpecialAbilitiesBonus > 0)
					dps += dps * SpecialAbilitiesBonus;

				if (AutoDispel)
					dps *= 1.1;

				return dps;
			}
		}

		public double Life => HitsFactor * MeleeSkillFactor * AvgResFactor * MagicResFactor;

		public double HitsFactor => Math.Pow(Math.Log(HitsMax), 2);

		public double MeleeSkillValue => Skills[((BaseWeapon)Weapon).GetUsedSkill(this, true)].Value;
		
		public double MeleeSkillFactor => MeleeSkillValue * 0.02;

		public double AvgRes =>
			(double)(PhysicalResistance + FireResistance + ColdResistance + PoisonResistance + EnergyResistance) / 5;

		public double AvgResFactor => 1 + AvgRes * 0.01;

		public double MagicResFactor => 1 + Skills[SkillName.MagicResist].Value * 0.001;

		public double GetPoisonBonus(Poison p)
		{
			if (p == Poison.Lethal) return 1;
			if (p == Poison.Deadly) return 0.9;
			if (p == Poison.Greater) return 0.70;
			if (p == Poison.Regular) return 0.40;
			if (p == Poison.Lesser) return 0.20;
			return 0;
		}

		public double HitPoisonBonus => GetPoisonBonus(HitPoison) * HitPoisonChance * 0.1;
		
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

		public double SpecialAbilitiesBonus
		{
			get
			{
				double sum = 0;
				Dictionary<SpecialAbility, double> abilities = new Dictionary<SpecialAbility, double>();
				abilities[SpecialAbility.AngryFire] = 0.45;
				abilities[SpecialAbility.ConductiveBlast] = 0.45;
				abilities[SpecialAbility.DragonBreath] = 0.5;
				abilities[SpecialAbility.GraspingClaw] = 0.35;
				abilities[SpecialAbility.Inferno] = 0.5;
				abilities[SpecialAbility.LightningForce] = 0.35;
				abilities[SpecialAbility.ManaDrain] = 0.75;
				abilities[SpecialAbility.RagingBreath] = 0.25;
				abilities[SpecialAbility.Repel] = 0.75;
				abilities[SpecialAbility.SearingWounds] = 0.6;
				abilities[SpecialAbility.StealLife] = 0.35;
				abilities[SpecialAbility.VenomousBite] = 0.5;
				abilities[SpecialAbility.ViciousBite] = 0.65;
				abilities[SpecialAbility.RuneCorruption] = 0.65;
				abilities[SpecialAbility.LifeLeech] = 0.4;
				abilities[SpecialAbility.StickySkin] = 0.25;
				abilities[SpecialAbility.TailSwipe] = 0.3;
				abilities[SpecialAbility.FlurryForce] = 0.4;
				abilities[SpecialAbility.Rage] = 0.15;
				abilities[SpecialAbility.Heal] = 0.4;
				abilities[SpecialAbility.HowlOfCacophony] = 0.8;
				abilities[SpecialAbility.Webbing] = 0.5;
				abilities[SpecialAbility.Anemia] = 0.65;
				abilities[SpecialAbility.BloodDisease] = 0.45;
				abilities[SpecialAbility.PoisonSpit] = 0.4;
				abilities[SpecialAbility.TrueFear] = 0.35;
				abilities[SpecialAbility.ColossalBlow] = 0.35;
				abilities[SpecialAbility.LifeDrain] = 0.55;
				abilities[SpecialAbility.ColossalRage] = 0.1;

				if (_Profile != null && _Profile.SpecialAbilities != null)
				{
					foreach (SpecialAbility ab in _Profile.SpecialAbilities)
					{
						if (abilities.ContainsKey(ab))
						{
							if (ab is DragonBreath)
								sum += ComputeDragonBreathBonus();
							else
							{
								double chance = ab.TriggerChance;
								sum += chance * abilities[ab];
							}
						}
					}
				}

				return sum * 0.5;
			}
		}

		public double ComputeDragonBreathBonus()
		{
			var chance = SpecialAbility.DragonBreath.TriggerChance;
			var definition = DragonBreath.DragonBreathDefinition.GetDefinition(this);
			var avgDelay = (definition.MinDelay + definition.MaxDelay) / 2; //Seconds
			var dmg = DragonBreath.BreathComputeDamage(this, definition);
			return chance * dmg / avgDelay;
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

		//Use DifficultyScalar only if mobile have special modification/ability that changes difficulty and cannot be calculated automatically
		public virtual double DifficultyScalar => 1.0;

		public void CalculateDifficulty()
		{
			_Difficulty = Math.Round(BaseDifficulty * DifficultyScalar, 4);
		}

		private double? _Difficulty;

		[CommandProperty(AccessLevel.GameMaster)]
		public double Difficulty
		{
			get
			{
				if (!_Difficulty.HasValue)
					CalculateDifficulty();
				return _Difficulty.Value;
			}
		}

		// difficulty=1     fame=100
		// difficulty=10    fame=400
		// difficulty=100   fame=1600
		// difficulty=1000  fame=6400
		// difficulty=10000 fame=25600
		public int NelderimFame => Math.Min(32000, (int)(100 * Math.Pow(4, Math.Log(Difficulty) / Math.Log(10))));

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

		public override int Fame => NConfig.CustomFameKarma ? NelderimFame : base.Fame;

		public override int Karma => NConfig.CustomFameKarma ? NelderimKarma : base.Karma;
		
		[CommandProperty(AccessLevel.GameMaster)]
		public DifficultyLevelValue DifficultyLevel {
			get => DifficultyLevelExt.Get(this).DifficultyLevel;
			set
			{
				if(DifficultyLevelExt.Get(this).DifficultyLevel != DifficultyLevelValue.Normal)
					DifficultyLevelExt.Restore(this);
				
				DifficultyLevelExt.Get(this).DifficultyLevel = value;
				
				if (!Summoned && value != DifficultyLevelValue.Normal)
					DifficultyLevelExt.Apply(this);
				
				CalculateDifficulty();
			}
		}

		public virtual string DifficultyLevelPrefix
		{
			get
			{
				switch (DifficultyLevel)
				{
					case DifficultyLevelValue.VeryWeak: return "Bardzo slaby";
					case DifficultyLevelValue.Weak: return "Slaby";
					case DifficultyLevelValue.Lesser: return "Mniejszy";
					case DifficultyLevelValue.Greater: return "Wiekszy";
					case DifficultyLevelValue.Strong: return "Silny";
					case DifficultyLevelValue.VeryStrong: return "Potezny";
					default: return "";
				}
			}
		}
		
		public virtual int DifficultyLevelBody
		{
			get
			{
				switch (DifficultyLevel)
				{
					case DifficultyLevelValue.VeryWeak:
					case DifficultyLevelValue.Weak:
					case DifficultyLevelValue.Lesser:
					case DifficultyLevelValue.Greater:
					case DifficultyLevelValue.Strong:
					case DifficultyLevelValue.VeryStrong:
					default: return BodyValue;
				}
			}
		}
		
		public virtual int DifficultyLevelHue
		{
			get
			{
				switch (DifficultyLevel)
				{
					case DifficultyLevelValue.VeryWeak:
					case DifficultyLevelValue.Weak:
					case DifficultyLevelValue.Lesser:
					case DifficultyLevelValue.Greater:
					case DifficultyLevelValue.Strong:
					case DifficultyLevelValue.VeryStrong:
					default: return Hue;
				}
			}
		}
	}
}
