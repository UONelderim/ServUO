namespace Server.Items
{
	[FlipableAttribute(0x171C, 0x171C)]
	public class HelmMagaBojowego : LeatherCap
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance => 10;
		public override int BaseFireResistance => 5;
		public override int BaseColdResistance => 5;
		public override int BasePoisonResistance => -10;
		public override int BaseEnergyResistance => 5;

		public override int StrReq => 65;

		public override ArmorMaterialType MaterialType => ArmorMaterialType.Leather;
		public override CraftResource DefaultResource => CraftResource.RegularLeather;

		public override ArmorMeditationAllowance DefMedAllowance => ArmorMeditationAllowance.All;

		[Constructable]
		public HelmMagaBojowego()
		{
			Hue = 942;
			Name = "Helm maga bojowego";
			Attributes.CastSpeed = 1;
			Attributes.RegenHits = 2;
			Attributes.ReflectPhysical = 5;
			Weight = 2.0;
			LootType = LootType.Cursed;
		}

		public HelmMagaBojowego(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			if (Weight == 1.0)
			{
				Weight = 2.0;
			}
		}
	}
}
