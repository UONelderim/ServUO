namespace Server.Items
{
    public class PalecKoscianegoSmoka : PeerlessKey
    {
        [Constructable]
        public PalecKoscianegoSmoka()
            : base(7585)
        {
            Weight = 1;
            Hue = 38; 
            LootType = LootType.Blessed;
        }

        public PalecKoscianegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070020;//palec koscinego smoka
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
