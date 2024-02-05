using System;


namespace Server.Items
{
    public class SmokingPipe : BaseEarrings
    {
        //private Type TobbacoType { get; set; }
        //private int TobbacoQuantity { get; set; }
        public int UsesRemaining { get; set; }
        private bool ShowUsesRemaining { get; set; }

        private class SmokeTimer : Timer
        {
            private readonly Mobile m_Mobile;
            private readonly SmokingPipe m_Fajka;

            private int TobaccoRequired => 4;

            public SmokeTimer(SmokingPipe fajka, Mobile mobile) : base(TimeSpan.FromSeconds(1), TimeSpan.Zero)
            {
                m_Mobile = mobile;
                m_Fajka = fajka;
                Priority = TimerPriority.OneSecond;

                Start();
            }

            private Item GetTobaccoFromBackpack()
            {
                if (m_Mobile == null || m_Mobile.Backpack == null)
                    return null;

                Item[] tobaccos = m_Mobile.Backpack.FindItemsByType(typeof(ISmokable));
                foreach (Item tob in tobaccos)
                {
                    if (tob != null && tob.Amount >= TobaccoRequired)
                        return tob;
                }

                return null;
            }

            protected override void OnTick()
            {
                Item tobacco = GetTobaccoFromBackpack();

                if (tobacco == null || (tobacco.Amount < TobaccoRequired)) // Get rid of the deprecated 'Tyton' instances
                {
                    m_Mobile.SendMessage("Za malo tytoniu w plecaku.");
                    m_Mobile.Emote("*z pustej fajki nie unosi sie ani troche dymu*");
                    Stop();
                    return;
                }

                if (tobacco is ISmokable)
                {
                    ((ISmokable)tobacco).OnSmoke(m_Mobile);
                }
                else
                {
                    m_Mobile.SendMessage("Dym tytoniowy napelnia twoje pluca.");

                    m_Mobile.Emote("*wypuszcza z ust odrobine fajkowego dymu*");
                    Effects.SendLocationParticles(EffectItem.Create(m_Mobile.Location, m_Mobile.Map, EffectItem.DefaultDuration), 0x3728, 1, 13, 9965);
                    m_Mobile.PlaySound(0x15F);
                    m_Mobile.RevealingAction();
                }

                tobacco.Consume(TobaccoRequired);
                
                m_Fajka.UsesRemaining--;
                m_Fajka.InvalidateProperties();

                Stop();
                if (m_Fajka.UsesRemaining <= 0)
                {
                    m_Mobile.SendMessage("Fajka jest zbyt zuzyta, by z niej palic");
                    m_Fajka.Delete(); // Optionally remove the item when uses are exhausted
                }
            }
        }

        [Constructable]
        public SmokingPipe() : base(0x17B3)
        {
            Weight = 0.1;
            Name = "Fajka do palenia";
            Light = LightType.Circle150;
            UsesRemaining = 100;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1060584, UsesRemaining.ToString()); // uses remaining: ~1_val~

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (UsesRemaining > 0)
            {
                new SmokeTimer(this, from);
                from.SendMessage("Zaczynasz zaciagac sie dymem z fajki.");
                from.Emote("*wciaga powietrze przez fajke*");
            }
            else
            {
                from.SendMessage("Fajka jest pusta.");
                from.Emote("*spoglada na zatkana fajke*");
            }
        }

        public SmokingPipe(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((int)UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            UsesRemaining = reader.ReadInt();
        }
    }
}
