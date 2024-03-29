namespace Server.Items
{
    public class LeafySeaDragon : BaseFish
    {
        [Constructable]
        public LeafySeaDragon()
            : base(0xA390)
        {
            Name = "lisciasty smok morski";
         }

        public LeafySeaDragon(Serial serial)
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
