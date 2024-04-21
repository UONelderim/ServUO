using System;
using Server;

namespace Server.Items
{
	public class OchronaZPolnocy : StuddedMempo
	{

        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }
        
        public override int BaseColdResistance { get { return 5; } }
        public override int BaseEnergyResistance { get { return 25; } }
        public override int BasePhysicalResistance { get { return 5; } }
        public override int BasePoisonResistance { get { return 10; } }
        public override int BaseFireResistance { get { return 10; } } 

		[Constructable]
		public OchronaZPolnocy()
		{
			Hue = 2883;
			Name = "Ochrona Z Polnocy";
			ArmorAttributes.MageArmor = 1;
			Attributes.Luck = 300;
			Attributes.LowerRegCost = 23;
			Attributes.BonusStam = 11;
			SkillBonuses.SetValues( 0, SkillName.Provocation, 10.0 );

		}
		
		public OchronaZPolnocy( Serial serial ) : base( serial )
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
		}
	}
}

