namespace Server.Items
{
    [Furniture]

    public class FilletedFish : Item
    {
        [Constructable]
        public FilletedFish()
            : base(0xA8ED)
        {
            Weight = 3.0;
        }

        public FilletedFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3060078;
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
