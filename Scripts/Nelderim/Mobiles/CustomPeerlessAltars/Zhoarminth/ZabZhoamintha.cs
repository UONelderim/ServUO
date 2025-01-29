namespace Server.Items
{
    public class ZabZhoamintha : PeerlessKey
    {
        [Constructable]
        public ZabZhoamintha()
            : base(0x10E8)
        {
            Weight = 1;
            Hue = 1151; 
            LootType = LootType.Blessed;
        }

        public ZabZhoamintha(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070010; //zab zhoamintha
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
