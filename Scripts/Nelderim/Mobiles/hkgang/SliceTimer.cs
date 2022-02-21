#region References

using System;

#endregion

namespace Server.Engines.HunterKiller
{
	public class SliceTimer : Timer
	{
		private readonly HKGangSpawn spawn;

		public SliceTimer(HKGangSpawn hkspawn) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
		{
			spawn = hkspawn;

			Priority = TimerPriority.OneSecond;
		}

		protected override void OnTick()
		{
			spawn.OnSlice();
		}
	}
}
