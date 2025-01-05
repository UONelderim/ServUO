using System;
using System.Collections.Generic;

using Server.Mobiles;

namespace Server.Items
{
    public class KompendiumWiedzyDrowow : Spellbook
    {
        private static List<SlayerName> SlayerTypes = new List<SlayerName>
        {
            SlayerName.Silver,
            SlayerName.Repond,
            SlayerName.ReptilianDeath,
            SlayerName.Exorcism,
            SlayerName.ArachnidDoom,
            SlayerName.ElementalBan,
            SlayerName.Fey
        };

        private bool IsEquipped; // Flag to track if the item is equipped

        [Constructable]
        public KompendiumWiedzyDrowow() : base()
        {
            Hue = 2571;
            Name = "Kompendium Wiedzy Drowow";

            Slayer = SlayerTypes[Utility.Random(SlayerTypes.Count)];

            Attributes.SpellDamage = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 3;
            Attributes.RegenHits = 3;
            Label1 = "*wyryto na niej napis w jezyku Drowow, ktorego tlumaczenie oznacza mniej wiecej 'Oddaje Swa Sile Loethe'";
        }

        public KompendiumWiedzyDrowow(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((bool)IsEquipped); // Serialize whether the item is equipped
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            IsEquipped = reader.ReadBool(); // Deserialize whether the item is equipped
        }

        public override bool OnEquip(Mobile from)
        {
            bool baseResult = base.OnEquip(from);

            if (from is PlayerMobile)
            {
                PlayerMobile player = (PlayerMobile)from;
                IsEquipped = true;

                // Send message about mana drain
                player.SendMessage("Starozytna Magia wysysa Twoja energie");

                // Start the mana drain timer
                DrainManaTimer timer = new DrainManaTimer(player, this);
                timer.Start();
            }

            return baseResult;
        }

        public void OnRemoved(IEntity parent)
        {
            base.OnRemoved(parent);

            if (parent is Mobile)
            {
                PlayerMobile player = (PlayerMobile)parent;
                IsEquipped = false;

                // Send message about magic stopping
                player.SendMessage("Starozytna Magia przestala wywysac Twa energie.");
            }
        }

        /// <summary>
        /// Timer that drains the player's mana while the KompendiumWiedzyDrowow is equipped.
        /// </summary>
        private class DrainManaTimer : Timer
        {
            private PlayerMobile Player;
            private KompendiumWiedzyDrowow Item;
            private int ManaDrainAmount = 1; // Configure the amount of mana to drain per tick

            public DrainManaTimer(PlayerMobile player, KompendiumWiedzyDrowow item) : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            {
                Player = player;
                Item = item;
                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                if (Player == null || Player.Deleted || !Player.Alive)
                {
                    Stop();
                    return;
                }

                if (Player.Mana > 0)
                {
                    Player.Mana -= ManaDrainAmount;
                }
                else
                {
                    Stop();
                }
            }
        }
    }
}
