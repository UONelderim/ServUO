#region References

using System.Collections.Generic;
using Server;

#endregion

namespace Nelderim.Races
{
	class NNaur : NRace
	{
		public NNaur(int raceId, int raceIndex) : base(raceId, raceIndex)
		{
		}

		public override string[] Names => new[] { "Naur", "Naura", "Naurowi", "Naura", "Naurem", "Naurze", "Naur" };

		public override string[] PluralNames =>
			new[] { "Naurowie", "Naurow", "Naurom", "Naurow", "Naurami", "Naurach", "Naurowie" };

		public override int DescNumber => 505817;
		public override int[] SkinHues => new[] { 1146, 1147, 1148, 1149 };

		public override int[] HairHues => new[]
		{
			1102, 1103, 1104, 1105, 1106, 1107, 1108, 1109, 1007, 1008, 1014, 1015, 1021, 1022, 1028, 1029, 1035,
			1036, 1043, 1044, 1049, 1050, 1051, 1057, 1058
		};

		public override int[][] HairTable { get; set; } =
		{
			new[] // Female
			{
				Hair.Human.Bald, Hair.Human.Short, Hair.Human.PonyTail, Hair.Human.Mohawk, Hair.Human.Afro,
				Hair.Human.Receeding,
			},
			new[] // Male
			{
				Hair.Human.Bald, Hair.Human.Short, Hair.Human.PonyTail, Hair.Human.Mohawk, Hair.Human.Afro,
				Hair.Human.Receeding,
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
				Beard.Human.Clean, Beard.Human.Goatee, Beard.Human.Vandyke,
			}
		};
		
		public override Dictionary<Language, ushort> DefaultLanguages => new()
		{
			{ Language.Powszechny, 1000 },
		};
	}
}
