namespace Server.OneTime.Events
{
    public static class OneTimeEventHelper
    {
        public static void SendIOneTime(int type)
        {
            foreach (Item oneTimeItem in World.Items.Values)
            {
                if (oneTimeItem is IOneTime)
                {
                    IOneTime oneTime = oneTimeItem as IOneTime;

                    SendTick(oneTime, type);
                }
            }

            foreach (Mobile oneTimeMobile in World.Mobiles.Values)
            {
                if (oneTimeMobile is IOneTime)
                {
                    IOneTime oneTime = oneTimeMobile as IOneTime;

                    SendTick(oneTime, type);
                }
            }
        }

        private static void SendTick(IOneTime oneTime, int type)
        {
            if (oneTime.OneTimeType == type)
            {
                oneTime.OneTimeTick();
            }
        }
    }
}
