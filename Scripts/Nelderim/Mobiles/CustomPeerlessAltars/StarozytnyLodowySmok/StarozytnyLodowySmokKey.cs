namespace Server.Items
{
    public class StarozytnyLodowySmokKey : MasterKey
    {
        public StarozytnyLodowySmokKey()
            : base(0xFF3)
        {
        }

        public StarozytnyLodowySmokKey(Serial serial)
            : base(serial)
        {
        }
        public override int LabelNumber => 3070006;// klucz do jamy starozytnego lodowego smoka
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
            if (from.Region != null && from.Region.IsPartOf("LezeLodowychSmokow_LVL3_Boss"))
                return base.CanOfferConfirmation(from);

            return false;
        }
    }
}
