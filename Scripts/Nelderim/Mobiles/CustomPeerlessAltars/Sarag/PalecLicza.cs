namespace Server.Items
{
    public class PalecLicza : PeerlessKey
    {
        [Constructable]
        public PalecLicza()
            : base(4327)
        {
            Weight = 1;
            Hue = 1121; 
            LootType = LootType.Blessed;
        }

        public PalecLicza(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070035; //palec licza
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
