using System;
using Server;

namespace Server.Items
{
    public class BonerPowder : Item
    {
        [Constructable]
		public BonerPowder() : this( 1 )
		{
		}

		[Constructable]
		public BonerPowder( int amount ) : base( 0xF8F )
		{
            Name = "Proch kościeja";
			Stackable = true;
            Hue = 38;
			Amount = amount;
		}

        public BonerPowder(Serial serial)
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