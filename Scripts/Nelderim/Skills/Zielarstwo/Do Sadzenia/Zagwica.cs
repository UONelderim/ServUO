using System;

namespace Server.Items.Crops
{

	
	public class SzczepkaZagwica : BaseSeedling
	{
		public override bool CanGrowCave => true;
		public override bool CanGrowDirt => true;
		public override Type PlantType => typeof(KrzakZagwica);

		[Constructable]
		public SzczepkaZagwica( int amount ) : base( amount, 0x0F23)	// zenszen
		{
			Hue = 1236;
			Name = "Zarodniki zagwicy";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaZagwica() : this( 1 )
		{
		}

		public SzczepkaZagwica( Serial serial ) : base( serial )
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
	
	public class KrzakZagwica : Plant
    {
        public override Type SeedType => typeof(SzczepkaZagwica);
        public override Type CropType => typeof(PlonZagwica);
		protected override int YoungPlantGraphics => 0xD13;
		protected override int MaturePlantGraphics => 0xD13;

		[Constructable] 
		public KrzakZagwica() : base(0xD13)
		{
			Hue = 1235;
			Name = "Zagwica";
			Stackable = true;
		}

		public KrzakZagwica( Serial serial ) : base( serial ) 
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
	
	public class PlonZagwica : Crop
    {
        public override Type ReagentType => typeof(Ginseng);
		
		[Constructable]
		public PlonZagwica( int amount ) : base( amount, 0xD13)
		{
			Hue = 1235;
			Name = "Swieza zagwica";
			Stackable = true;
		}

		public override void MutateReagent(Item reagent)
		{
			reagent.Hue = 1236;
			reagent.Name = "Suszona zagwica";
			reagent.ItemID = ItemID;
		}

		[Constructable]
		public PlonZagwica() : this( 1 )
		{
		}

		public PlonZagwica( Serial serial ) : base( serial )
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