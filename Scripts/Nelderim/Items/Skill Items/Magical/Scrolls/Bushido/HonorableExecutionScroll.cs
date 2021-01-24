using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class HonorableExecutionScroll : BushidoSpellScroll
	{
		[Constructable]
		public HonorableExecutionScroll() : this( 1 )
		{
		}

		[Constructable]
        public HonorableExecutionScroll(int amount) : base(400, 0x1F71, amount)
		{
            Name = "Honorable Execution";
		}

        public HonorableExecutionScroll(Serial serial) : base(serial)
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