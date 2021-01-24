namespace Server.OneTime
{
    public interface IOneTime
    {
        int OneTimeType { get; set; } //1 = tick, 2 = millisecond, 3 = second, 4 = minute, 5 = hour, 6 = day

        void OneTimeTick();
    }
}
