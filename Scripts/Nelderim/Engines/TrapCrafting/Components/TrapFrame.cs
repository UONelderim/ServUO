using System;
using Server;

namespace Server.Items
{
	[Flipable( 0x1053, 0x1054 )]
	public class TrapFrame : Item
	{
		[Constructable]
		public TrapFrame() : this( 1 )
		{
		}

		[Constructable]
        public TrapFrame(int amount) : base(0x2AAA)
		{
			Stackable = true;
			Amount = amount;
			Weight = 2.0;
            Name = "podstawa pułapki";
		}

		public TrapFrame( Serial serial ) : base( serial )
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