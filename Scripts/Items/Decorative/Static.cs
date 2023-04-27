namespace Server.Items
{
    public class Static : Item
    {
        public Static()
	        : base(0x80)
        {
	        Movable = false;
        }

        [Constructable]
        public Static(int itemID) : base(itemID)
        {
            Movable = false;
        }

       [Constructable]
        public Static(int itemID, int count) : this(Utility.Random(itemID, count))
        {
        }

        [CommandProperty(AccessLevel.Counselor)]
        public string DecorName
        {
            get { return Name; }
            set { Name = value; }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public string DecorLabel
        {
            get { return Label1; }
            set { Label1 = value; }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public LootType DecorLootType
        {
            get { return LootType; }
            set { LootType = value; }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public int DecorItemID
        {
            get { return ItemID; }
            set { ItemID = value; }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public int DecorHue
        {
            get { return Hue; }
            set { Hue = value; }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public bool DecorMovable
        {
            get { return Movable; }
            set { Movable = value; }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public Point3D DecorLocation
        {
            get { return Location; }
            set { Location = value; }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public double DecorWeight
        {
            get { return Weight; }
            set { Weight = value; }
        }

        public Static(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && Weight == 0)
                Weight = -1;
        }
    }

    public class LocalizedStatic : Static
    {
        private int m_LabelNumber;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Number
        {
            get { return m_LabelNumber; }
            set
            {
                m_LabelNumber = value;
                InvalidateProperties();
            }
        }

        public override int LabelNumber
        {
            get { return m_LabelNumber; }
        }

        [Constructable]
        public LocalizedStatic(int itemID) : this(itemID, 1020000 + itemID)
        {
        }

        [Constructable]
        public LocalizedStatic(int itemID, int labelNumber) : base(itemID)
        {
            m_LabelNumber = labelNumber;
        }

        public LocalizedStatic(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((byte)0); // version
            writer.WriteEncodedInt((int)m_LabelNumber);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadByte();

            switch (version)
            {
                case 0:
                {
                    m_LabelNumber = reader.ReadEncodedInt();
                    break;
                }
            }
        }
    }
}
