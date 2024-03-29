namespace Server.Items
{
    public class BritainCrownFish : BaseFish
    {
        [Constructable]
        public BritainCrownFish()
            : base(0x3AFF)
        {
            Name = "tasandorska ryba krolewska";
        }

        public BritainCrownFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 1074589;// Britain Crown Fish
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