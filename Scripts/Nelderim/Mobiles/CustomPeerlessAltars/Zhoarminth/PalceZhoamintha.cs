namespace Server.Items
{
    public class PalceZhoamintha : PeerlessKey
    {
        [Constructable]
        public PalceZhoamintha()
            : base(4328)
        {
            Weight = 1;
            Hue = 1151; 
            LootType = LootType.Blessed;
        }

        public PalceZhoamintha(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070008; //palce zhoamintha
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
