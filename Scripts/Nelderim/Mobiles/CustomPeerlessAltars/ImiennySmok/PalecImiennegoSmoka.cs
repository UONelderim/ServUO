namespace Server.Items
{
    public class PalecImiennegoSmoka : PeerlessKey
    {
        [Constructable]
        public PalecImiennegoSmoka()
            : base(7585)
        {
            Weight = 1;
            Hue = 22; 
            LootType = LootType.Blessed;
        }

        public PalecImiennegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070024; //palec imiennego smoka
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
