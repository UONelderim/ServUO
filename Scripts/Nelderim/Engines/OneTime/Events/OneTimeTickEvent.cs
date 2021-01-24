using System;

namespace Server.OneTime.Events
{
    public static class OneTimeTickEvent
    {
        public static event EventHandler TickTimerTick;

        public static void SendTick(object o, int time)
        {
            if (time == 1)
            {
                if(TickTimerTick != null)
                    TickTimerTick.Invoke(o, EventArgs.Empty);

                OneTimeEventHelper.SendIOneTime(1);
            }
        }
    }
}
