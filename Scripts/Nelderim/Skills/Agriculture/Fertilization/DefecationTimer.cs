using System;
using Server.Items;

namespace Server.Mobiles
{
	public class DefecationTimer : Timer
	{
		public static TimeSpan DefaultDefecationInterval => TimeSpan.FromMinutes(4);

		private Mobile m_From;

		public DefecationTimer(Mobile m) : this(m, DefaultDefecationInterval)
		{
		}

		public DefecationTimer(Mobile from, TimeSpan interval) : base(TimeSpan.FromSeconds(Utility.Random(0, 15)), interval)
		{
			m_From = from;
		}

		protected override void OnTick()
		{
			if (m_From.Deleted)
			{
				Stop();
				return;
			}

			if (m_From.Map != null && m_From.Map != Map.Internal)
			{
				DungPile spawn = new DungPile();
				spawn.MoveToWorld(m_From.Location, m_From.Map);
			}
		}
	}
}