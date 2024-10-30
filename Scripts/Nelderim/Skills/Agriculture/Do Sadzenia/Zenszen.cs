using System;

namespace Server.Items.Crops
{

	
	public class SzczepkaZenszen : BaseSeedling
    {
        public override Type PlantType => typeof(KrzakZenszen);

		[Constructable]
		public SzczepkaZenszen( int amount ) : base( amount, 0x18EB ) 
		{
			Hue = 0;
			Name = "Szczepka zen-szeniu";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaZenszen() : this( 1 )
		{
		}

		public SzczepkaZenszen( Serial serial ) : base( serial )
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
	
	public class KrzakZenszen : Plant
    {
        public override Type SeedType => typeof(SzczepkaZenszen);
        public override Type CropType => typeof(PlonZenszen);
		protected override int YoungPlantGraphics => 0x18E9;
		protected override int MaturePlantGraphics => 0x18EA;

		[Constructable] 
		public KrzakZenszen() : base( 0x18E9 )
		{
			Hue = 0;
			Name = "Zen-szen";
			Stackable = true;
		}

		public KrzakZenszen( Serial serial ) : base( serial ) 
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
	
	public class PlonZenszen : Crop
    {
        public override Type ReagentType => typeof(Ginseng);
		
		[Constructable]
		public PlonZenszen( int amount ) : base( amount, 0x18EC )
		{
			Hue = 0;
			Name = "Surowy zen-szen";
			Stackable = true;
		}

		[Constructable]
		public PlonZenszen() : this( 1 )
		{
		}

		public PlonZenszen( Serial serial ) : base( serial )
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