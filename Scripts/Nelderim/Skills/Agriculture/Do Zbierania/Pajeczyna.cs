using System;

namespace Server.Items.Crops
{
	public class ZrodloPajeczyna : ResourceVein
    {
        public override Type CropType => typeof(SurowiecPajeczyna);
		protected override int MaturePlantGraphics => 0x10D6;

		[Constructable] 
		public ZrodloPajeczyna() : base( 0x10D6 ) //0x26A1
		{ 
			Hue = 0x481;
			Name = "Klab pajeczych sieci";	
			Stackable = true;			
		}

		public ZrodloPajeczyna( Serial serial ) : base( serial ) 
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
	
	public class SurowiecPajeczyna : ResourceCrop
    {
        public override Type ReagentType => typeof(SpidersSilk);
		
		[Constructable]
		public SurowiecPajeczyna( int amount ) : base( amount, 0x0DF6 )
		{
			Hue = 0;
			Name = "Nawinieta pajeczyna";
			Stackable = true;
		}

		[Constructable]
		public SurowiecPajeczyna() : this( 1 )
		{
		}

		public SurowiecPajeczyna( Serial serial ) : base( serial )
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