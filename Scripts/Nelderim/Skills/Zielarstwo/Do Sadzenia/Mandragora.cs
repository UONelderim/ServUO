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

	
	public class SzczepkaMandragora : WeedSeedZiolaUprawne
	{
		public override Item CreateWeed() { return new KrzakMandragora(); }

		[Constructable]
		public SzczepkaMandragora( int amount ) : base( amount, 0x18DD ) 
		{
			Hue = 0;
			Name = "Szczepka mandragory";
		}

		[Constructable]
		public SzczepkaMandragora() : this( 1 )
		{
		}

		public SzczepkaMandragora( Serial serial ) : base( serial )
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
	
	public class KrzakMandragora : WeedPlantZiolaUprawne
	{ 
		public override void CreateCrop(Mobile from, int count) { from.AddToBackpack( new PlonMandragora(count) ); }

		public override void CreateSeed(Mobile from, int count) { from.AddToBackpack( new SzczepkaMandragora(count) ); } 

		[Constructable] 
		public KrzakMandragora() : base( 0x18E0 )
		{ 
			Hue = 0;
			Name = "Mandragora";			
		}

		public KrzakMandragora( Serial serial ) : base( serial ) 
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
	
	public class PlonMandragora : WeedCropZiolaUprawne
	{
		public override void CreateReagent(Mobile from, int count) { from.AddToBackpack( new MandrakeRoot(count) ); }
		
		[Constructable]
		public PlonMandragora( int amount ) : base( amount, 0x18DE )
		{
			Hue = 0;
			Name = "Swiezy korzen mandragory";
		}

		[Constructable]
		public PlonMandragora() : this( 1 )
		{
		}

		public PlonMandragora( Serial serial ) : base( serial )
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