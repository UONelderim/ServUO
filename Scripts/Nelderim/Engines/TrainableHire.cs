using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Targeting;
using Server.Accounting;

namespace Server.Mobiles
{
    public abstract class TrainableHire : BaseHire
    {
        private DateTime m_LastFed;
        private int m_Happiness;
        private int m_Experience;
        private int m_Level;
        private int m_Deaths;
        private int m_MaxDeaths = 3;
        private List<Type> m_PreferredFoods;
        
        private OrderType m_LastOrder = OrderType.Follow;

        private static TimeSpan FoodInterval = TimeSpan.FromHours(1);
        private DateTime m_NextFoodTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Happiness { get => m_Happiness; set => m_Happiness = Math.Max(0, Math.Min(100, value)); }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Experience { get => m_Experience; set => m_Experience = value; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Level { get => m_Level; set => m_Level = value; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Deaths { get => m_Deaths; set => m_Deaths = value; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDeaths { get => m_MaxDeaths; set => m_MaxDeaths = value; }

        public TrainableHire(AIType ai) : base(ai)
        {
            m_Happiness = 100;
            m_Experience = 0;
            m_Level = 1;
            m_Deaths = 0;
            m_LastFed = DateTime.UtcNow;
            m_NextFoodTime = DateTime.UtcNow + FoodInterval;
            m_PreferredFoods = GetPreferredFoodsByClass();

            PayTimer.RegisterTimer(this);
            RestoreLastOrder();
        }

        public TrainableHire(Serial serial) : base(serial) { }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.UtcNow >= m_NextFoodTime)
            {
                HandleFood();
                m_NextFoodTime = DateTime.UtcNow + FoodInterval;
            }

            if (CanHeal && Hits < HitsMax && Mana >= 10)
            {
                DoBandageHeal();
            }
        }

        private void HandleFood()
        {
            if (ConsumeFood())
            {
                Happiness += 10;
            }
            else
            {
                Happiness -= 20;

                if (Happiness <= 0)
                    Say("Jestem zbyt głodny, by dalej służyć.");
            }
        }

        private bool ConsumeFood()
        {
            Container pack = Backpack;
            if (pack == null)
                return false;

            foreach (Item item in pack.Items)
            {
                if (m_PreferredFoods.Contains(item.GetType()))
                {
                    item.Delete();
                    return true;
                }
            }

            return false;
        }

        protected int CalculateHourlyCost()
        {
            double skillFactor = GetTotalSkill() / 100.0;
            double experienceFactor = Math.Log(Experience + 1);
            return 5 + (int)(skillFactor + experienceFactor * 2);
        }

        protected int GetTotalSkill()
        {
            double total = 0;
            foreach (Skill sk in Skills)
                total += sk.Base;
            return (int)total;
        }

        private void CheckLevelUp()
        {
            int requiredExp = 100 * m_Level;
            while (m_Experience >= requiredExp)
            {
                m_Experience -= requiredExp;
                m_Level++;
                Say("Awansowałem na poziom " + m_Level + "!");
                ApplyLevelUpBonuses();
                requiredExp = 100 * m_Level;
            }
        }

        private void ApplyLevelUpBonuses()
        {
            Str += Utility.RandomMinMax(1, 3);
            Dex += Utility.RandomMinMax(1, 3);
            Int += Utility.RandomMinMax(1, 3);

            foreach (Skill skill in Skills)
            {
                skill.Base += Utility.RandomDouble() < 0.25 ? 1.0 : 0.0;
            }
        }

        protected virtual List<Type> GetPreferredFoodsByClass()
        {
            List<Type> list = new List<Type> { typeof(BreadLoaf), typeof(CookedBird), typeof(FishSteak), typeof(Ribs) };

            if (this is HireMage)
                list.Add(typeof(FruitBasket));
            else if (this is HirePaladin)
                list.Add(typeof(Sausage));
            else if (this is HireThief)
                list.Add(typeof(CheeseWheel));

            return list;
        }

        public override bool OnBeforeDeath()
        {
            Deaths++;
            if (Deaths >= MaxDeaths || Utility.RandomDouble() < 0.25)
            {
                Say("To był mój koniec...");
                return base.OnBeforeDeath();
            }
            else
            {
                Say("Powrócę jako duch...");
                this.Body = 0x3CA;
                this.Hits = 1;
                this.Frozen = true;
                return false;
            }
        }

        public override bool OnDragDrop(Mobile from, Item item)
        {
            if (item != null && m_PreferredFoods.Contains(item.GetType()))
            {
                item.Delete();
                Happiness = Math.Min(100, Happiness + 15);
                Effects.PlaySound(Location, Map, 0x3A);
                Effects.SendLocationEffect(Location, Map, 0x373A, 10);
                Say("Dziękuję za jedzenie!");
                GainExperience(10);
                return true;
            }

            return base.OnDragDrop(from, item);
        }

        public void GainExperience(int amount)
        {
            Experience += amount;
            CheckLevelUp();
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            base.OnSpeech(e);

            if (!e.Handled && e.Mobile.InRange(this, 8))
            {
                string speech = e.Speech.ToLower();
                Mobile owner = GetOwner();

                if (owner == null || e.Mobile != owner)
                {
                    Say("Nie rozpoznaję cię jako mojego właściciela.");
                    return;
                }

                if (speech.Contains("status"))
                {
                    e.Handled = true;
                    int mins = (int)(m_NextFoodTime - DateTime.UtcNow).TotalMinutes;
                    Say($"Poziom: {Level}, Doświadczenie: {Experience}, Szczęście: {Happiness}/100, Kolejny posiłek za {mins} minut, Koszt utrzymania: {CalculateHourlyCost()} zł/h");
                }
                else if (speech.Contains("nakarm się") || speech.Contains("jedz"))
                {
                    e.Handled = true;
                    HandleFood();
                }
                else if (speech.Contains("zakończ służbę") || speech.Contains("rozstańmy się"))
                {
                    e.Handled = true;
                    Say("Dziękuję za współpracę. Odchodzę.");
                    this.Delete();
                }
                else if (speech.Contains("ekwipunek"))
                {
                    e.Handled = true;
                    Say("Otwieram swój plecak...");
                    Backpack?.DisplayTo(owner);
                }
                else if (speech.Contains("wróć"))
                {
                    e.Handled = true;
                    Say("Wracam do Ciebie.");
                    this.Warmode = false;
                    this.ControlTarget = owner;
                    this.ControlOrder = OrderType.Follow;
                    m_LastOrder = OrderType.Follow;
                }
                else if (speech.Contains("czekaj"))
                {
                    e.Handled = true;
                    Say("Zostaję tutaj.");
                    this.Warmode = false;
                    this.ControlTarget = null;
                    this.ControlOrder = OrderType.Stay;
                    m_LastOrder = OrderType.Stay;
                }
            }
        }

        public bool OnHireCommand(Mobile from)
        {
            int existing = 0;
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is TrainableHire hire && hire.GetOwner() == from)
                    existing++;
            }

