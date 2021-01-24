using Server;

namespace Nelderim.Races
{
    class Elf : NRace
    {
        protected static string[] m_Names = new string[7] { "Elf", "Elfa", "Elfowi", "Elfa", "Elfem", "Elfie", "Elfie" };
        protected static string[] m_PluralNames = new string[7] { "Elfy", "Elfow", "Elfom", "Elfow", "Elfami", "Elfach", "Elfy" };

        public Elf( int raceID, int raceIndex ) : base( raceID, raceIndex, m_Names[(int)Cases.Mianownik], m_PluralNames[(int)Cases.Mianownik] )
        {
        }

        protected override string[] Names => m_Names;
        protected override string[] PluralNames => m_PluralNames;
        public override int DescNumber => 505818;
        public override int[] SkinHues => new int[] { 1410, 1411, 1412, 1413, 1414, 1415, 1416, 1417, 1418, 1419, 1420, 1421,
                                     1422, 1423, 1424, 1425, 1426, 1427, 1428, 1429, 1430, 1431, 1432, 1433,
                                     1434, 1435, 1436, 1437, 1438, 1439, 1440, 1441, 1442, 1443, 1444, 1445 };
        public override int[] HairHues => new int[] { 1045, 1046, 1047, 1048, 1049, 1050, 1051, 1052, 1053, 1054, 1055, 1056,
                                     1057, 1058, 1110, 1112, 1113, 1114, 1115, 1116, 1117, 1118, 1119, 1120,
                                     1121, 1122, 1123, 1124, 1125, 1126, 1127, 1128, 1129, 1130, 1131, 1132,
                                     1133, 1134, 1135, 1136, 1137, 1138, 1139, 1140, 1141, 1142, 1143, 1144,
                                     1145, 1146, 1147, 1148, 1149 };

        public override FacialHairItemID[] FacialHairStyles => new FacialHairItemID[]
        {
            FacialHairItemID.None,              
            FacialHairItemID.Goatee,    
            FacialHairItemID.Mustache,          
            FacialHairItemID.Vandyke,
        };

        public override HairItemID[] MaleHairStyles => new HairItemID[]
        {
            HairItemID.None,
            HairItemID.Short,
            HairItemID.Long,
            HairItemID.Pageboy,
            HairItemID.Receeding,
        };


        public override HairItemID[] FemaleHairStyles => new HairItemID[]
        {
            HairItemID.Short,
            HairItemID.Long,
            HairItemID.PonyTail,
            HairItemID.Pageboy,
            HairItemID.Buns,
            HairItemID.TwoPigTails,
        };
    }
}
