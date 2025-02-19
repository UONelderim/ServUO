namespace Server.Items
{
    public class ZabKoscianegoSmoka : PeerlessKey
    {
        [Constructable]
        public ZabKoscianegoSmoka()
            : base(0x10E8)
        {
            Weight = 1;
            Hue = 38; 
            LootType = LootType.Blessed;
        }

        public ZabKoscianegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070022;//zab koscinego smoka
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
