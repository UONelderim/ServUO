namespace Server.Items
{
    public class BurughKey : MasterKey
    {
        public BurughKey()
            : base(0xFF3)
        {
        }

        public BurughKey(Serial serial)
            : base(serial)
        {
        }

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
            if (from.Region != null && from.Region.IsPartOf("Alcala_Boss"))
                return base.CanOfferConfirmation(from);

            return false;
        }
    }
}
