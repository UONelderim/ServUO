using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;

namespace Server.Commands
{
    public static class AFK
    {
        public static bool Enabled = true; 
        public static TimeSpan TimerInterval = TimeSpan.FromMinutes(1);
        public static long IdleThreshold = (long)TimeSpan.FromMinutes(10).TotalMilliseconds;  
        public static long LongIdleThreshold = (long)TimeSpan.FromMinutes(30).TotalMilliseconds;  

        public static void Initialize()
        {
            if (Enabled)
            {
                EventSink.Speech += e => UpdateActivity(e.Mobile);
                EventSink.CraftSuccess += e => UpdateActivity(e.Crafter);
                EventSink.Login += e => UpdateActivity(e.Mobile);;
                EventSink.Logout += OnLogout;
                
                new AfkTimer().Start();
            }
        }

        static Dictionary<PlayerMobile, long> PlayerActivity = [];

        public static void UpdateActivity(Mobile m)
        {
	        if (m is PlayerMobile pm) 
		        PlayerActivity[pm] = Core.TickCount;
        }

        private static void OnLogout(LogoutEventArgs e)
        {
	        if (e.Mobile is PlayerMobile pm) 
		        PlayerActivity.Remove(pm);
        }

        private static bool IsIdle(PlayerMobile pm)
        {
	        return InternalIsIdle(pm, IdleThreshold);
        }

        private static bool IsLongIdle(PlayerMobile pm)
        {
	        return InternalIsIdle(pm, LongIdleThreshold);
        }

        private static bool InternalIsIdle(PlayerMobile pm, long durationThreshold) {
	        var idleThreshold = Core.TickCount - durationThreshold;
	        var hasActivity = PlayerActivity.TryGetValue(pm, out var lastActivity);
	        return hasActivity && lastActivity < idleThreshold && pm.LastMoveTime < idleThreshold && pm.NextActionTime < idleThreshold;
        }

        private class AfkTimer() : Timer(TimeSpan.FromMilliseconds(IdleThreshold), TimerInterval)
        {
	        protected override void OnTick()
	        {
		        var now = DateTime.Now;
		        foreach (var ns in NetState.Instances)
		        {
			        if(ns.Mobile is not PlayerMobile pm)
				        continue;
			        
			        if(IsLongIdle(pm))
				        pm.Emote("*głośno chrapie*");
			        else if (IsIdle(pm))
						pm.Emote("*chrapie*");
		        }
	        }
        }
    }
}
