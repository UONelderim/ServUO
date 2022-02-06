using System;
using Server;

namespace Server.Items
{
    public class GreenPowder : Item
    {
        [Constructable]
		public GreenPowder() : this( 1 )
		{
		}

		[Constructable]
		public GreenPowder( int amount ) : base( 0xF8F )
		{
            Name = "Green Powder";
			Stackable = true;
            Hue = 0x506;
			Amount = amount;
		}

        public GreenPowder(Serial serial)
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