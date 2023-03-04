// **********
// ServUO - MobileExtensions.cs
// **********

namespace Server.Nelderim.CoreExtensions
{
	public static class MobileExtensions
	{
		public static double CarvingBonus(this Mobile from)
		{
			var camping = from.Skills[SkillName.Camping].Value;
			var bonus = 0.0;

			if (camping >= 25.0)
				bonus += (camping - 25.0) / 75.0; // 25-100 camping == 0-100% bonus

			if (camping >= 90.0)
				bonus += (camping - 90.0) * 0.01; // Every skillpoint above 90 gives 1 extra % 

			return bonus;
		}
	}
}
