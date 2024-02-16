using System.Collections.Generic;
using Nelderim;

namespace Server
{
	public partial class Race
	{
		public static readonly int NRaceOffset = 0x20;

		public static Race NTamael => Races[NRaceOffset + 1];
		public static Race NJarling => Races[NRaceOffset + 2];
		public static Race NNaur => Races[NRaceOffset + 3];
		public static Race NElf => Races[NRaceOffset + 4];
		public static Race NDrow => Races[NRaceOffset + 5];
		public static Race NKrasnolud => Races[NRaceOffset + 6];

		public virtual string[] Names { get; }
		public virtual string[] PluralNames { get; }

		public virtual int DescNumber => 1072202; // Description

		public string GetName(Cases c = Cases.Mianownik)
		{
			return GetName(c, Names);
		}

		public string GetPluralName(Cases c)
		{
			return GetName(c, PluralNames);
		}

		private string GetName(Cases c, string[] list)
		{
			if (list == null || list.Length == 0) return Name;

			int index = (int)c;
			if (list[index] != null)
				return list[index];

			return "~ERROR~";
		}

		public bool MakeRandomAppearance(Mobile m)
		{
			if (!(m.BodyValue == 400 || m.BodyValue == 401))
				return false;

			m.HairItemID = RandomHair(m.Female);
			m.FacialHairItemID = RandomFacialHair(m.Female);
			m.HairHue = ClipHairHue(RandomHairHue());
			m.FacialHairHue = m.HairHue;
			m.Hue = ClipSkinHue(RandomSkinHue());

			return true;
		}

		public void AssignDefaultLanguages(Mobile m)
		{
			m.LanguagesKnown = new KnownLanguages();
			foreach (var keyValuePair in DefaultLanguages)
			{
				m.LanguagesKnown[keyValuePair.Key] = keyValuePair.Value;
			}
		}

		public virtual Dictionary<Language, ushort> DefaultLanguages => new Dictionary<Language, ushort>();
	}

	public enum Cases
	{
		Mianownik,
		Dopelniacz,
		Celownik,
		Biernik,
		Narzednik,
		Miejscownik,
		Wolacz
	}
}
