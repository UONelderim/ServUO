namespace Server.Items
{
    public class RedDartFish : BaseFish
    {
        [Constructable]
        public RedDartFish()
            : base(0x3B00)
        {
            Name = "czerwona ryba z tasandory";
        }

        public RedDartFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 1073834;// A Red Dart Fish 
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