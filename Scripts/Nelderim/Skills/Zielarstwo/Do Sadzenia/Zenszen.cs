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

	
	public class SzczepkaZenszen : WeedSeedZiolaUprawne
	{
		public override Item CreateWeed() { return new KrzakZenszen(); }

		[Constructable]
		public SzczepkaZenszen( int amount ) : base( amount, 0x18EB ) 
		{
			Hue = 0;
			Name = "Szczepka zen-szeniu";
		}

		[Constructable]
		public SzczepkaZenszen() : this( 1 )
		{
		}

		public SzczepkaZenszen( Serial serial ) : base( serial )
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
	
	public class KrzakZenszen : WeedPlantZiolaUprawne
	{ 
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack( new PlonZenszen(count) ); }

		public override void CreateSeed(Mobile from, int count) { from.AddToBackpack( new SzczepkaZenszen(count) ); } 

		[Constructable] 
		public KrzakZenszen() : base( 0x18E9 )
		{ 
			Hue = 0;
			Name = "Zen-szen";			
		}

		public KrzakZenszen( Serial serial ) : base( serial ) 
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
	
	public class PlonZenszen : WeedCropZiolaUprawne
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack( new Ginseng(count) ); }
		
		[Constructable]
		public PlonZenszen( int amount ) : base( amount, 0x18EC )
		{
			Hue = 0;
			Name = "Surowy zen-szen";
		}

		[Constructable]
		public PlonZenszen() : this( 1 )
		{
		}

		public PlonZenszen( Serial serial ) : base( serial )
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