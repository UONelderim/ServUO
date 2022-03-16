using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Nelderim
{
	public class RegionsEngineRegion
	{
		public RegionsEngineRegion(string name, string parent)
		{
			Name = name;
			Parent = parent;
			RegionPopulation = null;
			Guards = new GuardEngine[] { null, null, null, null, null, null, null };
			Schools = null;
			BannedFollowers = null;
			TameLimit = 0;
			Intolerance = null;
			ResourceVeins = null;
			DifficultyLevelWeights = new Dictionary<DifficultyLevelValue, int>();
		}

		public string Name { get; }

		public string Parent { get; }

		public Population RegionPopulation { get; set; }

		public Race GetRace => RegionPopulation.GetRace;


		public double GetFemaleChance => RegionPopulation.Female;

		public GuardEngine[] Guards { get; set; }

		public bool[] Schools { get; set; }

		public bool MageryIsBanned
		{
			get { return Schools[0]; }
			set { Schools[0] = value; }
		}

		public bool ChivalryIsBanned
		{
			get { return Schools[1]; }
			set { Schools[1] = value; }
		}

		public bool NecromantionIsBanned
		{
			get { return Schools[2]; }
			set { Schools[2] = value; }
		}

		public bool DruidismIsBanned
		{
			get { return Schools[3]; }
			set { Schools[3] = value; }
		}

		public bool[] BannedFollowers { get; set; }

		public bool SummonsAreBanned
		{
			get { return BannedFollowers[0]; }
			set { BannedFollowers[0] = value; }
		}

		public bool FamiliarsAreBanned
		{
			get { return BannedFollowers[1]; }
			set { BannedFollowers[1] = value; }
		}

		public bool PetsAreBanned
		{
			get { return BannedFollowers[2]; }
			set { BannedFollowers[2] = value; }
		}

		public int TameLimit { get; set; }

		public int[] Intolerance { get; set; }

		public double[] ResourceVeins { get; set; }
		
		public Dictionary<DifficultyLevelValue, int> DifficultyLevelWeights { get; set; }
	}
}
