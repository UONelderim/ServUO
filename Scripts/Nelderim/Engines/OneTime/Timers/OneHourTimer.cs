using System;
using Server.OneTime.Events;

namespace Server.OneTime.Timers
{
    class OneHourTimer : Timer
    {
        private int LastTime { get; set; }

        public OneHourTimer() : base(TimeSpan.FromHours(1), TimeSpan.FromHours(1))
        {
            LastTime = DateTime.Now.Hour;
        }

        protected override void OnTick()
        {
            int dateTime = DateTime.Now.Hour;

            if (LastTime != dateTime)
            {
                LastTime = dateTime;

                OneTimeHourEvent.SendTick(this, 1);
            }
        }
    }
}
