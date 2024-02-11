#region References

using System.Collections.Generic;
using Server;

#endregion

namespace Nelderim.Races
{
	class NElf : NRace
	{
		public NElf(int raceId, int raceIndex) : base(raceId, raceIndex)
		{
		}

		public override string[] Names => new[] { "Elf", "Elfa", "Elfowi", "Elfa", "Elfem", "Elfie", "Elfie" };
		public override string[] PluralNames => new[] { "Elfy", "Elfow", "Elfom", "Elfow", "Elfami", "Elfach", "Elfy" };
		public override int DescNumber => 505818;

		public override int[] SkinHues => new[]
		{
			1410, 1411, 1412, 1413, 1414, 1415, 1416, 1417, 1418, 1419, 1420, 1421, 1422, 1423, 1424, 1425, 1426,
			1427, 1428, 1429, 1430, 1431, 1432, 1433, 1434, 1435, 1436, 1437, 1438, 1439, 1440, 1441, 1442, 1443,
			1444, 1445
		};

		public override int[] HairHues => new[]
		{
			1045, 1046, 1047, 1048, 1049, 1050, 1051, 1052, 1053, 1054, 1055, 1056, 1057, 1058, 1110, 1112, 1113,
			1114, 1115, 1116, 1117, 1118, 1119, 1120, 1121, 1122, 1123, 1124, 1125, 1126, 1127, 1128, 1129, 1130,
			1131, 1132, 1133, 1134, 1135, 1136, 1137, 1138, 1139, 1140, 1141, 1142, 1143, 1144, 1145, 1146, 1147,
			1148, 1149
		};

		public override int[][] HairTable { get; set; } =
		{
			new[] // Female
			{
				Hair.Elf.MidLong,
				Hair.Elf.LongFeather,
				Hair.Elf.Short,
				Hair.Elf.Mullet,
				Hair.Elf.Flower,
				Hair.Elf.Long,
				Hair.Elf.Knob,
				Hair.Elf.Braided,
				Hair.Elf.Bun,
				Hair.Elf.Spiked
			},
			new[] // Male
			{
				Hair.Elf.MidLong,
				Hair.Elf.LongFeather,
				Hair.Elf.Short,
				Hair.Elf.Mullet,
				Hair.Elf.Flower,
				Hair.Elf.Long,
				Hair.Elf.Braided,
				Hair.Elf.Spiked
			}
		};

		public override int[][] BeardTable { get; set; } =
		{
			new[] // Female
			{
				Beard.Elf.Clean
			},
			new[] // Male
			{
				Beard.Elf.Clean
			}
		};
		
		public override Dictionary<Language, ushort> DefaultLanguages => new()
		{
			{ Language.Powszechny, 1000 }, { Language.Elficki, 1000}
		};
	}
}
