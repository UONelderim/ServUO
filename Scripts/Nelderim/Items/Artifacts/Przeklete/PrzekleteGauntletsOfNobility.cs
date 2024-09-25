namespace Server.Items
{
	public class PrzekleteGauntletsOfNobility : RingmailGloves
	{
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		public override int BasePhysicalResistance { get { return 18; } }
		public override int BasePoisonResistance { get { return 25; } }

		[Constructable]
		public PrzekleteGauntletsOfNobility()
		{
			Hue = 1180;
			Attributes.BonusStr = 18;
			Attributes.Luck = 120;
			Attributes.WeaponDamage = 20;
			LootType = LootType.Cursed;
			Name = "Przeklete Rekawice Szlachectwa";
		}

		public PrzekleteGauntletsOfNobility(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if (version < 1)
			{
				PhysicalBonus = 0;
				PoisonBonus = 0;
			}
		}
	}
}
