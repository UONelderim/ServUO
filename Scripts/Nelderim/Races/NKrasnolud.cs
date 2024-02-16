#region References

using System.Collections.Generic;
using Server;

#endregion

namespace Nelderim.Races
{
	class NKrasnolud : NRace
	{
		public NKrasnolud(int raceId, int raceIndex) : base(raceId, raceIndex)
		{
		}

		public override string[] Names => new[]
		{
			"Krasnolud", "Krasnoluda", "Krasnoludowi", "Krasnoluda", "Krasnoludem", "Krasnoludzie", "Krasnoludzie"
		};

		public override string[] PluralNames => new[]
		{
			"Krasnoludy", "Krasnoludow", "Krasnoludom", "Krasnoludow", "Krasnoludami", "Krasnoludach", "Krasnoludy"
		};

		public override int DescNumber => 505820;

		public override int[] SkinHues =>
			new[] { 1037, 1038, 1039, 1040, 1041, 1042, 1043, 2307, 2308, 2309, 2310, 2311 };

		public override int[] HairHues => new[]
		{
			1045, 1046, 1047, 1048, 1049, 1050, 1051, 1052, 1053, 1054, 1055, 1056, 1057, 1058, 1110, 1112, 1113,
			1114, 1115, 1116, 1117, 1118, 1119, 1120, 1121, 1122, 1123, 1124, 1125, 1126, 1127, 1128, 1129, 1130,
			1131, 1132, 1133, 1134, 1135, 1136, 1137, 1138, 1139, 1140, 1141, 1142, 1143, 1144, 1145, 1146, 1147,
			1148, 1149
		};

		public override int[][] BeardTable { get; set; } =
		{
			new[] // Female
			{
				Beard.Human.Long, Beard.Human.Short, Beard.Human.Goatee, Beard.Human.Mustache, Beard.Human.MidShort,
				Beard.Human.MidLong, Beard.Human.Vandyke,
			},
			new[] // Male
			{
				Beard.Human.Long, Beard.Human.Short, Beard.Human.Goatee, Beard.Human.Mustache, Beard.Human.MidShort,
				Beard.Human.MidLong, Beard.Human.Vandyke,
			}
		};
		
		public override Dictionary<Language, ushort> DefaultLanguages => new()
		{
			{ Language.Powszechny, 1000 }, { Language.Krasnoludzki, 1000}
		};
	}
}
