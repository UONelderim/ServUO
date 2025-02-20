namespace Server.Items
{
    public class ZabImiennegoSmoka : PeerlessKey
    {
        [Constructable]
        public ZabImiennegoSmoka()
            : base(0x10E8)
        {
            Weight = 1;
            Hue = 22; 
            LootType = LootType.Blessed;
        }

        public ZabImiennegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070022; //zab imiennego smoka
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
