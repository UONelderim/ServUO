namespace Server.Items
{
    public class RogZhoamintha : PeerlessKey
    {
        [Constructable]
        public RogZhoamintha()
            : base(4039)
        {
            Weight = 1;
            Hue = 1161; 
            LootType = LootType.Blessed;
        }

        public RogZhoamintha(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070009;//rog zhoamintha
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
