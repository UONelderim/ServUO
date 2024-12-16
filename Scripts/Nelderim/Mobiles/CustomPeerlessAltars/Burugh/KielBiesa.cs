namespace Server.Items
{
    public class KielBiesa : PeerlessKey
    {
        [Constructable]
        public KielBiesa()
            : base(0x10E8)
        {
            Weight = 1;
            Hue = 0x174; 
            LootType = LootType.Blessed;
        }

        public KielBiesa(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070002; //kiel biesa
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
