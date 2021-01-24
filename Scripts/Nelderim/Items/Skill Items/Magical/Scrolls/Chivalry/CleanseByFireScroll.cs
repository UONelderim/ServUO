using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class CleanseByFireScroll : ChivalrySpellScroll
	{
		[Constructable]
		public CleanseByFireScroll() : this( 1 )
		{
		}

		[Constructable]
        public CleanseByFireScroll(int amount) : base(200, 0x1F6D, amount)
		{
            Name = "Wrzaca krew";
		}

        public CleanseByFireScroll(Serial serial) : base(serial)
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