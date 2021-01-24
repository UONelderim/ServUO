using Server;

namespace Nelderim.Races
{
    class Krasnolud : NRace
    {
        protected static string[] m_Names = new string[7] { "Krasnolud", "Krasnoluda", "Krasnoludowi", "Krasnoluda", "Krasnoludem", "Krasnoludzie", "Krasnoludzie" };
        protected static string[] m_PluralNames = new string[7] { "Krasnoludy", "Krasnoludow", "Krasnoludom", "Krasnoludow", "Krasnoludami", "Krasnoludach", "Krasnoludy" };

        public Krasnolud( int raceID, int raceIndex ) : base( raceID, raceIndex, m_Names[(int)Cases.Mianownik], m_PluralNames[(int)Cases.Mianownik] )
        {
        }

        protected override string[] Names => m_Names;
        protected override string[] PluralNames => m_PluralNames;
        public override int DescNumber => 505820;
        public override int[] SkinHues => new int[] { 1037, 1038, 1039, 1040, 1041, 1042, 1043, 2307, 2308, 2309, 2310, 2311 };
        public override int[] HairHues => new int[] { 1045, 1046, 1047, 1048, 1049, 1050, 1051, 1052, 1053, 1054, 1055, 1056,
                                     1057, 1058, 1110, 1112, 1113, 1114, 1115, 1116, 1117, 1118, 1119, 1120,
                                     1121, 1122, 1123, 1124, 1125, 1126, 1127, 1128, 1129, 1130, 1131, 1132,
                                     1133, 1134, 1135, 1136, 1137, 1138, 1139, 1140, 1141, 1142, 1143, 1144,
                                     1145, 1146, 1147, 1148, 1149 };

        public override FacialHairItemID[] FacialHairStyles => new FacialHairItemID[]
        {
            FacialHairItemID.LongBeard,
            FacialHairItemID.ShortBeard,
            FacialHairItemID.Goatee,
            FacialHairItemID.Mustache,
            FacialHairItemID.MediumShortBeard,
            FacialHairItemID.MediumLongBeard,
            FacialHairItemID.Vandyke
        };

        public override bool ValidateFacialHair( bool female, int itemID )
        {
            foreach ( FacialHairItemID hairItem in FacialHairStyles )
            {
                if ( (int)hairItem == itemID ) return true;
            }
            return false;
        }
    }
}
