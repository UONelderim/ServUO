namespace Server
{
    public static partial class Utility
    {
        public static int Clamp( int value, int min, int max )
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }
}
