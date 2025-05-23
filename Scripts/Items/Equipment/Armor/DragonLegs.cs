namespace Server.Items
{
    [Flipable(0x2647, 0x2648)]
    public class DragonLegs : BaseArmor
    {
        [Constructable]
        public DragonLegs()
            : base(0x2647)
        {
            Weight = 6.0;
        }

        public DragonLegs(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance => 3;
        public override int BaseFireResistance => 3;
        public override int BaseColdResistance => 3;
        public override int BasePoisonResistance => 3;
        public override int BaseEnergyResistance => 3;
        public override int InitMinHits => 55;
        public override int InitMaxHits => 75;
        public override int StrReq => 75;
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Dragon;
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
