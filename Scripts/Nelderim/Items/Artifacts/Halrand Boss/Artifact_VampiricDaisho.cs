using System;
using Server.Network;
using Server.Items;
using Server.Targeting;


namespace Server.Items
{
	public class VampiricBladedWhip : BladedWhip
	{
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }
		
		public override int LabelNumber => 3070055;//Wampiryczny Dotyk


      [Constructable]
		public VampiricBladedWhip()
		{
			Name = "Wampiryczny Dotyk";
			Hue = 27;
			WeaponAttributes.HitHarm = 50;
			Attributes.WeaponDamage = 25;
			WeaponAttributes.HitManaDrain = 30;
			Attributes.LowerManaCost = 5;
			Slayer = SlayerName.Fey ;
		}
		


		public VampiricBladedWhip( Serial serial ) : base( serial )
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
