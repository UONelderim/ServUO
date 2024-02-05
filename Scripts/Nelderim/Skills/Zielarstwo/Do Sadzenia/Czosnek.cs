using System;

namespace Server.Items.Crops
{

	
	public class SzczepkaCzosnek : BaseSeedling
    {
        public override Type PlantType => typeof(KrzakCzosnek);

		[Constructable]
		public SzczepkaCzosnek( int amount ) : base( amount, 0x18E3 ) 
		{
			Hue = 178;
			Name = "Szczepka czosnku";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaCzosnek() : this( 1 )
		{
		}

		public SzczepkaCzosnek( Serial serial ) : base( serial )
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
	
	public class KrzakCzosnek : Plant
    {
        public override Type SeedType => typeof(SzczepkaCzosnek);
        public override Type CropType => typeof(PlonCzosnek);
		protected override int YoungPlantGraphics => 0x18E2;
		protected override int MaturePlantGraphics => 0x18E2;

		[Constructable] 
		public KrzakCzosnek() : base( 0x18E2 )
		{
			Hue = 0;
			Name = "Lodyga czosnku";
			Stackable = true;			
		}

		public KrzakCzosnek( Serial serial ) : base( serial ) 
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
	
	public class PlonCzosnek : Crop
    {
        public override Type ReagentType => typeof(Garlic);
		
		[Constructable]
		public PlonCzosnek( int amount ) : base( amount, 0x18E4 )
		{
			Hue = 0;
			Name = "Glowka czosnku";
		}

		[Constructable]
		public PlonCzosnek() : this( 1 )
		{
		}

		public PlonCzosnek( Serial serial ) : base( serial )
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