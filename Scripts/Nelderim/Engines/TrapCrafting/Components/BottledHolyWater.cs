using System;
using Server;

namespace Server.Items
{
    public class BottledHolyWater : Item
	{
		[Constructable]
		public BottledHolyWater() : this( 1 )
		{
		}

		[Constructable]
        public BottledHolyWater(int amount) : base(0xF0E) // Rare Purchase
		{
			Stackable = true;
			Amount = amount;
			Weight = 2.0;
            Name = "bottled holy water";
		}

		public BottledHolyWater( Serial serial ) : base( serial )
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