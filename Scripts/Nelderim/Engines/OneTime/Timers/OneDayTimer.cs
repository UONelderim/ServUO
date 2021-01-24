using System;
using Server.OneTime.Events;

namespace Server.OneTime.Timers
{
    class OneDayTimer : Timer
    {
        private int LastTime { get; set; }

        public OneDayTimer() : base(TimeSpan.FromDays(1), TimeSpan.FromDays(1))
        {
            LastTime = DateTime.Now.Day;
        }

        protected override void OnTick()
        {
            int dateTime = DateTime.Now.Hour;

            if (LastTime != dateTime)
            {
                LastTime = dateTime;

                OneTimeDayEvent.SendTick(this, 1);
            }
        }
    }
}
