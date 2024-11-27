using System;
using Server;


namespace Server.Items
{
	public class LodowaPrzeslona : BronzeShield
	{
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		public override int BasePhysicalResistance{ get{ return 25; } }
		public override int BaseFireResistance{ get{ return -4; } }
		public override int BaseColdResistance{ get{ return -16; } }
		public override int BaseEnergyResistance{ get{ return -5; } }

		[Constructable]
		public LodowaPrzeslona()
		{
			Hue = 1151;
			Name = "Lodowa Przeslona";
			SkillBonuses.SetValues( 0, SkillName.Parry, 15 );
			ArmorAttributes.LowerStatReq = 10;
			Attributes.DefendChance = 15;
			Attributes.BonusStr = 5;
			Attributes.Luck = 100;
		}


		public LodowaPrzeslona( Serial serial ) : base( serial )
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
