namespace Server.Spells
{
	public partial class Spell
	{
		private static int HerbalismLowerManaCost(Mobile m) => (int)(m.Skills[SkillName.Herbalism].Value / 20.0);

		private static int HerbalismLowerRegCost(Mobile m) => (int)(m.Skills[SkillName.Herbalism].Value / 5.0);
	}
}
