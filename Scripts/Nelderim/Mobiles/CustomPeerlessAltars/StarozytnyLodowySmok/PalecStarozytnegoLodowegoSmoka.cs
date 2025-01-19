namespace Server.Items
{
    public class PalecStarozytnegoLodowegoSmoka : PeerlessKey
    {
        [Constructable]
        public PalecStarozytnegoLodowegoSmoka()
            : base(7585)
        {
            Weight = 1;
            Hue = 1152; 
            LootType = LootType.Blessed;
        }

        public PalecStarozytnegoLodowegoSmoka(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 3070003;// palec starozytnego lodowego smoka
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
