namespace Server.Items
{
    public class PurpleFrog : BaseFish
    {
        [Constructable]
        public PurpleFrog()
            : base(0x3B0D)
        {
            Hue = 0x4FA;
            Name = "pospolita ryba z garlan";
        }

        public PurpleFrog(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 1073823;// A Purple Frog
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