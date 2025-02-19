namespace Server.Items
{
    public class ZabStarozytnegoDiamentowegoSmoka : PeerlessKey
    {
        [Constructable]
        public ZabStarozytnegoDiamentowegoSmoka()
            : base(0x10E8)
        {
            Weight = 1;
            Hue = 1153; 
            LootType = LootType.Blessed;
        }

        public ZabStarozytnegoDiamentowegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070005;// zab starozytnego lodowego smoka
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
