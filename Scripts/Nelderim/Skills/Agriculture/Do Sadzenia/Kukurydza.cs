using System;

namespace Server.Items.Crops
{
	public class SzczepkaKukurydza : VegetableSeedling
    {
        public override Type PlantType => typeof(KrzakKukurydza);

        [Constructable]
		public SzczepkaKukurydza( int amount ) : base( amount, 0xF27) 
		{
			Hue = 0x5E2;
			Name = "Nasiona kukurydzy";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaKukurydza() : this( 1 )
		{
		}

		public SzczepkaKukurydza( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	
	public class KrzakKukurydza : VegetablePlant
    {
        public override Type SeedType => typeof(SzczepkaKukurydza);
        public override Type CropType => typeof(EarOfCorn);
		protected override int YoungPlantGraphics => 0xC7E;
		protected override int MaturePlantGraphics => 0xC7D;

		[Constructable] 
		public KrzakKukurydza() : base(0xC7E)
		{
			// seedling -
			//plant.PickGraphic = (0xC7E);
			//plant.FullGraphic = (0xC7D);
			Hue = 0;
			Name = "kukurydza";
			Stackable = true;
        }

		public KrzakKukurydza( Serial serial ) : base( serial ) 
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
