using System;
using Server;


namespace Server.Items
{
	public class WloczniaNieudacznika : Spear
	{
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }


		[Constructable]
		public WloczniaNieudacznika()
		{
			Hue = 2631;
			Name = "Wlocznia Nieudacznika";
			SkillBonuses.SetValues( 0, SkillName.Fencing, 5 );
			SkillBonuses.SetValues( 0, SkillName.Hiding, 5 );
			Attributes.AttackChance = -10;
			Attributes.DefendChance = 35;
			Attributes.WeaponDamage = 50;
			WeaponAttributes.SplinteringWeapon = 25;
			WeaponAttributes.BloodDrinker = 10;

		}

		public WloczniaNieudacznika( Serial serial ) : base( serial )
		{
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}


		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
