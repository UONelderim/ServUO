using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RemoveCurseScroll : ChivalrySpellScroll
	{
		[Constructable]
		public RemoveCurseScroll() : this( 1 )
		{
		}

		[Constructable]
        public RemoveCurseScroll(int amount) : base(208, 0x1F6E, amount)
		{
            Name = "Oczyszczenie umyslu";
		}

        public RemoveCurseScroll(Serial serial) : base(serial)
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