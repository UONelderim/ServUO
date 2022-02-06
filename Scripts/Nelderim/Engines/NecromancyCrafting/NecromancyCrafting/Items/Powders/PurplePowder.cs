using System;
using Server;

namespace Server.Items
{
    public class PurplePowder : Item
    {
        [Constructable]
		public PurplePowder() : this( 1 )
		{
		}

		[Constructable]
		public PurplePowder( int amount ) : base( 0xF8F )
		{
            Name = "Purple Powder";
			Stackable = true;
            Hue = 0x503;
			Amount = amount;
		}

        public PurplePowder(Serial serial)
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