namespace Server.Items
{
    public class PalecStarozytnegoDiamentowegoSmoka : PeerlessKey
    {
        [Constructable]
        public PalecStarozytnegoDiamentowegoSmoka()
            : base(7585)
        {
            Weight = 1;
            Hue = 1153; 
            LootType = LootType.Blessed;
        }

        public PalecStarozytnegoDiamentowegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070011;//pazur starozytnego diamentowego smoka
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
