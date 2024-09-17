namespace Server.Items
{
	public class NaramiennikiStrazyObywatelskiej : RingmailArms
	{
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		public override int BasePhysicalResistance { get { return 10; } }
		public override int BaseFireResistance { get { return 10; } }
		public override int BaseColdResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 10; } }
		public override int BaseEnergyResistance { get { return 10; } }

		[Constructable]
		public NaramiennikiStrazyObywatelskiej()
		{
			Hue = 921;
			Name = "Naramienniki Strazy Obywatelskiej";
			Attributes.RegenHits = 10;
			Attributes.ReflectPhysical = 40;
		}

		public NaramiennikiStrazyObywatelskiej(Serial serial)
			: base(serial)
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

			int version = reader.ReadInt();
		}
	}
}
