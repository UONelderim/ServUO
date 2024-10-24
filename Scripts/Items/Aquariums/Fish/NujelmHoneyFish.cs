namespace Server.Items
{
    public class NujelmHoneyFish : BaseFish
    {
        [Constructable]
        public NujelmHoneyFish()
            : base(0x3B06)
        {
            Name = "miodowa ryba z thila";
        }

        public NujelmHoneyFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 1073830;// A Nujel'm Honey Fish
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