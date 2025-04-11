namespace Server.Items
{
    public class FishingGuideBook3 : BaseBook
    {
        [Constructable]
        public FishingGuideBook3() : base(Utility.Random(0xFF1, 2), false)
        {
            Name = "Tom 3 - Niezwykle Ryby Podmroczne";
        }

        public static readonly BookContent Content = new BookContent
(
    null, "Kpt. Piddlewash",

    new BookPageInfo
    (
        "Graniakowaty lucjan:",

        "Graniakowaty lucjan to",
        "wysmienita ryba.",
        "Tylko uwazaj na palce."
    ),

    new BookPageInfo
    (
        "Pstrag Podrzynacz:",

        "Ten lochowy postrach",
        "to wlasnie ten, ktory",
        "dal poczatek staremu",
        "powiedzeniu: 'Nigdy nie",
        "kap sie w wodzie z lochow.'"
    ),

    new BookPageInfo
    (
        "Mroczna ryba:",

        "Znajdziesz te rybe w",
        "podziemnych rzekach i",
        "jeziorach. Ale tylko w",
        "ciemnych podziemnych",
        "rzekach i jeziorach."
    ),

    new BookPageInfo
    (
        "Demoniczny pstrag:",

        "Uwazaj, ten diabelski",
        "potwor wychodzi z wody",
        "juz przyprawiony na ostro."
    ),

    new BookPageInfo
    (
        "Smocza ryba:",

        "Mniejszy kuzyn smoczej",
        "ryby, ten piekny okaz",
        "jest duzo latwiejszy",
        "do zlapania i dlatego",
        "czesciej uzywany w kuchni."
    ),

    new BookPageInfo
    (
        "Lochowy sledz:",

        "To jedyny podziemny",
        "przedstawiciel rodziny",
        "sledzi."
    ),

    new BookPageInfo
    (
        "Ponury Cisco:",

        "Ta ryba jest poszukiwana",
        "ze wzgledu na jej",
        "wlasciwosci lecznicze.",
        "Mowia, ze to najlepsze",
        "lekarstwo na histerie."
    ),

    new BookPageInfo
    (
        "Piekielny tunczyk:",

        "Ta ryba jest smiertelnie",
        "trujaca, chyba ze ja",
        "usmazysz na masle",
        "z odrobina tymianku",
        "i podasz z piwem."
    ),

    new BookPageInfo
    (
        "Ryba czatujaca:",

        "Te ryby lubia ukrywac",
        "sie pod unoszacymi sie",
        "w podziemnych rzekach",
        "zwlokami."
    ),

    new BookPageInfo
    (
        "Orczy okon:",

        "Jesli kiedys beda cie",
        "gonic orkowie, rzuc",
        "jednego i uciekaj dalej!",
        "Odkad zaczalem to",
        "opowiadac, sprzedaje",
        "wiecej orczych okoni."
    ),

    new BookPageInfo
    (
        "Zebata bassica:",

        "Ten lochowy drapieznik",
        "przypomina okonia",
        "szerokogebego, z ta",
        "roznica, ze ma ogromne,",
        "postrzepione zeby."
    ),

    new BookPageInfo
    (
        "Udreczony szczupak:",

        "Ten szczupak jest celem",
        "polowan niemal kazdego",
        "potwora w Sosarii,",
        "z wyjatkiem kilku."
    )
);


        public override BookContent DefaultContent => Content;

        public FishingGuideBook3(Serial serial) : base(serial)
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
