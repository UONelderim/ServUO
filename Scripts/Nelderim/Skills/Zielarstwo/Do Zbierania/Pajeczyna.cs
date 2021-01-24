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
	public class ZrodloPajeczyna : WeedPlantZbieractwo
	{ 
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack( new SurowiecPajeczyna(count) ); }

		public override bool GivesSeed{ get{ return false; } }

		[Constructable] 
		public ZrodloPajeczyna() : base( 0x10D6 ) //0x26A1
		{ 
			Hue = 0x481;
			Name = "Klab pajeczych sieci";			
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
	
	public class SurowiecPajeczyna : WeedCropZbieractwo
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack( new SpidersSilk(count) ); }
		
		[Constructable]
		public SurowiecPajeczyna( int amount ) : base( amount, 0x0DF6 )
		{
			Hue = 0;
			Name = "Nawinieta pajeczyna";
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