using System;
using Server;

namespace Server.Items
{
	public class TrapCrystalTrigger : Item
	{
		[Constructable]
		public TrapCrystalTrigger() : this( 1 )
		{
		}

		[Constructable]
        public TrapCrystalTrigger(int amount) : base(0x1EA7)
		{
			Stackable = true;
			Amount = amount;
			Weight = 2.0;
            Name = "Kryształ do pułapki";
		}

		public TrapCrystalTrigger( Serial serial ) : base( serial )
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