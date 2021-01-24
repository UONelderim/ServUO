using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ConfidenceScroll : BushidoSpellScroll
	{
		[Constructable]
		public ConfidenceScroll() : this( 1 )
		{
		}

		[Constructable]
        public ConfidenceScroll(int amount) : base(401, 0x1F72, amount)
		{
            Name = "Confidence";
		}

        public ConfidenceScroll(Serial serial) : base(serial)
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