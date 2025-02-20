namespace Server.Items
{
    public class PrzekletaKosc : PeerlessKey
    {
        [Constructable]
        public PrzekletaKosc()
            : base(0xf7e)
        {
            Weight = 1;
            Hue = 0; 
            LootType = LootType.Blessed;
        }

        public PrzekletaKosc(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070036; //przekleta kosc
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
