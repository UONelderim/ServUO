using System;
using Server;

namespace Server.Items
{
	public class SmoczeKosci : BoneChest
	{
        public override int LabelNumber { get { return 1065817; } } // Smocze Kosci
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance { get { return 15; } }
		public override int BaseFireResistance { get { return 5; } }
		public override int BaseColdResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 5; } }

		[Constructable]
		public SmoczeKosci()
		{
			Hue = 0x497;
			Attributes.BonusHits = 5;
			Attributes.ReflectPhysical = 10;
			ArmorAttributes.MageArmor = 1;
			Attributes.LowerManaCost = 5;
		}

		public SmoczeKosci( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Hue == 0x551 )
			{
				Hue = 0x497;
			}
		}
	}
}
