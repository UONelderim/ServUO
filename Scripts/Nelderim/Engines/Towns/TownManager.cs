using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Items;

namespace Nelderim.Towns
{
    /* Building template 
    tmpBuilding = new TownBuilding(TownBuildingName.MiejsceSpotkan, 0, TownBuildingStatus.Dziala, 1063813);
    tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto,    0, 0, 0));
    tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski,    0, 0, 0));
    tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby,   0, 0, 0));
    tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora,    0, 0, 0));
    tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 0, 0, 0));
    tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci,    0, 0, 0));
    tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 0, 0, 0));
    tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek,   0, 0, 0));
    tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 0, 0, 0));
    tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola,    0, 0, 0));
    tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zbroje,   0, 0, 0));
    tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Bronie,   0, 0, 0));
    AddBuilding(tmpBuilding);
     * 
     * Wyjasnienie
     * tmpBuilding.Resources.Resources.Add(new TownResource(Jaki resource,    ile resourca trzeba zaplacic, o ile maximum jest zwiekszone w skarbcu, jak wplywa dziennie na roznice));
     * Wplyw dzienny na roznice jest liczony osobno do oplaty za budynek, oplata za budynek jest oplacana jako 1 promil ceny dla wszystkich zasobow ktore byly potrzebne do budowy
     * dodatkowo budynek moze miec umiejetnosc zmiany resourca w skarbcu (dodatnio lub ujemnie)
    */
    class TownManager
    {
        public Towns Town;
        private TownResources m_resources = new TownResources();
        public TownResources Resources
        {
            get { return m_resources; }
            set { m_resources = value; }
        }

        private TownResources m_baseResources = new TownResources();
        public TownResources BaseResources
        {
            get { return m_baseResources; }
            set { m_baseResources = value; }
        }

        private List<TownBuilding> m_buildings = new List<TownBuilding>();
        public List<TownBuilding> Buildings
        {
            get { return m_buildings; }
            set { m_buildings = value; }
        }

        private List<TownPost> m_posts = new List<TownPost>();
        public List<TownPost> TownPosts
        {
            get { return m_posts; }
            set { m_posts = value; }
        }

        private List<TownLog> m_logs = new List<TownLog>();
        public List<TownLog> TownLogs
        {
            get { return m_logs; }
            set { m_logs = value; }
        }

        private List<TownRelation> m_relations = new List<TownRelation>();
        public List<TownRelation> TownRelations
        {
            get { return m_relations; }
            set { m_relations = value; }
        }

        public int MaxGuards
        {
            get { return CalculateMaxGuards(); }
        }

        public int MaxPosts
        {
            get { return 35; }
        }

        public int MaxTownLogs
        {
            get { return 35; }
        }

        private bool m_informLeader = true;
        public bool InformLeader
        {
            get { return m_informLeader; }
            set { m_informLeader = value; }
        }

        public void AddLog(TownLogTypes tlp, string txt = "", int a = 0, int b = 0, int c = 0)
        {
            if (TownLogs.Count >= MaxTownLogs)
            {
                TownLogs.RemoveAt(0);
            }
            TownLogs.Add(new TownLog(DateTime.Now, tlp, txt, a, b, c));
            if (InformLeader)TownDatabase.InformLeader(Town, TownLogs[TownLogs.Count - 1].ToString());
        }

        public void AddLogOnDeserialization(DateTime time, TownLogTypes tlp, string txt = "", int a = 0, int b = 0, int c = 0)
        {
            if (TownLogs.Count >= MaxTownLogs)
            {
                TownLogs.RemoveAt(0);
            }
            TownLogs.Add(new TownLog(time, tlp, txt, a, b, c));
        }

        private int m_RessurecFrequency = 60;
        public int RessurectFrequency
        {
            get { return m_RessurecFrequency; }
            set { m_RessurecFrequency = value; }
        }

