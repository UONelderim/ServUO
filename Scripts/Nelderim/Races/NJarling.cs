using System.Collections.Generic;

namespace Nelderim.Races
{
	class NJarling : NRace
	{
		public NJarling(int raceId, int raceIndex) : base(raceId, raceIndex)
		{
		}

		public override string[] Names => new[]
		{
			"Jarling", "Jarlinga", "Jarlingowi", "Jarlinga", "Jarlingiem", "Jarlingu", "Jarlingu"
		};

		public override string[] PluralNames => new[]
		{
			"Jarlingowie", "Jarlingow", "Jarlingom", "Jarlingow", "Jarlingami", "Jarlingch", "Jarlingowie"
		};

		public override int DescNumber => 505817;
		public override int[] SkinHues => new[] { 1002, 1003, 1009, 1010, 1016, 1017, 1018, 1019, 1024 };

		public override int[] HairHues => new[]
		{
			1110, 1111, 1112, 1113, 1114, 1115, 1118, 1119, 1120, 1121, 1122, 1123, 1126, 1127, 1128, 1129
		};
		
		public override Dictionary<NLanguage, ushort> DefaultLanguages => new()
		{
			{ NLanguage.Powszechny, 1000 }, { NLanguage.Jarlowy, 1000}
		};
	}
}
