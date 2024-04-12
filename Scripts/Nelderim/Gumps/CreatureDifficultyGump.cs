#region References

using System;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Targeting;

#endregion

namespace Server.Mobiles
{
	public class CreatureDifficultyGump : BaseGump
	{
		public static void Initialize()
		{
			CommandSystem.Register("mobdiff", AccessLevel.GameMaster, MobDiff);
		}

		private static void MobDiff(CommandEventArgs e)
		{
			e.Mobile.BeginTarget(18, false, TargetFlags.None, (from, targeted) =>
			{
				if (from is PlayerMobile pm && targeted is BaseCreature bc)
				{
					SendGump(new CreatureDifficultyGump(pm, bc));
				}
			});
		}

		private readonly int _Label = 0xF424E5;

		public BaseCreature Creature { get; }

		public CreatureDifficultyGump(PlayerMobile pm, BaseCreature bc)
			: base(pm, 250)
		{
			Creature = bc;
		}

		public override void AddGumpLayout()
		{
			AddPage(0);
			AddBackground(0, 24, 310, 285, 0x24A4);
			AddHtml(47, 32, 210, 18, ColorAndCenter("#000080", Creature.Name), false, false);

			AddImage(40, 62, 0x82B);
			AddImage(40, 258, 0x82B);

			AddPage(1);

			AddHtml(47, 74, 160, 18, "General", false, false);

			AddHtml(53, 92, 160, 18, "Difficulty", false, false); 
			AddHtml(180, 92, 75, 18, FormatDouble(Creature.Difficulty), false, false);

			AddHtml(53, 110, 160, 18, "BaseDifficulty", false, false);
			AddHtml(180, 110, 75, 18, FormatDouble(Creature.BaseDifficulty), false, false);

			AddHtml(53, 128, 160, 18, "DifficultyScalar", false, false);
			AddHtml(180, 128, 75, 18, FormatDouble(Creature.DifficultyScalar), false, false);

			AddHtml(53, 146, 160, 18, "DPS", false, false);
			AddHtml(180, 146, 75, 18, FormatDouble(Creature.DPS), false, false);

			AddHtml(53, 164, 160, 18, "Life", false, false);
			AddHtml(180, 164, 75, 18, FormatDouble(Creature.Life), false, false);
			
			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 2);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 5);

			AddPage(2);
			
			AddHtml(47, 74, 160, 18, "Life",false, false);
			AddHtml(180, 74, 75, 18, FormatDouble(Creature.Life), false, false);

			AddHtml(53, 92, 160, 18, "HitsMax", false, false);
			AddHtml(180, 92, 75, 18, FormatStat(Creature.HitsMax), false, false);

			AddHtml(53, 110, 160, 18, "AvgResFactor", false, false);
			AddHtml(180, 110, 75, 18, FormatDouble(Creature.AvgResFactor), false, false);

			AddHtml(53, 128, 160, 18, "MagicResFactor", false, false);
			AddHtml(180, 128, 75, 18, FormatDouble(Creature.MagicResFactor), false, false);

			AddHtml(53, 146, 160, 18, "MeleeSkillFactor", false, false);
			AddHtml(180, 146, 75, 18, FormatDouble(Creature.MeleeSkillFactor), false, false);

			AddHtml(53, 182, 160, 18, "Multiplied", false, false);
			AddHtml(180, 182, 75, 18, FormatDouble(Creature.HitsMax * Creature.AvgResFactor * Creature.MagicResFactor * Creature.MeleeSkillFactor), false, false);

