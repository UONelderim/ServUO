namespace Server.Items
{
	public class PrzekletyQuell : Bardiche
	{
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		public override bool CanFortify { get { return true; } }

		[Constructable]
		public PrzekletyQuell()
		{
			Hue = 1180;

			Attributes.SpellChanneling = 1;
			Attributes.WeaponSpeed = 25;
			Attributes.WeaponDamage = 55;
			Attributes.AttackChance = 10;
			LootType = LootType.Cursed;
			Name = "Przeklety Zdlawiony";
			WeaponAttributes.HitLeechMana = 100;
			WeaponAttributes.UseBestSkill = 1;
		}

		public PrzekletyQuell(Serial serial) : base(serial)
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
