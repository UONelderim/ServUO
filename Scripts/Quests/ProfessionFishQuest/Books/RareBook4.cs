namespace Server.Items
{
	public class FishingGuideBook4 : BaseBook
	{
		[Constructable]
		public FishingGuideBook4() : base(Utility.Random(0xFF1, 2), false)
		{
			Name = "Tom 4 - Niezwykle wody";
		}

		public static readonly BookContent Content = new BookContent
		(
			null, "Cpt. Piddlewash",

			new BookPageInfo
			(
				"krab jablkowy:",

				"Niektorzy mowia, ze krab",
				"jablkowy tak sie nazywa",
				"poniewaz robi dobry",
				"cydr. Na to",
				"mowie: fuj! "
			),

			new BookPageInfo
			(
				"Niebieski krab:",

				"Niebieskiego kraba mozna",
				"rozpoznac po tym, ze",
				"od spodu jest niebieski. ",
				" "
			),

			new BookPageInfo
			(
				"Krab lochowy:",

				"Krab lochowy zostal tak",
				"nazwany, poniewaz zostal",
				"po raz pierwszy odkryty w",
				"lochach, pozniej",
				"odkryto, ze mozna go",
				"znalezc wszedzie."
			),

			new BookPageInfo
			(
				"Krab krolewski:",

				"Zakon nie jest",
				"pewien, kto zrobil z",
				"tego lobuza krola, ale",
				"sadzimy, ze potrzeba",
				"bylo niezlego gadania."
			),

			new BookPageInfo
			(
				"Krab skalny:",

				"Krab skalny jest",
				"rzadki, glownie",
				"dlatego, ze",
				"czesto przypadkowo",
				"sie go depcze."

			),

			new BookPageInfo
			(
				"Krab sniezny:",

				"Wbrew powszechnemu",
				"przekonaniu, kraba",
				"snieznego nie znajdziesz",
				"w sniegu. Znajduja sie",
				"w wodzie, jak",
				"reszta krabow."
			),

			new BookPageInfo
			(
				"Kruchy homar:",

				"Juka lubi uzywac",
				"skorupy tego",
				"homara do ciasta na tarte. "
			),

			new BookPageInfo
			(
				"Homar Fred:",

				"Czasami zastanawiam",
				"sie, kim jest Fred?",
				"i jak nazwal homara?"
			),

			new BookPageInfo
			(
				"Homar brzeczacy:",

				"Niektorzy zeglarze mowia,",
				"ze slysza brzeczenie",
				"homara brzeczacego. Ale",
				"nie widze, zeby",
				"lapali wiecej niz",
				"inni."
			),

			new BookPageInfo
			(
				"Homar skalny:",

				"Homar skalny jest",
				"rzadki, glownie",
				"dlatego, ze",
				"czesto sa... Chwila,",
				"chyba juz to",
				"uzylem."
			),

			new BookPageInfo
			(
				"Homar o nosie lopatowym:",

				"Homar o nosie lopatowym",
				"ma plaski, nos",
				"podobny do lopaty,",
				"ktorego uzywa do kopania",
				"w piasku i ukrywania sie. "
			),

			new BookPageInfo
			(
				"Homar kolczasty:",

				"Homary kolczaste sa",
				"trudne dla pulapek,",
				"czasami, gdy probuja",
				"wejsc do pulapki,",
				"rozrywaja ja na kawalki. "
			)
		);

		public override BookContent DefaultContent => Content;

		public FishingGuideBook4(Serial serial) : base(serial)
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
