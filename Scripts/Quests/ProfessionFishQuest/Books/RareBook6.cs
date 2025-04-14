namespace Server.Items
{
	public class FishingGuideBook6 : BaseBook
	{
		[Constructable]
		public FishingGuideBook6() : base(Utility.Random(0xFF1, 2), false)
		{
			Name = "Tom 6 - Legendarne kreatury";
		}

		public static readonly BookContent Content = new BookContent
		(
			null, "Cpt. Piddlewash",

			new BookPageInfo
			(
				"Glebinowa Ryba Smocza:",

				"W bezdennych studniach",
				"Destard czai sie wiele",
				"niebezpieczenstw. Niektorzy mowia, ze",
				"plywa tam czarna ryba smocza."
			),

			new BookPageInfo
			(
				"Czarny Marlin:",

				"Gdzies na glebokich",
				"wodach Felucca, zeglarze",
				"mowia, ze zlapali czarnego",
				"marlina, ale uciekl."
			),

			new BookPageInfo
			(
				"Niebieski Marlin:",

				"Pewien stary zeglarz powiedzial",
				"mi kiedys, ze widzial niebieskiego",
				"marlina wyskakujacego z",
				"morz Trammel. To",
				"nigdy nie zostalo potwierdzone."
			),

			new BookPageInfo
			(
				"Lochowy Szczupak:",

				"W Terathan Keep znaleziono",
				"dziennik przy stercie kosci. ",
				"Zmarly rybak twierdzil, ze",
				"zlapal tam te rybe."
			),

			new BookPageInfo
			(
				"Gigantyczna Ryba Samuraj:",

				"Rybacy z Tokuno opowiadaja",
				"historie o starozytnych",
				"rybach samurajach o legendarnej",
				"wielkosci. Zadna z ich",
				"historii nigdy nie zostala",
				"potwierdzona."
			),

			new BookPageInfo
			(
				"Zloty Tunczyk:",

				"Ta ryba jest znana tylko",
				"w micie. Ale niektorzy",
				"wierza, ze istnieja w",
				"glebokich wodach",
				"Tokuno."
			),

			new BookPageInfo
			(
				"Zimowa Ryba Smocza:",

				"Lodowy Loch kryje",
				"wiele tajemnic, wiekszosc",
				"z nich cie zabije. ",
				"Ale jest legenda",
				"o rybie smoczej, ktora",
				"rzadzi tamtejszymi rzekami."
			),

			new BookPageInfo
			(
				"Krolewska Ryba:",

				"Krolewska ryba jest",
				"niezwykle rzadka.",
				"Mowia, ze Lord",
				"British zlapal jedna",
				"raz, ale to nigdy",
				"nie zostalo potwierdzone."
			),

			new BookPageInfo
			(
				"Ryba Latarnia:",

				"Mowi sie, ze ta ryba",
				"zyje w Pryzmacie",
				"Swiatla. Jednak, jak",
				"wiele legend, nigdy",
				"nie zostala potwierdzona."
			),

			new BookPageInfo
			(
				"Ryba Teczowa:",

				"Elfy opowiadaja historie",
				"o ksiezniczce, ktora wpadla",
				"do rzeki Skreconego",
				"Gaju i zostala",
				"zjedzona przez te nieuchwytna",
				"rybe."
			),

			new BookPageInfo
			(
				"Ryba Poszukiwacz:",

				"Historia tej ryby",
				"mowi, ze zablakala sie",
				"do Labiryntu Malas i",
				"zaginela. To dziwna",
				"historia z wieloma lukami,",
				"ale moze tam byc."
			),

			new BookPageInfo
			(
				"Wiosenna Ryba Smocza:",

				"Zanim Mistrzyni Kegwood",
				"zlapala jedna w Ilshenar,",
				"byly nieznane. Wisza w",
				"tajnej sali Zakonu",
				"Ryby Smoczej."
			),

			new BookPageInfo
			(
				"Ryba Kamienna:",

				"Kamienne harpie czcza",
				"wielka Kamienna rybe, ktora",
				"podobno spi na dnie",
				"morza w Zaginionych",
				"Ziemiach. Wielu czlonkow",
				"naszego zakonu stara sie ja zlapac."
			),

			new BookPageInfo
			(
				"Ryba Zombie:",

				"Mowi sie, ze w wodach",
				"Malas jest Nieumarla ryba.",
				"Niektorzy mowia, ze to bezbozny eksperyment,",
				"inni, ze to klamstwo."
			),

			new BookPageInfo
			(
				"Krwawy Homar:",

				"W glebinach lochu",
				"Wstydu podobno czai sie",
				"to dziwne stworzenie. Niektorzy",
				"mowia, ze zywi sie krwia",
				"poleglych."
			),

			new BookPageInfo
			(
				"Homar Grozy:",

				"Mowi sie, ze ten",
				"homar jest powodem,",
				"dla ktorego potwory nie wchodza",
				"do wod Zaglady."
			),

			new BookPageInfo
			(
				"Krab Tunelowy:",

				"Mowi sie, ze to stworzenie",
				"zyje w",
				string.Format("{0} pod Ognista", FishInfo.GetFishLocation(typeof(TunnelCrab))),
				"Wyspa. To goblinia",
				"legenda, wiec jest nieco",
				"podejrzana."
			),

			new BookPageInfo
			(
				"Krab Pustki:",

				"Niektorzy starzy rybacy w",
				string.Format("{0} mowia, ze widzieli", FishInfo.GetFishLocation(typeof(VoidCrab))),
				"kraba, ktory przypomina",
				"demona pustki w rzekach. To",
				"nie zostalo potwierdzone."
			),

			new BookPageInfo
			(
				"Homar Pustki:",

				"Gobliny z",
				string.Format("{0} opowiadaja o", FishInfo.GetFishLocation(typeof(VoidLobster))),
				"stworzeniu, ktore wyglada jak",
				"krzyzowka demona pustki",
				"i homara. Mowia, ze zyje",
				"tam w lawie."
			)
		);

		public override BookContent DefaultContent => Content;

		public FishingGuideBook6(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}
	}
}
