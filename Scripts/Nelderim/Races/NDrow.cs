using Server;

namespace Nelderim.Races
{
    class NDrow : NRace
    {
        public NDrow( int raceId, int raceIndex ) : base( raceId, raceIndex)
        {
        }

        public override string[] Names => new[] { "Drow", "Drowa", "Drowowi", "Drowa", "Drowem", "Drowie", "Drowie" };
        public override string[] PluralNames => new[] { "Drowy", "Drowow", "Drowom", "Drowy", "Drowami", "Drowach", "Drowy" };
        public override int DescNumber => 505819;
        public override int[] SkinHues => new[] { 1109, 1108, 1107, 1106, 1409, 1897, 1898, 1899, 1908, 1907, 1906, 1905, 2106, 2105, 2306, 2305 };
        public override int[] HairHues => new[] { 1102, 1103, 1104, 1105, 1106, 1107, 1108, 1109, 1900, 1901, 1902, 1903, 1904, 1905, 1906, 1907, 1908, 2101, 2102, 2103, 2104, 2105, 2106, 2301 };

        public override int[][] HairTable { get; set; } =
        {
	        new[] // Female
	        {
		        Hair.Human.Bald,
		        Hair.Human.Short,
		        Hair.Human.Long,
		        Hair.Human.PonyTail,
		        Hair.Human.Pageboy,
		        Hair.Human.Buns,
		        Hair.Human.PigTails,
		        Hair.Human.Krisna,
	        },
	        new[] // Male
	        {
		        Hair.Human.Bald,
		        Hair.Human.Short,
		        Hair.Human.Long,
		        Hair.Human.PonyTail,
		        Hair.Human.Pageboy,
		        Hair.Human.Receeding,
		        Hair.Human.Krisna,
	        }
        };
        public override int[][] BeardTable { get; set; } =
        {
	        new[] // Female
	        {
		        Beard.Human.Clean,
	        },
	        new[] // Male
	        {
		        Beard.Human.Clean,
		        Beard.Human.Goatee,
		        Beard.Human.Mustache,
		        Beard.Human.Vandyke,
	        }
        };
    }
}
