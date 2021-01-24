using System;
using Server;

namespace Server.Items
{
	[Flipable( 0x1053, 0x1054 )]
	public class TrapCrystalSensor : Item
	{
		[Constructable]
		public TrapCrystalSensor() : this( 1 )
		{
		}

		[Constructable]
        public TrapCrystalSensor(int amount) : base(0x2AAA)
		{
			Stackable = true;
			Amount = amount;
			Weight = 2.0;
            Name = "Kryształowy wykrywacz";
		}

        public TrapCrystalSensor(Serial serial) : base(serial)
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