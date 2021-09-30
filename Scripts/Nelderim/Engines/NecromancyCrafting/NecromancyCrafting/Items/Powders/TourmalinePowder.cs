using System;
using Server;

namespace Server.Items
{
    public class TourmalinePowder : Item
    {
        [Constructable]
		public TourmalinePowder() : this( 1 )
		{
		}

		[Constructable]
		public TourmalinePowder( int amount ) : base( 0xF8F )
		{
            Name = "Tourmaline Powder";
			Stackable = true;
            Hue = 0x48F;
			Amount = amount;
		}

        public TourmalinePowder(Serial serial)
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