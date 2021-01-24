using System;
using Server;

namespace Server.Items
{
	[Flipable( 0x1053, 0x1054 )]
	public class GazerEye : Item
	{
		[Constructable]
		public GazerEye() : this( 1 )
		{
		}

		[Constructable]
        public GazerEye(int amount) : base(0xF21)   // Drops off gazers.
		{
			Stackable = true;
			Amount = amount;
			Weight = 2.0;
            Name = "a gazer eye";
		}

		public GazerEye( Serial serial ) : base( serial )
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