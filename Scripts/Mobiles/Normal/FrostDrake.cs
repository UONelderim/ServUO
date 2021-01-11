namespace Server.Mobiles
{
    [CorpseName("zwloki smoczego pisklaka mrozu")]
    public class FrostDrake : ColdDrake
    {
        [Constructable]
        public FrostDrake()
        {
            Name = "smocze piskle mrozu";
        }

        public FrostDrake(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}