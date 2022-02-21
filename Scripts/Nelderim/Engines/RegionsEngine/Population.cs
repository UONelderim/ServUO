// 05.05.11 :: troyan

#region References

using Nelderim.Races;

#endregion

namespace Server.Nelderim
{
	public class Population
	{
		public Population(double[] proportions, double female)
		{
			Proportions = proportions;
			Female = female;
		}

		public Race GetRace
		{
			get
			{
				double rand = Utility.Random(0, 99);
				double cumsum = 0;
				int index = 0;

				for (int i = 0; i < NRace.AllRaces.Count; i++)
				{
					if ((cumsum += Proportions[i]) > rand)
					{
						index = i;
						break;
					}
				}

				return NRace.AllRaces[index];
			}
		}

		public double[] Proportions { get; }

		public double Female { get; }
	}
}
