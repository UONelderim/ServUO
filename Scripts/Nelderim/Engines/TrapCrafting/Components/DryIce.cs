using System;
using Server;

namespace Server.Items
{
	public class DryIce : Item
	{
		[Constructable]
		public DryIce() : this( 1 )
		{
		}

		[Constructable]
        public DryIce(int amount) : base(0xF8F)   // Rare purchase.
		{
			Stackable = true;
			Amount = amount;
			Weight = 2.0;
            Name = "suchy lód";
			Hue = 2120;
		}

		public DryIce( Serial serial ) : base( serial )
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