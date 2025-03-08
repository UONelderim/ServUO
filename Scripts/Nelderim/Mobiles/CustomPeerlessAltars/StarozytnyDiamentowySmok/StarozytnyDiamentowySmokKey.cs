namespace Server.Items
{
    public class StarozytnyDiamentowySmokKey : MasterKey
    {
        public StarozytnyDiamentowySmokKey()
            : base(0xFF3)
        {
        }

        public StarozytnyDiamentowySmokKey(Serial serial)
            : base(serial)
        {
        }
        public override int LabelNumber => 3070014; //klucz do jamy starozytnego diamentowego smoka
        public override int Lifespan => 180;
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

        public override bool CanOfferConfirmation(Mobile from)
        {
            if (from.Region != null && from.Region.IsPartOf("KrysztaloweSmoki_Boss"))
                return base.CanOfferConfirmation(from);

            return false;
        }
    }
}
