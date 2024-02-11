using System.Collections.Generic;

namespace Nelderim.Races
{
	class NTamael : NRace
	{
		public NTamael(int raceId, int raceIndex) : base(raceId, raceIndex)
		{
		}

		public override string[] Names =>
			new[] { "Tamael", "Tamaela", "Tamaelowi", "Tamaela", "Tamaelem", "Tamaelu", "Tamaelu" };

		public override string[] PluralNames => new[]
		{
			"Tamaelowie", "Tamaelow", "Tamaelom", "Tamaelow", "Tamaelami", "Tamaelach", "Tamaelowie"
		};

		public override int DescNumber => 505817;

		public override int[] SkinHues => new[]
		{
			1023, 1026, 1027, 1030, 1031, 1033, 1034, 1035, 1037, 1038, 1039, 1040, 1041, 1042, 1043
		};

		public override int[] HairHues => new[]
		{
			1102, 1103, 1104, 1105, 1106, 1107, 1108, 1109, 1110, 1111, 1112, 1113, 1114, 1115, 1116, 1117, 1118,
			1119, 1120, 1121, 1122, 1123, 1124, 1125, 1126, 1127, 1128, 1129, 1130, 1131, 1132, 1133, 1134, 1135,
			1136, 1137, 1138, 1139, 1140, 1141, 1142, 1143, 1144, 1145, 1146, 1147, 1148, 1149
		};
		
		public override Dictionary<Language, ushort> DefaultLanguages => new()
		{
			{ Language.Powszechny, 1000 },
		};
	}
}
