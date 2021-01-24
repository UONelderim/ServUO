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
	public class ZrodloGrobowyPyl : WeedPlantZbieractwo
	{ 
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack( new SurowiecGrobowyPyl(count) ); }

		public override bool GivesSeed{ get{ return false; } }

		[Constructable] 
		public ZrodloGrobowyPyl() : base( 0x0F35 )
		{ 
			Hue = 0x481;
			Name = "Prochy ze zwlok";			
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
	
	public class SurowiecGrobowyPyl : WeedCropZbieractwo
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack( new GraveDust(count) ); }
		
		[Constructable]
		public SurowiecGrobowyPyl( int amount ) : base( amount, 0x2233 )
		{
			Hue = 0x481;
			Name = "Zanieczyszczone prochy";
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