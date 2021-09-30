using System;
using Server;

namespace Server.Items
{
    public class WhitePowder : Item
    {
        [Constructable]
		public WhitePowder() : this( 1 )
		{
		}

		[Constructable]
		public WhitePowder( int amount ) : base( 0xF8F )
		{
            Name = "White Powder";
			Stackable = true;
            Hue = 0x7F8;
			Amount = amount;
		}

        public WhitePowder(Serial serial)
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