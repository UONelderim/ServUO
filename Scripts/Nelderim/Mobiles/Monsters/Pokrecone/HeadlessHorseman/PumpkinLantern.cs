namespace Server.Items
{
    public class PumpkinLantern : Item
    {
        private InternalItem m_Item;

        [Constructable]
        public PumpkinLantern() : base(0x1647)
        {
            Weight = 0.0;
            Light = LightType.Circle300;

            m_Item = new InternalItem(this);
        }

        public PumpkinLantern(Serial serial) : base(serial)
        {
        }


        public override void OnLocationChange(Point3D oldLocation)
        {
            if (m_Item != null)
                m_Item.Location = new Point3D(X, Y, Z);
        }

        public override void OnMapChange()
        {
            if (m_Item != null)
                m_Item.Map = Map;
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (m_Item != null)
                m_Item.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_Item);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Item = reader.ReadItem() as InternalItem;
        }

        private class InternalItem : Item
        {
            private PumpkinLantern m_Item;

            public InternalItem(PumpkinLantern item) : base(0xC6A)
            {
                Name = "Dyniowa Latarnia";
                m_Item = item;
            }

            public InternalItem(Serial serial) : base(serial)
            {
            }

            public override void OnLocationChange(Point3D oldLocation)
            {
                if (m_Item != null)
                    m_Item.Location = new Point3D(X, Y, Z);
            }

            public override void OnMapChange()
            {
                if (m_Item != null)
                    m_Item.Map = Map;
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

                if (m_Item != null)
                    m_Item.Delete();
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);

                writer.Write((int)0); // version
                writer.Write(m_Item);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                int version = reader.ReadInt();
                m_Item = reader.ReadItem() as PumpkinLantern;
            }
        }
    }
}