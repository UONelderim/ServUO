using System;

namespace Server.Items.Crops
{

	
	public class NobleTobaccoSapling : BaseSeedling
    {
        public override Type PlantType => typeof(NobleTobaccoPlant);

        [Constructable]
		public NobleTobaccoSapling( int amount ) : base( amount, 0x0CB6) 
		{
			Hue = 2126;
			Name = "Szczepka tytoniu szlachetnego";
			Stackable = true;
		}

		[Constructable]
		public NobleTobaccoSapling() : this( 1 )
		{
		}

		public NobleTobaccoSapling( Serial serial ) : base( serial )
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
	
	public class NobleTobaccoPlant : Plant
    {
        public override Type SeedType => typeof(NobleTobaccoSapling);
        public override Type CropType => typeof(NobleTobaccoCrop);
		protected override int YoungPlantGraphics => 0x0C97;
		protected override int MaturePlantGraphics => 0x0C97;

		[Constructable] 
		public NobleTobaccoPlant() : base(0x0C97)
		{
			Hue = 2126;
			Name = "Tyton szlachetny";
			Stackable = true;
		}

		public NobleTobaccoPlant( Serial serial ) : base( serial ) 
		{ 
			//m_plantedTime = DateTime.Now;	// ???
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
	
	public class NobleTobaccoCrop : Crop
    {
        public override Type ReagentType => typeof(NobleTobacco);
		
		[Constructable]
		public NobleTobaccoCrop( int amount ) : base( amount, 0x0C93)
		{
			Hue = 2126;
			Name = "Swieze liscie tytoniu szlachetnego";
			Stackable = true;
		}

		[Constructable]
		public NobleTobaccoCrop() : this( 1 )
		{
		}

		public NobleTobaccoCrop( Serial serial ) : base( serial )
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