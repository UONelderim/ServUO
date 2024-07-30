using System;

namespace Server.Items.Crops
{

	
	public class SzczepkaLysiczka : BaseSeedling
    {
		public override bool CanGrowCave => true;
		public override bool CanGrowDirt => true;
		public override Type PlantType => typeof(KrzakLysiczka);

		[Constructable]
		public SzczepkaLysiczka( int amount ) : base( amount, 0x0F23) // mandragora
		{
			Hue = 798;
			Name = "Zarodniki lysiczki";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaLysiczka() : this( 1 )
		{
		}

		public SzczepkaLysiczka( Serial serial ) : base( serial )
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
	
	public class KrzakLysiczka : Plant
    {
        public override Type SeedType => typeof(SzczepkaLysiczka);
        public override Type CropType => typeof(PlonLysiczka);
		protected override int YoungPlantGraphics => 0x0D14;
		protected override int MaturePlantGraphics => 0x0D14;

		[Constructable] 
		public KrzakLysiczka() : base(0x0D14)
		{
			Hue = 1363;
			Name = "Lysiczka";
			Stackable = true;
		}

		public KrzakLysiczka( Serial serial ) : base( serial ) 
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
	
	public class PlonLysiczka : Crop
    {
        public override Type ReagentType => typeof(MandrakeRoot);
		
		[Constructable]
		public PlonLysiczka( int amount ) : base( amount, 0x0D14)
		{
			Hue = 1363;
			Name = "Swieza lysiczka";
			Stackable = true;
		}

		public override void MutateReagent(Item reagent)
		{
			reagent.Hue = 1364;
			reagent.Name = "Suszona lysiczka";
			reagent.ItemID = ItemID;
		}

		[Constructable]
		public PlonLysiczka() : this( 1 )
		{
		}

		public PlonLysiczka( Serial serial ) : base( serial )
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