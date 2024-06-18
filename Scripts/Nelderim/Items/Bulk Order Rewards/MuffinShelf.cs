namespace Server.Items
{
    [Furniture]
    [Flipable(0xA8EE, 0xA8EF)]
    public class MuffinShelf : Item
    {
        [Constructable]
        public MuffinShelf()
            : base(0xA8EE)
        {
            Weight = 20.0;
        }

        public MuffinShelf(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3060079;
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
