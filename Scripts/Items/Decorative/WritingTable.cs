namespace Server.Items
{
    [Furniture]
    [Flipable(0xB4A, 0xB49, 0xB4B, 0xB4C)]
    public class WritingTable : CraftableFurniture
    {
        [Constructable]
        public WritingTable()
            : base(0xB4A)
        {
            Weight = 15.0;
        }

        public WritingTable(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
