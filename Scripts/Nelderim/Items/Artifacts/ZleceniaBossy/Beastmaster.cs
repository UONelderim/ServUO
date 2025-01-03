using System;
using Server;
using System.Collections.Generic;
using Server.Network;


namespace Server.Items
{
    public class Beastmaster : GoldEarrings
    {
        private DateTime m_LastStaminaLoss;
        private Timer m_StaminaLossTimer;

        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public Beastmaster()
        {
            Name = "Kolczyki Wladcy Bestii";
            Hue = 1153;

            Attributes.BonusStr = -20;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 5.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 5.0);
            Label1 = "*grawer w języku krasnoludow rzecze, iz owe kolczyki wysysaja wytrzymalosc noszacego*";
            m_LastStaminaLoss = DateTime.UtcNow;
        }

        public Beastmaster(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
            writer.Write(m_LastStaminaLoss);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 1)
            {
                m_LastStaminaLoss = reader.ReadDateTime();
            }
        }

        public override bool OnEquip(Mobile from)
        {
            if (base.OnEquip(from))
            {
                from.SendMessage("Zakładając te kolczyki czujesz jak krasnoludzkie runy powoduja spadek Twojej wytrzymałości.");
                StartStaminaLossTimer(from);
                return true;
            }
            return false;
        }

        public void OnRemoved(IEntity parent)
        {
            if (parent is Mobile from)
            {
                from.SendMessage("Zdejmując kolczyki, runy przestaja działać.");
                StopStaminaLossTimer();
            }
            base.OnRemoved(parent);
        }

        private void StartStaminaLossTimer(Mobile from)
        {
            StopStaminaLossTimer();
            m_StaminaLossTimer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), () => DoStaminaLoss(from));
        }

        private void StopStaminaLossTimer()
        {
            if (m_StaminaLossTimer != null)
            {
                m_StaminaLossTimer.Stop();
                m_StaminaLossTimer = null;
            }
        }

        private void DoStaminaLoss(Mobile from)
        {
            if (from != null && !from.Deleted && from.Alive && from.FindItemOnLayer(Layer.Earrings) == this)
            {
                TimeSpan timeSinceLastStaminaLoss = DateTime.UtcNow - m_LastStaminaLoss;
                if (timeSinceLastStaminaLoss.TotalSeconds >= 1)
                {
                    from.Stam -= 1;
                    m_LastStaminaLoss = DateTime.UtcNow;
                }
            }
            else
            {
                StopStaminaLossTimer();
            }
        }
    }
}
