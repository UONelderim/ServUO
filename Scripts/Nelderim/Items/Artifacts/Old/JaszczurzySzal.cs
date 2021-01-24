using System;
using Server;

namespace Server.Items
{
	public class JaszczurzySzal : HammerPick
	{
        public override int LabelNumber { get { return 1065808; } } // Jaszczurzy Szal
        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public JaszczurzySzal()
		{
			Hue = 0x783;
			Slayer = SlayerName.ReptilianDeath;
			Attributes.WeaponDamage = 40;
			WeaponAttributes.UseBestSkill = 1;
			Attributes.AttackChance = 10;
			Attributes.WeaponSpeed = 15;
		}

		public JaszczurzySzal( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Slayer == SlayerName.None )
			{
				Slayer = SlayerName.DragonSlaying;
			}
		}
	}
}