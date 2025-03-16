
using System;

namespace Server.Items
{
	public class PoisonHorn : FireHorn
	{
		public override Type ResourceType => typeof(NoxCrystal);
		public override string MissingResourceName => "krysztalu trucizny";
		public override int[] DamageValues => [0, 0,0, 100, 0];
		public override int LabelNumber => 3070048;

		[Constructable]
		public PoisonHorn()
		{
		}

		public PoisonHorn(Serial serial) : base(serial)
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
