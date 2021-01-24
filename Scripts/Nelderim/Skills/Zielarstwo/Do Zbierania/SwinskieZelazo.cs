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
	public class ZrodloSwinskieZelazo : WeedPlantZbieractwo
	{ 
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack( new SurowiecSwinskieZelazo(count) ); }

		public override bool GivesSeed{ get{ return false; } }

		[Constructable] 
		public ZrodloSwinskieZelazo() : base( 0x266C )
		{ 
			Hue = 0x383;
			Name = "Zardzewiala surowka";			
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
	
	public class SurowiecSwinskieZelazo : WeedCropZbieractwo
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack( new PigIron(count) ); }
		
		[Constructable]
		public SurowiecSwinskieZelazo( int amount ) : base( amount, 0x0FB7 )
		{
			Hue = 0x383;
			Name = "Ordzewiale zeliwo";
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