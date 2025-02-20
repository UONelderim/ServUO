namespace Server.Items
{
    public class PazurImiennegoSmoka : PeerlessKey
    {
        [Constructable]
        public PazurImiennegoSmoka()
            : base(4039)
        {
            Weight = 1;
            Hue = 22; 
            LootType = LootType.Blessed;
        }

        public PazurImiennegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070025; //pazur imiennego smoka
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
