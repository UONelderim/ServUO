namespace Server.Items
{
	public class PrzekletyArcticDeathDealer : WarMace
	{
		//public override int LabelNumber { get { return 1063481; } } // Pocalunek Lodu
		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public PrzekletyArcticDeathDealer()
		{
			Hue = 2700;
			WeaponAttributes.HitHarm = 66;
			LootType = LootType.Cursed;
			Name = "Przeklety Pocalunek Lodu";
			WeaponAttributes.HitLowerAttack = 40;
			Attributes.WeaponSpeed = 50;
			Attributes.WeaponDamage = 40;
			WeaponAttributes.ResistColdBonus = 20;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			cold = 50;
			phys = 50;

			pois = fire = nrgy = chaos = direct = 0;
		}

		public PrzekletyArcticDeathDealer(Serial serial) : base(serial)
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
