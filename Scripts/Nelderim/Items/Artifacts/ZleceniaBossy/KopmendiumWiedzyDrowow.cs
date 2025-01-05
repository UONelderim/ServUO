using System;
using System.Collections.Generic;

using Server.Mobiles;

namespace Server.Items
{
    public class KompendiumWiedzyDrowow : Spellbook
    {
        private static List<SlayerName> SlayerTypes = 
        [
            SlayerName.Silver,
            SlayerName.Repond,
            SlayerName.ReptilianDeath,
            SlayerName.Exorcism,
            SlayerName.ArachnidDoom,
            SlayerName.ElementalBan,
            SlayerName.Fey
        ];

        [Constructable]
        public KompendiumWiedzyDrowow()
        {
            Hue = 2571;
            Name = "Kompendium Wiedzy Drowow";

            Slayer = Utility.RandomList(SlayerTypes);

            Attributes.SpellDamage = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 3;
            Attributes.RegenHits = 3;
        }
        
        public override void AddNameProperties(ObjectPropertyList list)
        {
	        base.AddNameProperties(list);
	        list.Add("*wyryto na niej napis w jezyku Drowow, ktorego tlumaczenie oznacza mniej wiecej 'Oddaje Swa Sile Loethe'");
        }

        public override bool OnEquip(Mobile from)
        {
            var baseResult = base.OnEquip(from);

            if (from is PlayerMobile pm)
            {
                pm.SendMessage("Starozytna Magia wysysa Twoja energie");
                new DrainManaTimer(pm, this).Start();
            }

            return baseResult;
        }

        public KompendiumWiedzyDrowow(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
	        base.Serialize(writer);
	        writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
	        base.Deserialize(reader);
	        int version = reader.ReadInt();
        }

        private class DrainManaTimer : Timer
        {
	        private PlayerMobile Player;
	        private KompendiumWiedzyDrowow Item;

	        public DrainManaTimer(PlayerMobile player, KompendiumWiedzyDrowow item) : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
	        {
		        Player = player;
		        Item = item;
	        }

	        protected override void OnTick()
	        {
		        if (Player == null || Player.Deleted || !Player.Alive || Item.Parent != Player)
		        {
			        Stop();
			        return;
		        }
		        Player.Mana--;
	        }
        }
    }
}
