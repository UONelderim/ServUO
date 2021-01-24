using System;
using Server.OneTime.Events;

namespace Server.OneTime.Timers
{
    class OneMilliTimer : Timer
    {
        private bool InDebug { get; set; }
        private int Count { get; set; }
        private int StoreTimeDiff { get; set; }

        private int LastTime { get; set; }

        public OneMilliTimer(bool debug) : base(TimeSpan.FromMilliseconds(1), TimeSpan.FromMilliseconds(1))
        {
            InDebug = debug;
            Count = 1000;
            StoreTimeDiff = 0;

            LastTime = DateTime.Now.Millisecond;
        }

        protected override void OnTick()
        {
            int dateTime = DateTime.Now.Millisecond;

            if (LastTime != dateTime)
            {
                if (InDebug)
                {
                    int timeDiff = 0;

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

                        World.Broadcast(0x35, true, string.Format("Millisecond : 1000 Tick Interval [{0}]", StoreTimeDiff));

                        StoreTimeDiff = 0;
                    }
                    Count++;
                }

                LastTime = dateTime;

                OneTimeMilliEvent.SendTick(this, 1);
            }
        }
    }
}
