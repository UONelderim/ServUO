
using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.ACC.CSS;

namespace Server.Items
{
    public class RunicStaffDisassemblyTarget : Target
    {
        private RunicStaffDisassemblyDeed m_Deed;

        public RunicStaffDisassemblyTarget(RunicStaffDisassemblyDeed deed) : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (target is RunicStaff)
            {
                RunicStaff t = target as RunicStaff;

                if (!t.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    return;
                }

                if (t.ComponentBook == null || t.ComponentShield == null)
                {
                    from.SendMessage("Cos naruszylo delikatna strukture zaklecia.");
                    return;
                }

                Give(from, t.ComponentBook);
                Give(from, t.ComponentShield);
                t.Delete();
                m_Deed.Delete();
            }
            else
            {
                from.SendMessage("To nie jest kostur runiczny.");
            }
        }

        void Give(Mobile to, Item item)
        {
            Container pack = to.Backpack;

            if (pack == null || !pack.TryDropItem(to, item, false))
            {
                if (to.BankBox != null && to.BankBox.TryDropItem(to, item, false))
                {
                    to.BankBox.DropItem(item);
                    to.SendMessage("Przedmiot laduje w banku.");
                }
                else
                {
                    item.MoveToWorld(to.Location, to.Map);
                    to.SendMessage("Przedmiot upada na ziemie");
                }
            }
        }
    }

    public class RunicStaffDisassemblyDeed : Item
    {
        public static string NameText { get { return "zwoj na rozmontowanie runicznego kostura"; } } // uzyte rowniez w menu rzemieslniczym

        [Constructable]
        public RunicStaffDisassemblyDeed() : base(0x14F0)
        {
            Weight = 1.0;
            Name = NameText;
            LootType = LootType.Blessed;
            Hue = 612;
        }

        public RunicStaffDisassemblyDeed(Serial serial) : base(serial)
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
            LootType = LootType.Blessed;

            int version = reader.ReadInt();
        }

        public override bool DisplayLootType { get { return false; } }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it. 
            }
            else
            {
                from.SendMessage("Ktory kostur runiczny chcesz rozmontowac?");
                from.Target = new RunicStaffDisassemblyTarget(this);
            }
        }
    }
}
