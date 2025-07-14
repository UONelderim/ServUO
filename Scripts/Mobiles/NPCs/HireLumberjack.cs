using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Targeting;
using Server.Accounting;

namespace Server.Mobiles
{
    public enum WoodQuality { Mokre, Wysuszone, Spróchniałe }

    public static class LumberHelper
    {
        public static string GetQualitySuffix(WoodQuality quality) => quality switch
        {
            WoodQuality.Mokre => " (mokre)",
            WoodQuality.Wysuszone => " (wysuszone)",
            WoodQuality.Spróchniałe => " (spróchniałe)",
            _ => ""
        };

        public static WoodQuality DetermineQuality(int happiness)
        {
            double roll = Utility.RandomDouble();
            if (happiness > 80 && roll < 0.7) return WoodQuality.Wysuszone;
            if (happiness < 40 && roll < 0.4) return WoodQuality.Spróchniałe;
            return WoodQuality.Mokre;
        }

        public static bool IsNightTime() => DateTime.UtcNow.Hour < 6 || DateTime.UtcNow.Hour > 20;
    }
	
    public class HireLumberjack : TrainableHire
    {
        private DateTime m_NextHarvestTime;
        private static readonly TimeSpan BaseHarvestInterval = TimeSpan.FromMinutes(10);
        private const int MaxBackpackItems = 100;
        private bool m_ShouldWork = true;

        
        [Constructable]
        public HireLumberjack() : base(AIType.AI_Melee)
        {
            Title = "- najemny drwal ";
            SetSkill(SkillName.Lumberjacking, 60.0);
            m_NextHarvestTime = DateTime.UtcNow + BaseHarvestInterval;
            BodyValue = 400;
            Race = Race.NTamael;
            Female = false;

        }

        public HireLumberjack(Serial serial) : base(serial) { }

        public override void OnThink()
        {
            base.OnThink();

            if (!m_ShouldWork)
                return;

            double efficiency = 1.0 - (100 - Happiness) * 0.005;
            TimeSpan adjustedInterval = TimeSpan.FromTicks((long)(BaseHarvestInterval.Ticks / efficiency));

            if (DateTime.UtcNow >= m_NextHarvestTime && ControlOrder == OrderType.Stay)
            {
                if (WithdrawPayment())
                    ChopWood();

                m_NextHarvestTime = DateTime.UtcNow + adjustedInterval;
            }

            Name = m_ShouldWork ? "[pracuje] drwal najemny" : "drwal najemny";
        }

        private void ChopWood()
        {
            if (!HasRequiredTool(out Hatchet hatchet))
            {
                Say("Nie mam odpowiedniego narzędzia do pracy!");
                m_ShouldWork = false;
                return;
            }

            SayRandomWorkQuote();
            Animate(11, 5, 1, true, false, 0);
            Effects.SendLocationEffect(Location, Map, 0x3728, 10);

            if (LumberHelper.IsNightTime() && Utility.RandomDouble() < 0.25)
                Say("Ciemno tu jak w grobie...");

            if (Utility.RandomDouble() < 0.25 && Utility.RandomBool())
                Say("Pogoda nie sprzyja rąbaniu.");

            if (Utility.RandomDouble() < 0.15)
            {
                Say("Coś poszło nie tak... straciłem część drewna.");
                return;
            }

            if (Utility.RandomDouble() < 0.05)
            {
                hatchet.Delete();
                Say("Moja siekiera się zużyła i pękła!");
            }

            if (Backpack == null || Backpack.Items.Count >= MaxBackpackItems)
            {
                Say("Mój plecak jest przepełniony! Przestaję pracować.");
                m_ShouldWork = false;
                ControlOrder = OrderType.Follow;
                return;
            }

            int bonus = Math.Max(0, Level - 1);
            Item logs = CreateRandomWood(bonus);

            Mobile owner = GetOwner();
            if (owner != null && owner.BankBox != null)
            {
                Say("Przekazuję drewno do banku właściciela.");
                owner.BankBox.DropItem(logs);
            }
            else
            {
                Backpack?.DropItem(logs);
            }
        }

        private Item CreateRandomWood(int bonus)
        {
            Type[] woodTypes;

            if (Level >= 6)
                woodTypes = new Type[] { typeof(Log), typeof(OakLog), typeof(AshLog), typeof(YewLog) };
            else if (Level >= 4)
                woodTypes = new Type[] { typeof(Log), typeof(OakLog), typeof(AshLog) };
            else if (Level >= 2)
                woodTypes = new Type[] { typeof(Log), typeof(OakLog) };
            else
                woodTypes = new Type[] { typeof(Log) };

            Type chosen = woodTypes[Utility.Random(woodTypes.Length)];
            Item logs = (Item)Activator.CreateInstance(chosen);
            logs.Amount = Utility.RandomMinMax(1 + bonus, 3 + bonus);

            WoodQuality quality = LumberHelper.DetermineQuality(Happiness);
            logs.Name = $"{logs.Name}{LumberHelper.GetQualitySuffix(quality)}";

            return logs;
        }

        private bool HasRequiredTool(out Hatchet tool)
        {
            tool = Backpack?.FindItemByType(typeof(Hatchet)) as Hatchet;
            return tool != null;
        }

        private void SayRandomWorkQuote()
        {
            if (Utility.RandomDouble() > 0.25) return;
            string[] quotes =
            {
                "Tniemy dalej!",
                "Znowu korzeń...",
                "Lepsze to niż kopanie!",
                "Ale sęk!",
                "Z tego zrobię meble.",
                "Świeże drewno pachnie najlepiej.",
                "Siekiera ledwo zipie...",
                "Jeszcze jedna deska...",
                "To idzie do tartaku.",
                "Las nigdy się nie kończy..."
            };
            Say(quotes[Utility.Random(quotes.Length)]);
        }

        private bool WithdrawPayment()
        {
            int cost = 5 + Level;
            Mobile owner = GetOwner();

            if (owner == null || !Banker.Withdraw(owner, cost))
            {
                Say("Brakuje złota w banku mojego właściciela, by pracować.");
                return false;
            }

            return true;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            base.OnSpeech(e);

            if (!e.Handled && e.Mobile.InRange(this, 8) && e.Mobile == GetOwner())
            {
                string speech = e.Speech.ToLower();

                if (speech.Contains("pracuj"))
                {
                    Say("Rozpoczynam pracę!");
                    m_ShouldWork = true;
                    e.Handled = true;
                }
                else if (speech.Contains("przestań pracować"))
                {
                    Say("Kończę pracę.");
                    m_ShouldWork = false;
                    e.Handled = true;
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);
            writer.Write(m_NextHarvestTime);
            writer.Write(m_ShouldWork);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextHarvestTime = reader.ReadDateTime();
            m_ShouldWork = version >= 1 && reader.ReadBool();
        }
    }
}
