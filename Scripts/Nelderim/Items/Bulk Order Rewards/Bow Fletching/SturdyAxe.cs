using System;
using Server;
using Server.Engines.Harvest;

namespace Server.Items
{
	public class SturdyAxe : Axe
	{
		public override int LabelNumber{ get{ return 1046483; } } // wzmocniona siekiera

		[Constructable]
		public SturdyAxe() : this( 100 )
		{
		}

		[Constructable]
		public SturdyAxe( int uses ) : base()
		{
			Weight = 11.0;
			Hue = 0x973;
			UsesRemaining = uses;
			ShowUsesRemaining = true;
		}

		public SturdyAxe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}