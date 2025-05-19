using System;
using Server;
using Server.Targeting;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
    public class BloodyBandage : Item
    {
        [Constructable]
        public BloodyBandage() : this(1) { }

        [Constructable]
        public BloodyBandage(int amount) : base(0xE20)
        {
            Name = "Zakrwawione Bandaże";
            Stackable = true;
            Weight = 0.02;
            Amount = amount;
        }
        

        public BloodyBandage(Serial serial) : base(serial) { }

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
            if (!from.InRange(this.GetWorldLocation(), 1))
            {
                from.SendLocalizedMessage(500446); // Too far
                return;
            }
            from.SendMessage("Wskaż obiekt lub przywołańca nekro do użycia.");
            from.BeginTarget(3, false, TargetFlags.Beneficial, new TargetCallback(OnTarget));
        }

        private void OnTarget(Mobile from, object targeted)
        {
            if (this.Deleted)
                return;

            // Healing necro summon
            if (targeted is BaseCreature bc && bc.Controlled && bc.ControlMaster == from)
            {
                int healAmount = Utility.RandomMinMax(10, 20);
                bc.Hits = Math.Min(bc.Hits + healAmount, bc.HitsMax);
                from.SendMessage($"Przywołańca uleczono o {healAmount} punktów życia.");
                ConsumeOne();
                return;
            }

            // Original bandage cleaning behavior
            switch (targeted)
            {
                case BaseBeverage bev when bev.Content == BeverageType.Water:
                {
                    int maxClean = this.Amount;
                    int waterNeeded = ((maxClean - 1) / 10) + 1;

                    if (waterNeeded <= bev.Quantity)
                    {
                        from.AddToBackpack(new Bandage(this.Amount));
                        from.SendMessage("Myjesz bandaże.");
                        bev.Quantity -= waterNeeded;
                        this.Delete();
                    }
                    else
                    {
                        int cleanCount = bev.Quantity * 10;
                        from.AddToBackpack(new Bandage(cleanCount));
                        bev.Quantity = 0;
                        this.Amount -= cleanCount;
                        from.SendMessage("Myjesz część bandaży.");
                    }
                    break;
                }

                case StaticTarget s when InternalTarget.isValidItemId(s.ItemID):
                case Item i when InternalTarget.isValidItemId(i.ItemID):
                case LandTarget c when InternalTarget.isValidLandId(c.TileID):
                    from.AddToBackpack(new Bandage(this.Amount));
                    from.SendMessage("Myjesz bandaże.");
                    this.Delete();
                    break;

                default:
                    from.SendMessage("Nie możesz tego zrobić.");
                    break;
            }
        }

        private void ConsumeOne()
        {
            if (Amount > 1)
                Amount--;
            else
                Delete();
        }

        private class InternalTarget : Target
        {
            public InternalTarget(Item bandage) : base(3, true, TargetFlags.Beneficial)
            {
            }

            public static bool isValidItemId(int itemId) =>
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

            public static bool isValidLandId(int landId) =>
                (landId >= 168 && landId <= 171) ||
                (landId >= 310 && landId <= 311);

            protected override void OnTarget(Mobile from, object targeted) { }
        }
    }
}
