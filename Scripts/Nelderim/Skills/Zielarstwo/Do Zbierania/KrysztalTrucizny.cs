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
	public class ZrodloKrysztalTrucizny : WeedPlantZbieractwo
	{ 
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack( new SurowiecKrysztalTrucizny(count) ); }

		public override bool GivesSeed{ get{ return false; } }

		[Constructable] 
		public ZrodloKrysztalTrucizny() : base( 0x35DA )
		{ 
			Hue = 0x44;
			Name = "Krysztaly trucizny";			
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
	
	public class SurowiecKrysztalTrucizny : WeedCropZbieractwo
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack( new NoxCrystal(count) ); }
		
		[Constructable]
		public SurowiecKrysztalTrucizny( int amount ) : base( amount, 0x2244 )
		{
			Hue = 0x44;
			Name = "Nieksztaltny krysztal trucizny";
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