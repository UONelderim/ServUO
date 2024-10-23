using Server.Mobiles;

namespace Nelderim.Achievements
{
	public interface IComplexGoal
	{
		string GetDetailedProgress(PlayerMobile pm);
	}
}
