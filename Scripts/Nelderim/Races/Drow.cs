using Server;

namespace Nelderim.Races
{
    class Drow : NRace
    {
        protected static string[] m_Names = new string[7] { "Drow", "Drowa", "Drowowi", "Drowa", "Drowem", "Drowie", "Drowie" };
        protected static string[] m_PluralNames = new string[7] { "Drowy", "Drowow", "Drowom", "Drowy", "Drowami", "Drowach", "Drowy" };

        public Drow( int raceID, int raceIndex ) : base( raceID, raceIndex, m_Names[(int)Cases.Mianownik], m_PluralNames[(int)Cases.Mianownik] )
        {
        }

        protected override string[] Names => m_Names;
        protected override string[] PluralNames => m_PluralNames;
        public override int DescNumber => 505819;
        public override int[] SkinHues => new int[] { 1109, 1108, 1107, 1106, 1409, 1897, 1898, 1899, 1908, 1907, 1906, 1905, 2106, 2105, 2306, 2305 };
        public override int[] HairHues => new int[] { 1102, 1103, 1104, 1105, 1106, 1107, 1108, 1109, 1900, 1901, 1902, 1903, 1904, 1905, 1906, 1907, 1908, 2101, 2102, 2103, 2104, 2105, 2106, 2301 };

        public override FacialHairItemID[] FacialHairStyles
        {
            get
            {
                return new FacialHairItemID[]
                {
                    FacialHairItemID.None,              
                    FacialHairItemID.Goatee,    
                    FacialHairItemID.Mustache,          
                    FacialHairItemID.Vandyke,
                };
            }
        }

        public override HairItemID[] MaleHairStyles
        {
            get
            {
                return new HairItemID[]
                {
                    HairItemID.None,
                    HairItemID.Short,
                    HairItemID.Long, 
                    HairItemID.PonyTail,
                    HairItemID.Pageboy, 
                    HairItemID.Receeding, 
                    HairItemID.Krisna
                };
            }
        }

        public override HairItemID[] FemaleHairStyles
        {
            get 
            {
                return new HairItemID[]
                {
                    HairItemID.None,
                    HairItemID.Short,
                    HairItemID.Long,
                    HairItemID.PonyTail,
                    HairItemID.Pageboy,
                    HairItemID.Buns,
                    HairItemID.TwoPigTails,
                    HairItemID.Krisna
                };
            }
        }
    }
}
