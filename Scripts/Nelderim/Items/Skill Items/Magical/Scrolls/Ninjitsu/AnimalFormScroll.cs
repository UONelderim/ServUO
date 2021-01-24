using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class AnimalFormScroll : NinjitsuSpellScroll
	{
		[Constructable]
		public AnimalFormScroll() : this( 1 )
		{
		}

		[Constructable]
        public AnimalFormScroll(int amount) : base(502, 0x1F6F, amount)
		{
            Name = "Animal Form";
		}

        public AnimalFormScroll(Serial serial) : base(serial)
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