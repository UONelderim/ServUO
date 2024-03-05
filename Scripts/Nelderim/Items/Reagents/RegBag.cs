using Server;
using Server.Items;
using Server.Mobiles;
using System;
using Server.ACC.CSS;

namespace Items.RegBag
{
    public class RegBag : Bag
    {
        public double Reduction
        {
            get { return m_Reduction; }
            set
            {
                if (value < 0)
                    m_Reduction = 0;
                else if (value >= 1)
                    m_Reduction = 1;
                else
                    m_Reduction = value;
            }
        }

        private double m_Reduction = 0.1;

        [Constructable]
        public RegBag() : this(0.5)
        { }

        [Constructable]
        public RegBag(double reduction)
        {
            this.Name = "worek na reagenty";
            this.Weight = 1;
            this.Hue = 0;
            Reduction = reduction; // Set the Reduction property
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (!(dropped is BaseReagent || dropped is Kindling || dropped is CReagent || dropped  is BaseTobacco ))
            {
                from.SendMessage("Nie mozesz tego umiescic w wroku na reagenty.");
                return false;
            }
            return base.OnDragDrop(from, dropped);
        }

        public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
        {
            if (!(item is BaseReagent || item is Kindling || item is CReagent || item is BaseTobacco ))
            {
                from.SendMessage("Nie mozesz tego umiescic w wroku na reagenty.");
                return false;
            }
            return base.OnDragDropInto(from, item, p);
        }

        public override int GetTotal(TotalType type)
        {
            if (type != TotalType.Weight)
                return base.GetTotal(type);
            else
            {
                return (int)(TotalItemWeights() * m_Reduction);
            }
        }

        public override void UpdateTotal(Item sender, TotalType type, int delta)
        {
            if (type != TotalType.Weight)
                base.UpdateTotal(sender, type, delta);
            else
                base.UpdateTotal(sender, type, (int)(delta * m_Reduction));
        }

        private double TotalItemWeights()
        {
            double weight = 0.0;

            foreach (Item item in Items)
                weight += (item.Weight * (double)(item.Amount));

            return weight;
        }

        public RegBag(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version 
            writer.Write(m_Reduction);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 1)
                m_Reduction = reader.ReadDouble();
        }
    }
}
