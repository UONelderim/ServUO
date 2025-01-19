namespace Server.Items
{
    public class PazurStarozytnegoLodowegoSmoka : PeerlessKey
    {
        [Constructable]
        public PazurStarozytnegoLodowegoSmoka()
            : base(4039)
        {
            Weight = 1;
            Hue = 1152; 
            LootType = LootType.Blessed;
        }

        public PazurStarozytnegoLodowegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070004; //pazur starozytnego lodowego smoka
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
