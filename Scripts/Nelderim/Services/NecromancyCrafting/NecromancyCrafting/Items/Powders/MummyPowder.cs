using System;
using Server;

namespace Server.Items
{
    public class MummyPowder : Item
    {
        [Constructable]
		public MummyPowder() : this( 1 )
		{
		}

		[Constructable]
		public MummyPowder( int amount ) : base( 0xF8F )
		{
            Name = "Proch mumii";
			Stackable = true;
            Hue = 1161;
			Amount = amount;
		}

        public MummyPowder(Serial serial)
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