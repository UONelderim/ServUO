using System;
using Server;

namespace Server.Items
{
    public class SkeletonPowder : Item
    {
        [Constructable]
		public SkeletonPowder() : this( 1 )
		{
		}

		[Constructable]
		public SkeletonPowder( int amount ) : base( 0xF8F )
		{
            Name = "Proch szkieleta";
			Stackable = true;
            Hue = 1293;
			Amount = amount;
		}

        public SkeletonPowder(Serial serial)
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
