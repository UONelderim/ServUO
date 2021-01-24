using System;
using Server;

namespace Server.Items
{
	public class PieknoscMuzykanta : Tambourine
	{
        public override int LabelNumber { get { return 1065813; } } // Pieknosc Muzykanta
        public override int InitMinUses { get { return 300; } }
        public override int InitMaxUses { get { return 300; } }

		[Constructable]
		public PieknoscMuzykanta()
		{
			Hue = 0x47E;
			Slayer = SlayerName.ArachnidDoom;
			Slayer2 = SlayerName.Repond;
		}
		public PieknoscMuzykanta( Serial serial ) : base( serial )
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

