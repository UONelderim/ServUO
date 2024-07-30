using System;

namespace Server.Items.Crops
{

	
	public class SzczepkaMuchomor : BaseSeedling
	{
		public override bool CanGrowCave => true;
		public override bool CanGrowDirt => true;
		public override Type PlantType => typeof(KrzakMuchomor);

		[Constructable]
		public SzczepkaMuchomor( int amount ) : base( amount, 0x0F23) // wilcza jagoda
		{
			Hue = 1509;
			Name = "Zarodniki muchomora";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaMuchomor() : this( 1 )
		{
		}

		public SzczepkaMuchomor( Serial serial ) : base( serial )
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
	
	public class KrzakMuchomor : Plant
    {
        public override Type SeedType => typeof(SzczepkaMuchomor);
        public override Type CropType => typeof(PlonMuchomor);
		protected override int YoungPlantGraphics => 0x0D16;
		protected override int MaturePlantGraphics => 0x0D16;

		[Constructable] 
		public KrzakMuchomor() : base(0x0D16)
		{
			Hue = 0;
			Name = "Muchomor";
			Stackable = true;
		}

		public KrzakMuchomor( Serial serial ) : base( serial ) 
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
	
	public class PlonMuchomor : Crop
    {
        public override Type ReagentType => typeof(Nightshade);
		
		[Constructable]
		public PlonMuchomor( int amount ) : base( amount, 0x0D16)
		{
			Hue = 0;
			Name = "Swiezy muchomor";
			Stackable = true;
		}

		public override void MutateReagent(Item reagent)
		{
			reagent.Name = "Suszony muchomor";
			reagent.ItemID = 0x0D15;
		}

		[Constructable]
		public PlonMuchomor() : this( 1 )
		{
		}

		public PlonMuchomor( Serial serial ) : base( serial )
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