            if (existing >= 1)
            {
                from.SendMessage("Możesz wynająć tylko jednego najemnika naraz.");
                return false;
            }

            return OnHireCommand(from);
        }

        protected virtual bool CanHeal => this is HirePaladin || this is HireThief || this is HireSailor || this is HireBard || this is HireBardArcher || this is HireBeggar || this is HireFighter || this is HireMage || this is HireLumberjack || this is HirePeasant || this is HireRanger || this is HireRangerArcher;

        private void DoBandageHeal()
        {
            Bandage bandage = Backpack?.FindItemByType(typeof(Bandage)) as Bandage;
            if (bandage != null)
            {
                bandage.Consume();
                Heal(Utility.RandomMinMax(10, 20));
                Say("Zakładam sobie bandaż...");
            }
        }

        private void RestoreLastOrder()
        {
            this.ControlOrder = m_LastOrder;
            this.ControlTarget = m_LastOrder == OrderType.Follow ? GetOwner() : null;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(2);
            writer.Write(m_Happiness);
            writer.Write(m_Experience);
            writer.Write(m_Level);
            writer.Write(m_Deaths);
            writer.Write(m_MaxDeaths);
            writer.Write(m_LastFed);
            writer.Write((int)m_LastOrder);

            writer.Write(m_PreferredFoods.Count);
            foreach (var type in m_PreferredFoods)
                writer.Write(type.FullName);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Happiness = reader.ReadInt();
            m_Experience = reader.ReadInt();
            m_Level = reader.ReadInt();
            m_Deaths = reader.ReadInt();
            m_MaxDeaths = reader.ReadInt();
            m_LastFed = reader.ReadDateTime();

            if (version >= 2)
                m_LastOrder = (OrderType)reader.ReadInt();
            else
                m_LastOrder = OrderType.Follow;

            if (version >= 2)
            {
                int count = reader.ReadInt();
                m_PreferredFoods = new List<Type>();
                for (int i = 0; i < count; i++)
                {
                    string typeName = reader.ReadString();
                    Type type = ScriptCompiler.FindTypeByName(typeName);
                    if (type != null)
                        m_PreferredFoods.Add(type);
                }
            }
            else
            {
                m_PreferredFoods = GetPreferredFoodsByClass();
            }

            PayTimer.RegisterTimer(this);
            RestoreLastOrder();
        }
    }
}
