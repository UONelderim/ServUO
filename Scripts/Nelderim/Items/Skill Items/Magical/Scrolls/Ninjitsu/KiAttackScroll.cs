using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class KiAttackScroll : NinjitsuSpellScroll
	{
		[Constructable]
		public KiAttackScroll() : this( 1 )
		{
		}

		[Constructable]
        public KiAttackScroll(int amount) : base(503, 0x1F70, amount)
		{
            Name = "Ki Attack";
		}

        public KiAttackScroll(Serial serial) : base(serial)
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