namespace Server.Items
{
    public class FishingGuideBook5 : BaseBook
    {
        [Constructable]
        public FishingGuideBook5() : base(Utility.Random(0xFF1, 2), false)
        {
            Name = "Tom 5 - Kreatury z zaczarowanych morz";
        }

        public static readonly BookContent Content = new BookContent
(
null, "Cpt. Piddlewash",

new BookPageInfo
(
    "Jesienna Ryba Smocza:",

    "To piekno mozna",
    "znalezc w Ilshenar.",
    "Jesli przygotowane",
    "poprawnie i zjedzone,",
    "poprawia twoja",
    "zdolnosc do medytacji."
),

new BookPageInfo
(
    "Ryba Byk:",

    "Ryba byk jest",
    "znajdowana w",
    "labiryncie Malas.",
    "Jesli przygotowane",
    "poprawnie i zjedzone,",
    "zwieksza moc",
    "twojej reki z mieczem."
),

new BookPageInfo
(
    "Krysztalowa Ryba:",

    "Ta mistyczna ryba",
    "znajduje sie w",
    "pryzmacie swiatla. Jesli",
    "przygotowana poprawnie",
    "i zjedzona, chroni",
    "zeglarza przed",
    "uderzeniem pioruna."
),

new BookPageInfo
(
    "Bajkowy Losos:",

    "Ta smiala ryba",
    "plywa w rzekach",
    string.Format("{0}. Jesli przygotowany", FishInfo.GetFishLocation(typeof(FairySalmon))),
    "poprawnie i zjedzony,",
    "pomaga poprawic",
    "koncentracje zeglarza",
    "podczas rzucania zaklec."
),

new BookPageInfo
(
    "Ryba Ognista:",

    "Ta ryba znajduje sie",
    "w lochu Wstydu. ",
    "Jesli przygotowana",
    "poprawnie i zjedzona,",
    "chroni zeglarza",
    "przed ogniem."

),

new BookPageInfo
(
    "Gigantyczny Koi:",

    "Ta ryba znajduje sie w",
    "glebokich wodach Tokuno.",
    "Jesli przygotowana poprawnie",
    "i zjedzona, daje",
    "zeglarzowi zdolnosc",
    "do unikania ciosow."
),

new BookPageInfo
(
    "Wielka Barakuda:",

    "Ta ryba znajduje sie w",
    "glebokich wodach",
    "Felucca. Jesli przygotowana",
    "poprawnie i zjedzona,",
    "zwieksza twoja",
    "celnosc bronia."
),

new BookPageInfo
(
    "Swieta Makrela:",

    "Ta ryba znajduje sie w",
    "wodach Malas, pelnych duchow.",
    "Jesli przygotowana poprawnie i",
    "zjedzona, sprawia, ze",
    "szybciej zyskujesz mane."
),

new BookPageInfo
(
    "Ryba Lawowa:",

    "Ta ryba znajduje sie w",
    "lawowych rzekach",
    string.Format("{0}. Gdy ", FishInfo.GetFishLocation(typeof(LavaFish))),
    "przygotowana poprawnie i",
    "zjedzona, zwieksza twoja",
    "mane, gdy jestes",
    "ranny."
),

new BookPageInfo
(
    "Ryba Zniwiarz:",

    "Ta ryba znajduje sie w",
    "jeziorach lochu",
    "Zaglady. Jesli przygotowana",
    "poprawnie i zjedzona",
    "chroni cie przed",
    "obrazeniami od trucizny."
),

new BookPageInfo
(
    "Letnia Ryba Smocza:",

    "Ta piekna ryba",
    "znajduje sie w sadzawkach",
    "lochu Destard. ",
    "Jesli przygotowana poprawnie",
    "i zjedzona, zwiekszy",
    "obrazenia od zaklec."
),

new BookPageInfo
(
    "Ryba Jednorozec:",

    "Ta wspaniala ryba",
    "znajduje sie w Skreconym",
    "Gaju. Jesli przygotowana",
    "poprawnie i zjedzona,",
    "szybciej odzyskasz sily",
    "po zmeczeniu."
),

new BookPageInfo
(
    "Zoltoogonowa Barakuda:",

    "Ten diabel znajduje sie",
    "w glebokich wodach",
    "Trammel. Jesli przygotowana",
    "poprawnie i zjedzona,",
    "szybciej sie wyleczysz."
),

new BookPageInfo
(
    "Niebieski Homar:",

    "Ten homar wystepuje wylacznie",
    "w Lodowym Lochu. Jesli",
    "przygotowany poprawnie i",
    "zjedzony, chroni cie",
    "przed obrazeniami od zimna."
),

new BookPageInfo
(
    "Krab Pajak:",

    "Znaleziony w wodach",
    "Twierdzy Terathan. Jesli",
    "przygotowany poprawnie i",
    "zjedzony, poprawia twoja",
    "zdolnosc koncentracji."
),

new BookPageInfo
(
    "Krab Kamienny:",

    "Ten twardy klient",
    "znajduje sie w glebokim",
    "morzu Zaginionych Ziem.",
    "Jesli przygotowany poprawnie",
    "i zjedzony, sprawia, ze",
    "twoja skora jest twardsza."
)
);
        public override BookContent DefaultContent => Content;

        public FishingGuideBook5(Serial serial) : base(serial)
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
