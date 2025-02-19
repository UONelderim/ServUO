namespace Server.Items
{
    public class KoscianegoSmokKey : MasterKey
    {
        public KoscianegoSmokKey()
            : base(0xFF3)
        {
        }

        public KoscianegoSmokKey(Serial serial)
            : base(serial)
        {
        }
        public override int LabelNumber => 3070023;//klucz do jamy koscinego smoka
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
            if (from.Region != null && from.Region.IsPartOf("Garth_LVL2_Boss"))
                return base.CanOfferConfirmation(from);

            return false;
        }
    }
}
