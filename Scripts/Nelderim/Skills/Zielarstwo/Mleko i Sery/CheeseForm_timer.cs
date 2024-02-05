#region References

using System;

#endregion

namespace Server.Items
{
	// Le timer de fermentation
	public class FromQuiFermente : Timer
	{
		private readonly CheeseForm m_TimerMoulVar;
		private int QualityContest;

		public FromQuiFermente(CheeseForm m_TimerCheeseForm) :
			base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(72),
				100) //base( TimeSpan.FromSeconds(delai avant premier tick),TimeSpan.FromSeconds(durée du tick),nombre de répétition)
		{
			Priority = TimerPriority.FiftyMS;
			m_TimerMoulVar = m_TimerCheeseForm;
		}

		protected override void OnTick()
		{
			if (m_TimerMoulVar.m_StadeFermentation <= 99)
			{
				++m_TimerMoulVar.m_StadeFermentation;
			}

			if (m_TimerMoulVar.m_StadeFermentation == 100)
			{
				m_TimerMoulVar.Name = m_TimerMoulVar.Name + " *GOTOWE*";
				m_TimerMoulVar.m_StadeFermentation = 0;
				m_TimerMoulVar.m_Fermentation = false;
				m_TimerMoulVar.m_ContientUnFromton = true;
				m_TimerMoulVar.m_FromageQual = Utility.Random(1, 100);
			}
		}
	}
}
