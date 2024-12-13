namespace Server.Items
{
    public class OdnogaStaregoGazera : PeerlessKey
    {
        [Constructable]
        public OdnogaStaregoGazera()
            : base(7585)
        {
            Weight = 1;
            Hue = 2281; 
            LootType = LootType.Blessed;
        }

        public OdnogaStaregoGazera(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070000;// Odnoga Starego Gazera
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