			AddHtml(42, 218, 230, 18, "Life=ln((Multiplied / 100) + 1) * 100 ", false, false);
			
			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 3);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 1);

			AddPage(3);
			
			AddHtml(47, 74, 160, 18, "DPS", false, false); 
			AddHtml(180, 74, 75, 18, FormatDouble(Creature.DPS), false, false);

			AddHtml(53, 92, 160, 18, "Melee DPS", false, false); 
			AddHtml(180, 92, 75, 18, FormatDouble(Creature.MeleeDPS), false, false);

			AddHtml(53, 110, 160, 18, "Magic DPS", false, false); 
			AddHtml(180, 110, 75, 18, FormatDouble(Creature.MagicDPS), false, false);

			AddHtml(53, 128, 160, 18, "Sum", false, false);
			AddHtml(180, 128, 75, 18, FormatDouble(Creature.MeleeDPS + Creature.MagicDPS), false, false);

			AddHtml(53, 146, 160, 18, "HitPoisonBonus", false, false);
			AddHtml(180, 146, 75, 18, FormatDouble(1 + Creature.HitPoisonBonus), false, false);

			AddHtml(53, 164, 160, 18, "WeaponAbilitiesBonus", false, false);
			AddHtml(180, 164, 75, 18, FormatDouble(1 + Creature.WeaponAbilitiesBonus), false, false);

			AddHtml(53, 182, 160, 18, "AreaEffectBonus", false, false);
			AddHtml(180, 182, 75, 18, FormatDouble(1 + Creature.AreaEffectsBonus), false, false);

			AddHtml(53, 200, 160, 18, "SpecialAbilitiesBonus", false, false);
			AddHtml(180, 200, 75, 18, FormatDouble(1 + Creature.SpecialAbilitiesBonus), false, false);
			
			AddHtml(53, 218, 160, 18, "AutoDispelBonus", false, false);
			AddHtml(180, 218, 75, 18, FormatDouble(Creature.AutoDispel ? 1.1 :1.0), false, false);
			
			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 4);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 2);
			
			AddPage(4);

			AddHtml(47, 74, 160, 18, "Melee DPS", false, false);
			AddHtml(180, 74, 75, 18, FormatDouble(Creature.MeleeDPS), false, false);

			AddHtml(53, 92, 160, 18, "Min Dmg", false, false);
			AddHtml(180, 92, 75, 18, FormatStat(Creature.DamageMin), false, false);

			AddHtml(53, 110, 160, 18, "Max Dmg", false, false);
			AddHtml(180, 110, 75, 18, FormatStat(Creature.DamageMax), false, false);

			var avgDamage = (Creature.DamageMin + Creature.DamageMax) / 2;

			AddHtml(53, 128, 160, 18, "Avg Dmg", false, false); 
			AddHtml(180, 128, 75, 18, FormatStat(avgDamage), false, false);

			var bw = Creature.Weapon as BaseWeapon;
			
			AddHtml(53, 146, 160, 18, "Scaled Dmg", false, false); 
			AddHtml(180, 146, 75, 18, FormatDouble(bw?.ScaleDamageAOS(Creature, avgDamage, false) ?? 0.0), false, false);

			AddHtml(53, 164, 160, 18, "Attack Delay(s)", false, false);
			AddHtml(180, 164, 75, 18, FormatDouble(bw?.GetDelay(Creature).TotalSeconds ?? 0.0), false, false);

			AddHtml(53, 182, 160, 18, "MeleeSkillFactor", false, false);
			AddHtml(180, 182, 75, 18, FormatDouble(Creature.MeleeSkillFactor), false, false);
			
			AddHtml(42, 218, 240, 18, "DPS = (ScaledDmg / AttackDelay) * MeleeSkillFactor", false, false);

			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 5);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 3);

			AddPage(5);

			AddHtml(47, 74, 160, 18, "Magic DPS", false, false);
			AddHtml(180, 74, 75, 18, FormatDouble(Creature.MagicDPS), false, false);

			if (Creature.AIObject is MageAI ai)
			{
				var y = 92;
				var magery = Creature.Skills[SkillName.Magery].Value;
				var mageryValue = magery * BaseCreature.MdpsMageryScalar;
				
				AddHtml(53, y, 160, 18, $"Magery({magery})", false, false);
				AddHtml(180, y, 75, 18, FormatDouble(mageryValue), false, false);
				y += 18;

				var maxCircleValue = (1 + ai.GetMaxCircle() * BaseCreature.MdpsMaxCircleScalar);
				AddHtml(53, y, 160, 18, $"MaxCircle({ai.GetMaxCircle()})", false, false);
				AddHtml(180, y, 75, 18, FormatDouble(maxCircleValue), false, false);
				y += 18;

				var mageryResult = mageryValue * maxCircleValue;
				
				AddHtml(63, y, 160, 18, $"Magery*MaxCircle", false, false);
				AddHtml(180, y, 75, 18, FormatDouble(mageryResult), false, false);
				y += 18;
				
				AddHtml(53, y, 160, 18, $"+EvalInt({Creature.Skills[SkillName.EvalInt].Value})", false, false);
				AddHtml(180, y, 75, 18, FormatDouble(mageryResult * Creature.Skills[SkillName.EvalInt].Value * BaseCreature.MdpsEvalScalar), false, false);
				y += 18;

				AddHtml(53, y, 160, 18, $"+Medit({Creature.Skills[SkillName.Meditation].Value})", false, false);
				AddHtml(180, y, 75, 18, FormatDouble(mageryResult * Creature.Skills[SkillName.Meditation].Value * BaseCreature.MdpsMeditScalar), false, false);
				y += 18;
				
				AddHtml(53, y, 160, 18, $"+Mana({Creature.ManaMax})", false, false);
				AddHtml(180, y, 75, 18, FormatDouble( mageryResult * Math.Min(Creature.ManaMax, BaseCreature.MdpsManaCap) * BaseCreature.MdpsManaScalar), false, false);
				y += 18;
				
				if (Creature.AIObject is NecroMageAI)
				{
					AddHtml(53, y, 160, 18, $"+Necromancy*{BaseCreature.MdpsNecroScalar}", false, false);
					AddHtml(180, y, 75, 18, FormatDouble(Creature.Skills[SkillName.Necromancy].Value), false, false);
					y += 18;

					AddHtml(53, y, 160, 18, $"+SpiritSpeak*{BaseCreature.MdpsSsScalar}", false, false);
					AddHtml(180, y, 75, 18, FormatDouble(Creature.Skills[SkillName.SpiritSpeak].Value), false, false);
				}
			}

			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 1);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 4);
		}

		public int Pages(AbilityProfile profile)
		{
			return 6;
		}

		private static string FormatString(string s)
		{
			return $"<div align=right>{s}</div>";
		}

		private static string FormatSkill(BaseCreature c, SkillName name)
		{
			if (c.Skills[name].Base < 10.0)
				return "<div align=right>---</div>";

			return String.Format("<div align=right>{0:F1}/{1}</div>", c.Skills[name].Value, c.Skills[name].Cap);
		}

		private static string FormatAttributes(int cur, int max)
		{
			if (max == 0)
				return "<div align=right>---</div>";

			return String.Format("<div align=right>{0}/{1}</div>", cur, max);
		}

		private static string FormatStat(int val)
		{
			if (val == 0)
				return "<div align=right>---</div>";

			return String.Format("<div align=right>{0}</div>", val);
		}

		public static string FormatDouble(double val)
		{
			if (val == 0)
				return "<div align=right>---</div>";

			return String.Format("<div align=right>{0:F2}</div>", val);
		}

		public static string FormatDouble(double val, bool dontshowzero = true, bool percentage = false)
		{
			if (dontshowzero)
			{
				return FormatDouble(val);
			}

			if (percentage)
			{
				return String.Format("<div align=right>{0:F1}%</div>", val);
			}

			return String.Format("<div align=right>{0:F1}</div>", val);
		}

		public static string FormatElement(int val, string color)
		{
			if (color == null)
			{
				if (val <= 0)
					return "<div align=right>---</div>";

				return String.Format("<div align=right>{0}%</div>", val);
			}

			if (val <= 0)
				return String.Format("<BASEFONT COLOR={0}><div align=right>---</div>", color);

			return String.Format("<BASEFONT COLOR={1}><div align=right>{0}%</div>", val, color);
		}

		public static string FormatDamage(int min, int max)
		{
			if (min <= 0 || max <= 0)
				return "<div align=right>---</div>";

			return String.Format("<div align=right>{0}-{1}</div>", min, max);
		}

		public string FormatPetSlots(int min, int max)
		{
			return String.Format("<BASEFONT COLOR=#57412F>{0} => {1}", min.ToString(), max.ToString());
		}
	}
}
