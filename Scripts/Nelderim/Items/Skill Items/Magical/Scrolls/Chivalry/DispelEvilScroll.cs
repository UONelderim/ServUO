using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class DispelEvilScroll : ChivalrySpellScroll
	{
		[Constructable]
		public DispelEvilScroll() : this( 1 )
		{
		}

		[Constructable]
        public DispelEvilScroll(int amount) : base(203, 0x1F6D, amount)
		{
            Name = "Przeciwna wola";
		}

        public DispelEvilScroll(Serial serial) : base(serial)
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