using System;

namespace Server.Items.Crops
{
	public class SzczepkaKapusta : VegetableSeedling
    {
        public override Type PlantType => typeof(KrzakKapusta);

        [Constructable]
		public SzczepkaKapusta( int amount ) : base( amount, 0xF27) 
		{
			Hue = 0x5E2;
			Name = "Nasiona kapusty";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaKapusta() : this( 1 )
		{
		}

		public SzczepkaKapusta( Serial serial ) : base( serial )
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
	
	public class KrzakKapusta : VegetablePlant
    {
        public override Type SeedType => typeof(SzczepkaKapusta);
        public override Type CropType => typeof(Cabbage);
		protected override int YoungPlantGraphics => 0x0C61;
		protected override int MaturePlantGraphics => Utility.RandomList(0xC7C, 0x0C7B);

		[Constructable] 
		public KrzakKapusta() : base(0x0C61)
		{
			// seedling -
			//plant.PickGraphic = (0xC61); 'turnip', wiekszy niz salata
			//plant.FullGraphic = (0xC7C); + 0x0C7B
			Hue = 0;
			Name = "Kapusta";
			Stackable = true;
        }

		public KrzakKapusta( Serial serial ) : base( serial ) 
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