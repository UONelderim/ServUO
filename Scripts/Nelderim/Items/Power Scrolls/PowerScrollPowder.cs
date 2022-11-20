using System;
using Server;

namespace Server
{
	public class PowerScrollPowder : Item
	{
		public override double DefaultWeight => 0.01;

		[Constructable]
		public PowerScrollPowder() : this(1)
		{
		}

		[Constructable]
		public PowerScrollPowder(int amount) : base(0x26B8)
		{
			Name = "Pyl mocy";
			Hue = 118;
			Stackable = true;
			Amount = amount;
		}

		public PowerScrollPowder(Serial serial) : base(serial)
		{
		}
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
