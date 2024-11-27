using System;
using Server;


namespace Server.Items
{
	public class InquisitorsHelm : PlateHelm
	{
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }


		public override int BaseColdResistance{ get{ return 25; } }
		public override int BaseFireResistance{ get{ return -25; } }
		public override int BaseEnergyResistance{ get{ return 7; } }


		[Constructable]
		public InquisitorsHelm()
		{
			Name = "Helm Jarla Frozena";
			Hue = 0x4F2;
			Attributes.CastRecovery = 2;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			ArmorAttributes.MageArmor = 1;
			Attributes.BonusMana = 6;
		}


		public InquisitorsHelm( Serial serial ) : base( serial )
		{
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );


			writer.Write( (int) 1 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );


			int version = reader.ReadInt();


			if ( version < 1 )
			{
				ColdBonus = 0;
				EnergyBonus = 0;
			}
		}
	}
}
