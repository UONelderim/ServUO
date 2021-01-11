namespace Server.Items
{
    public class BlackDyeTub : DyeTub
    {
        [Constructable]
        public BlackDyeTub()
        {
            Hue = DyedHue = 0x0001;
            Redyable = false;
            Name = "czarna farba";
        }

        public BlackDyeTub(Serial serial)
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