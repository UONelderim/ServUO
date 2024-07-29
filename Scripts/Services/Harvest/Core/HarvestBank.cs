using Server.Commands;
using Server.Items;
using System;

namespace Server.Engines.Harvest
{
    public class HarvestBank
    {
        private int m_X, m_Y;
        private Map m_Map;
        public int X => m_X;
        public int Y => m_Y;
        public Map Map => m_Map;

        private readonly int m_Maximum;
        readonly HarvestDefinition m_Definition;
        private int m_Current;
        private DateTime m_NextRespawn;
        private HarvestVein m_Vein, m_DefaultVein;
        public HarvestBank(HarvestDefinition def, Map map, int x, int y)
        {
            m_X = x * def.BankWidth;
            m_Y = y * def.BankHeight;
            m_Map = map;
            m_Maximum = Utility.RandomMinMax(def.MinTotal, def.MaxTotal);
            m_Current = m_Maximum;
            m_DefaultVein = def.GetVeinAt(m_Map, m_X, m_Y);
            m_Vein = m_DefaultVein;

            m_Definition = def;
        }

        public HarvestDefinition Definition => m_Definition;
        public int Current
        {
            get
            {
                CheckRespawn();
                return m_Current;
            }
        }
        public HarvestVein Vein
        {
            get
            {
                CheckRespawn();
                return m_Vein;
            }
            set
            {
                m_Vein = value;
            }
        }
        public HarvestVein DefaultVein
        {
            get
            {
                CheckRespawn();
                return m_DefaultVein;
            }
        }
        public void CheckRespawn()
        {
            if (m_Current == m_Maximum || m_NextRespawn > DateTime.UtcNow)
                return;

            m_Current = m_Maximum;

            if (m_Definition.RandomizeVeins)
            {
                m_DefaultVein = m_Definition.GetVeinFrom(Utility.RandomDouble(), m_Map, m_X, m_Y);
            }

            m_Vein = m_DefaultVein;
        }

        public void Consume(int amount, Mobile from)
        {
            CheckRespawn();

            if (m_Current == m_Maximum)
            {
                double min = m_Definition.MinRespawn.TotalMinutes;
                double max = m_Definition.MaxRespawn.TotalMinutes;
                double rnd = Utility.RandomDouble();

                m_Current = m_Maximum - amount;

                double minutes = min + (rnd * (max - min));
                if (m_Definition.RaceBonus && from.Race == Race.Elf)
                    minutes *= .75;	//25% off the time.  

                m_NextRespawn = DateTime.UtcNow + TimeSpan.FromMinutes(minutes);
            }
            else
            {
                m_Current -= amount;
            }

            if (m_Current < 0)
                m_Current = 0;
        }
    }
}
