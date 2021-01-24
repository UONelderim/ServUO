using System;
using Server;

namespace Server.Items
{
	public class StudniaOdnowy : GoldRing
	{

        public override int InitMinHits { get { return 45; } }
        public override int InitMaxHits { get { return 45; } }

		[Constructable]
		public StudniaOdnowy()
		{
			Name = "Pierścień Odnowy";
			Hue = 0x4FD;
			Attributes.RegenMana = 8;
			Attributes.RegenHits = 8;
			Attributes.RegenStam = 8;


		}

		public StudniaOdnowy( Serial serial ) : base( serial )
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