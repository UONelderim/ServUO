using Server.Targeting;

namespace Server.Items
{
    public class BloodyBandage : Item
    {
        [Constructable]
        public BloodyBandage() : this(1)
        {
        }

        [Constructable]
        public BloodyBandage(int amount) : base(0xE20)
        {
            Name = "Zakrwawione Bandaze";
            Stackable = true;
            Weight = 0.02;
            Amount = amount;
            Hue = 41;
        }

        public BloodyBandage(Serial serial) : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Wskaz cel");
            from.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private BloodyBandage _bandage;

            public InternalTarget(BloodyBandage bandage) : base(3, true, TargetFlags.Beneficial)
            {
                _bandage = bandage;
            }

            private static bool isValidItemId(int itemId) =>
                (itemId >= 2881 && itemId <= 2884) ||
                itemId == 3707 || itemId == 4088 ||
                itemId == 4104 || itemId == 4089 ||
                itemId == 5453 || itemId == 5465 ||
                (itemId >= 5458 && itemId <= 5460) ||
                (itemId >= 5937 && itemId <= 5978) ||
                (itemId >= 6038 && itemId <= 6066) ||
                (itemId >= 6595 && itemId <= 6636) ||
                (itemId >= 8093 && itemId <= 8094) ||
                (itemId >= 8099 && itemId <= 8138) ||
                (itemId >= 9299 && itemId <= 9309) ||
                (itemId >= 13422 && itemId <= 13525) ||
                (itemId >= 13549 && itemId <= 13616);

            private static bool isValidLandId(int landId) =>
                (landId >= 168 && landId <= 171) ||
                (landId >= 310 && landId <= 311);

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_bandage.Deleted)
                    return;

                switch (targeted)
                {
                    case BaseBeverage bev when bev.Content == BeverageType.Water:
                    {
                        if (_bandage.Amount / 10 <= bev.Quantity)
                        {
                            from.AddToBackpack(new Bandage(_bandage.Amount));
                            from.SendMessage("Myjesz bandaze");
                            bev.Quantity -= ((_bandage.Amount - 1) / 10) + 1;
                            _bandage.Delete();
                        }
                        else
                        {
                            var newAmount = bev.Quantity * 10;
                            from.AddToBackpack(new Bandage(newAmount));
                            bev.Quantity = 0;
                            _bandage.Amount -= newAmount;
                            from.SendMessage("Myjesz czesc bandaze");
                        }
                        break;
                    }
                    case StaticTarget s when isValidItemId(s.ItemID):
                    case Item i when isValidItemId(i.ItemID):
                    case LandTarget c when isValidLandId(c.TileID):
                        from.AddToBackpack(new Bandage(_bandage.Amount));
                        from.SendMessage("Myjesz bandaze");
                        _bandage.Delete();
                        break;
                    default:
                        from.SendMessage("Nie mozesz tego zrobic");
                        break;
                }
            }
        }
    }
}
