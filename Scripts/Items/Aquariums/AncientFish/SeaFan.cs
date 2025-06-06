namespace Server.Items
{
    public class SeaFan : BaseFish
    {
        [Constructable]
        public SeaFan()
            : base(0xA38C)
        {
            Name = "wachlarz morski";
        }

        public SeaFan(Serial serial)
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
