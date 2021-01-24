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

	
	public class SzczepkaWilczaJagoda : WeedSeedZiolaUprawne
	{
		public override Item CreateWeed() { return new KrzakWilczaJagoda(); }

		[Constructable]
		public SzczepkaWilczaJagoda( int amount ) : base( amount, 0x18E7 ) 
		{
			Hue = 0;
			Name = "Szczepka wilczej jagody";
		}

		[Constructable]
		public SzczepkaWilczaJagoda() : this( 1 )
		{
		}

		public SzczepkaWilczaJagoda( Serial serial ) : base( serial )
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
	
	public class KrzakWilczaJagoda : WeedPlantZiolaUprawne
	{ 
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack( new PlonWilczaJagoda(count) ); }

		public override void CreateSeed(Mobile from, int count) { from.AddToBackpack( new SzczepkaWilczaJagoda(count) ); } 

		[Constructable] 
		public KrzakWilczaJagoda() : base( 0x18E6 )
		{ 
			Hue = 0;
			Name = "Krzak wilczych jagod";			
		}

		public KrzakWilczaJagoda( Serial serial ) : base( serial ) 
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
	
	public class PlonWilczaJagoda : WeedCropZiolaUprawne
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack( new Nightshade(count) ); }
		
		[Constructable]
		public PlonWilczaJagoda( int amount ) : base( amount, 0x18E8 )
		{
			Hue = 0;
			Name = "Galazka wilczej jagody";
		}

		[Constructable]
		public PlonWilczaJagoda() : this( 1 )
		{
		}

		public PlonWilczaJagoda( Serial serial ) : base( serial )
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