namespace Server.Items
{
    public class DecorativeTopiary : Item
    {
        [Constructable]
        public DecorativeTopiary()
            : base(0x2378)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;
            Name = "zimowe drzewko";
        }

        public DecorativeTopiary(Serial serial)
            : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

           // list.Add(1070880); // Winter 2004
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
}