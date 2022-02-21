namespace Server.Items.Crops
{
	// Tutaj zmieniaj wlasnosci dotyczace wszystkich ziol uprawnych.


	// Klasa ogolnie reprezentujaca szczepke dowolnego ziela.
	public class WeedSeedZiolaUprawne : WeedSeed
	{
		public override string MsgCantBeMounted { get { return "Musisz stac na ziemi, aby moc sadzic rosliny."; } }
		public override string MsgBadTerrain { get { return "Roslina na pewno nie urosnie na tym terenie."; } }
		public override string MsgPlantAlreadyHere { get { return "W tym miejscu cos juz rosnie."; } }
		public override string MsgTooLowSkillToPlant { get { return "Nie wiesz zbyt wiele o sadzeniu ziol."; } }
		public override string MsgPlantSuccess { get { return "Udalo ci sie zasadzic rosline."; } }

		public override string MsgPlantFail
		{
			get { return "Nie udalo ci sie zasadzic rosliny, zmarnowales szczepke."; }
		}

		[Constructable]
		public WeedSeedZiolaUprawne(int amount, int itemID) : base(itemID)
		{
			Amount = amount;
			//Weight = 0.5;
		}

		[Constructable]
		public WeedSeedZiolaUprawne(int itemID) : this(1, itemID)
		{
		}

		public WeedSeedZiolaUprawne(Serial serial) : base(serial)
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

	// Klasa ogolnie reprezentujaca krzaczek dowolnego ziela.
	public class WeedPlantZiolaUprawne : WeedPlant
	{
		public override string MsgCantBeMounted { get { return "Nie mozesz zbierac roslin bedac konno."; } }
		public override string MsgMustGetCloser { get { return "Musisz podejsc blizej, aby to zebrac."; } }
		public override string MsgPlantTooYoung { get { return "Roslina jest jeszcze niedojrzala."; } }

		public override string MsgNoChanceToGet
		{
			get { return "Twoja wiedza o tej roslinie jest za mala, aby ja zebrac."; }
		}

		public override string MsgSuccesfull { get { return "Udalo ci sie zebrac rosline."; } }
		public override string MsgGotSeed { get { return "Udalo ci sie zebrac szczepke rosliny!"; } }
		public override string MsgFailToGet { get { return "Nie udalo ci sie zebrac ziela."; } }
		public override string MsgPlantDestroyed { get { return "Zniszczyles rosline."; } }

		public override bool GivesSeed { get { return true; } }

		[Constructable]
		public WeedPlantZiolaUprawne(int itemID) : base(itemID)
		{
			GrowingTime =
				0; // Dotyczy spawnowanych na mapie. Sadzone przez graczy maja ustawiany czas w metodzie klasy SeedPlant.
			SkillMin = 0;
			SkillMax = 100;
			SkillDestroy = 35;
		}

		public WeedPlantZiolaUprawne(Serial serial) : base(serial)
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

	// Klasa ogolnie reprezentujaca plon z dowolnego ziela.
	public abstract class WeedCropZiolaUprawne : WeedCrop
	{
		public override string MsgCreatedZeroReagent { get { return "Nie uzyskales wystarczajacej ilosci reagentu."; } }
		public override string MsgFailedToCreateReagents { get { return "Nie udalo ci sie uzyskac reagentow."; } }
		public override string MsgCreatedReagent { get { return "Uzyskales nieco reagentu."; } }
		public override string MsgStartedToCut { get { return "Zaczynasz obrabiac surowe ziolo..."; } }

		public WeedCropZiolaUprawne(int amount, int itemID) : base(itemID)
		{
			Amount = amount;
			//Weight = 0.5;
		}

		public WeedCropZiolaUprawne(Serial serial) : base(serial)
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
