using System;
using Server;

namespace Server.Items
{
    public class AzulPowder : Item
    {
        [Constructable]
		public AzulPowder() : this( 1 )
		{
		}

		[Constructable]
		public AzulPowder( int amount ) : base( 0xF8F )
		{
            Name = "Blue Powder";
			Stackable = true;
            Hue = 0x4F2;
			Amount = amount;
		}

        public AzulPowder(Serial serial)
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