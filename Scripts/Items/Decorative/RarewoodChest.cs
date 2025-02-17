namespace Server.Items
{
    [Furniture]
    [Flipable(0x2DF1, 0x2DF2)]
    public class RarewoodChest : LockableContainer
    {
        [Constructable]
        public RarewoodChest()
            : base(0x2DF1)
        {
            Weight = 5.0;
        }

        public RarewoodChest(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultGumpID => 0x10C;
        public override int DefaultDropSound => 0x42;
        public override int LabelNumber => 1073402;// rarewood chest
        public override Rectangle2D Bounds => new Rectangle2D(80, 5, 140, 70);
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
