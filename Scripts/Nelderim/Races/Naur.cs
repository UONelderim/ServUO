using Server;

namespace Nelderim.Races
{
    class Naur : NRace
    {
        protected static string[] m_Names = new string[7] { "Naur", "Naura", "Naurowi", "Naura", "Naurem", "Naurze", "Naur" };
        protected static string[] m_PluralNames = new string[7] { "Naurowie", "Naurow", "Naurom", "Naurow", "Naurami", "Naurach", "Naurowie" };

        public Naur( int raceID, int raceIndex ) : base( raceID, raceIndex, m_Names[(int)Cases.Mianownik], m_PluralNames[(int)Cases.Mianownik] )
        {
        }

        protected override string[] Names => m_Names;
        protected override string[] PluralNames => m_PluralNames;
        public override int DescNumber => 505817;
        public override int[] SkinHues => new int[] { 1146, 1147, 1148, 1149 };
        public override int[] HairHues => new int[] { 1102, 1103, 1104, 1105, 1106, 1107, 1108, 1109, 1007, 1008, 1014, 1015,
                                     1021, 1022, 1028, 1029, 1035, 1036, 1043, 1044, 1049, 1050, 1051, 1057,
                                     1058 };

 
        public override FacialHairItemID[] FacialHairStyles
        {
            get
            {
                return new FacialHairItemID[]
                {
                    FacialHairItemID.None,
                    FacialHairItemID.Goatee,
                    FacialHairItemID.Vandyke                    
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
                    HairItemID.PonyTail,
                    HairItemID.Mohawk,
                    HairItemID.Afro,
                    HairItemID.Receeding,

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
                    HairItemID.PonyTail,
                    HairItemID.Mohawk,
                    HairItemID.Afro,
                    HairItemID.Receeding,
                };
            }
        }
    }
}
