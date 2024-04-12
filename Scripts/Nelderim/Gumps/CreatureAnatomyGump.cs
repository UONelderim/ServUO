#region References

using System;
using Server.Gumps;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class CreatureAnatomyGump : BaseGump
	{
		private readonly int _Label = 0xF424E5;

		public BaseCreature Creature { get; }

		public CreatureAnatomyGump(PlayerMobile pm, BaseCreature bc)
			: base(pm, 250)
		{
			Creature = bc;
		}

		public override void AddGumpLayout()
		{
			AbilityProfile profile = PetTrainingHelper.GetAbilityProfile(Creature);
			TrainingProfile trainProfile = PetTrainingHelper.GetTrainingProfile(Creature, true);

			AddPage(0);
			AddBackground(0, 24, 310, 285, 0x24A4); // 285 was 325
			AddHtml(47, 32, 210, 18, ColorAndCenter("#000080", Creature.NGetName(null)), false, false);

			AddButton(140, 0, 0x82D, 0x82D, 0, GumpButtonType.Reply, 0);

			AddImage(40, 62, 0x82B);
			AddImage(40, 258, 0x82B);


			AddPage(1);

			AddImage(28, 76, 0x826);
			AddHtmlLocalized(47, 74, 160, 18, 1049593, 0xC8, false, false); // Attributes

			AddHtmlLocalized(53, 92, 160, 18, 1049578, _Label, false, false); // Hits
			AddHtml(180, 92, 75, 18, FormatAttributes(Creature.Hits, Creature.HitsMax), false, false);

			AddHtmlLocalized(53, 110, 160, 18, 1049579, _Label, false, false); // Stamina
			AddHtml(180, 110, 75, 18, FormatAttributes(Creature.Stam, Creature.StamMax), false, false);

			AddHtmlLocalized(53, 128, 160, 18, 1049580, _Label, false, false); // Mana
			AddHtml(180, 128, 75, 18, FormatAttributes(Creature.Mana, Creature.ManaMax), false, false);

			AddHtmlLocalized(53, 146, 160, 18, 1028335, _Label, false, false); // Strength
			AddHtml(180, 146, 75, 18, FormatStat(Creature.Str), false, false);

			AddHtmlLocalized(53, 164, 160, 18, 3000113, _Label, false, false); // Dexterity
			AddHtml(180, 164, 75, 18, FormatStat(Creature.Dex), false, false);

			AddHtmlLocalized(53, 182, 160, 18, 3000112, _Label, false, false); // Intelligence
			AddHtml(180, 182, 75, 18, FormatStat(Creature.Int), false, false);

			double bd = BaseInstrument.GetBaseDifficulty(Creature);

			if (Creature.Uncalmable)
				bd = 0;

			AddHtmlLocalized(53, 200, 160, 18, 1070793, _Label, false, false); // Barding Difficulty
			AddHtml(180, 200, 75, 18, FormatDouble(bd), false, false);

			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 2);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 6);

			AddPage(2);

			AddImage(28, 76, 0x826);

			AddHtmlLocalized(47, 74, 160, 18, 1061645, 0xC8, false, false); // Resistances

			AddHtmlLocalized(53, 92, 160, 18, 1061646, _Label, false, false); // Physical
			AddHtml(180, 92, 75, 18, FormatElement(Creature.PhysicalResistance, null), false, false);

			AddHtmlLocalized(53, 110, 160, 18, 1061647, _Label, false, false); // Fire
			AddHtml(180, 110, 75, 18, FormatElement(Creature.FireResistance, "#FF0000"), false, false);

			AddHtmlLocalized(53, 128, 160, 18, 1061648, _Label, false, false); // Cold
			AddHtml(180, 128, 75, 18, FormatElement(Creature.ColdResistance, "#000080"), false, false);

			AddHtmlLocalized(53, 146, 160, 18, 1061649, _Label, false, false); // Poison
			AddHtml(180, 146, 75, 18, FormatElement(Creature.PoisonResistance, "#008000"), false, false);

			AddHtmlLocalized(53, 164, 160, 18, 1061650, _Label, false, false); // Energy
			AddHtml(180, 164, 75, 18, FormatElement(Creature.EnergyResistance, "#BF80FF"), false, false);

			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 3);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 1);

			AddPage(3);

			AddImage(28, 76, 0x826);

			AddHtmlLocalized(47, 74, 160, 18, 1017319, 0xC8, false, false); // Damage

			AddHtmlLocalized(53, 92, 160, 18, 1061646, _Label, false, false); // Physical
			AddHtml(180, 92, 75, 18, FormatElement(Creature.PhysicalDamage, null), false, false);

			AddHtmlLocalized(53, 110, 160, 18, 1061647, _Label, false, false); // Fire
			AddHtml(180, 110, 75, 18, FormatElement(Creature.FireDamage, "#FF0000"), false, false);

			AddHtmlLocalized(53, 128, 160, 18, 1061648, _Label, false, false); // Cold
			AddHtml(180, 128, 75, 18, FormatElement(Creature.ColdDamage, "#000080"), false, false);

			AddHtmlLocalized(53, 146, 160, 18, 1061649, _Label, false, false); // Poison
			AddHtml(180, 146, 75, 18, FormatElement(Creature.PoisonDamage, "#008000"), false, false);

			AddHtmlLocalized(53, 164, 160, 18, 1061650, _Label, false, false); // Energy
			AddHtml(180, 164, 75, 18, FormatElement(Creature.EnergyDamage, "#BF80FF"), false, false);

			AddHtmlLocalized(53, 182, 160, 18, 1076750, _Label, false, false); // Base Damage
			AddHtml(180, 182, 75, 18, FormatDamage(Creature.DamageMin, Creature.DamageMax), false, false);

			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 4);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 2);

			AddPage(4);

			AddImage(28, 76, 0x826);

			AddHtmlLocalized(47, 74, 160, 18, 3001030, 0xC8, false, false); // Combat Ratings

			AddHtmlLocalized(53, 92, 160, 18, 1044103, _Label, false, false); // Wrestling
			AddHtml(180, 92, 100, 18, FormatSkill(Creature, SkillName.Wrestling), false, false);

			AddHtmlLocalized(53, 110, 160, 18, 1044087, _Label, false, false); // Tactics
			AddHtml(180, 110, 100, 18, FormatSkill(Creature, SkillName.Tactics), false, false);

			AddHtmlLocalized(53, 128, 160, 18, 1044086, _Label, false, false); // Magic Resistance
			AddHtml(180, 128, 100, 18, FormatSkill(Creature, SkillName.MagicResist), false, false);

			AddHtmlLocalized(53, 146, 160, 18, 1044061, _Label, false, false); // Anatomy
			AddHtml(180, 146, 100, 18, FormatSkill(Creature, SkillName.Anatomy), false, false);

			AddHtmlLocalized(53, 164, 160, 18, 1002082, _Label, false, false); // Healing
			AddHtml(180, 164, 100, 18, FormatSkill(Creature, SkillName.Healing), false, false);

			AddHtmlLocalized(53, 182, 160, 18, 1002122, _Label, false, false); // Poisoning
			AddHtml(180, 182, 100, 18, FormatSkill(Creature, SkillName.Poisoning), false, false);

			AddHtmlLocalized(53, 200, 160, 18, 1044074, _Label, false, false); // Detect Hidden
			AddHtml(180, 200, 100, 18, FormatSkill(Creature, SkillName.DetectHidden), false, false);

			AddHtmlLocalized(53, 218, 160, 18, 1002088, _Label, false, false); // Hiding
			AddHtml(180, 218, 100, 18, FormatSkill(Creature, SkillName.Hiding), false, false);

			AddHtmlLocalized(53, 236, 160, 18, 1002118, _Label, false, false); // Parrying
			AddHtml(180, 236, 100, 18, FormatSkill(Creature, SkillName.Parry), false, false);

			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 5);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 3);

			AddPage(5);

			AddImage(28, 76, 0x826);

			AddHtmlLocalized(47, 74, 160, 18, 3001032, 0xC8, false, false); // Lore & Knowledge

			AddHtmlLocalized(53, 92, 160, 18, 1044085, _Label, false, false); // Magery
			AddHtml(180, 92, 100, 18, FormatSkill(Creature, SkillName.Magery), false, false);

			AddHtmlLocalized(53, 110, 160, 18, 1044076, _Label, false, false); // Eval Int
			AddHtml(180, 110, 100, 18, FormatSkill(Creature, SkillName.EvalInt), false, false);

			AddHtmlLocalized(53, 128, 160, 18, 1044106, _Label, false, false); // Meditation
			AddHtml(180, 128, 100, 18, FormatSkill(Creature, SkillName.Meditation), false, false);

			AddHtmlLocalized(53, 146, 160, 18, 1044109, _Label, false, false); // Necromancy
			AddHtml(180, 146, 100, 18, FormatSkill(Creature, SkillName.Necromancy), false, false);

			AddHtmlLocalized(53, 164, 160, 18, 1002140, _Label, false, false); // Spirit Speak
			AddHtml(180, 164, 100, 18, FormatSkill(Creature, SkillName.SpiritSpeak), false, false);

			AddHtmlLocalized(53, 182, 160, 18, 1044115, _Label, false, false); // Mysticism
			AddHtml(180, 182, 100, 18, FormatSkill(Creature, SkillName.Mysticism), false, false);

			AddHtmlLocalized(53, 200, 160, 18, 1044110, _Label, false, false); // Focus
			AddHtml(180, 200, 100, 18, FormatSkill(Creature, SkillName.Focus), false, false);

			AddHtmlLocalized(53, 218, 160, 18, 1044114, _Label, false, false); // Spellweaving
			AddHtml(180, 218, 100, 18, FormatSkill(Creature, SkillName.Spellweaving), false, false);

			AddHtmlLocalized(53, 236, 160, 18, 1044075, _Label, false, false); // Discordance
			AddHtml(180, 236, 100, 18, FormatSkill(Creature, SkillName.Discordance), false, false);

			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 6);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 4);

			AddPage(6);

			AddImage(28, 76, 0x826);

			AddHtmlLocalized(47, 74, 160, 18, 3001032, 0xC8, false, false); // Lore & Knowledge

			AddHtmlLocalized(53, 92, 160, 18, 1044112, _Label, false, false); // Bushido
			AddHtml(180, 92, 100, 18, FormatSkill(Creature, SkillName.Bushido), false, false);

			AddHtmlLocalized(53, 110, 160, 18, 1044113, _Label, false, false); // Ninjitsu
			AddHtml(180, 110, 100, 18, FormatSkill(Creature, SkillName.Ninjitsu), false, false);

			AddHtmlLocalized(53, 128, 160, 18, 1044111, _Label, false, false); // Chivalry
			AddHtml(180, 128, 100, 18, FormatSkill(Creature, SkillName.Chivalry), false, false);


			AddButton(240, 288, 0x15E1, 0x15E5, 0, GumpButtonType.Page, 1);
			AddButton(217, 288, 0x15E3, 0x15E7, 0, GumpButtonType.Page, 5);
		}

		public int Pages(AbilityProfile profile)
		{
			return 6;
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

			return String.Format("<div align=right>{0:F1}</div>", val);
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
