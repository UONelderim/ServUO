using Server.Gumps;
using Server.Multis;
using Server.Network;
using Server.Targeting; // Required for Target and TargetFlags

namespace Server.Items.Addons.TheKingsCollection
{
    public class FancyLoveseatDeed : BaseAddonDeed
    {
        private int m_LastSelection;

        [Constructable]
        public FancyLoveseatDeed()
        {
            Name = "Wytowrna Mala Sofa";
            m_LastSelection = 0; // Default to no selection
        }

        public FancyLoveseatDeed(Serial serial) : base(serial)
        {
        }

        public override BaseAddon Addon
        {
            get
            {
                if (m_LastSelection == 0)
                    return null; // No item selected yet

                return new FancyLoveseatAddon(this, m_LastSelection);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // Version
            writer.Write(m_LastSelection);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 1)
            {
                m_LastSelection = reader.ReadInt();
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // Must be in your backpack to use.
                return;
            }

            from.SendGump(new DualItemSelectionGump(from, this));
        }

        public void CreateItem(Mobile from, int selection)
        {
            m_LastSelection = selection; // Save the selected item
            FancyLoveseatAddon addon = new FancyLoveseatAddon(this, selection);

            from.Target = new AddonPlacementTarget(from, addon, this);
        }

        private class AddonPlacementTarget : Target
        {
            private readonly Mobile m_From;
            private readonly FancyLoveseatAddon m_Addon;
            private readonly Item m_ItemDeed;

            public AddonPlacementTarget(Mobile from, FancyLoveseatAddon addon, Item itemDeed) : base(10, true, TargetFlags.None)
            {
                m_From = from;
                m_Addon = addon;
                m_ItemDeed = itemDeed;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point)
                {
                    Map map = from.Map;

                    if (map == null)
                        return;

                    Point3D location = new Point3D(point);

                    // Check if the location belongs to a house
                    BaseHouse house = BaseHouse.FindHouseAt(location, map, 16);

                    if (house == null)
                    {
                        from.SendMessage("Mozesz to postawic tylko w domu.");
                        return;
                    }

                    if (map.CanFit(location, 16, false, true)) // Check if the location is valid
                    {
                        m_Addon.MoveToWorld(location, map);
                        m_ItemDeed.Delete();
                        from.SendMessage("Postawiles przedmiot.");
                    }
                    else
                    {
                        from.SendMessage("To nie moze byc tu postawione.");
                    }
                }
                else
                {
                    from.SendMessage("To nie jest prawidlowa lokalizacja");
                }
            }
        }
    }

    public class DualItemSelectionGump : Gump
    {
        private readonly Mobile m_From;
        private readonly FancyLoveseatDeed m_Deed;

        public DualItemSelectionGump(Mobile from, FancyLoveseatDeed deed) : base(50, 50)
        {
            m_From = from;
            m_Deed = deed;

            AddPage(0);
            AddBackground(0, 0, 300, 250, 9270); // Increased size for longer names
            AddAlphaRegion(10, 10, 280, 230);

            AddLabel(50, 20, 1152, "Wybierz orientacje przedmiotu:");

            AddButton(30, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddLabel(70, 50, 1152, "Wytworna Mala Sofa (E)");

            AddButton(30, 80, 4005, 4007, 2, GumpButtonType.Reply, 0);
            AddLabel(70, 80, 1152, "Wytworna Mala Sofa (N)");

            AddButton(30, 110, 4005, 4007, 3, GumpButtonType.Reply, 0);
            AddLabel(70, 110, 1152, "Wytworna Mala Sofa (S)");

            AddButton(30, 140, 4005, 4007, 4, GumpButtonType.Reply, 0);
            AddLabel(70, 140, 1152, "Wytworna Mala Sofa (W)");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Deed.Deleted || !m_Deed.IsChildOf(m_From.Backpack))
                return;

            switch (info.ButtonID)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    m_Deed.CreateItem(m_From, info.ButtonID);
                    break;
                default:
                    m_From.SendMessage("Zdecydowales nie uzywac przedmiotu.");
                    break;
            }
        }
    }

    public class FancyLoveseatAddon : BaseAddon
    {
        private readonly FancyLoveseatDeed m_Deed;
        private readonly int m_Selection;

        public FancyLoveseatAddon(FancyLoveseatDeed deed, int selection)
        {
            m_Deed = deed;
            m_Selection = selection;

            switch (selection)
            {
                case 1:
                    AddComponent(new AddonComponent(0x4C88), 0, 0, 0);
                    AddComponent(new AddonComponent(0x4C89), 0, 1, 0);
                    break;
                case 2:
                    AddComponent(new AddonComponent(0x9C5A), 0, 0, 0);
                    AddComponent(new AddonComponent(0x9C59), 1, 0, 0);
                    break;
                case 3:
                    AddComponent(new AddonComponent(0x4C87), 0, 0, 0);
                    AddComponent(new AddonComponent(0x4C86), 1, 0, 0);
                    break;
                case 4:
                    AddComponent(new AddonComponent(0x9C58), 0, 0, 0);
                    AddComponent(new AddonComponent(0x9C57), 0, 1, 0);
                    break;
            }
        }

        public override BaseAddonDeed Deed => new FancyLoveseatDeed();

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            Mobile owner = m_Deed?.RootParent as Mobile;

            if (owner != null && owner.Backpack != null)
            {
                owner.Backpack.DropItem(Deed); // Return the original FancyLoveseatDeed
            }
        }
    }
}
