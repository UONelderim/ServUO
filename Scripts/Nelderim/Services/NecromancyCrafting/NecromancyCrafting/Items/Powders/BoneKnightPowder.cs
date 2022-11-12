using System;
using Server;

namespace Server.Items
{
    public class BoneKnightPowder : Item
    {
        [Constructable]
		public BoneKnightPowder() : this( 1 )
		{
		}

		[Constructable]
		public BoneKnightPowder( int amount ) : base( 0xF8F )
		{
            Name = "Proch kościanego rycerza";
			Stackable = true;
            Hue = 2874;
			Amount = amount;
		}

        public BoneKnightPowder(Serial serial)
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