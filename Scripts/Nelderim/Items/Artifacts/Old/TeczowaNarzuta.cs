using System;
using Server;

namespace Server.Items
{
	public class TeczowaNarzuta : RingmailArms
	{

        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance { get { return 5; } }
		public override int BaseFireResistance { get { return 8; } }
		public override int BaseColdResistance { get { return 8; } }
		public override int BasePoisonResistance { get { return 9; } }
		public override int BaseEnergyResistance { get { return 12; } }

		[Constructable]
		public TeczowaNarzuta()
		{
			Hue = 0xBA0;
			Name = "Ogniste Naramienniki";
			ArmorAttributes.MageArmor = 1;
			Attributes.Luck = 140;
			Attributes.LowerRegCost = 10;
			Attributes.ReflectPhysical = 10;
			Attributes.RegenMana = 1;
		}
		public TeczowaNarzuta( Serial serial ) : base( serial )
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
		}
	}
}

