using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class FocusAttackScroll : NinjitsuSpellScroll
	{
		[Constructable]
		public FocusAttackScroll() : this( 1 )
		{
		}

		[Constructable]
        public FocusAttackScroll(int amount) : base(500, 0x1F6F, amount)
		{
            Name = "Focus Attack";
		}

        public FocusAttackScroll(Serial serial) : base(serial)
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