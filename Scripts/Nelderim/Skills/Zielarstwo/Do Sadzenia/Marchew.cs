using System;

namespace Server.Items.Crops
{
	public class SzczepkaMarchew : VegetableSeedling
    {
        public override Type PlantType => typeof(KrzakMarchew);

        [Constructable]
		public SzczepkaMarchew( int amount ) : base( amount, 0xF27) 
		{
			Hue = 0x5E2;
			Name = "Nasiona marchwi";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaMarchew() : this( 1 )
		{
		}

		public SzczepkaMarchew( Serial serial ) : base( serial )
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
	
	public class KrzakMarchew : VegetablePlant
    {
        public override Type SeedType => typeof(SzczepkaMarchew);
        public override Type CropType => typeof(Carrot);
		protected override int YoungPlantGraphics => 0xC69;
		protected override int MaturePlantGraphics => 0xC76;

		[Constructable] 
		public KrzakMarchew() : base(0xC69)
		{
			// seedling 0xC68
			//plant.PickGraphic = (0xC69);
			//plant.FullGraphic = (0xC76);
			Hue = 0;
			Name = "marchew";
			Stackable = true;
        }

		public KrzakMarchew( Serial serial ) : base( serial ) 
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