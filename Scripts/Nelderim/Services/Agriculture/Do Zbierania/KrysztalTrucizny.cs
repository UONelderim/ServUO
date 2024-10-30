using System;

namespace Server.Items.Crops
{
	public class ZrodloKrysztalTrucizny : ResourceVein
    {
        public override Type CropType => typeof(SurowiecKrysztalTrucizny);
		protected override int MaturePlantGraphics => 0x35DA;

		[Constructable] 
		public ZrodloKrysztalTrucizny() : base( 0x35DA )
		{ 
			Hue = 0x44;
			Name = "Krysztaly trucizny";		
			Stackable = true;
		}

		public ZrodloKrysztalTrucizny( Serial serial ) : base( serial ) 
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
	
	public class SurowiecKrysztalTrucizny : ResourceCrop
    {
        public override Type ReagentType => typeof(NoxCrystal);
		
		[Constructable]
		public SurowiecKrysztalTrucizny( int amount ) : base( amount, 0x2244 )
		{
			Hue = 0x44;
			Name = "Nieksztaltny krysztal trucizny";
			Stackable = true;
		}

		[Constructable]
		public SurowiecKrysztalTrucizny() : this( 1 )
		{
		}

		public SurowiecKrysztalTrucizny( Serial serial ) : base( serial )
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