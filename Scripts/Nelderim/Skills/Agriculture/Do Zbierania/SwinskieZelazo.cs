using System;

namespace Server.Items.Crops
{
	public class ZrodloSwinskieZelazo : ResourceVein
    {
        public override Type CropType => typeof(SurowiecSwinskieZelazo);
		protected override int MaturePlantGraphics => 0x266C;

		[Constructable] 
		public ZrodloSwinskieZelazo() : base( 0x266C )
		{ 
			Hue = 0x383;
			Name = "Zardzewiala surowka";
			Stackable = true;			
		}

		public ZrodloSwinskieZelazo( Serial serial ) : base( serial ) 
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
	
	public class SurowiecSwinskieZelazo : ResourceCrop
    {
        public override Type ReagentType => typeof(PigIron);
		
		[Constructable]
		public SurowiecSwinskieZelazo( int amount ) : base( amount, 0x0FB7 )
		{
			Hue = 0x383;
			Name = "Ordzewiale zeliwo";
			Stackable = true;
		}

		[Constructable]
		public SurowiecSwinskieZelazo() : this( 1 )
		{
		}

		public SurowiecSwinskieZelazo( Serial serial ) : base( serial )
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