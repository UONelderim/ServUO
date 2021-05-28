namespace Server.Items
{
	public class PrzekletyFleshRipper : AssassinSpike
	{
		// public override int LabelNumber { get { return 1075045; } } // Wypruwacz Flakow
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public PrzekletyFleshRipper()
		{
			Hue = 2700;
			LootType = LootType.Cursed;
			SkillBonuses.SetValues(0, SkillName.Anatomy, 30.0);
			Name = "Przeklety Wypruwacz Flakow";
			Attributes.BonusStr = 5;
			Attributes.AttackChance = 25;
			Attributes.WeaponSpeed = 45;

			WeaponAttributes.UseBestSkill = 1;
		}

		public PrzekletyFleshRipper(Serial serial) : base(serial)
		{
		}

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
