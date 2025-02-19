namespace Server.Items
{
    public class PazurStarozytnegoDiamentowegoSmoka : PeerlessKey
    {
        [Constructable]
        public PazurStarozytnegoDiamentowegoSmoka()
            : base(4039)
        {
            Weight = 1;
            Hue = 1153; 
            LootType = LootType.Blessed;
        }

        public PazurStarozytnegoDiamentowegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070012; //pazur starozytnego diamentowego smoka
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
