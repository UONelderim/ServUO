using System;
using System.Collections.Generic;

namespace Nelderim.Towns
{
	class TownCitizenship
	{
		private List<TownResource> Resources = new List<TownResource>();
		public DateTime JoinedDate { get; set; }

		public TownStatus CurrentTownStatus { get; set; } = TownStatus.None;

		public TownCounsellor CurrentTownConselourStatus { get; set; } = TownCounsellor.None;

		public Towns CurrentTown { get; set; } = Towns.None;

		public int SpentDevotion { get; set; }

		public int GetCurrentDevotion()
		{
			int m_devotion = 0;

			m_devotion += (int)(ResourceAmount(TownResourceType.Zloto) * 0.16f);

			m_devotion += (int)(ResourceAmount(TownResourceType.Material) * 0.2f);

			m_devotion += (int)(ResourceAmount(TownResourceType.Deski) * 0.25f);
			m_devotion += (int)(ResourceAmount(TownResourceType.Sztaby) * 0.25f);

			m_devotion += (int)(ResourceAmount(TownResourceType.Skora) * 0.33f);

			m_devotion += (int)(ResourceAmount(TownResourceType.Ziola) * 0.5f);
			m_devotion += (int)(ResourceAmount(TownResourceType.Klejnoty) * 0.5f);
			m_devotion += (int)(ResourceAmount(TownResourceType.Kosci) * 0.5f);

			m_devotion += (int)(ResourceAmount(TownResourceType.Zbroje) * 0.75f);
			m_devotion += (int)(ResourceAmount(TownResourceType.Piasek) * 0.75f);

			m_devotion += (int)(ResourceAmount(TownResourceType.Bronie) * 1.0f);
			m_devotion += (int)(ResourceAmount(TownResourceType.Kamienie) * 1.0f);

			return m_devotion;
		}

		public bool HasDevotion(int howMuch)
		{
			return (SpentDevotion + howMuch) <= GetCurrentDevotion();
		}

		public void UseDevotion(int howMuch)
		{
			SpentDevotion += howMuch;
		}

		public TownCitizenship(Towns newCurrentTown)
		{
			InitResources();
			ChangeCurrentTown(newCurrentTown);
			JoinedDate = DateTime.Now;
		}

		private void ChangeCurrentTown(Towns newCurrentTown, TownStatus newStatus)
		{
			InitResources();
			SpentDevotion = 0;
			CurrentTownStatus = TownStatus.Citizen;
			CurrentTownConselourStatus = TownCounsellor.None;
			CurrentTown = newCurrentTown;
			JoinedDate = DateTime.Now;
		}

		public void ChangeCurrentTown(Towns newCurrentTown)
		{
			ChangeCurrentTown(newCurrentTown, TownStatus.Citizen);
		}

		public void LeaveCurrentTown()
		{
			ChangeCurrentTown(Towns.None, TownStatus.None);
		}

		public void ChangeCurrentTownStatus(TownStatus newStatus)
		{
			CurrentTownStatus = newStatus;
		}

		public void ChangeCurrentTownConselourStatus(TownCounsellor newStatus)
		{
			CurrentTownConselourStatus = newStatus;
		}

		private void InitResources()
		{
			Resources = new List<TownResource>();
			Resources.Add(new TownResource(TownResourceType.Zloto, 0, -1, 0));
			Resources.Add(new TownResource(TownResourceType.Deski, 0, -1, 0));
			Resources.Add(new TownResource(TownResourceType.Sztaby, 0, -1, 0));
			Resources.Add(new TownResource(TownResourceType.Skora, 0, -1, 0));
			Resources.Add(new TownResource(TownResourceType.Material, 0, -1, 0));
			Resources.Add(new TownResource(TownResourceType.Kosci, 0, -1, 0));
			Resources.Add(new TownResource(TownResourceType.Kamienie, 0, -1, 0));
			Resources.Add(new TownResource(TownResourceType.Piasek, 0, -1, 0));
			Resources.Add(new TownResource(TownResourceType.Klejnoty, 0, -1, 0));
			Resources.Add(new TownResource(TownResourceType.Ziola, 0, -1, 0));
			Resources.Add(new TownResource(TownResourceType.Zbroje, 0, -1, 0));
			Resources.Add(new TownResource(TownResourceType.Bronie, 0, -1, 0));
		}

		public void ResourceIncreaseAmount(TownResourceType nType, int amount)
		{
			if (nType != TownResourceType.Invalid)
				Resources.Find(obj => obj.ResourceType == nType).Amount += amount;
		}

		public int ResourceAmount(TownResourceType nType)
		{
			if (nType != TownResourceType.Invalid)
				return Resources.Find(obj => obj.ResourceType == nType).Amount;
			return -1;
		}
	}
}
