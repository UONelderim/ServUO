namespace Server.Items
{
    public class ParoxysmusKey2 : MasterKey
    {
        public ParoxysmusKey2()
            : base(0xEFB)
        {
        }

        public ParoxysmusKey2(Serial serial)
            : base(serial)
        {
		    Weight = 1.0;
            Hue = 0x497;
        }
		
        public override int LabelNumber => 1074330;  // slimy ointment
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
            if (from.Region != null && from.Region.IsPartOf("Parox_Boss"))
                return base.CanOfferConfirmation(from);

            return false;
        }
    }
}
