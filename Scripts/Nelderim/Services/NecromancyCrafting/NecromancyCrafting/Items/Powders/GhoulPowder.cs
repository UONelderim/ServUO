using System;
using Server;

namespace Server.Items
{
    public class GhoulPowder : Item
    {
        [Constructable]
		public GhoulPowder() : this( 1 )
		{
		}

		[Constructable]
		public GhoulPowder( int amount ) : base( 0xF8F )
		{
            Name = "Proch ghoula";
			Stackable = true;
            Hue = 1167;
			Amount = amount;
		}

        public GhoulPowder(Serial serial)
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