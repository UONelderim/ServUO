namespace Server.Items
{
    public class Fireclam : BaseFish
    {
        [Constructable]
        public Fireclam()
            : base(0xA385)
        {
            Name = "malz ognisty";
        }

        public Fireclam(Serial serial)
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
