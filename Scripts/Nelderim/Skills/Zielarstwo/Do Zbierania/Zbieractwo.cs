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
	// Tutaj zmieniaj wlasnosci dotyczace wszystkich surowcow zbieranych.

	// Klasa ogolnie reprezentujaca dowolne zrodlo surowica (systemu zielarstwa) wystepujacy na mapie.
	public class WeedPlantZbieractwo : WeedPlant
	{ 
		public override string MsgCantBeMounted		{ get{ return "Nie mozesz zbierac surowcow bedac konno."; } }
		public override string MsgMustGetCloser		{ get{ return "Musisz podejsc blizej, aby to zebrac."; } }
		public override string MsgPlantTooYoung		{ get{ return "Ilosc surowca w tym miejscu nie jest jeszcze wystarczajaca."; } }
		public override string MsgNoChanceToGet		{ get{ return "Twoja wiedza o tym surowcu jest za mala, aby go wykorzystac."; } }
		public override string MsgSuccesfull		{ get{ return "Udalo ci sie zebrac surowiec."; } }
		//public override string MsgGotSeed			{ get{ return "Udalo ci sie zebrac szczepke rosliny!"; } }
		public override string MsgFailToGet			{ get{ return "Nie udalo ci sie zebrac surowca."; } }
		public override string MsgPlantDestroyed	{ get{ return "Zmarnowales okazje."; } }

		public override bool GivesSeed{ get{ return false; } }

		[Constructable] 
		public WeedPlantZbieractwo( int itemID ) : base( itemID )
		{ 
			GrowingTime = 0;
			SkillMin = 0;
			SkillMax = 100;
			SkillDestroy = 35;		
		}

		public WeedPlantZbieractwo( Serial serial ) : base( serial ) 
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
	
	// Klasa ogolnie reprezentujaca dowolny surowiec (systemu zielarstwa) zebrany juz do plecaka.
	public abstract class WeedCropZbieractwo : WeedCrop
	{
		public override string MsgCreatedZeroReagent		{ get{ return "Nie uzyskales wystarczajacej ilosci reagentu."; } }
		public override string MsgFailedToCreateReagents	{ get{ return "Nie udalo ci sie uzyskac reagentow."; } }
		public override string MsgCreatedReagent			{ get{ return "Uzyskales nieco reagentu."; } }
		public override string MsgStartedToCut				{ get{ return "Zaczynasz obrabiac surowiec..."; } }
		
		public WeedCropZbieractwo( int amount, int itemID ) : base( itemID )
		{
			Amount = amount;
			//Weight = 0.2;
		}
		
		public WeedCropZbieractwo( Serial serial ) : base( serial )
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
