using System;

namespace Server.Items.Crops
{
	public class ZrodloGrobowyPyl : ResourceVein
    {
        public override Type CropType => typeof(SurowiecGrobowyPyl);
		protected override int MaturePlantGraphics => 0x0F35;

		[Constructable] 
		public ZrodloGrobowyPyl() : base( 0x0F35 )
		{ 
			Hue = 0x481;
			Name = "Prochy ze zwlok";	
			Stackable = true;
		}

		public ZrodloGrobowyPyl( Serial serial ) : base( serial ) 
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
	
	public class SurowiecGrobowyPyl : ResourceCrop
    {
        public override Type ReagentType => typeof(GraveDust);
		
		[Constructable]
		public SurowiecGrobowyPyl( int amount ) : base( amount, 0x2233 )
		{
			Hue = 0x481;
			Name = "Zanieczyszczone prochy";
			Stackable = true;
		}

		[Constructable]
		public SurowiecGrobowyPyl() : this( 1 )
		{
		}

		public SurowiecGrobowyPyl( Serial serial ) : base( serial )
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