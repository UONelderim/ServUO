using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class MomentumStrikeScroll : BushidoSpellScroll
	{
		[Constructable]
		public MomentumStrikeScroll() : this( 1 )
		{
		}

		[Constructable]
        public MomentumStrikeScroll(int amount) : base(405, 0x1F72, amount)
		{
            Name = "Momentum Strike";
		}

        public MomentumStrikeScroll(Serial serial) : base(serial)
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