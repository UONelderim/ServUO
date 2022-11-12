using System;
using Server;

namespace Server.Items
{
    public class AncientLichPowder : Item
    {
        [Constructable]
		public AncientLichPowder() : this( 1 )
		{
		}

		[Constructable]
		public AncientLichPowder( int amount ) : base( 0xF8F )
		{
            Name = "Proch straożytnego licza";
			Stackable = true;
            Hue = 2903;
			Amount = amount;
		}

        public AncientLichPowder(Serial serial)
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