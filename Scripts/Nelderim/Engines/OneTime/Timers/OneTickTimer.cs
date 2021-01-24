using System;
using Server.OneTime.Events;

namespace Server.OneTime.Timers
{
    class OneTickTimer : Timer
    {
        private bool InDebug { get; set; }
        private int Count { get; set; }
        private long StoreTimeDiff { get; set; }

        private long LastTime { get; set; }

        public OneTickTimer(bool debug) : base(TimeSpan.FromTicks(1), TimeSpan.FromTicks(1))
        {
            InDebug = debug;
            Count = 1000;
            StoreTimeDiff = 0;

            LastTime = DateTime.Now.Ticks;
        }

        protected override void OnTick()
        {
            long dateTime = DateTime.Now.Ticks;

            if (LastTime != dateTime)
            {
                if (InDebug)
                {
                    long timeDiff = 0;

                    if (LastTime < dateTime)
                    {
                        timeDiff = (dateTime - LastTime);
                    }
                    else
                    {
                        timeDiff = (LastTime - dateTime);
                    }

                    StoreTimeDiff = (StoreTimeDiff + timeDiff);

                    if (Count >= 1000)
                    {
                        Count = 0;

                        World.Broadcast(0x35, true, string.Format("Tick : 1000 Tick Interval [{0}]", StoreTimeDiff));

                        StoreTimeDiff = 0;
                    }
                    Count++;
                }

                LastTime = dateTime;

                OneTimeTickEvent.SendTick(this, 1);
            }
        }
    }
}
