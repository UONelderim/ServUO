using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class SacredJourneyScroll : ChivalrySpellScroll
	{
		[Constructable]
		public SacredJourneyScroll() : this( 1 )
		{
		}

		[Constructable]
        public SacredJourneyScroll(int amount) : base(209, 0x1F6D, amount)
		{
            Name = "Lotem ptaka";
		}

        public SacredJourneyScroll(Serial serial) : base(serial)
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