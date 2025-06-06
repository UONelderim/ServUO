namespace Server.Items
{
    public class TwistedWealdKey : MasterKey
    {
        public TwistedWealdKey()
            : base(0xE26)
        {
            Weight = 1.0;
            Hue = 0x481;
        }

        public TwistedWealdKey(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070038; //klucz do groty spczonego jednorozca
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
            if (from.Region != null && from.Region.IsPartOf("TylReviaren_Boss"))
                return base.CanOfferConfirmation(from);

            return false;
        }
    }
}
