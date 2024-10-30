using System;

namespace Server.Items.Crops
{
	public class ZrodloKrewDemona : ResourceVein
    {
        public override Type CropType => typeof(SurowiecKrewDemona);
		protected override int MaturePlantGraphics => 0x1CF3;

		[Constructable] 
		public ZrodloKrewDemona() : base( 0x1CF3 )
		{ 
			Hue = 0;
			Name = "Krew demona";
			Stackable = true;			
		}

		public ZrodloKrewDemona( Serial serial ) : base( serial ) 
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
	
	public class SurowiecKrewDemona : ResourceCrop
    {
        public override Type ReagentType => typeof(DaemonBlood);
		
		[Constructable]
		public SurowiecKrewDemona( int amount ) : base( amount, 0x0E23 )
		{
			Hue = 0;
			Name = "Porcja krwi demona";
			Stackable = true;
		}

		[Constructable]
		public SurowiecKrewDemona() : this( 1 )
		{
		}

		public SurowiecKrewDemona( Serial serial ) : base( serial )
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