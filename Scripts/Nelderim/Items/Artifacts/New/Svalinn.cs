namespace Server.Items
{
	public class Svalinn : Buckler
	{
		public override int LabelNumber { get { return 1065851; } } // Svalinn
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public Svalinn()
		{
			Hue = 0x558; // dark green

			Attributes.SpellChanneling = 1;
			Attributes.CastRecovery = 2;
			Attributes.AttackChance = 10;
		}

		public Svalinn(Serial serial)
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

			if (Attributes.NightSight == 0)
				Attributes.NightSight = 1;
		}
	}
}
