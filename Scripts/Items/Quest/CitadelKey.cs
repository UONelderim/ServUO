namespace Server.Items
{
    public class CitadelKey : MasterKey
    {
        public CitadelKey()
            : base(0x1012)
        {
            Hue = 0x489;
        }

        public CitadelKey(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 1074344;// black order key
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
            if (from.Region != null && from.Region.IsPartOf("VoxPopuli_Boss"))
                return base.CanOfferConfirmation(from);

            return false;
        }
    }
}
