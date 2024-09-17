namespace Server.Items
{
	public class KulawyMagik : BoneHarvester
	{
		public override int LabelNumber { get { return 1065852; } } // Kulawy Magik
		public override int InitMinHits => 255;
		public override int InitMaxHits => 255;

		[Constructable]
		public KulawyMagik()
		{
			Hue = 0x558;

			Attributes.SpellChanneling = 1;
			WeaponAttributes.MageWeapon = 25;
			Attributes.SpellDamage = 8;
			Attributes.WeaponDamage = 25;
			Attributes.DefendChance = 12;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois,
			out int nrgy, out int chaos, out int direct)
		{
			phys = 60;
			fire = 0;
			cold = 0;
			pois = 40;
			nrgy = 0;
			chaos = 0;
			direct = 0;
		}

		public KulawyMagik(Serial serial)
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

			if (Attributes.WeaponDamage != 25)
				Attributes.WeaponDamage = 25;
		}
	}
}
