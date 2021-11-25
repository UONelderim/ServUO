using System;
using Server;

namespace Server.Items
{
	public class SzponySzalenstwa : StuddedGloves
	{
        public override int LabelNumber { get { return 1065820; } } // Szpony Szalenstwa
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 12; } }
		public override int BaseColdResistance { get { return 7; } }
		public override int BasePoisonResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 10; } }

		[Constructable]
		public SzponySzalenstwa()
		{
			Hue = 0xA8E;
			Attributes.LowerManaCost = 2;
			Attributes.RegenStam = 3;
			Attributes.AttackChance = 5;
			Attributes.DefendChance = 5;
		}
		public SzponySzalenstwa( Serial serial ) : base( serial )
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

