using System;

namespace Server.Items.Crops
{

	public class SwampTobaccoSapling : BaseSeedling
    {
		public override bool CanGrowFurrows { get { return false; } }
		public override bool CanGrowGrass { get { return false; } }
		public override bool CanGrowForest { get { return false; } }
		public override bool CanGrowJungle { get { return false; } }
		public override bool CanGrowCave { get { return false; } }
		public override bool CanGrowSand { get { return false; } }
		public override bool CanGrowSnow { get { return false; } }
		public override bool CanGrowSwamp { get { return true; } }
		public override bool CanGrowGarden { get { return false; } }

		public override Type PlantType => typeof(SwampTobaccoPlant);

        [Constructable]
		public SwampTobaccoSapling( int amount ) : base( amount, 0x0CB5) 
		{
			Hue = 73;
			Name = "Szczepka bagiennego ziela";
			Stackable = true;
		}

		[Constructable]
		public SwampTobaccoSapling() : this( 1 )
		{
		}

		public SwampTobaccoSapling( Serial serial ) : base( serial )
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
	
	public class SwampTobaccoPlant : Plant
    {
        public override Type SeedType => typeof(SwampTobaccoSapling);
        public override Type CropType => typeof(SwampTobaccoCrop);
		protected override int YoungPlantGraphics => 0x0CB5;
		protected override int MaturePlantGraphics => 0x0CB0;

		[Constructable] 
		public SwampTobaccoPlant() : base(0x0CB5)
		{
			Hue = 73;
			Name = "Bagienne ziele";
			Stackable = true;
		}

		public SwampTobaccoPlant( Serial serial ) : base( serial ) 
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
	
	public class SwampTobaccoCrop : Crop
    {
        public override Type ReagentType => typeof(SwampTobacco);
		
		[Constructable]
		public SwampTobaccoCrop( int amount ) : base( amount, 0x0C93)
		{
			Hue = 72;
			Name = "Swieze liscie bagiennego ziela";
			Stackable = true;
		}

		[Constructable]
		public SwampTobaccoCrop() : this( 1 )
		{
		}

		public SwampTobaccoCrop( Serial serial ) : base( serial )
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