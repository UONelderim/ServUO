using System;
using Server.Network;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Targets;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Mobiles;

namespace Server.Items.Crops
{

	
	public class SzczepkaKrwawyMech : WeedSeedZiolaUprawne
	{
		public override Item CreateWeed() { return new KrzakKrwawyMech(); }

		[Constructable]
		public SzczepkaKrwawyMech( int amount ) : base( amount, 0x0DCD ) 
		{
			Hue = 438;
			Name = "Szczepka krwawego mchu";
		}

		[Constructable]
		public SzczepkaKrwawyMech() : this( 1 )
		{
		}

		public SzczepkaKrwawyMech( Serial serial ) : base( serial )
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
	
	public class KrzakKrwawyMech : WeedPlantZiolaUprawne
	{ 
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack( new PlonKrwawyMech(count) ); }

		public override void CreateSeed(Mobile from, int count) { from.AddToBackpack( new SzczepkaKrwawyMech(count) ); } 

		[Constructable] 
		public KrzakKrwawyMech() : base( 0x0F3B )
		{ 
			Hue = 0x20;
			Name = "Krwawy mech";			
		}

		public KrzakKrwawyMech( Serial serial ) : base( serial ) 
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
	
	public class PlonKrwawyMech : WeedCropZiolaUprawne
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack( new Bloodmoss(count) ); }
		
		[Constructable]
		public PlonKrwawyMech( int amount ) : base( amount, 0x3183 )
		{
			Hue = 0x20;
			Name = "Swiezy krwawy mech";
		}

		[Constructable]
		public PlonKrwawyMech() : this( 1 )
		{
		}

		public PlonKrwawyMech( Serial serial ) : base( serial )
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