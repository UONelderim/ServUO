namespace Server.Items.Crops
{
	// Tutaj zmieniaj wlasnosci dotyczace wszystkich surowcow zbieranych.

	// Klasa ogolnie reprezentujaca dowolne zrodlo surowica (systemu zielarstwa) wystepujacy na mapie.
	public class WeedPlantZbieractwo : WeedPlant
	{
		public override string MsgCantBeMounted => "Nie mozesz zbierac surowcow bedac konno.";
		public override string MsgMustGetCloser => "Musisz podejsc blizej, aby to zebrac.";
		public override string MsgPlantTooYoung => "Ilosc surowca w tym miejscu nie jest jeszcze wystarczajaca.";
		public override string MsgNoChanceToGet => "Twoja wiedza o tym surowcu jest za mala, aby go wykorzystac.";

		public override string MsgSuccesfull => "Udalo ci sie zebrac surowiec.";

		//public override string MsgGotSeed			{ get{ return "Udalo ci sie zebrac szczepke rosliny!"; } }
		public override string MsgFailToGet => "Nie udalo ci sie zebrac surowca.";
		public override string MsgPlantDestroyed => "Zmarnowales okazje.";

		public override bool GivesSeed => false;

		[Constructable]
		public WeedPlantZbieractwo(int itemID) : base(itemID)
		{
			GrowingTime = 0;
			SkillMin = 0;
			SkillMax = 100;
			SkillDestroy = 35;
		}

		public WeedPlantZbieractwo(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	// Klasa ogolnie reprezentujaca dowolny surowiec (systemu zielarstwa) zebrany juz do plecaka.
	public class WeedCropZbieractwo : WeedCrop
	{
		public override string MsgCreatedZeroReagent => "Nie uzyskales wystarczajacej ilosci reagentu.";
		public override string MsgFailedToCreateReagents => "Nie udalo ci sie uzyskac reagentow.";
		public override string MsgCreatedReagent => "Uzyskales nieco reagentu.";
		public override string MsgStartedToCut => "Zaczynasz obrabiac surowiec...";

		public WeedCropZbieractwo(int amount, int itemID) : base(itemID)
		{
			Amount = amount;
			//Weight = 0.2;
		}

		public WeedCropZbieractwo(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
