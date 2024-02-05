using System;
using Server;
using System.Collections;
using Server.Network;
using Server.Gumps;
using Server.Items; 
using Server.Mobiles;
using Server.Engines;
using Server.Engines.Harvest;

namespace Server.Items.Crops
{
	public class SeedlingMsgs
	{
		public virtual string CantBeMounted { get { return "Musisz stac na ziemi, aby moc sadzic rosliny."; } }
		public virtual string BadTerrain { get { return "Roslina na pewno nie urosnie na tym terenie."; } }
		public virtual string PlantAlreadyHere { get { return "W tym miejscu cos juz rosnie."; } }
		public virtual string Obstacle { get { return "Cos blokuje to miejsce."; } }
		public virtual string TooLowSkillToPlant { get { return "Nie wiesz zbyt wiele o sadzeniu ziol."; } }
		public virtual string PlantSuccess { get { return "Udalo ci sie zasadzic rosline."; } }
		public virtual string PlantFail { get { return "Nie udalo ci sie zasadzic rosliny. Sprobuj ponownie."; } }
		public virtual string PlantFailWithLoss { get { return "Nie udalo ci sie zasadzic rosliny, zmarnowales szczepke."; } }
	}

	public class PlantMsgs
	{

		public virtual string CantBeMounted { get { return "Nie mozesz zbierac roslin bedac konno."; } }
		public virtual string MustGetCloser { get { return "Musisz podejsc blizej, aby zebrac plon."; } }
		public virtual string PlantTooYoung { get { return "Roslina jest jeszcze niedojrzala."; } }
		public virtual string EmptyCrop { get { return "Roslina nie zrodzila jeszcze plonu."; } }
		public virtual string NoChanceToGet { get { return "Twoja wiedza o tej roslinie jest za mala, aby zebrac plon."; } }
		public virtual string Succesfull { get { return "Udalo ci sie zebrac troche plonow."; } }
		public virtual string GotSeed { get { return "Udalo ci sie zebrac szczepke rosliny!"; } }
		public virtual string FailToGet { get { return "Nie udalo ci sie zebrac plonu."; } }
		public virtual string PlantDestroyed { get { return "Zniszczyles rosline."; } }
		public virtual string FertilizeSuccess { get { return "Uzyles nawozu, aby wzmocnic rosline."; } }
		public virtual string AlreadyFertilized { get { return "Ta roslina nie potrzebuje wiecej nawozu."; } }

	}

	public class ResourceMsgs : PlantMsgs
	{
		public override string CantBeMounted { get { return "Nie mozesz zbierac surowcow bedac konno."; } }
		public override string MustGetCloser { get { return "Musisz podejsc blizej, aby to zebrac."; } }
		public override string PlantTooYoung { get { return "Ilosc surowca w tym miejscu nie jest jeszcze wystarczajaca."; } }
		public override string EmptyCrop { get { return "Ilosc surowca w tym miejscu nie jest wystarczajaca"; } }
		public override string NoChanceToGet { get { return "Twoja wiedza o tym surowcu jest za mala, aby go pozyskac."; } }
		public override string Succesfull { get { return "Udalo ci sie zebrac troche surowca."; } }
		public override string GotSeed { get { return "Przy okazji zebrales rowniez cos dziwnego."; } }
		public override string FailToGet { get { return "Nie udalo ci sie zebrac surowca."; } }
		public override string PlantDestroyed { get { return "Wyeksploatowales znalezisko."; } }
		public virtual string FertilizeSuccess { get { return "Uzyles nawozu."; } }
		public virtual string AlreadyFertilized { get { return "Nie potrzeba tu wiecej nawozu."; } }
	}

	public class CropMsgs
	{
		public virtual string CreatedZeroReagent { get { return "Nie uzyskales wystarczajacej ilosci reagentu."; } }
		public virtual string FailedToCreateReagents { get { return "Nie udalo ci sie uzyskac reagentow."; } }
		public virtual string CreatedReagent { get { return "Uzyskales nieco reagentu."; } }
		public virtual string StartedToCut { get { return "Zaczynasz obrabiac surowiec..."; } }
	}

}