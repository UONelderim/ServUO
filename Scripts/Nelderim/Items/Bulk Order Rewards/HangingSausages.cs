namespace Server.Items
{
    [Furniture]
    [Flipable(0xA8F0, 0xA8F1)]
    public class HangingSausages : Item
    {
        [Constructable]
        public HangingSausages()
            : base(0xA8F0)
        {
            Weight = 3.0;
        }

        public HangingSausages(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3060080;
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
