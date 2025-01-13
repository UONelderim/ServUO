using Server.Targeting;
using Server.Engines.Craft;

namespace Server.Items
{
    public class PowderForLeather : Item
    {
        private int m_UsesRemaining;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return m_UsesRemaining; }
            set { m_UsesRemaining = value; InvalidateProperties(); }
        }

        public PowderForLeather() : this(5) { }

        public PowderForLeather(int uses) : base(4102)
        {
            Name = "Proszek wzmocnienia do wyrobow krawieckich";
            Weight = 1.0;
            Hue = 244;
            UsesRemaining = uses;
        }

        public PowderForLeather(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write((int)m_UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_UsesRemaining = reader.ReadInt();
                        break;
                    }
            }
            ReplaceWith(new TailoringPowderOfTemperament(m_UsesRemaining));
        }
    }
}
