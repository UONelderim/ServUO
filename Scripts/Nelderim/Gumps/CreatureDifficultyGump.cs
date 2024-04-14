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

		public BaseCreature Creature { get; }

		public CreatureDifficultyGump(PlayerMobile pm, BaseCreature bc)
			: base(pm, 250)
		{
			Creature = bc;
		}

		public override void AddGumpLayout()
		{
			int y = 0;
			AddPage(0);
			AddBackground(0, 24, 310, 285, 0x24A4);
			AddHtml(47, 32, 210, 18, ColorAndCenter("#000080", Creature.Name), false, false);

			AddImage(40, 62, 0x82B);
			AddImage(40, 258, 0x82B);

			AddPage(1);

			AddHtml(47, 74, 160, 18, "General", false, false);

			AddHtml(53, 92, 160, 18, "Difficulty", false, false); 
			AddHtml(180, 92, 75, 18, FormatDouble(Creature.Difficulty), false, false);
			AddTooltip("BaseDifficulty * DifficultyScalar");

			AddHtml(53, 110, 160, 18, "BaseDifficulty", false, false);
			AddHtml(180, 110, 75, 18, FormatDouble(Creature.BaseDifficulty), false, false);
			AddTooltip("DPS * Life");

			AddHtml(53, 128, 160, 18, "DifficultyScalar", false, false);
			AddHtml(180, 128, 75, 18, FormatDouble(Creature.DifficultyScalar), false, false);

			AddHtml(53, 146, 160, 18, "DPS", false, false);
			AddHtml(180, 146, 75, 18, FormatDouble(Creature.DPS), false, false);

			AddHtml(53, 164, 160, 18, "Life", false, false);
			AddHtml(180, 164, 75, 18, FormatDouble(Creature.Life), false, false);
			
			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 2);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 5);

			AddPage(2);

			y = 74;
			AddHtml(47, y, 160, 18, "Life",false, false);
			AddHtml(180, y, 75, 18, FormatDouble(Creature.Life), false, false);
			AddTooltip("All multiplied");
			y += 18;

			AddHtml(53, y, 160, 18, $"HitsFactor({Creature.HitsMax})", false, false);
			AddHtml(180, y, 75, 18, FormatDouble(Creature.HitsFactor), false, false);
			AddTooltip("Math.Pow(Math.Log(HitsMax), 2)");
			y += 18;

			AddHtml(53, y, 160, 18, $"AvgResFactor({Creature.AvgRes}", false, false);
			AddHtml(180, y, 75, 18, FormatDouble(Creature.AvgResFactor), false, false);
			AddTooltip("1 + AvgRes * 0.01");
			y += 18;

			AddHtml(53, y, 160, 18, $"MeleeSkillFactor({Creature.MeleeSkillValue})", false, false);
			AddHtml(180, y, 75, 18, FormatDouble(Creature.MeleeSkillFactor), false, false);
			AddTooltip("1 + (MeleeSkillValue * 0.02)");
			y += 18;

			AddHtml(53, y, 160, 18, $"MagicResFactor({Creature.Skills[SkillName.MagicResist].Value})", false, false);
			AddHtml(180, y, 75, 18, FormatDouble(Creature.MagicResFactor), false, false);
			AddTooltip("1 + Skills[SkillName.MagicResist].Value * 0.001");

			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 3);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 1);

			AddPage(3);
			
			AddHtml(47, 74, 160, 18, "DPS", false, false); 
			AddHtml(180, 74, 75, 18, FormatDouble(Creature.DPS), false, false);
			AddTooltip("(MeleeDPS + MagicDPS) * Bonuses");

			AddHtml(53, 92, 160, 18, "Melee DPS", false, false); 
			AddHtml(180, 92, 75, 18, FormatDouble(Creature.MeleeDPS), false, false);

			AddHtml(53, 110, 160, 18, "Magic DPS", false, false); 
			AddHtml(180, 110, 75, 18, FormatDouble(Creature.MagicDPS), false, false);

			AddHtml(53, 128, 160, 18, "Melee+Magic DPS", false, false);
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
			AddTooltip("(ScaledDmg / AttackDelay) * MeleeSkillFactor");

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
			AddTooltip("Avg Dmg with all the bonuses");

			AddHtml(53, 164, 160, 18, "Attack Delay(s)", false, false);
			AddHtml(180, 164, 75, 18, FormatDouble(bw?.GetDelay(Creature).TotalSeconds ?? 0.0), false, false);

			AddHtml(53, 182, 160, 18, $"MeleeSkillFactor({Creature.MeleeSkillValue})", false, false);
			AddHtml(180, 182, 75, 18, FormatDouble(Creature.MeleeSkillFactor), false, false);
			AddTooltip("1 + (MeleeSkillValue * 0.02)");

			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 5);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 3);

			AddPage(5);

			AddHtml(47, 74, 160, 18, "Magic DPS", false, false);
			AddHtml(180, 74, 75, 18, FormatDouble(Creature.MagicDPS), false, false);

			if (Creature.AIObject is MageAI ai)
			{
				y = 92;
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
					var necro = Creature.Skills[SkillName.Necromancy].Value;
					var spiritSpeak = Creature.Skills[SkillName.SpiritSpeak].Value;
					AddHtml(53, y, 160, 18, $"+Necromancy({necro})", false, false);
					AddHtml(180, y, 75, 18, FormatDouble(necro * BaseCreature.MdpsNecroScalar), false, false);
					y += 18;

					AddHtml(53, y, 160, 18, $"+SpiritSpeak({spiritSpeak})", false, false);
					AddHtml(180, y, 75, 18, FormatDouble(spiritSpeak * BaseCreature.MdpsSsScalar), false, false);
				}
			}

			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 1);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 4);
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

			return String.Format("<div align=right>{0:F3}</div>", val);
		}
	}
}
