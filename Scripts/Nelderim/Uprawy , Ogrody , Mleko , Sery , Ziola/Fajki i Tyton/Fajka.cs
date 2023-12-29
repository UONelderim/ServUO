using System;


namespace Server.Items
{
    public class Fajka : BaseEarrings
    {
        public int UsesRemaining { get; set; }
        private bool ShowUsesRemaining { get; set; }

        private class SmokeTimer : Timer
        {
            private readonly Mobile m_Mobile;
            private readonly Fajka m_Fajka;

            public SmokeTimer(Fajka fajka, Mobile mobile) : base(TimeSpan.FromSeconds(1), TimeSpan.Zero)
            {
                m_Mobile = mobile;
                m_Fajka = fajka;
                Priority = TimerPriority.OneSecond;

                Start();
            }

            protected override void OnTick()
            {
                int tyton = Core.AOS ? 4 : 5;

                if (m_Mobile.Backpack == null || m_Mobile.Backpack.GetAmount(typeof(Tyton)) < tyton)
                {
                    m_Mobile.SendMessage("Za malo tytoniu, moj Panie.");
                    m_Mobile.Emote("*spoglada w plecak w poszukiwaniu tytoniu, jednakze, plecak okazuje sie byc pusty*");
                    Stop();
                    return;
                }

                Effects.SendLocationParticles(EffectItem.Create(m_Mobile.Location, m_Mobile.Map, EffectItem.DefaultDuration), 0x3728, 1, 13, 9965);
                m_Mobile.Emote("*zaciaga sie tytoniem*");
                m_Mobile.PlaySound(0x15F);
                m_Mobile.SendMessage("Dym tytoniowy napelnia Twe pluca");
                m_Mobile.RevealingAction();

                m_Mobile.Backpack.ConsumeUpTo(typeof(Tyton), tyton);
                
                m_Fajka.UsesRemaining--;
                Stop();
                if (m_Fajka.UsesRemaining <= 0)
                {
                    m_Mobile.SendMessage("Fajka jest zbyt zużyta, by z niej palić");
                    m_Fajka.Delete(); // Optionally remove the item when uses are exhausted
                }
            }
        }

        [Constructable]
        public Fajka() : base(0x17B3)
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
                from.SendMessage("Probujesz odpalic fajke");
                from.Emote("*probuje zapalic fajke*");
            }
            else
            {
                from.SendMessage("Fajka jest pusta.");
                from.Emote("*z duza doza zdenerwowania spoglada na zniszczona fajke*");
            }
        }

        public Fajka(Serial serial) : base(serial)
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
