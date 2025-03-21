using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x13eb, 0x13f2 )]
	public class FieryGloves : BaseArmor
	{
		public override int BasePhysicalResistance{ get{ return 11; } }
		public override int BaseFireResistance{ get{ return 20; } }
		public override int BaseColdResistance{ get{ return 11; } }
		public override int BasePoisonResistance{ get{ return -9; } }
		public override int BaseEnergyResistance{ get{ return 15; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Ringmail; } }

		[Constructable]
		public FieryGloves() : base( 0x13EB )
		{
			Name = "Rekawice Ferionu";
			Weight = 5.0;
			Hue = 1161;
			AbsorptionAttributes.CastingFocus = 3;
			AbsorptionAttributes.ResonanceFire = 10;
			ArmorAttributes.SoulCharge = 10;
			ArmorAttributes.LowerStatReq = 10;
			Attributes.BonusInt = 5;



		}

		public FieryGloves( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( Weight == 1.0 )
				Weight = 15.0;
		}
	}
}
