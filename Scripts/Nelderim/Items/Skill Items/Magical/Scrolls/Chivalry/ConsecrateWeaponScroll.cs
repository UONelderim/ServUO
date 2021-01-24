using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ConsecrateWeaponScroll : ChivalrySpellScroll
	{
		[Constructable]
		public ConsecrateWeaponScroll() : this( 1 )
		{
		}

		[Constructable]
        public ConsecrateWeaponScroll(int amount) : base(202, 0x1F6D, amount)
		{
            Name = "Jednosc z orezem";
		}

        public ConsecrateWeaponScroll(Serial serial) : base(serial)
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