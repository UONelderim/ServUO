namespace Server.Items
{
    public class FishingGuideBook2 : BaseBook
    {
        [Constructable]
        public FishingGuideBook2() : base(Utility.Random(0xFF1, 2), false)
        {
            Name = "Tom 2 - Niezwykle Ryby Morskie";
        }

        public static readonly BookContent Content = new BookContent
(
    null, "Kpt. Piddlewash",

    new BookPageInfo
    (
        "Amberjack:",

        "Pewnego dnia odwiesze",
        "wedke na kolek i otworze",
        "swoja wlasna browarnie,",
        "a moje piwo nazwe",
        "Amberjack."
    ),

    new BookPageInfo
    (
        "Czarny okon morski:",

        "Czarny okon morski ma",
        "moim zdaniem bardziej",
        "fioletowy kolor,",
        "ale to nie ja go nazwalem."
    ),

    new BookPageInfo
    (
        "Granika blekitna:",

        "Niech zielony kolor",
        "cie nie odstraszy,",
        "bo jest pyszna! Ci, co ja",
        "jedli, mowia, ze od niej",
        "oczy robia sie zielone."
    ),

    new BookPageInfo
    (
        "Niebieska ryba:",

        "Sa rzadkie, bo wiekszosc",
        "rybakow myli je z innymi",
        "rybami, poniewaz wcale",
        "nie sa niebieskie. To ich",
        "naturalna obrona."
    ),

    new BookPageInfo
    (
        "Ryba kostna:",

        "Ta ryba ma mnostwo kosci.",
        "Jakby podwojna normalna",
        "ilosc. Widzialem takie,",
        "ktore nie mogly sie",
        "nawet ruszyc!"
    ),

    new BookPageInfo
    (
        "Bonito:",

        "Bonito jest swietne, gdy",
        "jest wedzone i suszone.",
        "To przysmak Tokuno."
    ),

    new BookPageInfo
    (
        "Dorsz z Przyladka:",

        "Te rybe mozna znalezc",
        "przy przyladku. Daleko",
        "od przyladka. Tak daleko,",
        "ze az na srodku morza."
    ),

    new BookPageInfo
    (
        "Kapitan snook:",

        "Ktorys tam oszalaly",
        "marynarz nazwal tego",
        "biedaka kapitanem Snookiem,",
        "za co powinien zostac",
        "przeciagniety pod kilem!",
        "Znalem kapitana Snooka,",
        "to nie jest kapitan Snook."
    ),

    new BookPageInfo
    (
        "Kobia:",

        "Lepiej nie mylic kobii",
        "z kobra, bo kobra wymaga",
        "zupelnie innej przynety."
    ),

    new BookPageInfo
    (
        "Szary lucjan:",

        "Starzy marynarze mowia,",
        "ze wiele pokolen temu",
        "szary lucjan byl kiedys",
        "blond lucjanem."
    ),

    new BookPageInfo
    (
        "Lupacz:",

        "Gdy wiatr rozwiewa ci wlosy,",
        "sol osiada na wargach,",
        "a haczyk zaplatal sie",
        "w wedke - to znak,",
        "ze lupacz jest blisko."
    ),

    new BookPageInfo
    (
        "Mahi mahi:",

        "Mowia, ze najlepszy argument",
        "to powtarzanie.",
        "Moze to dlatego mahi mahi",
        "jest tak popularne."
    ),

    new BookPageInfo
    (
        "Czerwony drum:",

        "Czerwony drum nosi swoja",
        "nazwe od dzwieku, jaki",
        "wydaje, gdy uderzysz go",
        "w glowe."
    ),

    new BookPageInfo
    (
        "Czerwony granik:",

        "Czerwony granik smakuje",
        "jeszcze lepiej z odrobina",
        "pikantnego sosu Madam Beamy."
    ),

    new BookPageInfo
    (
        "Czerwony snook:",

        "Te rybe mozna znalezc",
        "wszedzie tam, gdzie sa",
        "pozostale ryby w tym poradniku."
    ),

    new BookPageInfo
    (
        "Sledz:",

        "Sledz to jedna z moich",
        "ulubionych rzadkich",
        "ryb glebinowych."
    ),

    new BookPageInfo
    (
        "Tarpon:",

        "Pewien gosc powiedzial mi,",
        "ze slowo ,,Tarpon,, pochodzi",
        "od slowa ,,Tarpaulin,,,",
        "ale jestem pewien, ze byl szalony."
    ),

    new BookPageInfo
    (
        "Tunczyk zoltopletwy:",

        "Najlepsze w tunczyku jest to,",
        "ze smakuje jak kurczak,",
        "ktorego zjadla ryba."
    )


);

        public override BookContent DefaultContent => Content;

        public FishingGuideBook2(Serial serial) : base(serial)
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
