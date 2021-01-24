using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class SurpriseAttackScroll : NinjitsuSpellScroll
	{
		[Constructable]
		public SurpriseAttackScroll() : this( 1 )
		{
		}

		[Constructable]
        public SurpriseAttackScroll(int amount) : base(504, 0x1F6F, amount)
		{
            Name = "Surprise Attack";
		}

        public SurpriseAttackScroll(Serial serial) : base(serial)
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