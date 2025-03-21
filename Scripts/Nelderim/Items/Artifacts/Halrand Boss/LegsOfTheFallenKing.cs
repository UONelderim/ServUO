using System;
using Server;


namespace Server.Items
{
	public class LegsOfTheFallenKing : LeatherLegs
	{
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }
		
		public override int LabelNumber => 3070053;//Nogawnice Martwego Wladcy

		public override int BaseColdResistance{ get{ return 17; } }
		public override int BaseEnergyResistance{ get{ return 17; } }
		public override int BaseFireResistance{ get{ return 11; } }
		public override int BasePoisonResistance{ get{ return 12; } }


		[Constructable]
		public LegsOfTheFallenKing()
		{
			Name = "Nogawnice Martwego Wladcy";
			Hue = 0x76D;
			Attributes.BonusStr = 5;
			Attributes.RegenHits = 15;
			Attributes.RegenStam = 5;
			AbsorptionAttributes.EaterKinetic = 10;
		}



		public LegsOfTheFallenKing( Serial serial ) : base( serial )
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
				if ( Hue == 0x551 )
					Hue = 0x76D;


				ColdBonus = 0;
				EnergyBonus = 0;
			}
		}
	}
}
