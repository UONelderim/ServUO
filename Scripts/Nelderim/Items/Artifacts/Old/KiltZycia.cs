using System;
using Server;

namespace Server.Items
{
	public class KiltZycia : ChainLegs
	{
        public override int LabelNumber { get { return 1065809; } } // Kilt Zycia
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance { get { return 8; } }
		public override int BaseFireResistance { get { return 11; } }
		public override int BaseColdResistance { get { return 8; } }
		public override int BasePoisonResistance { get { return 9; } }
		public override int BaseEnergyResistance { get { return 19; } }

		[Constructable]
		public KiltZycia()
		{
			Hue = 0x4EA;
			Attributes.BonusHits = 8;
			Attributes.BonusMana = 3;
			ArmorAttributes.MageArmor = 1;
			Attributes.RegenHits = 4;
			Attributes.RegenMana = 1;
		}
		public KiltZycia( Serial serial ) : base( serial )
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

