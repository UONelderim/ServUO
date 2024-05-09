using System;

namespace Server
{
    public static class ShardSettings
	{
		[CallPriority(Int32.MinValue)]
		public static void Configure()
		{
			Core.OnExpansionChanged += Invalidate;
			
			Invalidate();
		}

		public static void Invalidate()
		{
			Mobile.AOSStatusHandler = AOS.GetStatus;
		}
    }
}
