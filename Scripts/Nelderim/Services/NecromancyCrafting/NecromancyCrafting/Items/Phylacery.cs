using System;
using Server;

namespace Server.Items
{
    public class Phylacery : Item
    {
        [Constructable]
		public Phylacery() : this( 1 )
		{
		}

		[Constructable]
		public Phylacery( int amount ) : base( 0x193F )
		{
            Name = "filakterium";
			Stackable = false;
            Hue = 0x0;
			Amount = amount;
		}

        public Phylacery(Serial serial)
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