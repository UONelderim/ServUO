using System;
using Server.OneTime.Events;

namespace Server.OneTime.Timers
{
    class OneSecTimer : Timer
    {
        private bool InDebug { get; set; }
        private int Count { get; set; }

        private int LastTime { get; set; }

        public OneSecTimer(bool debug) : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
        {
            InDebug = debug;
            Count = 5;

            LastTime = DateTime.Now.Second;
        }

        protected override void OnTick()
        {
            int dateTime = DateTime.Now.Second;

            if (LastTime != dateTime)
            {
                if (InDebug)
                {
                    if (Count >= 5)
                    {
                        Count = 0;

                        World.Broadcast(0x35, true, string.Format("Second : 5 Sec Interval [{0}]", LastTime));
                    }
                    Count++;
                }

                LastTime = dateTime;

                OneTimeSecEvent.SendTick(this, 1);
            }
        }
    }
}
