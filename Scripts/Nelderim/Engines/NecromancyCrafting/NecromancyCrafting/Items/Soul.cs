using System;
using Server;

namespace Server.Items
{
    public class Soul : Item
    {
        [Constructable]
		public Soul() : this( 1 )
		{
		}

		[Constructable]
		public Soul( int amount ) : base( 0x2100 )
		{
            Name = "dusza";
			Stackable = false;
            Hue = 1367;
			Amount = amount;
		}

        public Soul(Serial serial)
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