namespace Server.Items
{
    public class WiekszyKrysztalMocy : PeerlessKey
    {
        [Constructable]
        public WiekszyKrysztalMocy()
            : base(0x1F1C)
        {
            Weight = 1;
            Hue = 2391; 
            LootType = LootType.Blessed;
        }

        public WiekszyKrysztalMocy(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070028; //wiekszy krysztal mocy
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
