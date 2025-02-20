namespace Server.Items
{
    public class PlatynowaZbroja : PeerlessKey
    {
        [Constructable]
        public PlatynowaZbroja()
            : base(0x1415)
        {
            Weight = 1;
            Hue = 2391; 
            LootType = LootType.Blessed;
        }

        public PlatynowaZbroja(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070029; //platynowa zbroja
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
