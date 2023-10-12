namespace Nelderim.Achievements
{
	public abstract class Goal
	{
		public Goal(int amount)
		{
			Amount = amount;
		}
		
		public int Amount { get; set; }
		
		public Achievement Achievement { get; internal set; }
		
		public virtual int GetProgress(AchievementStatus status)
		{
			return status.Progress;
		}
	}
}
