#region References

using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Misc;

#endregion

namespace Nelderim.Races
{
	public abstract class NRace : Race
	{
		new public static NRace[] Races { get; } = new NRace[0x100];

		new public static List<NRace> AllRaces { get; } = new();

		public static void Configure()
		{
			/* Here we configure all races. Some notes:
			* 
			* 1) The first 32 races are reserved for core use.
			* 2) Race 0x7F is reserved for core use.
			* 3) Race 0xFF is reserved for core use.
			* 4) Changing or removing any predefined races may cause server instability.
			*/
			RegisterNRace(new None(0, 0));
			RegisterNRace(new NTamael(1, 1));
			RegisterNRace(new NJarling(2, 2));
			RegisterNRace(new NNaur(3, 3));
			RegisterNRace(new NElf(4, 4));
			RegisterNRace(new NDrow(5, 5));
			RegisterNRace(new NKrasnolud(6, 6));
		}

		private static void RegisterNRace(NRace race)
		{
			Races[race.RaceIndex] = race;
			AllRaces.Add(race);
			RaceDefinitions.RegisterRace(race);
		}

		public static void Initialize()
		{
			EventSink.RaceChanged += OnRaceChanged;
		}
		
		private static void OnRaceChanged(RaceChangedEventArgs e)
		{
			e.NewRace.MakeRandomAppearance(e.Mobile);
			e.NewRace.AssignDefaultLanguages(e.Mobile);
		}

		public NRace(int raceId, int raceIndex) : base(raceId + NRaceOffset, raceIndex + NRaceOffset)
		{
		}

		public override string Name
		{
			get => GetName(Cases.Mianownik);
			set => throw new NotImplementedException();
		}

		public override string PluralName
		{
			get => GetPluralName(Cases.Mianownik);
			set => throw new NotImplementedException();
		}

		public override int MaleBody { get; set; } = 400;
		public override int FemaleBody { get; set; } = 401;

		public override int MaleGhostBody { get; set; } = 402;
		public override int FemaleGhostBody { get; set; } = 403;

		public override Expansion RequiredExpansion { get; set; } = Expansion.None;

		public override int[] ExclusiveEquipment { get; set; } = { };

		public override int[][] HairTable { get; set; } =
		{
			new[] // Female
			{
				Hair.Human.Short, Hair.Human.Long, Hair.Human.PonyTail, Hair.Human.Mohawk,
				Hair.Human.Pageboy, Hair.Human.Buns, Hair.Human.Afro, Hair.Human.PigTails, Hair.Human.Krisna,
			},
			new[] // Male
			{
				Hair.Human.Bald, Hair.Human.Short, Hair.Human.Long, Hair.Human.PonyTail, Hair.Human.Mohawk,
				Hair.Human.Pageboy, Hair.Human.Buns, Hair.Human.Afro, Hair.Human.Receeding, Hair.Human.Krisna,
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
				Beard.Human.Clean, Beard.Human.Long, Beard.Human.Short, Beard.Human.Goatee, Beard.Human.Mustache,
				Beard.Human.MidShort, Beard.Human.MidLong, Beard.Human.Vandyke,
			}
		};

		public override int[][] FaceTable { get; set; } =
		{
			new[] // Female
			{
				Face.Human.None, Face.Human.Face1, Face.Human.Face2, Face.Human.Face3, Face.Human.Face4,
				Face.Human.Face5, Face.Human.Face6, Face.Human.Face7, Face.Human.Face8, Face.Human.Face9,
				Face.Human.Face10,
			},
			new[] // Male
			{
				Face.Human.None, Face.Human.Face1, Face.Human.Face2, Face.Human.Face3, Face.Human.Face4,
				Face.Human.Face5, Face.Human.Face6, Face.Human.Face7, Face.Human.Face8, Face.Human.Face9,
				Face.Human.Face10,
			}
		};

		public override int[] SkinHues { get; set; } = Enumerable.Range(1002, 57).ToArray();
		public override int[] HairHues { get; set; } = Enumerable.Range(1102, 48).ToArray();
	}
}
