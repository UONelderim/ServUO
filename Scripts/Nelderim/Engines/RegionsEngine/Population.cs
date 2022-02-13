// 05.05.11 :: troyan

using Nelderim.Races;

namespace Server.Nelderim
{
	public class Population
	{
		private double[] m_Proportions;
		private double m_Female;

		public Population(double[] proportions, double female)
		{
			m_Proportions = proportions;
			m_Female = female;
		}

		public Race GetRace
		{
			get
			{
				double rand = Server.Utility.Random(0, 99);
				double cumsum = 0;
				int index = 0;

				for (int i = 0; i < NRace.AllRaces.Count; i++)
				{
					if ((cumsum += m_Proportions[i]) > rand)
					{
						index = i;
						break;
					}
				}

				return NRace.AllRaces[index];
			}
		}

		public double[] Proportions
		{
			get { return m_Proportions; }
		}

		public double Female
		{
			get { return m_Female; }
		}
	}
}
