namespace Server.Items
{
    public class QueenTrigger : BaseFish
    {
        [Constructable]
        public QueenTrigger()
            : base(0xA361)
        {
            Name = "Rogatnica piekna";
        }

        public QueenTrigger(Serial serial)
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
