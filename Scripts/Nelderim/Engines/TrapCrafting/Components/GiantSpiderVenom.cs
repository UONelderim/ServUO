using System;
using Server;

namespace Server.Items
{
	[Flipable( 0x1053, 0x1054 )]
	public class GiantSpiderVenom : Item
	{
		[Constructable]
		public GiantSpiderVenom() : this( 1 )
		{
		}

		[Constructable]
        public GiantSpiderVenom(int amount) : base(0xF8E)   // Drops off  giant spiders.
		{
			Stackable = true;
			Amount = amount;
			Weight = 2.0;
            Name = "a giant spider venom sac";
		}

		public GiantSpiderVenom( Serial serial ) : base( serial )
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