using System;
using Server;


namespace Server.Items
{
	public class ToporLodowegoWladcy : LargeBattleAxe
	{
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }


		[Constructable]
		public ToporLodowegoWladcy()
		{
			Hue = 1151;
			Name = "Topor Lodowego Wladcy";
			SkillBonuses.SetValues( 0, SkillName.Swords, 15 );
			Attributes.DefendChance = 25;
			Attributes.WeaponDamage = 40;
			WeaponAttributes.UseBestSkill = 1;
			WeaponAttributes.HitColdArea = 100;
			WeaponAttributes.HitLeechMana = 20;
		}

		public ToporLodowegoWladcy( Serial serial ) : base( serial )
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
