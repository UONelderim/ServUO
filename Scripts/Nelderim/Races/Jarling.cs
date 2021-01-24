using Server;

namespace Nelderim.Races
{
    class Jarling : NRace
    {
        protected static string[] m_Names = new string[7] { "Jarling", "Jarlinga", "Jarlingowi", "Jarlinga", "Jarlingiem", "Jarlingu", "Jarlingu" };
        protected static string[] m_PluralNames = new string[7] { "Jarlingowie", "Jarlingow", "Jarlingom", "Jarlingow", "Jarlingami", "Jarlingch", "Jarlingowie" };

        public Jarling( int raceID, int raceIndex ) : base( raceID, raceIndex, m_Names[(int)Cases.Mianownik], m_PluralNames[(int)Cases.Mianownik] )
        {
        }

        protected override string[] Names => m_Names;
        protected override string[] PluralNames => m_PluralNames;
        public override int DescNumber => 505817;
        public override int[] SkinHues => new int[] { 1002, 1003, 1009, 1010, 1016, 1017, 1018, 1019, 1024 };
        public override int[] HairHues => new int[] { 1110, 1111, 1112, 1113, 1114, 1115, 1118, 1119, 1120, 1121, 1122, 1123,
                                     1126, 1127, 1128, 1129 }; 
    }
}
