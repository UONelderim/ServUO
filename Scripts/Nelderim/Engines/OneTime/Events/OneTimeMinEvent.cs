using System;

namespace Server.OneTime.Events
{
    public static class OneTimeMinEvent
    {
        public static event EventHandler MinTimerTick;

        public static void SendTick(object o, int time)
        {
            if (time == 1)
            {
                if(MinTimerTick != null)
                    MinTimerTick.Invoke(o, EventArgs.Empty);

                OneTimeEventHelper.SendIOneTime(4);
            }
        }
    }
}
