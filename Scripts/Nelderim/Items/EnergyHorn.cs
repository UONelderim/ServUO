
using System;

namespace Server.Items
{
	public class EnergyHorn : FireHorn
	{
		public override Type ResourceType => typeof(GraveDust);
		public override string MissingResourceName => "grobowego pylu";
		public override int[] DamageValues => [0, 0,0, 0, 100];
		public override int LabelNumber => 3070049;

		[Constructable]
		public EnergyHorn()
		{
		}

		public EnergyHorn(Serial serial) : base(serial)
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
