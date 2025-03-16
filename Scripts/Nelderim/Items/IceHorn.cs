
using System;

namespace Server.Items
{
	public class IceHorn : FireHorn
	{
		public override Type ResourceType => typeof(BlackPearl);
		public override string MissingResourceName => "czarnych perel";
		public override int[] DamageValues => [0, 0, 100, 0, 0];
		public override int LabelNumber => 3070047;

		[Constructable]
		public IceHorn()
		{
		}

		public IceHorn(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			var version = reader.ReadInt();
		}
	}
}
