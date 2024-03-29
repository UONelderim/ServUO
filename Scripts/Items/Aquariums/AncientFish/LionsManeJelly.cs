namespace Server.Items
{
    public class LionsManeJelly : BaseFish
    {
        [Constructable]
        public LionsManeJelly()
            : base(0xA387)
        {
            Name = "Beltwa festonowa";
        }

        public LionsManeJelly(Serial serial)
            : base(serial)
        {
        }

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
