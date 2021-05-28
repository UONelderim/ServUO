namespace Server.Items
{
	public class PrzekletaArcaneShield : WoodenKiteShield
	{
		//public override int LabelNumber { get { return 1061101; } } // Arkana Obronne
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public PrzekletaArcaneShield()
		{
			ItemID = 0x1B78;
			Hue = 2700;
			Name = "Przeklete Arkana Obronne";
			Attributes.NightSight = 1;
			Attributes.SpellChanneling = 1;
			Attributes.DefendChance = 25;
			Attributes.CastSpeed = 2;
			LootType = LootType.Cursed;
		}

		public PrzekletaArcaneShield(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
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
