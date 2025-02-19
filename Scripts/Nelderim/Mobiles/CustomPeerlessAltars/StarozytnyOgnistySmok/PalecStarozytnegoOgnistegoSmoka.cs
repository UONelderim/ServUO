namespace Server.Items
{
    public class PalecStarozytnegoOgnistegoSmoka : PeerlessKey
    {
        [Constructable]
        public PalecStarozytnegoOgnistegoSmoka()
            : base(7585)
        {
            Weight = 1;
            Hue = 1161; 
            LootType = LootType.Blessed;
        }

        public PalecStarozytnegoOgnistegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070016; //pazur starozytnego ognistego smoka
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
