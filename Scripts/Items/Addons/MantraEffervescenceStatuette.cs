namespace Server.Items
{
    public class MantraEffervescenceStatuette : ShimmeringEffusionStatuette
    {
        [Constructable]
        public MantraEffervescenceStatuette()
        {
			ItemID = 0x2D94;

			Weight = 1.0;
        }

        public MantraEffervescenceStatuette(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 1074505;// Mantra Effervescence Statuette
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
    }
}