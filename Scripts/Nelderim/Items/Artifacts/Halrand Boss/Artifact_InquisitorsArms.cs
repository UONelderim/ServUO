using System;
using Server;


namespace Server.Items
{
	public class InquisitorsArms : PlateArms
	{
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }


		public override int BaseColdResistance{ get{ return 12; } }
		public override int BaseEnergyResistance{ get{ return 7; } }


		[Constructable]
		public InquisitorsArms()
		{
			Name = "Naramienniki Inwizytora";
			Hue = 0x4F2;
			Attributes.CastRecovery = 2;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			ArmorAttributes.MageArmor = 1;
			Attributes.BonusMana = 10;
		}



		public InquisitorsArms( Serial serial ) : base( serial )
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
