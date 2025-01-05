using System;

using Server.Network;
using System.Collections.Generic;


namespace Server.Items
{
    public class EarringsOfTheMagician : GoldEarrings
    {
        private static Timer m_Timer;
        private static readonly TimeSpan m_Interval = TimeSpan.FromSeconds(1);

        public override int InitMinHits => 50;
        public override int InitMaxHits => 50;

        [Constructable]
        public EarringsOfTheMagician()
        {
            Name = "Kolczyki Krasnoludzkiego Maga";
            Hue = 0x554;
            Attributes.CastRecovery = 1;
            Attributes.LowerRegCost = 10;
            Attributes.Luck = -200;
            Resistances.Energy = 5;
            Resistances.Fire = 5;
            Label1 = "*grawer w języku krasnoludow rzecze, iz owe kolczyki wysysaja wytrzymalosc noszacego*";

            if (m_Timer == null)
            {
                m_Timer = Timer.DelayCall(m_Interval, m_Interval, new TimerCallback(OnTick));
            }
        }

        public EarringsOfTheMagician(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (m_Timer == null)
            {
                m_Timer = Timer.DelayCall(m_Interval, m_Interval, new TimerCallback(OnTick));
            }
        }

        public override bool OnEquip(Mobile from)
        {
            from.SendLocalizedMessage(1062470, Name); // You feel the ~1_NAME~ drain your stamina as you equip it.
            return base.OnEquip(from);
        }

        public void OnRemoved(IEntity parent)
        {
            if (parent is Mobile from)
            {
                from.SendLocalizedMessage(1062471, Name); // You feel the effects of the ~1_NAME~ fade as you remove it.
            }
            base.OnRemoved(parent);
        }

        private static void OnTick()
        {
            foreach (Mobile player in GetPlayersWithEarringsEquipped())
            {
                if (player.Alive)
                {
                    player.Stam -= 1;
                }
            }
        }

        private static List<Mobile> GetPlayersWithEarringsEquipped()
        {
            List<Mobile> players = new List<Mobile>();

            foreach (NetState state in NetState.Instances)
            {
                Mobile player = state.Mobile;

                if (player?.Alive == true)
                {
                    Item earrings = player.FindItemOnLayer(Layer.Earrings);

                    if (earrings is EarringsOfTheMagician)
                    {
                        players.Add(player);
                    }
                }
            }

            return players;
        }
    }
}
