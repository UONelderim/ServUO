namespace Server.Items
{
    public class ZhoaminthKey : MasterKey
    {
        public ZhoaminthKey()
            : base(0xFF3)
        {
        }

        public ZhoaminthKey(Serial serial)
            : base(serial)
        {
        }
        public override int LabelNumber => 3070007; //klucz do mrocznej komnaty
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
            if (from.Region != null && from.Region.IsPartOf("VelkynAto_Boss"))
                return base.CanOfferConfirmation(from);

            return false;
        }
    }
}
