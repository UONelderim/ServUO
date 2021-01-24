using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class EvasionScroll : BushidoSpellScroll
	{
		[Constructable]
		public EvasionScroll() : this( 1 )
		{
		}

		[Constructable]
        public EvasionScroll(int amount) : base(402, 0x1F71, amount)
		{
            Name = "Evasion";
		}

        public EvasionScroll(Serial serial) : base(serial)
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