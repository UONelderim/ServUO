using System;
using System.Collections.Generic;
using System.Text;

namespace Nelderim.Towns
{
    class TownCitizenship
    {
        private List<TownResource> Resources = new List<TownResource>();
        private DateTime m_joined;
        public DateTime JoinedDate
        {
            get { return m_joined; }
            set { m_joined = value; }
        }

        private TownStatus m_CurrentTownStatus = TownStatus.None;
        public TownStatus CurrentTownStatus
        {
            get { return m_CurrentTownStatus; }
            set { m_CurrentTownStatus = value; }
        }

        private TownCounsellor m_CurrentTownConselourStatus = TownCounsellor.None;
        public TownCounsellor CurrentTownConselourStatus
        {
            get { return m_CurrentTownConselourStatus; }
            set { m_CurrentTownConselourStatus = value; }
        }

        private Towns m_CurrentTown = Towns.None;
        public Towns CurrentTown
        {
            get { return m_CurrentTown; }
            set { m_CurrentTown = value; }
        }

        private int m_SpentDevotion = 0;
        public int SpentDevotion
        {
            get { return m_SpentDevotion; }
            set { m_SpentDevotion = value; }
        }

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
            return (m_SpentDevotion + howMuch) <= GetCurrentDevotion();
        }

        public void UseDevotion(int howMuch)
        {
            m_SpentDevotion += howMuch;
        }

        public TownCitizenship(Towns newCurrentTown) 
        {
            InitResources();
            ChangeCurrentTown(newCurrentTown);
            m_joined = DateTime.Now;
        }

        private void ChangeCurrentTown(Towns newCurrentTown, TownStatus newStatus)
        {
            InitResources();
            m_SpentDevotion = 0;
            CurrentTownStatus = TownStatus.Citizen;
            CurrentTownConselourStatus = TownCounsellor.None;
            CurrentTown = newCurrentTown;
            m_joined = DateTime.Now;
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
            else
                return -1;
        }
    }
}
