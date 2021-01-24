using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ShadowJumpScroll : NinjitsuSpellScroll
	{
		[Constructable]
		public ShadowJumpScroll() : this( 1 )
		{
		}

		[Constructable]
        public ShadowJumpScroll(int amount) : base(506, 0x1F6F, amount)
		{
            Name = "Shadow Jump";
		}

        public ShadowJumpScroll(Serial serial) : base(serial)
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