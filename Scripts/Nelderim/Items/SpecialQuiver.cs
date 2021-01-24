namespace Server.Items
{
	public class SpecialQuiver : ElvenQuiver
	{
		public override int LabelNumber { get { return 1032657; } } // elven quiver

		[Constructable]
		public SpecialQuiver() : base()
		{
			Hue = 0x4E7; // zmienic
		}

		public SpecialQuiver(Serial serial) : base(serial)
		{
		}
		/*
		public override void AlterBowDamage( ref int phys, ref int fire, ref int cold, ref int pois, ref int nrgy, ref int chaos, ref int direct )
		{
			cold = pois = nrgy = chaos = direct = 0;
			phys = fire = 50;
		}*/

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}
	}
}
