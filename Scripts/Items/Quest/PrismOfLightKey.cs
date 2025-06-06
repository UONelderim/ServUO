namespace Server.Items
{
    public class PrismOfLightKey : MasterKey
    {
        public PrismOfLightKey()
            : base(0xE27)
        {
        }

        public PrismOfLightKey(Serial serial)
            : base(serial)
        {
        }

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
            if (from.Region != null && from.Region.IsPartOf("Shimmering_LV2_Boss"))
                return base.CanOfferConfirmation(from);

            return false;
        }
    }
}
