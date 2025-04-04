namespace Server.Items
{
    [Furniture]
    [Flipable(0x4FFE, 0x4FFF)]
    public class ScribesDesk : Item
    {
        [Constructable]
        public ScribesDesk()
            : base(0x4FFE)
        {
            Weight = 20.0;
        }

        public ScribesDesk(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3060066;
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
