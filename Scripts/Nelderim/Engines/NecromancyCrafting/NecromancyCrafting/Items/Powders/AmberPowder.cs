using System;
using Server;

namespace Server.Items
{
    public class AmberPowder : Item
    {
        [Constructable]
		public AmberPowder() : this( 1 )
		{
		}

		[Constructable]
		public AmberPowder( int amount ) : base( 0xF8F )
		{
            Name = "Amber Powder";
			Stackable = true;
            Hue = 0x496;
			Amount = amount;
		}

        public AmberPowder(Serial serial)
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