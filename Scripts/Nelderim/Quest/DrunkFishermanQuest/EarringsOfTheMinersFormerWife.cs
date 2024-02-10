namespace Server.Items
{
    public class EarringsOfTheMinersFormerWife : GoldEarrings
    {
     
        [Constructable]
        public EarringsOfTheMinersFormerWife()
        {
            Hue = 0x21;
            Attributes.BonusHits = 2;
            Attributes.BonusInt = 1;
        }

        public EarringsOfTheMinersFormerWife(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3050029; //Kolczyki Bylej Zony Gornika

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