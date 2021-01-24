using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class CounterAttackScroll : BushidoSpellScroll
	{
		[Constructable]
		public CounterAttackScroll() : this( 1 )
		{
		}

		[Constructable]
        public CounterAttackScroll(int amount) : base(403, 0x1F72, amount)
		{
            Name = "Counter Attack";
		}

        public CounterAttackScroll(Serial serial) : base(serial)
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