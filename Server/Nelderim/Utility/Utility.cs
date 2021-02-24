namespace Server
{
    public static partial class Utility
    {
        public static int Clamp( int value, int min, int max )
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
        
        public static double Clamp( double value, double min, double max )
        {
	        return (value < min) ? min : (value > max) ? max : value;
        }
        
        public static int RandomIndex( double[] chances )
        {
	        double rand = RandomDouble();

	        for ( int i = 0; i < chances.Length; i++ )
	        {
		        double chance = chances[ i ];

		        if ( rand < chance )
			        return i;
		        rand -= chance;
	        }

	        return chances.Length - 1;
        }
    }
}
