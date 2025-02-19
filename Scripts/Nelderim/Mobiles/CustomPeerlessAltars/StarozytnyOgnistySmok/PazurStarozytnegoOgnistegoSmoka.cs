namespace Server.Items
{
    public class PazurStarozytnegoOgnistegoSmoka : PeerlessKey
    {
        [Constructable]
        public PazurStarozytnegoOgnistegoSmoka()
            : base(4039)
        {
            Weight = 1;
            Hue = 1161; 
            LootType = LootType.Blessed;
        }

        public PazurStarozytnegoOgnistegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070017; //pazur starozytnego ognistego smoka
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
