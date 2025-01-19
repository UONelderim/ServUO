namespace Server.Items
{
    public class RogZhoarmintha : PeerlessKey
    {
        [Constructable]
        public RogZhoarmintha()
            : base(4039)
        {
            Weight = 1;
            Hue = 1161; 
            LootType = LootType.Blessed;
        }

        public RogZhoarmintha(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070009;//rog zhoarmintha
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
