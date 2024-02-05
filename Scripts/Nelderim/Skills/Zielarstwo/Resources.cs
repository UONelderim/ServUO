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
	public abstract class ResourceVein : BasePlant
	{
		public override PlantMsgs msg => new ResourceMsgs();
		public override bool GivesSeed => false;
		protected override int YoungPlantGraphics => MaturePlantGraphics;

		public override void GetProperties(ObjectPropertyList list)
		{
			AddNameProperty(list);
		}

		public ResourceVein( int itemID ) : base( itemID )
		{
		}

		public ResourceVein( Serial serial ) : base( serial ) 
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
	
	public abstract class ResourceCrop : BaseCrop
	{	
		public ResourceCrop( int amount, int itemID ) : base( amount, itemID )
		{
		}

		public ResourceCrop( Serial serial ) : base( serial )
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