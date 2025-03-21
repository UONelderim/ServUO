namespace Server.Items
{
    public class RockBeauty : BaseFish
    {
        [Constructable]
        public RockBeauty()
            : base(0xA376)
        {
            Name = "Nefrytek trojbarwny";
        }

        public RockBeauty(Serial serial)
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
