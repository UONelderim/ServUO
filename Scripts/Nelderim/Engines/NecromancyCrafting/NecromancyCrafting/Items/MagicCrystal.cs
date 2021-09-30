using System;
using Server;

namespace Server.Items
{
    public class MagicCrystal : Item
    {
        [Constructable]
		public MagicCrystal() : this( 1 )
		{
		}

		[Constructable]
		public MagicCrystal( int amount ) : base( 0x2240 )
		{
            Name = "magiczny krysztal";
			Stackable = false;
            Hue = 0x26F;
			Amount = amount;
		}

        public MagicCrystal(Serial serial)
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