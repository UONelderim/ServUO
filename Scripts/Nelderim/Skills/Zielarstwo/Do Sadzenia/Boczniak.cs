using System;

namespace Server.Items.Crops
{

	
	public class SzczepkaBoczniak : BaseSeedling
	{
		public override bool CanGrowCave => true;
		public override bool CanGrowDirt => true;
		public override Type PlantType => typeof(KrzakBoczniak);

		[Constructable]
		public SzczepkaBoczniak( int amount ) : base( amount, 0x0F23) // czosnek
		{
			Hue = 872;
			Name = "Zarodniki boczniaka";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaBoczniak() : this( 1 )
		{
		}

		public SzczepkaBoczniak( Serial serial ) : base( serial )
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
	
	public class KrzakBoczniak : Plant
    {
        public override Type SeedType => typeof(SzczepkaBoczniak);
        public override Type CropType => typeof(PlonBoczniak);
		protected override int YoungPlantGraphics => 0xD19;
		protected override int MaturePlantGraphics => 0xD19;

		[Constructable] 
		public KrzakBoczniak() : base(0xD19)
		{
			Hue = 867;
			Name = "Boczniak";
			Stackable = true;
		}

		public KrzakBoczniak( Serial serial ) : base( serial ) 
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
	
	public class PlonBoczniak : Crop
    {
        public override Type ReagentType => typeof(Garlic);
		
		[Constructable]
		public PlonBoczniak( int amount ) : base( amount, 0xD19)
		{
			Hue = 867;
			Name = "Swiezy boczniak";
			Stackable = true;
		}

		public override void MutateReagent(Item reagent)
		{
			reagent.Hue = 892;
			reagent.Name = "Suszony boczniak";
			reagent.ItemID = ItemID;
		}

		[Constructable]
		public PlonBoczniak() : this( 1 )
		{
		}

		public PlonBoczniak( Serial serial ) : base( serial )
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