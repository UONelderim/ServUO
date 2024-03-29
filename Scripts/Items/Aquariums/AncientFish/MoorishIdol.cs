namespace Server.Items
{
    public class MoorishIdol : BaseFish
    {
        [Constructable]
        public MoorishIdol()
            : base(0xA35F)
        {
            Name = "Idolek z Ened-en-Glad";
        }

        public MoorishIdol(Serial serial)
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
