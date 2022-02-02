namespace Server.Items
{
	public class ZgubaSoteriosa : SilverRing
	{
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }


		[Constructable]
		public ZgubaSoteriosa()
		{
			Hue = 38;
			Name = "Zguba Soteriosa";
			Attributes.DefendChance = 10;
			Attributes.DefendChance = 10;
			Attributes.WeaponDamage = 15;
			SkillBonuses.SetValues(0, SkillName.Bushido, 10);
			Resistances.Poison = 15;
		}

		public ZgubaSoteriosa(Serial serial)
			: base(serial)
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
		}
	}
}
