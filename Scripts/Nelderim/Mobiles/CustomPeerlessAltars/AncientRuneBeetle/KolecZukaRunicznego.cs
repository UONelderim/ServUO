namespace Server.Items
{
    public class KolecZukaRunicznego : PeerlessKey
    {
        [Constructable]
        public KolecZukaRunicznego()
            : base(0x2F61)
        {
            Weight = 1;
            Hue = 2127; 
            LootType = LootType.Blessed;
        }

        public KolecZukaRunicznego(Serial serial)
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
