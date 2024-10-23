using Server.Mobiles;

namespace Nelderim.Achievements
{
	public abstract class Goal(int amount)
	{
		public int Amount { get; set; } = amount;

		public Achievement Achievement { get; internal set; }

		public abstract int GetProgress(PlayerMobile pm);
	}
}
