using Server.Mobiles;


namespace Server.Items
{
	public class SoulRipper : Katana
	{
		public override int InitMinHits => 50;

		public override int InitMaxHits => 50;

		[Constructable]
		public SoulRipper()
		{
			Name = "Wysysacz Dusz";
			Hue = 1152;
			Weight = 5.0;
			
			Slayer = SlayerName.Silver;
			Attributes.WeaponDamage = 50;
			Attributes.WeaponSpeed = 20;
			WeaponAttributes.HitLeechMana = 50;
			WeaponAttributes.HitHarm = 30;
			AosElementDamages.Energy = 100;
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add("Z kazdym Twym uderzeniem, Twa dusza slabnie, jesli nie jestes morderca");
		}

		public override void OnHit(Mobile attacker, IDamageable defender, double damageBonus)
		{
			base.OnHit(attacker, defender, damageBonus);

			if (attacker is PlayerMobile pm && attacker.Kills < 5)
			{
				pm.Damage(8);
				pm.FixedParticles(0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot);
				pm.PlaySound(0x208);
				pm.SendMessage("Twoja dusza cierpi czarny pomiocie!");
			}
		}

		public SoulRipper(Serial serial) : base(serial)
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
		}
	}
}
