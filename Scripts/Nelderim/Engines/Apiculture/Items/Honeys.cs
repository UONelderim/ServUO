namespace Server.Items
{
	public class JarHoneyGryczany : JarHoney
	{
		[Constructable]
		public JarHoneyGryczany()
		{
			Weight = 1.0;
			Stackable = true;
			Name = "Sloik miodu gryczanego";
			Hue = 1126;
		}

		public JarHoneyGryczany(Serial serial)
			: base(serial)
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
			Stackable = true;
		}
	}

	public class JarHoneyLesny : JarHoney
	{
		[Constructable]
		public JarHoneyLesny()
		{
			Weight = 1.0;
			Stackable = true;
			Name = "Sloik miodu lesnego";
			Hue = 2126;
		}

		public JarHoneyLesny(Serial serial)
			: base(serial)
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
			Stackable = true;
		}
	}

	public class JarHoneySpadziowy : JarHoney
	{
		[Constructable]
		public JarHoneySpadziowy()
		{
			Weight = 1.0;
			Stackable = true;
			Name = "Sloik miodu spadziowego";
			Hue = 1214;
		}

		public JarHoneySpadziowy(Serial serial)
			: base(serial)
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
			Stackable = true;
		}
	}

	public class JarHoneyNostrzykowy : JarHoney
	{
		[Constructable]
		public JarHoneyNostrzykowy()
		{
			Weight = 1.0;
			Stackable = true;
			Name = "Sloik miodu nostrzykowy";
			Hue = 1105;
		}

		public JarHoneyNostrzykowy(Serial serial)
			: base(serial)
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
			Stackable = true;
		}
	}
}
