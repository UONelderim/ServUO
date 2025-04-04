namespace Server.Items
{
    [Furniture]

    public class FruitBasket2 : Item
    {
        [Constructable]
        public FruitBasket2()
            : base(0x5059)
        {
            Weight = 3.0;
        }

        public FruitBasket2(Serial serial)
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
