#region References

using System.Collections.Generic;
using Server;

#endregion

namespace Nelderim.Races
{
	class NDrow : NRace
	{
		public NDrow(int raceId, int raceIndex) : base(raceId, raceIndex)
		{
		}

		public override string[] Names => new[] { "Drow", "Drowa", "Drowowi", "Drowa", "Drowem", "Drowie", "Drowie" };

		public override string[] PluralNames =>
			new[] { "Drowy", "Drowow", "Drowom", "Drowy", "Drowami", "Drowach", "Drowy" };

		public override int DescNumber => 505819;

		public override int[] SkinHues => new[]
		{
			1109, 1108, 1107, 1106, 1409, 1897, 1898, 1899, 1908, 1907, 1906, 1905, 2106, 2105, 2306, 2305
		};

		public override int[] HairHues => new[]
		{
			1102, 1103, 1104, 1105, 1106, 1107, 1108, 1109, 1900, 1901, 1902, 1903, 1904, 1905, 1906, 1907, 1908,
			2101, 2102, 2103, 2104, 2105, 2106, 2301
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
		
		public override Dictionary<NLanguage, ushort> DefaultLanguages => new()
		{
			{ NLanguage.Powszechny, 1000 }, { NLanguage.Drowi, 1000}
		};
	}
}
