using Server.Items;
using Server.Mobiles;

namespace Server.Engines.VoidPool
{
    public class VelaTheSorceress : BaseCreature
    {
        public override bool IsInvulnerable => true;

        [Constructable]
        public VelaTheSorceress()
            : base(AIType.AI_Vendor, FightMode.None, 2, 1, 0.5, 2)
        {
            Name = "Vela";
            Title = "the sorceress";
            Blessed = true;

            SetStr(110);
            SetDex(100);
            SetInt(1000);

            Hue = Race.RandomSkinHue();
            Body = 0x191;
            HairItemID = 0x203C;
            HairHue = 0x46D;

            SetWearable(new FancyShirt(), 1928);
            SetWearable(new LeatherLegs(), 1928);
            SetWearable(new ThighBoots(), 1917);

            Item item = new BraceletOfProtection
            {
                Movable = false
            };
            PackItem(item);

            item = new Hephaestus
            {
                Movable = false
            };
            PackItem(item);

            item = new GargishHephaestus
            {
                Movable = false
            };
            PackItem(item);

            item = new BlightOfTheTundra
            {
                Movable = false
            };
            PackItem(item);

            item = new GargishBlightOfTheTundra
            {
                Movable = false
            };
            PackItem(item);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile && from.InRange(Location, 5))
                from.SendGump(new VoidPoolRewardGump(this, from as PlayerMobile));
        }

        public VelaTheSorceress(Serial serial)
            : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1152664); // Covetous Void Pool Reward Vendor
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version == 0)
            {
                Blessed = true;

                Item item = FindItemOnLayer(Layer.Shirt);
                if (item != null)
                    item.Delete();

                item = FindItemOnLayer(Layer.Pants);
                if (item != null)
                    item.Delete();

                item = FindItemOnLayer(Layer.Shoes);
                if (item != null)
                    item.Delete();

                SetWearable(new FancyShirt(), 1928);
                SetWearable(new LeatherLegs(), 1928);
                SetWearable(new ThighBoots(), 1917);
            }
        }
    }
}