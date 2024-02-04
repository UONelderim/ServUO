using System;

namespace Server.Items.Crops
{
	public class SzczepkaCebula : VegetableSeedling
    {
        public override Type PlantType => typeof(KrzakCebula);

        [Constructable]
		public SzczepkaCebula( int amount ) : base( amount, 0xF27) 
		{
			Hue = 0x5E2;
			Name = "Sadzonka cebuli";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaCebula() : this( 1 )
		{
		}

		public SzczepkaCebula( Serial serial ) : base( serial )
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
	
	public class KrzakCebula : VegetablePlant
    {
        public override Type SeedType => typeof(SzczepkaCebula);
        public override Type CropType => typeof(Onion);
		protected override int YoungPlantGraphics => 0xC69;
		protected override int MaturePlantGraphics => 0xC6F;

		[Constructable] 
		public KrzakCebula() : base(0xC69)
		{
			// seedling 0xC68
			//plant.PickGraphic = (0xC69);
			//plant.FullGraphic = (0xC6F);
			Hue = 0;
			Name = "Cebula";
			Stackable = true;
        }

		public KrzakCebula( Serial serial ) : base( serial ) 
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