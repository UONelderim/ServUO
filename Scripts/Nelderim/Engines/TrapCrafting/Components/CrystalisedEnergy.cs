using System;
using Server;

namespace Server.Items
{
	[Flipable( 0x1053, 0x1054 )]
	public class CrystalisedEnergy : Item
	{
		[Constructable]
		public CrystalisedEnergy() : this( 1 )
		{
		}

		[Constructable]
        public CrystalisedEnergy(int amount) : base(0xF26)   //Rare purchase or drops of wisps.
		{
			Stackable = true;
			Amount = amount;
			Weight = 2.0;
            Name = "skrystalizowana energia";
		}

		public CrystalisedEnergy( Serial serial ) : base( serial )
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