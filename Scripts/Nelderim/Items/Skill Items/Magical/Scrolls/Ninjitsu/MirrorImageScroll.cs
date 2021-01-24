using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class MirrorImageScroll : NinjitsuSpellScroll
	{
		[Constructable]
		public MirrorImageScroll() : this( 1 )
		{
		}

		[Constructable]
        public MirrorImageScroll(int amount) : base(507, 0x1F70, amount)
		{
            Name = "Mirror Image";
		}

        public MirrorImageScroll(Serial serial) : base(serial)
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