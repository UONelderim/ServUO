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
	// Rosliny customable, umozliwiajace zmiane nazwy, mozliwego podloza, itp.
	// Przydatne dla GMow chcacych wykorzystac w evencie swoje wlasne rosliny.
	// Szczepka tego typu rosliny zawiera dodatkowe informacje o krzaczku jaki z niej powstanie.
	// Krzaczek takiej rosliny rowniez zawiera dodatkowe informacje o szczepkach ktore moze dac, a takze o plonie.
	/*
	// Szczepka rosliny. Zawiera w sobie dodatkowe
	class SzczepkaDziwnejRosliny : WeedSeed
	{
		public string m_SeedName;
		public string m_CropName;
		
		[Constructable]
		public SzczepkaDziwnejRosliny() : this( 1 )
		{
		}

		[Constructable]
		public SzczepkaDziwnejRosliny( int amount ) : base( 45 ) // ustawic jakas domyslna grafike roslinki!
		{
			Hue = 0;
			Amount = amount;
			Name = "Szczepka dziwnej rosliny";
		}

		public override void OnDoubleClick( Mobile from ) 
		{
			base.OnDoubleClick(from);
		}

		public SzczepkaDziwnejRosliny( Serial serial ) : base( serial )
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


	public class KrzakDziwnejRosliny : WeedPlant
	{
		public bool m_GiveSeed;
		public string m_SeedName;
		public string m_CropName;

		[Constructable] 
		public KrzakDziwnejRosliny() : base( 0x18E2 )
		{ 
			Hue = 0;
			Name = "Krzak dziwnej rosliny";
			m_SeedName = "Szczepka dziwnej rosliny";
			m_CropName = "Plon dziwnej rosliny";
		}

		public KrzakDziwnejRosliny( Serial serial ) : base( serial ) 
		{ 
			//m_plantedTime = DateTime.Now;	// ???
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int SkillDestroy
		{
			get{ return m_SkillDestroy; }
			set{ m_SkillDestroy = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int GiveSeed
		{
			get{ return m_GiveSeed; }
			set{ m_GiveSeed = value; }
		}

		public override void CreateCrop(int count)
		{
			from.AddToBackpack( new PlonCzosnek(count) );
		}

		public override void CreateCrop(int count)
		{
			from.AddToBackpack( new SzczepkaCzosnku(count) );
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


	public class PlonRozne : WeedCrop
	{
	}

	*/
}
