using System;
using Server;

namespace Server.Items
{
    public class ClearPowder : Item
    {
        [Constructable]
		public ClearPowder() : this( 1 )
		{
		}

		[Constructable]
		public ClearPowder( int amount ) : base( 0xF8F )
		{
            Name = "Clear Powder";
			Stackable = true;
            Hue = 0x7F8;
			Amount = amount;
		}

        public ClearPowder(Serial serial)
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