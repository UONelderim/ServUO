using System;
using Server;

namespace Server.Items
{
    public class BoneMagiPowder : Item
    {
        [Constructable]
		public BoneMagiPowder() : this( 1 )
		{
		}

		[Constructable]
		public BoneMagiPowder( int amount ) : base( 0xF8F )
		{
            Name = "Proch kościanego maga";
			Stackable = true;
            Hue = 1172;
			Amount = amount;
		}

        public BoneMagiPowder(Serial serial)
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