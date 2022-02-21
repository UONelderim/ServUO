namespace Server.Items
{
	[FlipableAttribute(0x171C, 0x171C)]
	public class BlazenskieSzczescie : BaseArmor
	{
		public override int LabelNumber { get { return 1065805; } } // Blazenskie Szczescie
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance { get { return 5; } }
		public override int BaseFireResistance { get { return 5; } }
		public override int BaseColdResistance { get { return 5; } }
		public override int BasePoisonResistance { get { return 5; } }
		public override int BaseEnergyResistance { get { return 5; } }

		public override int StrReq { get { return 20; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }
		public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

		public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

		[Constructable]
		public BlazenskieSzczescie() : base(0x171C)
		{
			Hue = 0x501;
			Attributes.Luck = 180;
			Attributes.BonusInt = 5;
			Attributes.LowerRegCost = 20;
			Weight = 2.0;
		}

		public BlazenskieSzczescie(Serial serial) : base(serial)
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
