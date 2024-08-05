using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class PetBondingPotion : Item
    {
        public override int LabelNumber => 1152921;  // Wywar oswajacza

        [Constructable]
        public PetBondingPotion() : base(0x0F04)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;
            Hue = 2629;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it. 
            }
            else
            {
                from.SendLocalizedMessage(1152922); // Wskaz zwierze do uwiernienia
                from.Target = new BondingTarget(this);
            }
        }

        public PetBondingPotion(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BondingTarget : Target
    {
        private readonly Item m_Item;

        public BondingTarget(Item item) : base(1, false, TargetFlags.None)
        {
            m_Item = item;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (m_Item == null || m_Item.Deleted || !m_Item.IsChildOf(from.Backpack))
                return;

            if (target is BaseCreature bc)
            {
                if (bc.IsBonded)
                {
                    from.SendLocalizedMessage(1152925); // To zwierze jest juz wierne
                }
                else if (bc.ControlMaster != from)
                {
                    from.SendLocalizedMessage(1114368); // To nie twoje zwierze!
                }
                else if (bc.Allured || bc.Summoned)
                {
                    from.SendLocalizedMessage(1152924); // Nie mozesz tego uwiernic.
                }
                else if (target is BaseTalismanSummon)
                {
                    from.SendLocalizedMessage(1152924); // Nie mozesz tego uwiernic.
                }
                else if (!bc.CanStartBonding)
                {
	                from.SendLocalizedMessage(1075268); // Twoje zwierze nie bedzie ci wierne bez odpowiednich umiejetnosci.
                }
                else
                {
                    bc.IsBonded = true;
                    from.SendLocalizedMessage(1049666); // Twoje zwierze jest ci teraz wierne!
                    m_Item.Consume();
                }
            }
            else
            {
                from.SendLocalizedMessage(1152924);  // Nie mozesz tego uwiernic.
            }
        }
    }
}
