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

	
	public class SzczepkaCzosnek : WeedSeedZiolaUprawne
	{
		public override Item CreateWeed() { return new KrzakCzosnek(); }

		[Constructable]
		public SzczepkaCzosnek( int amount ) : base( amount, 0x18E3 ) 
		{
			Hue = 178;
			Name = "Szczepka czosnku";
		}

		[Constructable]
		public SzczepkaCzosnek() : this( 1 )
		{
		}

		public SzczepkaCzosnek( Serial serial ) : base( serial )
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
	
	public class KrzakCzosnek : WeedPlantZiolaUprawne
	{ 
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack( new PlonCzosnek(count) ); }

		public override void CreateSeed(Mobile from, int count) { from.AddToBackpack( new SzczepkaCzosnek(count) ); } 

		[Constructable] 
		public KrzakCzosnek() : base( 0x18E2 )
		{ 
			Hue = 0;
			Name = "Lodyga czosnku";			
		}

		public KrzakCzosnek( Serial serial ) : base( serial ) 
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
	
	public class PlonCzosnek : WeedCropZiolaUprawne
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack( new Garlic(count) ); }
		
		[Constructable]
		public PlonCzosnek( int amount ) : base( amount, 0x18E4 )
		{
			Hue = 0;
			Name = "Glowka czosnku";
		}

		[Constructable]
		public PlonCzosnek() : this( 1 )
		{
		}

		public PlonCzosnek( Serial serial ) : base( serial )
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