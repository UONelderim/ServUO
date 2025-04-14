namespace Server.Items
{
    public class FishingGuideBook1 : BaseBook
    {
        [Constructable]
        public FishingGuideBook1() : base(Utility.Random(0xFF1, 2), false)
        {
            Name = "Tom 1 - Niezwykle ryby brzegowe";
        }

        public static readonly BookContent Content = new BookContent
        (
            null, "Kapitan Fladroglowy",

            new BookPageInfo
            (
	            "Niebieski granik: ", 
        "Strzez sie tych niebieskich granikow, " ,
        "sa troche wyniosle, ale  ",
	       " swietnie smakuja na krakersie."
            ),

            new BookPageInfo
            (
	            "Pstrag potokowy: ", 
	           "Zwykle mozna go znalezc w strumykach",  
        "ale czasem tez w potokach, stawach",  
        "strumieniach, rzekach, doplywach",  
        "brodach, a okazjonalnie nawet w kaluzach."
            ),

            new BookPageInfo
            (
	            "Zielony sum:",  

	            "Niech zielony kolor",  
	            "cie nie odstraszy, bo jest", 
	            "pyszny! Ci, co go jedli",  
	            "mowia, ze od niego oczy",  
		            "robia sie zielone."
            ),

            new BookPageInfo
            (
	            "Losos Kokanee:",  

	            "Nazwalem go na czesc",  
	            "mojej ulubionej ciotki, liczac, ",
	            "ze zapisze mi swoj statek.",  
	            "Ale zostawila go swojemu",  
	            "chlopakowi, wiec zmienilem",  
	            "nazwe na Kokanee."
            ),

            new BookPageInfo
            (
	            "Szczupak:",

	            "Ta slodkowodna ryba wyglada",
	            "troche jak jej oceaniczny kuzyn,",
	            "barakuda. Ale nie daj sie",
	            "zmylic, gryzie!"

            ),

            new BookPageInfo
            (
	            "Slonecznica dyniowa:",

	            "Znajdowana w rzekach i innych",
	            "plytkich wodach, ta ryba nosi",
	            "swoja nazwe, bo jako pierwszy",
	            "zlowil ja moj przyjaciel,",
	            "Pumpkinseed Smith."
            ),

            new BookPageInfo
            (
	            "Pstrag teczowy:",

	            "Te pstragi maja kolory troche",
	            "jak teczowy losos, ale to nie losos,",
	            "to pstrag."
            ),

            new BookPageInfo
            (
	            "Leszcz czerwonobrzuchy:",

	            "Sekretem lowienia tych",
	            "szczegolnych leszczy jest",
	            "wedkowanie blisko brzegu."
            ),

            new BookPageInfo
            (
	            "Bass malogebowy:",

	            "Wierzy sie, ze ta ryba",
	            "jest rzadka po prostu dlatego,",
	            "ze jest wybredna w jedzeniu."
            ),

            new BookPageInfo
            (
	            "Niezwykly kielb:",

	            "Tej ryby nie nalezy mylic",
	            "ze zwyklym kielbem.",
	            "Niezwykly kielb smakuje",
	            "o wiele lepiej."
            ),

            new BookPageInfo
            (
	            "Sandacz:",

	            "To sprytny diabel,",
	            "bo widzi cie, jak nadchodzisz.",
	            "Najlepiej lowic go w nocy",
	            "albo zalozyc kostium robaka."
            ),

            new BookPageInfo
            (
	            "Okon zolty:",

	            "Czasami mozna je zobaczyc",
	            "plywajace w poblizu skal",
	            "i tym podobnych. Latwo je",
	            "dostrzec, bo maja zolty kolor",
	            "gdzies na ciele."
            )

        );

        public override BookContent DefaultContent => Content;

        public FishingGuideBook1(Serial serial) : base(serial)
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
