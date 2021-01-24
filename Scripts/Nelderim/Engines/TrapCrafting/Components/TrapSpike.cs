using System;
using Server;

namespace Server.Items
{
    public class TrapSpike : Item
	{
		[Constructable]
		public TrapSpike() : this( 1 )
		{
		}

		[Constructable]
        public TrapSpike(int amount) : base(0x1BFB)
		{
			Stackable = true;
			Amount = amount;
			Weight = 2.0;
            Name = "kolec do pułapek";
		}

		public TrapSpike( Serial serial ) : base( serial )
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