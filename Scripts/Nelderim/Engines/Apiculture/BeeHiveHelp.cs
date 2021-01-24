using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Network;
using Server.Multis;
using Server.Targeting;

namespace Server.Engines.Apiculture
{	
	public class apiBeeHiveHelpGump : Gump
	{
		public apiBeeHiveHelpGump( Mobile from, int type ) : base( 20, 20 )
		{
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);
			AddBackground(37, 25, 386, 353, 3600);
            AddLabel(177, 42, 92, @"Pszczelarstwo - Pomoc");

			AddItem(32, 277, 3311);
			AddItem(30, 193, 3311);
			AddItem(29, 107, 3311);
			AddItem(28, 24, 3311);
			AddItem(386, 277, 3307);
			AddItem(387, 191, 3307);
			AddItem(388, 108, 3307);
			AddItem(385, 26, 3307);

			AddHtml( 59, 67, 342, 257, HelpText(type), true, true);
			AddButton(202, 333, 247, 248, 0, GumpButtonType.Reply, 0);
		}

		public string HelpText(int type)
		{
			string text = "";

			switch( type )
			{
				case 0:
				{

                    text += "<p><b>Pszczelarstwo</b> to rzemioslo zajmujace sie hodowla pszczol, wywodzace sie od <b>bartnictwa</b>. By rozpoczac swa droge w <b>pszczelarstwie</b>, wszystko czego potrzba to <b>zlecenie na ul</b> i teren z <b>kwiatami</b> i <b>woda</b>.</p>";
					text += "<p>Istnieja 3 stopnie rozwoju kolonii:</p>";
                    text += "<p><b>Kolonizacja</b> - ul wysyla zwiadowcow, by przeczesaly okolice w poszukiwaniu wody i kwiatow.</p>";
                    text += "<p><b>Rozmnazanie</b> - rozpoczyna sie skladanie jaj, ul przygotowuje sie do produkcji.</p>";
					text += "<p><b>Produkcja</b> - dojrzaly ul rozpoczyna produkcje miodu i wosku.</p>";
					text += "<p>Zdrowie ula mierzone jest dwojako: <b>ogolne zdrowie</b> i <b>wielkosc populacji pszczol</b>.</p>";
					text += "<p><b>Ogolne zdrowie</b> odpowiada sredniemu zdrowiu pszczol:</p>";
                    text += "<p><b>Prosperujace</b> - pszczoly sa ekstremalnie zdrowe. Prosperujaca kolonia produkuje zwiekszona ilosc miodu i wosku.</p>";
                    text += "<p><b>Zdrowe</b> - pszczoly sa zdrowe, produkuja miod i wosk.</p>";
                    text += "<p><b>Chore</b> - pszczoly sa chore, nie produkuja surowcow.</p>";
                    text += "<p><b>Umierajace</b> - bez szybkiej interakcji, populacja zacznie malec.</p>";
					text += "<p><b>Wielkosc populacji pszczol </b>mowi o przyblizonej ilosci pszczol w ulu. Wieksza ilosc pszczol niesie ze soba pewne konsekwncje. Wieksze ule potrzebuja dokladniejszego utrzymania. Wiecej wody i kwiatow jest potrzebna do utrzymania wiekszego ula (jednakze rowniez zasieg ula zwieksza wraz ze wzrostem populacji).  Jesli kondycja ula obnizy sie drastycznie, kolonia zacznie malec, zostawiajac po sobie pusty ul.</p>";
					text += "<p>Jak kazde zywe stworzenie, pszczoly sa podatne na ataki z zewnatrz ula.  Moga to byc bakterie lub choroby, pszczelarz ma jednak wiele sposob na ochrone ula.</p>";
					text += "<p><b>Duza mikstura odtrucia</b> moze byc uzyta do zwalczania chorob jak zgnilica i dyzenteria. Moga rowniez zneutralizowac trucizne.</p>";
					text += "<p><b>Mikstura mocnej trucizny</b> moze byc uzyta do zwalczenia insektow (takich jak woskowa cma) lub pasozytow (jak pszczela wesz) ktore zagrozily ulowi. Nalezy to robic ostroznie! Za duzo trucizny moze zaszkodzic pszczolom.</p>";
					text += "<p><b>Duza mikstura sily</b> moze dac ulowi odpornosc na zakazenia i choroby.</p>";
					text += "<p><b>Duza mikstura leczenia</b> moze byc uzyta by uleczyc pszczoly.</p>";
					text += "<p><b>Duza mikstura zrecznosci</b> daje pszczolom dodatkowa energie, by moc produkowac wiecej miodu. Moze zwiekszyc ilosc produkowanego miodu i wosku jak rowniez zasieg, w ktorym pszczoly beda szukac kwiatow i wody.</p>";
					text += "<p>Zarzadzanie i dbanie o ul jest mozliwe za pomoca <b>pszczelarskiego gumpa</b>. Prawie kazdy aspekt ula jest mozliwy do monitorowania przez niego. Po lewej stronie mozna zobaczyc statusy:</p>";
					text += "<p><b>Produkcja</b> - ten przycisk otwiera <b>gump produkcji</b> gdzie pszczelarz moze zebrac zasoby z ula.</p>";
					text += "<p><b>Infekcja</b> - czerwony lub zolty myslnik oznacza, ze ul jest zainfekowany przez pasozyty lub inne owady. Uzyj <b>trucizny</b>, by je zabic.</p>";
                    text += "<p><b>Choroba</b> - czerwony lub zolty myslnik oznacza, ze ul jest chory. Uzycie <b>mikstury odtrucia</b> pomoze pszczolom zwalczyc chorobe.</p>";
					text += "<p><b>Woda</b> - informuje o ilosci wody w otoczeniu ula. Badz ostrozny, woda przenosi bakterie, zbyt duzo wody moze uczunic ul podatnym na choroby.</p>";
					text += "<p><b>Kwiaty</b> - informuje o ilosci kwiatow w otoczeniu ula. Zbyt duzo kwiatow moze uczynic ul podatnym na infekcje. Rodzaj kwiatow wplywa na rodzaj miodu, ktory psczoly wytworza oraz moze nieznacznie zwiekszyc jego produkcje. Rodzaj kwiatow nie wplywa na ilosc wytwarzanego wosku.</p>";
					text += "<p><b>Notatki:</b> ul moze pomiescic do 100 tysiecy pszczol. Zdrowy ul moze zyc w nieskonczonosc, jednakze starszy ul jest bardziej podatny na choroby i infekcje.</p>";
                    text += "<p>Zasoby z ula mozna wydobywac za pomoca <b>narzedzia bartniczego</b>.</p>";
                    text += "<p>Rozrost ula i jego parametry <b>sprawdzane sa raz dziennie podczas ktoregos z zapisow gry.</b> Prawy gorny rog glownego gumpa, pokazuje rezultat ostateniego sprawdzenia:</p>";
					text += "<p><b><basefont color=#FF0000>! </basefont></b>Nie zdrowy</p>";
					text += "<p><b><basefont color=#FFFF00>! </basefont></b>Malo zasobow</p>";
					text += "<p><b><basefont color=#FF0000>- </basefont></b>Spadek populacji</p>";
					text += "<p><b><basefont color=#00FF00>+ </basefont></b>Wzrost populacji</p>";
					text += "<p><b><basefont color=#0000FF>+ </basefont></b>Rozwoj/Produkcja</p>";
					break;
				}
				case 1:
				{
					text +="<p>Wosk pszczeli jest wydobywany z ula w postaci nieprzetworzonego wosku pszczelego. W tej postaci jest zabrudzony, co uniemozliwia jego praktyczne uzycie. Nalezy go wyczyscic za pomoca <b>malego garnka na wosk</b>.</p>";
					text +="<p>Nieprzetworzony wosk pszczeli moze byc umieszczony w <b>malym garnku na wosk</b>. Nastepnie wosk mozna podgrzac w poblizu zrodla ciepla i usunac z niego zabrudzenia znane jako <b>pastura</b>.</p>";
                    text += "<p>Po usunieciu zabrudzen powstaje czysty wosk pszczeli. Moze byc uzyty na wiele sposobow. Mozna go przechowywac w <b>duzym garnku na wosk</b></p>";
					break;
				}
			}

			return text;
		}
	}
}
