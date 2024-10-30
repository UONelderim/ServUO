using System;

namespace Server.Items.Crops
{

	
	public class SzczepkaKrwawyMech : BaseSeedling
    {
		public override bool CanGrowCave => true;
		public override bool CanGrowDirt => true;
		public override Type PlantType => typeof(KrzakKrwawyMech);

		[Constructable]
		public SzczepkaKrwawyMech( int amount ) : base( amount, 0x0DCD ) 
		{
			Hue = 438;
			Name = "Szczepka krwawego mchu";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaKrwawyMech() : this( 1 )
		{
		}

		public SzczepkaKrwawyMech( Serial serial ) : base( serial )
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
	
	public class KrzakKrwawyMech : Plant
    {
        public override Type SeedType => typeof(SzczepkaKrwawyMech);
        public override Type CropType => typeof(PlonKrwawyMech);
		protected override int YoungPlantGraphics => 0x0F3C;
		protected override int MaturePlantGraphics => 0x0F3B;

		[Constructable] 
		public KrzakKrwawyMech() : base(0x0F3C)
		{
			Hue = 0x20;
			Name = "Krwawy mech";	
			Stackable = true;
		}

		public KrzakKrwawyMech( Serial serial ) : base( serial ) 
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
	
	public class PlonKrwawyMech : Crop
    {
        public override Type ReagentType => typeof(Bloodmoss);
		
		[Constructable]
		public PlonKrwawyMech( int amount ) : base( amount, 0x3183 )
		{
			Hue = 0x20;
			Name = "Swiezy krwawy mech";
			Stackable = true;
		}

		[Constructable]
		public PlonKrwawyMech() : this( 1 )
		{
		}

		public PlonKrwawyMech( Serial serial ) : base( serial )
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