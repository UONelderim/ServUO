using System;
using Server;

namespace Server.Items
{
	public class ZnienawidzonyPrzezSmoki : TambourineTassel
	{
        public override int LabelNumber { get { return 1065823; } } // Znienawidzony Przez Smoki
        public override int InitMinUses { get { return 300; } }
        public override int InitMaxUses { get { return 300; } }

		[Constructable]
		public ZnienawidzonyPrzezSmoki()
		{
			Hue = 0x47F;
			Slayer = SlayerName.ReptilianDeath;
			Slayer2 = SlayerName.ElementalBan;
		}
		public ZnienawidzonyPrzezSmoki( Serial serial ) : base( serial )
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

