namespace Server.Items
{
    public class PazurKoscianegoSmoka : PeerlessKey
    {
        [Constructable]
        public PazurKoscianegoSmoka()
            : base(4039)
        {
            Weight = 1;
            Hue = 38; 
            LootType = LootType.Blessed;
        }

        public PazurKoscianegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070021;//pazur koscinego smoka
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