        private DateTime m_last_ressurect = DateTime.Now; // Zapisana data kiedy ostatni raz bylo wykonanie sprawdzenie wskrzeszenia straznikow
        public DateTime LastRessurectTime
        {
            get { return m_last_ressurect; }
            set { m_last_ressurect = value; }
        }

        private bool m_taxesChanges = true;
        public bool TaxChangeAvailable
        {
            get { return m_taxesChanges; }
            set { m_taxesChanges = value; }
        }

        private int m_taxesForThisTown = 0;
        public int TaxesForThisTown
        {
            get { return m_taxesForThisTown; }
            set { m_taxesForThisTown = value; }
        }

        private int m_taxesForOtherTowns = 0;
        public int TaxesForOtherTowns
        {
            get { return m_taxesForOtherTowns; }
            set { m_taxesForOtherTowns = value; }
        }

        private int m_taxesForNoTown = 0;
        public int TaxesForNoTown
        {
            get { return m_taxesForNoTown; }
            set { m_taxesForNoTown = value; }
        }

        public TownManager(Towns newCurrentTown)
        {
            Town = newCurrentTown;
            #region Ustawienia skarbca
            m_baseResources = new TownResources();
            m_baseResources.Resources.Add(new TownResource(TownResourceType.Zloto, 0, -1, 0));
            m_baseResources.Resources.Add(new TownResource(TownResourceType.Deski, 0, 100000, 0));
            m_baseResources.Resources.Add(new TownResource(TownResourceType.Sztaby, 0, 100000, 0));
            m_baseResources.Resources.Add(new TownResource(TownResourceType.Skora, 0, 20000, 0));
            m_baseResources.Resources.Add(new TownResource(TownResourceType.Material, 0, 20000, 0));
            m_baseResources.Resources.Add(new TownResource(TownResourceType.Kosci, 0, 5000, 0));
            m_baseResources.Resources.Add(new TownResource(TownResourceType.Kamienie, 0, 5000, 0));
            m_baseResources.Resources.Add(new TownResource(TownResourceType.Piasek, 0, 20000, 0));
            m_baseResources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 0, 2000, 0));
            m_baseResources.Resources.Add(new TownResource(TownResourceType.Ziola, 0, 50000, 0));
            m_baseResources.Resources.Add(new TownResource(TownResourceType.Zbroje, 0, 3000, 0));
            m_baseResources.Resources.Add(new TownResource(TownResourceType.Bronie, 0, 3000, 0));
            m_resources = new TownResources();
            m_resources.Resources.Add(new TownResource(TownResourceType.Zloto, 0, 0, 0));
            m_resources.Resources.Add(new TownResource(TownResourceType.Deski, 0, 0, 0));
            m_resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 0, 0, 0));
            m_resources.Resources.Add(new TownResource(TownResourceType.Skora, 0, 0, 0));
            m_resources.Resources.Add(new TownResource(TownResourceType.Material, 0, 0, 0));
            m_resources.Resources.Add(new TownResource(TownResourceType.Kosci, 0, 0, 0));
            m_resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 0, 0, 0));
            m_resources.Resources.Add(new TownResource(TownResourceType.Piasek, 0, 0, 0));
            m_resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 0, 0, 0));
            m_resources.Resources.Add(new TownResource(TownResourceType.Ziola, 0, 0, 0));
            m_resources.Resources.Add(new TownResource(TownResourceType.Zbroje, 0, 0, 0));
            m_resources.Resources.Add(new TownResource(TownResourceType.Bronie, 0, 0, 0));
            m_resources.ResourceMaxAmountSet(TownResourceType.Zloto, m_baseResources.ResourceMaxAmount(TownResourceType.Zloto));
            m_resources.ResourceMaxAmountSet(TownResourceType.Deski, m_baseResources.ResourceMaxAmount(TownResourceType.Deski));
            m_resources.ResourceMaxAmountSet(TownResourceType.Sztaby, m_baseResources.ResourceMaxAmount(TownResourceType.Sztaby));
            m_resources.ResourceMaxAmountSet(TownResourceType.Skora, m_baseResources.ResourceMaxAmount(TownResourceType.Skora));
            m_resources.ResourceMaxAmountSet(TownResourceType.Material, m_baseResources.ResourceMaxAmount(TownResourceType.Material));
            m_resources.ResourceMaxAmountSet(TownResourceType.Kosci, m_baseResources.ResourceMaxAmount(TownResourceType.Kosci));
            m_resources.ResourceMaxAmountSet(TownResourceType.Kamienie, m_baseResources.ResourceMaxAmount(TownResourceType.Kamienie));
            m_resources.ResourceMaxAmountSet(TownResourceType.Piasek, m_baseResources.ResourceMaxAmount(TownResourceType.Piasek));
            m_resources.ResourceMaxAmountSet(TownResourceType.Klejnoty, m_baseResources.ResourceMaxAmount(TownResourceType.Klejnoty));
            m_resources.ResourceMaxAmountSet(TownResourceType.Ziola, m_baseResources.ResourceMaxAmount(TownResourceType.Ziola));
            m_resources.ResourceMaxAmountSet(TownResourceType.Zbroje, m_baseResources.ResourceMaxAmount(TownResourceType.Zbroje));
            m_resources.ResourceMaxAmountSet(TownResourceType.Bronie, m_baseResources.ResourceMaxAmount(TownResourceType.Bronie));
            #endregion
            // Ustawienia budynkow
            TownBuilding tmpBuilding;
            #region LVL 0
            tmpBuilding = new TownBuilding(TownBuildingName.MiejsceSpotkan, 0, TownBuildingStatus.Dziala, 1063813);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto,    50000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski,    3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby,   3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora,    300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 30, 0, 0));
            tmpBuilding.Charge = false;
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.DomZdziercy, 0, TownBuildingStatus.Dziala, 1063814);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 50000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 30, 0, 0));
            tmpBuilding.Charge = false;
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Bank, 0, TownBuildingStatus.Dziala, 1063815);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 50000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 30, 0, 0));
            tmpBuilding.Charge = false;
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.DomUzdrowiciela, 0, TownBuildingStatus.Dziala, 1063816);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 30000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 30, 0, 0));
            tmpBuilding.Charge = false;
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Karczma, 0, TownBuildingStatus.Dziala, 1063817);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 50000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 30, 0, 0));
            tmpBuilding.Charge = false;
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Stajnia, 0, TownBuildingStatus.Dziala, 1063818);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 50000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 10000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 1000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 100, 0, 0));
            tmpBuilding.Charge = false;
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.DuszaMiasta, 0, TownBuildingStatus.Dziala, 1063819);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 200000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 30000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 5000, 0, 0));
            tmpBuilding.Charge = false;
            AddBuilding(tmpBuilding);
			tmpBuilding = new TownBuilding(TownBuildingName.ZagrodaNaOwce, 0, TownBuildingStatus.Dostepny, 1063947);
			tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski,    5000, 0, 0));
			tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby,   300, 0, 0));
			tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora,    250, 0, 0));
			tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 300, 0, 0));
			tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci,    30, 0, 0));
			tmpBuilding.Charge = false;
			AddBuilding(tmpBuilding);
            #endregion
            #region LVL 1
            tmpBuilding = new TownBuilding(TownBuildingName.WarsztatKrawiecki, 1, TownBuildingStatus.Dostepny, 1063820);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 100000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 5000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 3000, 100, 15));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 30, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 300, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.DomZdziercy);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.WarsztatKowalski, 1, TownBuildingStatus.Dostepny, 1063821);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 100000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 1000, 500, 50));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 1000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 1000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 500, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Bronie, 0, 100, 1));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zbroje, 0, 100, 1));
            tmpBuilding.Dependecies.Add(TownBuildingName.Bank);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.WarsztatStolarski, 1, TownBuildingStatus.Dostepny, 1063822);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 80000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 500, 50));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 100, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 500, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 30, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 300, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.DomZdziercy);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.WarsztatMajstra, 1, TownBuildingStatus.Dostepny, 1063823);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 80000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 100, 20));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 100, 20));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 100, 20));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 3000, 100, 20));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 30, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 300, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.DomZdziercy);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.WarsztatMaga, 1, TownBuildingStatus.Dostepny, 1063824);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 100000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 3000, 100, 5));
            tmpBuilding.Dependecies.Add(TownBuildingName.Karczma);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.WarsztatAlchemika, 1, TownBuildingStatus.Dostepny, 1063825);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 80000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 2000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 80, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 300, 100, 15));
            tmpBuilding.Dependecies.Add(TownBuildingName.Karczma);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.WarsztatLukmistrza, 1, TownBuildingStatus.Dostepny, 1063826);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 80000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 50, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Bronie, 0, 30, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Stajnia);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Piekarnia, 1, TownBuildingStatus.Dostepny, 1063827);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 80000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 300, 100, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 300, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Karczma);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Farma, 1, TownBuildingStatus.Dostepny, 1063828);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 80000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 300, 100, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 300, 10, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Stajnia);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Port, 1, TownBuildingStatus.Dostepny, 1063829);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 80000, 0, 50));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 1000, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 1000, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 3000, 1000, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 300, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Bank);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.DomGornika, 1, TownBuildingStatus.Dostepny, 1063830);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 80000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 100, 100));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 500, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 300, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.DomUzdrowiciela);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Palisada, 1, TownBuildingStatus.Dostepny, 1063831);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 200000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 100, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Stajnia);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.PlacTreningowy, 1, TownBuildingStatus.Dostepny, 1063857);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 50000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 250, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Bronie, 100, 200, -5));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zbroje, 100, 200, -5));
            tmpBuilding.Dependecies.Add(TownBuildingName.WarsztatLukmistrza);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.MostDrewniany, 1, TownBuildingStatus.Dostepny, 1063832);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 30000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 1000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 50, 0, 0));
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Targowisko, 1, TownBuildingStatus.Dostepny, 1063833);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 60000, 0, 10));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 300, 500, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 100, 100, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 300, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Bank);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Kapliczka, 1, TownBuildingStatus.Dostepny, 1063834);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 50000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 500, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 1000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 300, 100, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.DomUzdrowiciela);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Spichlerz, 1, TownBuildingStatus.Dostepny, 1063835);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 100000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 500, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 500, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 300, 500, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 3000, 500, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 150, 500, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 50, 500, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 50, 500, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 50, 500, -10));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 200, 500, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.DomZdziercy);
            AddBuilding(tmpBuilding);
            #endregion
            #region LVL 2
            tmpBuilding = new TownBuilding(TownBuildingName.Arena, 2, TownBuildingStatus.Dostepny, 1063843);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 100000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 2000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 2000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 1000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 1500, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 200, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 1000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Bronie, 50, 200, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zbroje, 50, 100, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.PlacTreningowy);
            tmpBuilding.Dependecies.Add(TownBuildingName.WarsztatMajstra);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Fosa, 2, TownBuildingStatus.Dostepny, 1063844);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 15000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 200, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 200, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 200, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 50, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Koszary);
            tmpBuilding.Dependecies.Add(TownBuildingName.DomGornika);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.BramaDrewniana, 2, TownBuildingStatus.Dostepny, 1063845);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 40000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 2000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 500, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 25, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Koszary);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.WiezaStraznica, 2, TownBuildingStatus.Dostepny, 1063846);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 50000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 1000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 1000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 200, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 25, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 25, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Bronie, 20, 50, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zbroje, 20, 50, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Koszary);
            tmpBuilding.Dependecies.Add(TownBuildingName.WarsztatMajstra);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Koszary, 2, TownBuildingStatus.Dostepny, 1063847);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 200000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 5000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 500, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 600, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Bronie, 200, 1000, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zbroje, 200, 1000, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.WarsztatKowalski);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.MostKamienny, 2, TownBuildingStatus.Dostepny, 1063848);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 90000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 2000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 200, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 25, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Koszary);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.RozbudowaTargowiska, 2, TownBuildingStatus.Dostepny, 1063849);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 40000, 0, 20));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 1000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 1000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 25, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 200, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Targowisko);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Swiatynia, 2, TownBuildingStatus.Dostepny, 1063850);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 200000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 5000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 200, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 1000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 2000, 100, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Kapliczka);
            tmpBuilding.Dependecies.Add(TownBuildingName.DomUzdrowiciela);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Biblioteka, 2, TownBuildingStatus.Dostepny, 1063851);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 200000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 5000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 500, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 1000, 100, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 100, 50, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 100, 50, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 1000, 50, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Kapliczka);
            tmpBuilding.Dependecies.Add(TownBuildingName.WarsztatMaga);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.SkladZapasow, 2, TownBuildingStatus.Dostepny, 1063852);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 150000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 500, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 4000, 1000, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 500, 200, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 1200, 3000, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 50, 500, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 50, 200, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 50, 1000, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 50, 3000, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 500, 3000, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Spichlerz);
            AddBuilding(tmpBuilding);
            #endregion
            #region LVL 3
            tmpBuilding = new TownBuilding(TownBuildingName.MurObronny, 3, TownBuildingStatus.Dostepny, 1063836);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 500000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 6000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 6000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 200, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 500, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 200, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 100, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Twierdza);
            tmpBuilding.Dependecies.Add(TownBuildingName.WarsztatMajstra);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.BramaStalowa, 3, TownBuildingStatus.Dostepny, 1063837);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 80000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 500, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 4000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 500, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 50, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Twierdza);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Twierdza, 3, TownBuildingStatus.Dostepny, 1063838);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 300000, 0, 100));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 5000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 10000, 100, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 1000, 100, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 1000, 100, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 200, 100, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 200, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 200, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 200, 0, -10));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Bronie, 300, 5000, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zbroje, 300, 5000, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Koszary);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Ratusz, 3, TownBuildingStatus.Dostepny, 1063839);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 150000, 0, 50));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 4000, 0, 100));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 4000, 0, 100));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 200, 0, 20));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 500, 0, 50));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 100, 0, 15));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 100, 0, 5));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 10, 0, 5));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 200, 1000, -5));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 0, 10, 50));
            tmpBuilding.Dependecies.Add(TownBuildingName.WarsztatMaga);
            tmpBuilding.Dependecies.Add(TownBuildingName.Bank);
            tmpBuilding.Dependecies.Add(TownBuildingName.WarsztatMajstra);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Ambasada, 3, TownBuildingStatus.Dostepny, 1063840);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 100000, 0, 200));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 400, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 800, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 100, 100, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Bronie, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zbroje, 50, 0, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Biblioteka);
            tmpBuilding.Dependecies.Add(TownBuildingName.Ratusz);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Torturownia, 3, TownBuildingStatus.Dostepny, 1063841);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 100000, 0, 100));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 3000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 400, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 800, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 50, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 100, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 500, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Bronie, 100, 500, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zbroje, 100, 500, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.Koszary);
            AddBuilding(tmpBuilding);
            tmpBuilding = new TownBuilding(TownBuildingName.Skarbiec, 3, TownBuildingStatus.Dostepny, 1063842);
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Zloto, 200000, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Deski, 4000, 500, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Sztaby, 6000, 2000, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Skora, 500, 1000, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Material, 1200, 500, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kosci, 50, 200, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Kamienie, 100, 100, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Piasek, 10, 0, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Klejnoty, 100, 5000, 0));
            tmpBuilding.Resources.Resources.Add(new TownResource(TownResourceType.Ziola, 500, 100, 0));
            tmpBuilding.Dependecies.Add(TownBuildingName.SkladZapasow);
            tmpBuilding.Dependecies.Add(TownBuildingName.Port);
            AddBuilding(tmpBuilding);
            #endregion
        }

        int CalculateMaxGuards()
        {
            int m_guards = 3;

            // Plac treningowy = 3
            if (m_buildings.Find(obj => obj.BuildingType == TownBuildingName.PlacTreningowy).Status == TownBuildingStatus.Dziala)
            {
                m_guards += 3;
            }
            // Koszary = 4
            if (m_buildings.Find(obj => obj.BuildingType == TownBuildingName.Koszary).Status == TownBuildingStatus.Dziala)
            {
                m_guards += 4;
            }
            // Twierdza = 5
            if (m_buildings.Find(obj => obj.BuildingType == TownBuildingName.Twierdza).Status == TownBuildingStatus.Dziala)
            {
                m_guards += 5;
            }
            return m_guards;
        }

        public List<TownGuards> GetAvailableGuards()
        {
            List<TownGuards> tgs = new List<TownGuards>();

            tgs.Add(TownGuards.Straznik); // Podstawowy straznik zawsze dostepny

            if (m_buildings.Find(obj => obj.BuildingType == TownBuildingName.PlacTreningowy).Status == TownBuildingStatus.Dziala)
            {
                tgs.Add(TownGuards.CiezkiStraznik);
                tgs.Add(TownGuards.Strzelec);
            }

            if (m_buildings.Find(obj => obj.BuildingType == TownBuildingName.Koszary).Status == TownBuildingStatus.Dziala)
            {
                tgs.Add(TownGuards.StraznikKonny);
            }

            if (m_buildings.Find(obj => obj.BuildingType == TownBuildingName.Biblioteka).Status == TownBuildingStatus.Dziala)
            {
                tgs.Add(TownGuards.StraznikMag);
            }

            if (m_buildings.Find(obj => obj.BuildingType == TownBuildingName.Twierdza).Status == TownBuildingStatus.Dziala)
            {
                tgs.Add(TownGuards.StraznikElitarny);
            }

            return tgs;
        }

        public void CreatePostHere(Mobile from)
        {
            List<string> prefix, center, suffix;
            string combineName = "";

            prefix = new List<string>(new string[] { "Zelazny", "Maly", "Duzy", "Graniczny", "Podmiejski", "Ostatni", 
                                                     "Pierwszy", "Drugi", "Trzeci", "Widoczny", "Miejski", "Warowny",
                                                     "Waleczny", "Cichy", "Ceglany", "Pikowy", "Kamienny", "Ukryty",
                                                     "Jasny", "Ciemny", "Ostrozny", "Grozny", "Wielki", "Sredni", 
                                                     "Krytyczny", "Karkolomny", "Podejrzliwy", "Drewniany", "Skalny",
                                                     "Mocny", "Slaby", "Skamienialy", "Cyklopowy", "Orkowy", "Harpiowy"});
            center = new List<string>(new string[] { "Posterunek", "Komisariat", "Punkt Warty", "Pikiet", "Dozor", 
                                                     "Punkt Obronny", "Punkt Strazy", "Punkt Gwardii", "Obiekt Strazy",
                                                     "Punkt", "Obszar", "Obszar Obronny", "Punkt Postoju", "Obszar Strazy",
                                                     "Obszar Dozoru", "Obszar Obrony", "Punkt Sily", "Punkt Miecza",
                                                     "Tarczowy Komisariat", "Halabardowy Punkt", "Cieply Punkt", "Cieply Posterunek",
                                                     "Zimny Posterunek", "Posterunek Sily", "Posterunek Nocny", "Posterunek Dzienny"});
            suffix = new List<string>(new string[] { "Czerwony", "Niebieski", "Zielony", "Czarny", "Zolty", "Bialy",
                                                     "Pomaranczowy", "Karmazynowy", "Fioletowy", "Krysztalowy",
                                                     "Bursztynowy", "Alabastrowy", "Cynamonowy", "Grafitowy",
                                                     "Kanarkowy", "Orzechowy", "Rubinowy", "Turkusowy", "Wrzosowy"});

            // Przy zalozeniu ze prefix.count = 34, center.count = 26, suffix.count = 19 - w sumie losowo mozliwych jest 18200 unikatowych nazw posterunkow
            // Prefix
            if (Utility.Random(0, 100) > 5)
            {
                combineName = string.Format("{0} ", prefix[Utility.Random(0, prefix.Count)]);
            }
            // Center
            combineName = string.Format("{0}{1}", combineName, center[Utility.Random(0, center.Count)]);
            // Suffix
            if (Utility.Random(0, 100) > 20)
            {
                combineName = string.Format("{0} - {1}", combineName, suffix[Utility.Random(0, suffix.Count)]);
            }

            TownPost m_tp = new TownPost(combineName, from.Location.X, from.Location.Y, from.Location.Z, Town, from.Map);
            TownPosts.Add(m_tp);
        }

        public int GetCreatedPosts()
        {
            return TownPosts.Count;
        }

        public int GetActiveGuards()
        {
            int m_active_guards = 0;

            foreach (TownPost tp in TownPosts)
            {
                if (tp.PostStatus == TownBuildingStatus.Dziala)
                {
                    m_active_guards++;
                }
            }

            return m_active_guards;
        }

        public TownResources GetGuardPrice(TownGuards tg)
        {
            TownResources tr = new TownResources();
            /* Wedlug schematu
             * tr.Resources.Add(new TownResource(TownResourceType.Zloto, 1000, 500, 500));
             * tr.Resources.Add(new TownResource(Surowiec, Cena aktywacji, Cena wznowienia, Cena wskrzeszenia));
             * */
            switch (tg)
            {
                case TownGuards.Straznik:
                    tr.Resources.Add(new TownResource(TownResourceType.Zloto,    1000, 750, 500));
                    tr.Resources.Add(new TownResource(TownResourceType.Zbroje,   6, 5, 3));
                    tr.Resources.Add(new TownResource(TownResourceType.Bronie,   4, 3, 2));
                    tr.Resources.Add(new TownResource(TownResourceType.Ziola,    0, 0, 0));
                    tr.Resources.Add(new TownResource(TownResourceType.Klejnoty, 0, 0, 0));
                    break;
                case TownGuards.CiezkiStraznik:
                    tr.Resources.Add(new TownResource(TownResourceType.Zloto,    1500, 1000, 750));
                    tr.Resources.Add(new TownResource(TownResourceType.Zbroje,   10, 7, 5));
                    tr.Resources.Add(new TownResource(TownResourceType.Bronie,   6, 5, 3));
                    tr.Resources.Add(new TownResource(TownResourceType.Ziola,    5, 0, 0));
                    tr.Resources.Add(new TownResource(TownResourceType.Klejnoty, 1, 0, 0));
                    break;
                case TownGuards.Strzelec:
                    tr.Resources.Add(new TownResource(TownResourceType.Zloto,    1500, 1000, 750));
                    tr.Resources.Add(new TownResource(TownResourceType.Zbroje,   6, 5, 3));
                    tr.Resources.Add(new TownResource(TownResourceType.Bronie,   10, 7, 5));
                    tr.Resources.Add(new TownResource(TownResourceType.Ziola,    5, 0, 0));
                    tr.Resources.Add(new TownResource(TownResourceType.Klejnoty, 1, 0, 0));
                    break;
                case TownGuards.StraznikKonny:
                    tr.Resources.Add(new TownResource(TownResourceType.Zloto,    2000, 1500, 1000));
                    tr.Resources.Add(new TownResource(TownResourceType.Zbroje,   14, 10, 7));
                    tr.Resources.Add(new TownResource(TownResourceType.Bronie,   8, 6, 4));
                    tr.Resources.Add(new TownResource(TownResourceType.Ziola,    0, 0, 0));
                    tr.Resources.Add(new TownResource(TownResourceType.Klejnoty, 9, 6, 3));
                    break;
                case TownGuards.StraznikMag:
                    tr.Resources.Add(new TownResource(TownResourceType.Zloto,    2000, 1500, 1000));
                    tr.Resources.Add(new TownResource(TownResourceType.Zbroje,   8, 6, 4));
                    tr.Resources.Add(new TownResource(TownResourceType.Bronie,   12, 9, 6));
                    tr.Resources.Add(new TownResource(TownResourceType.Ziola,    50, 30, 15));
                    tr.Resources.Add(new TownResource(TownResourceType.Klejnoty, 3, 2, 1));
                    break;
                case TownGuards.StraznikElitarny:
                    tr.Resources.Add(new TownResource(TownResourceType.Zloto,    2500, 2000, 1250));
                    tr.Resources.Add(new TownResource(TownResourceType.Zbroje,   20, 15, 10));
                    tr.Resources.Add(new TownResource(TownResourceType.Bronie,   16, 12, 8));
                    tr.Resources.Add(new TownResource(TownResourceType.Ziola,    30, 15, 8));
                    tr.Resources.Add(new TownResource(TownResourceType.Klejnoty, 20, 10, 5));
                    break;
                default:
                    break;
            }
            return tr;
        }

        public bool HasResourcesForGuard(TownGuards tg, TownBuildingStatus tbs)
        {
            /* ts - Dostepny - Aktywacja
             * Zawieszony - reaktywacja
             * Dziala - Wskrzeszenie
             * */
            TownResources trs = GetGuardPrice(tg);
            int m_amount = 0;
            foreach(TownResource tr in trs.Resources)
            {
                switch (tbs)
	            {
                    case TownBuildingStatus.Dostepny:
                        m_amount = tr.Amount;
                        break;
                    case TownBuildingStatus.Dziala:
                        m_amount = tr.MaxAmount;
                        break;
                    case TownBuildingStatus.Zawieszony:
                        m_amount = tr.DailyChange;
                        break;
                    default:
                     break;
	            }
                if (!Resources.HasResourceAmount(tr.ResourceType, m_amount))
                {
                    return false;
                }
            }
            return true;
        }

        public void UseResourcesForGuard(TownGuards tg, TownBuildingStatus tbs)
        {
            /* ts - Dostepny - Aktywacja
             * Zawieszony - reaktywacja
             * Dziala - Wskrzeszenie
             * */
            TownResources trs = GetGuardPrice(tg);
            int m_amount = 0;
            foreach (TownResource tr in trs.Resources)
            {
                switch (tbs)
                {
                    case TownBuildingStatus.Dostepny:
                        m_amount = tr.Amount;
                        break;
                    case TownBuildingStatus.Dziala:
                        m_amount = tr.MaxAmount;
                        break;
                    case TownBuildingStatus.Zawieszony:
                        m_amount = tr.DailyChange;
                        break;
                    default:
                        break;
                }
                Resources.ResourceDecreaseAmount(tr.ResourceType, m_amount);
            }
        }

        public void SetUpInitialRelations()
        {
            // Ustawienie relacji
            foreach (Towns t in TownDatabase.GetTownsNames())
            {
                if (t != Towns.None && t != Town)
                    m_relations.Add(new TownRelation(t, 0));
            }
        }

        void AddBuilding(TownBuilding buildingToAdd)
        {
            if (m_buildings.Contains(buildingToAdd))
            {
                throw new ArgumentException(String.Format("Proba dodania budynku '{0}' po raz wtory", buildingToAdd.BuildingType.ToString()));
            }
            else
            {
                m_buildings.Add(buildingToAdd);
            }
        }
    }
}

