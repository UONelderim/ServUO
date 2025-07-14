using System;
using Server.Items;
using System.Collections.Generic;
using Server.Accounting;

namespace Server.Mobiles
{
    public abstract class BaseAdvancedHire : BaseHire
    {
	    private Dictionary<string, DateTime> m_Cooldowns;
        private int m_Deaths;
        private DateTime m_LastFed;
        private int m_Happiness;
        private int m_Experience;
        private int m_Level;
        private Dictionary<SkillName, double> m_SkillGains;
        private DateTime m_NextFeeTime;
        private static readonly TimeSpan FeeInterval = TimeSpan.FromHours(1);
        
        [CommandProperty(AccessLevel.GameMaster)]
        public int Deaths 
        { 
            get => m_Deaths; 
            set 
            { 
                m_Deaths = value;
                if (m_Deaths >= MaxDeaths)
                    BeginPermaDeath();
            } 
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual int MaxDeaths => 5;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Happiness 
        { 
            get => m_Happiness; 
            set => m_Happiness = Math.Max(0, Math.Min(100, value)); 
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Experience 
        { 
            get => m_Experience; 
            set 
            {
                m_Experience = value;
                CheckLevelUp();
            } 
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Level { get => m_Level; set => m_Level = value; }

        public BaseAdvancedHire(AIType ai) : base(ai)
        {
	        m_Cooldowns = new Dictionary<string, DateTime>();
            m_Deaths = 0;
            m_Happiness = 100;
            m_Experience = 0;
            m_Level = 1;
            m_LastFed = DateTime.UtcNow;
            m_NextFeeTime = DateTime.UtcNow + FeeInterval;
            m_SkillGains = new Dictionary<SkillName, double>();
        }
        
        protected bool CheckCooldown(string key)
        {
	        if (!m_Cooldowns.ContainsKey(key))
		        return true;

	        return DateTime.UtcNow >= m_Cooldowns[key];
        }

        protected void SetCooldown(string key, TimeSpan duration)
        {
	        m_Cooldowns[key] = DateTime.UtcNow + duration;
        }

        protected BaseAdvancedHire(Serial serial)
        {
	        throw new NotImplementedException();
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.UtcNow >= m_NextFeeTime)
            {
                CollectHourlyFee();
                m_NextFeeTime = DateTime.UtcNow + FeeInterval;
            }

            // Happiness decay
            if (DateTime.UtcNow - m_LastFed > TimeSpan.FromHours(1))
            {
                Happiness -= 5;
                if (Happiness <= 0)
                    Say("Jestem zbyt głodny, by efektywnie pracować!");
            }
        }

        protected virtual int CalculateHourlyFee()
        {
            return 10 + (Level * 5) + (GetTotalSkills() / 20);
        }

        protected virtual void CollectHourlyFee()
        {
	        int fee = CalculateHourlyFee();
	        Mobile owner = GetOwner();

	        if (owner == null || !Banker.Withdraw(owner, fee))
	        {
		        Say("Nie otrzymałem zapłaty! Przestaję pracować.");
		        ControlOrder = OrderType.Stay; // Changed from SetControlTarget
		        ControlTarget = null;
		        return;
	        }

	        Say($"Pobieram {fee} sztuk złota za godzinę pracy.");
        }

        public override bool OnBeforeDeath()
        {
            Deaths++;
            
            if (Deaths >= MaxDeaths)
            {
                Say("To mój koniec... Żegnaj, panie!");
                return true;
            }

            // Ghost form
            Body = 0x3CA;
            Blessed = true;
            Say("Wracam jako duch... Potrzebuję pomocy!");
            return false;
        }

        protected virtual void BeginPermaDeath()
        {
            Say("Mój czas dobiegł końca...");
            Delete();
        }

        public virtual void Feed(Item food)
        {
            if (food == null)
                return;

            m_LastFed = DateTime.UtcNow;
            Happiness = Math.Min(100, Happiness + 25);
            food.Delete();
            
            Say("Dziękuję za posiłek!");
            Effects.PlaySound(Location, Map, 0x3A);
        }

        protected virtual void CheckLevelUp()
        {
            int expNeeded = 100 * Level;
            while (Experience >= expNeeded)
            {
                Experience -= expNeeded;
                Level++;
                OnLevelUp();
                expNeeded = 100 * Level;
            }
        }

        protected virtual void OnLevelUp()
        {
            Say($"Awansowałem na poziom {Level}!");
            
            // Random stat and skill increases
            switch (Utility.Random(3))
            {
                case 0: RawStr += 1; break;
                case 1: RawDex += 1; break;
                case 2: RawInt += 1; break;
            }

            foreach (var skill in Skills)
            {
                if (Utility.RandomDouble() < 0.3)
                    skill.Base += 1.0;
            }
        }

        protected int GetTotalSkills()
        {
            int total = 0;
            foreach (var skill in Skills)
                total += (int)skill.Base;
            return total;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
            writer.Write(m_Deaths);
            writer.Write(m_LastFed);
            writer.Write(m_Happiness);
            writer.Write(m_Experience);
            writer.Write(m_Level);
            writer.Write(m_NextFeeTime);
            
            writer.Write(m_Cooldowns.Count);
            foreach (var kvp in m_Cooldowns)
            {
	            writer.Write(kvp.Key);
	            writer.Write(kvp.Value);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Deaths = reader.ReadInt();
            m_LastFed = reader.ReadDateTime();
            m_Happiness = reader.ReadInt();
            m_Experience = reader.ReadInt();
            m_Level = reader.ReadInt();
            m_NextFeeTime = reader.ReadDateTime();
            
            m_SkillGains = new Dictionary<SkillName, double>();
            if (version >= 1)
            {
	            int count = reader.ReadInt();
	            m_Cooldowns = new Dictionary<string, DateTime>();
	            for (int i = 0; i < count; i++)
	            {
		            string key = reader.ReadString();
		            DateTime value = reader.ReadDateTime();
		            m_Cooldowns[key] = value;
	            }
            }
            else
            {
	            m_Cooldowns = new Dictionary<string, DateTime>();
            }
        }
    }
}
