// 05.05.11 :: troyan

using System;
using System.Collections;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Nelderim
{
	
	public class Population
	{
		private double[] m_Proportions;
		
		public Population ( double[] proportions )
		{
			m_Proportions = proportions;
		}
		
		public Race GetRace
		{
			get
			{
				double rand = Server.Utility.Random( 0, 99 );
                double cumsum = 0;
                int index = 0;
				
				for( int i = 0; i < Race.AllRaces.Count; i++)
				{
					if (  ( cumsum += m_Proportions[i] ) > rand )
					{
						index = i;
						break;
					}
				}

                return Race.AllRaces[ index ];
			}
		}
		
		public double[] Proportions
		{
			get { return m_Proportions; }
		}
	}
}
