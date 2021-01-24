using Server.OneTime.Timers;

namespace Server.OneTime
{
    public static class OneTimeStart
    {
        public static Timer TimeTick { get; set; }           //UOTicksPerTick(1000) = 15000 est -debug
        public static Timer TimeMillisecond { get; set; }     //UOTicksPerMillisecond(1000) = 5000 est -debug

        public static Timer TimeSecond { get; set; }          //Rest of the times work on time, no tick info needed! 
        public static Timer TimeMinute { get; set; }
        public static Timer TimeHour { get; set; }
        public static Timer TimeDay { get; set; }

        public static void Initialize()
        {
            EventSink.ServerStarted += new ServerStartedEventHandler(OneTimeStarted);
        }

        private static void OneTimeStarted()
        {
            if (TimeTick == null)
            {
                TimeTick = new OneTickTimer(false); //Debug by sending true, or turn off with false!
            }

            if (TimeMillisecond == null)
            {
                TimeMillisecond = new OneMilliTimer(false); //Debug by sending true, or turn off with false!
            }

            if (TimeSecond == null)
            {
                TimeSecond = new OneSecTimer(false); //Debug by sending true, or turn off with false!
            }

            if (TimeMinute == null)
            {
                TimeMinute = new OneMinTimer();
            }

            if (TimeHour == null)
            {
                TimeHour = new OneHourTimer();
            }

            if (TimeDay == null)
            {
                TimeDay = new OneDayTimer();
            }

            TimeTick.Start();
            TimeMillisecond.Start();
            TimeSecond.Start();
            TimeMinute.Start();
            TimeHour.Start();
            TimeDay.Start();
        }
    }
}
