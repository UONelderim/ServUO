namespace Server.Items
{
    [Flipable(0x2B69, 0x3160)]
    public class WoodlandGorget : BaseArmor
    {
        [Constructable]
        public WoodlandGorget()
            : base(0x2B69)
        {
			Name = "Inkrustowany karczek";
			Weight = 1.0;
        }

        public WoodlandGorget(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance => 5;
        public override int BaseFireResistance => 3;
        public override int BaseColdResistance => 2;
        public override int BasePoisonResistance => 3;
        public override int BaseEnergyResistance => 2;
        public override int InitMinHits => 50;
        public override int InitMaxHits => 65;
        public override int StrReq => 45;
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Wood;
        public override CraftResource DefaultResource => CraftResource.RegularWood;
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }
}
