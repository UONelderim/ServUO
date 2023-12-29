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

	
	public class SzczepkaTyton : WeedSeedZiolaUprawne
	{
		public override Item CreateWeed() { return new KrzakTyton(); }

		[Constructable]
		public SzczepkaTyton( int amount ) : base( amount, 0x166F ) 
		{
			Hue = 2129;
			Name = "Szczepka tytoniu";
			Stackable = true;
		}

		[Constructable]
		public SzczepkaTyton() : this( 1 )
		{
		}

		public SzczepkaTyton( Serial serial ) : base( serial )
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
	
	public class KrzakTyton : WeedPlantZiolaUprawne
	{ 
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack( new PlonTyton(count) ); }

		public override void CreateSeed(Mobile from, int count) { from.AddToBackpack( new SzczepkaTyton(count) ); } 

		[Constructable] 
		public KrzakTyton() : base( 0x0F88 )
		{ 
			Hue = 2129;
			Name = "Tyton";
			Stackable = true;
		}

		public KrzakTyton( Serial serial ) : base( serial ) 
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
	
	public class PlonTyton : WeedCropZiolaUprawne
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack( new Tyton(count) ); }
		
		[Constructable]
		public PlonTyton( int amount ) : base( amount, 0x16C0 )
		{
			Hue = 2129;
			Name = "Swieza lodyga tytoniu";
			Stackable = true;
		}

		[Constructable]
		public PlonTyton() : this( 1 )
		{
		}

		public PlonTyton( Serial serial ) : base( serial )
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