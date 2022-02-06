using System;
using Server;

namespace Server.Items
{
    public class RedPowder : Item
    {
        [Constructable]
		public RedPowder() : this( 1 )
		{
		}

		[Constructable]
		public RedPowder( int amount ) : base( 0xF8F )
		{
            Name = "Red Powder";
			Stackable = true;
            Hue = 0x494;
			Amount = amount;
		}

        public RedPowder(Serial serial)
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