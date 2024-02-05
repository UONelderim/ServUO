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
	public abstract class Plant : BasePlant
	{ 
		public override PlantMsgs msg => new PlantMsgs();

		//public override TimeSpan GrowMatureTime => TimeSpan.FromMinutes(15);
		
		public override bool GivesSeed => true;

		public Plant( int itemID ) : base( itemID )
		{
        }

		public Plant( Serial serial ) : base( serial ) 
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
	
	public abstract class Crop : BaseCrop
	{
		public Crop( int amount, int itemID ) : base( amount, itemID )
		{
		}
		
		public Crop( Serial serial ) : base( serial )
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