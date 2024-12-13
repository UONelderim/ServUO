namespace Server.Items
{
    public class PowiekaGazera : PeerlessKey
    {
        [Constructable]
        public PowiekaGazera()
            : base(6256)
        {
            Weight = 1;
            Hue = 2281; 
            LootType = LootType.Blessed;
        }

        public PowiekaGazera(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070001; //powieka gazera
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
