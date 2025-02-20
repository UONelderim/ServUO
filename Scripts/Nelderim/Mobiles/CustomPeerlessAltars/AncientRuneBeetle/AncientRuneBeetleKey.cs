namespace Server.Items
{
    public class AncientRuneBeetleKey : MasterKey
    {
        public AncientRuneBeetleKey()
            : base(0xFF3)
        {
        }

        public AncientRuneBeetleKey(Serial serial)
            : base(serial)
        {
        }
        public override int LabelNumber => 3070034;//klucz do jamy starozytnego runicznego zuka
        public override int Lifespan => 600;
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
            if (from.Region != null && from.Region.IsPartOf("LoenTorech_LVL2_Boss"))
                return base.CanOfferConfirmation(from);

            return false;
        }
    }
}
