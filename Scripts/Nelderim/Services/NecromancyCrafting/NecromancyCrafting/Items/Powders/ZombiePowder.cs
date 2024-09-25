using System;
using Server;

namespace Server.Items
{
    public class ZombiePowder : Item
    {
        [Constructable]
		public ZombiePowder() : this( 1 )
		{
		}

		[Constructable]
		public ZombiePowder( int amount ) : base( 0xF8F )
		{
            Name = "Proch zombie";
			Stackable = true;
            Hue = 2280;
			Amount = amount;
		}

        public ZombiePowder(Serial serial)
            : base(serial)
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
