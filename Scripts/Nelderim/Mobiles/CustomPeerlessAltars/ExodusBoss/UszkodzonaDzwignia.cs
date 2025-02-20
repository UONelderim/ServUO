namespace Server.Items
{
    public class UszkodzonaDzwignia : PeerlessKey
    {
        [Constructable]
        public UszkodzonaDzwignia()
            : base(0x1094)
        {
            Weight = 1;
            Hue = 2391; 
            LootType = LootType.Blessed;
        }

        public UszkodzonaDzwignia(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070030;//uszkodzona dzwignia
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
