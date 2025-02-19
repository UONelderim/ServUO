namespace Server.Items
{
    public class ZabStarozytnegoOgnistegoSmoka : PeerlessKey
    {
        [Constructable]
        public ZabStarozytnegoOgnistegoSmoka()
            : base(0x10E8)
        {
            Weight = 1;
            Hue = 1161; 
            LootType = LootType.Blessed;
        }

        public ZabStarozytnegoOgnistegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070018;//zab starozytnego ognistego smoka
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
