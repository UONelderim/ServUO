using System;
using Server;
using Server.Multis;
using Server.Mobiles;
using Server.Regions;
using Server.Targeting;
using System.Collections;
using Server.Network;
using System.Text;
using Server.Items;

namespace Server.Items
{
	public class QuestScroll : Item
	{
		///// CONFIGURE THE LOCATIONS HERE ///////////////////////////////////////////////////////////////
		// MAKE SURE YOU ADD THE X & Y & MAP TO THE "SECTION - LCXY1" LOWER IN THE SCRIPT ////////////////
		public static string[] Places1 { get { return m_Places1; } }
		public static string[] Places2 { get { return m_Places2; } }
		public static string[] Places3 { get { return m_Places3; } }
		public static string[] Places4 { get { return m_Places4; } }
		public static string[] Places5 { get { return m_Places5; } }
		public static string[] Places6 { get { return m_Places6; } }

		private static string[] m_Places1 = new string[]
		{
			"Tasandorskie Kanaly",
			"Lochy Ophidian",
			"Krolewskie Krypty"
		};

		private static string[] m_Places2 = new string[]
		{
			"Ruiny Elbrind",
			"Wulkan (LV 1)",
			"Krolewskie Krypty",
			"Ulnhyr Orbben",
			"Zwiedla Roza"
		};

		private static string[] m_Places3 = new string[]
		{
			"Jaskina Blyskow (LV 1)",
			"Krolewskie Krypty",
			"Zwiedla Roza",
			"Lochy Ophidian",
			"Wulkan (LV 1)",
			"Wulkan (LV 2)",
			"Saew",
			"Fort Orkow",
			"Tasandorskie Kanaly",
			"Swiatynia Hurengrav"
		};

		private static string[] m_Places4 = new string[]
		{
			"Lochy Ophidian",
			"Mechaniczna Krypta",
			"Swiatynia Hurengrav",
			"Krolewskie Krypty",
			"Ulnhyr Orbben",
			"Labirynt Minotaurow",
			"Alcala",
			"Hall Torech",
			"Wulkan (LV 2)",
			"Wulkan (LV 3)",
			"Tyr Reviaren"
		};

		private static string[] m_Places5 = new string[]
		{
			"Mechaniczna Krypta",
			"Wulkan (LV 3)",
			"Piramida",
			"Hall Torech",
			"Labirynt Minotaurow",
			"Alcala",
			"Lochy Ophidian",
			"Piaskowa Krypta",
			"Tasandorskie Kanaly",
			"Leze Lodowego Smoka"
		};

		private static string[] m_Places6 = new string[]
		{
			"Zwiedla Roza",
			"Tyr Reviaren",
			"Wulkan (LV 3)",
			"Wulkan (LV 4)",
			"Leze Lodowego Smoka",
			"Garth",
			"Lochy Ophidian"
		};

		///// CONFIGURE THE CREATURES HERE ///////////////////////////////////////////////////////////////
		// MAKE SURE YOU ADD THE TYPES TO THE "SECTION - MNTP1" LOWER IN THE SCRIPT //////////////////////
		public static string[] Monster1 { get { return m_Monster1; } }
		public static string[] Monster2 { get { return m_Monster2; } }
		public static string[] Monster3 { get { return m_Monster3; } }
		public static string[] Monster4 { get { return m_Monster4; } }
		public static string[] Monster5 { get { return m_Monster5; } }
		public static string[] Monster6 { get { return m_Monster6; } }

		private static string[] m_Monster1 = new string[]
		{
			

			"Szczuroczlek",
			"Szkielet",
			"Zombie",
			"Smok Bagienny"
		};

		private static string[] m_Monster2 = new string[]
		{
			"Banita",
			"Zywiolak Ziemi",
			"Ettin",
			"Mlody Gazer",
			"Ghul",
			"Ogromny Pajak",
		};

		private static string[] m_Monster3 = new string[]
		{
			"Zywiolak Agapitu",
			"Zywiolak Powietrza",
			"Kosciany Rycerz",
			"Kosciany Mag",
			"Zywiolak Brazu",
			"Zywiolak Miedzi",
			"Padliniak",
			"Zywiolak Krysztalu",
			"Cyclop",
			"Zywiolak Matowej Meidzi",
			"Ognisty Zuk",
			"Zywiolak Ognia",
			"Krwawy Golem",
			"Lodowy Sluz",
			"Sniezny Pajak",
			"Lodowy Troll",
			"Gargulec",
			"Gazer",
			"Ogromny Waz",
			"Zywiolak Zlota",
			"Rozpruwacz",
			"Piekielny Ogar",
			"Zywiolak Lodu",
			"Lodowy Waz",
			"Imp",
			"Ognisty Waz",
			"Minotaur",
			"Ogr",
			"Rycerz Ophidian",
			"Wojownik Ophidian",
			"Kapitan Orkow",
			"Lord Orkow",
			"Mag Orkow",
			"Pozszywany Szkielet",
			"Lucznik Szczuroczlekow",
			"Mag Szczuroczlekow",
			"Piaskowy Wir",
			"Dzikus",
			"Jezdziec Dzikusow",
			"Szaman Dzikusow",
			"Zywiolak Mrocznego Metalu",
			"Zywiolak Sniegu",
			"Kamienny Gargulec",
			"Kamienna Harpia",
			"Truten Terathan",
			"Wojownik Terathan",
			"Troll",
			"Zywiolak Valorytu",
			"Zywiolak Verytu"
		};

		private static string[] m_Monster4 = new string[]
		{
			"Moczarniak",
			"Centaur",
			"Smocze Piskle",
			"Tarantula",
			"Zly mag",
			"Mechaniczny Straznik",
			"Mechaniczny Obserwator",
			"Ognisty Gargulec",
			"Gargulec Niszczyciel",
			"Gargulec Msciciel",
			"Czarna Wdowa",
			"Golem",
			"Zarzaca Golemow",
			"Ice Serpent",
			"Ognisty Jaszczur",
			"Ogromny Ognisty Waz",
			"Licz",
			"Zwiadowca Minotaurow",
			"Mumia",
			"Lord Ogrow",
			"Starszy Mag Ophidian",
			"Mag Ophidian",
			"Matrichiani Ophidian",
			"Trzesawisko",
			"Bagienna Macka",
			"Msciciel Terathan",
			"Matrichiani Terathan",
			"Tytan",
			"Wywerna"
		};

		private static string[] m_Monster5 = new string[]
		{
			"Zywiolak Krwii",
			"Ifryt",
			"Prastary Gazer",
			"Lodowy Demon",
			"Juggernaut",
			"Lord Liczy",
			"Kapitan Minotaurow",
			"Bestia Plagi",
			"Zywiolak Trucizny",
			"Gnijace Zwloki",
			"Ogromny Srebrny Waz",
			"Zywiolak Kwasu"
		};

		private static string[] m_Monster6 = new string[]
		{
			
			"Wielki Demon Cienia",
			"Upadly Jednorozec",
			"Upadly Kirin",
			"Smok",
			"Lord Lodowych Ogrow",
			"Zuk Runiczny",
			"Ognisty Rumak",
			"Koszmar"

		};


		///// CONFIGURE THE ITEM STORY HERE ///////////////////////////////////////////////////////////////
		public static string[] Story1 { get { return m_Story1; } }

		private static string[] m_Story1 = new string[]
		{
			"Plotka glosi, ze jest w", "Przepowiednia mowi, ze jest w", "Starozytne teksty mowia, ze jest w",
			"Mowi sie, ze jest  w", "Mozna znalezc w", "Zagubiono te rzecz w",
			"Zostawiono w", "Ukryto w", "Stare mapy mowia, ze jest w", "Znaleziony zwoj mowi, ze jest w"
		};


		///// CONFIGURE THE MONSTER STORY HERE ///////////////////////////////////////////////////////////
		public static string[] Story2 { get { return m_Story2; } }
		public static string[] Story3 { get { return m_Story3; } }

		private static string[] m_Story2 = new string[]
		{
			"chce by zgladzic bestyje", "jest rzadny zemsty", "chce pomscic smierc syna",
			"chce je wyplenic z ziemi"
		};

		private static string[] m_Story3 = new string[]
		{
			"Dekretem", "Nagroda oferowana przez",
			"Za atak na", "Za sterroryzowanie", "Za smierc ojca", "Za smierc matki ",
			"Za napasc na", "Wydany przez"
		};


		///// CONFIGURE THE MAIN ITEMs HERE //////////////////////////////////////////////////////////////
		public static string[] Items1 { get { return m_Items1; } }

		private static string[] m_Items1 = new string[]
		{
			"Orb", "Kryszal", "Oko", "Klejnot", "Topor", "Pika", "Tarcz", "Rapier", "Miecz", "Kostur", "Kostka", "Glob", "Pierscien", 
			"Naszyjnik", "Amulet", "Ksiega", "Tom", "Mikstura", "Eliksir", "Zwoj", "Rozdzka", "Helm", "Rekawice", "Luk", "Naramienniki", "Kajdany",
			"Szata", "Plaszcz", "Rog", "Noz", "Sztylet", "Berlo", "Wlocznia", "Kozaki", "Kamien", "Wedka", "Harfa", "Kusza", "Korona", "Talizman",
			"Totem", "Idol", "Zbroja", "Pas", "Branzoleta"
		};


		///// CONFIGURE THE ENDING ADVJECTIVES HERE //////////////////////////////////////////////////////
		public static string[] Items2 { get { return m_Items2; } }

		private static string[] m_Items2 = new string[]
		{
			"Egzotyczny", "Tajemniczy", "Zaklety", "Wspanialy", "Nadzwyczajny", "Zatrwazajacy", "Mistyczny",
			 "Magiczny", "Boski",
			"Niesamowity", "Cudowny", "Nadzwyczajny", "Przerazajacy",
			"Straszliwy", "Straszny", "Niepokojacy", "Strachliwy", "Paskudny", "Posepny", "Ponury", "Zly", "Zagubiony",
			"Legendarny", "Mityczny", "Zagioniony", "Zgladzony", "Nieskonczony", "Tasandorski", "Elficki", "Drowii",
			"Sadystyczny", "Orkowy", "Duchowy", "Demoniczny", "Swiety", "Smierci", "Matki",
			"Pana", "Zdobiony", "Poszukiwacz", "Ogien", "Lod", "Zimno", "Energia", "Cialo", "Heretyk", "Kultysta", "Herdeista", "Mnich", "Straznik", "Orphidea", "Krasnolud", "Chciwy"
		};


		///// CONFIGURE THE STARTING ADJECTIVES HERE /////////////////////////////////////////////////////
		public static string[] Items3 { get { return m_Items3; } }

		private static string[] m_Items3 = new string[]
		{
			"Egzotyczny", "Tajemniczy", "Zaklety", "Wspanialy", "Nadzwyczajny", "Zatrwazajacy", "Mistyczny",
			 "Magiczny", "Boski",
			"Niesamowity", "Cudowny", "Nadzwyczajny", "Przerazajacy",
			"Straszliwy", "Straszny", "Niepokojacy", "Strachliwy", "Paskudny", "Posepny", "Ponury", "Zly", "Zagubiony",
			"Legendarny", "Mityczny", "Zagioniony", "Zgladzony", "Nieskonczony", "Tasandorski", "Elficki", "Drowii",
			"Sadystyczny", "Orkowy", "Duchowy", "Demoniczny", "Swiety", "Smierci", "Matki",
			"Pana", "Zdobiony"
		};

		///// CONFIGURE THE ITEM OWNERS HERE /////////////////////////////////////////////////////////////
		public static string[] Items4 { get { return m_Items4; } }

		private static string[] m_Items4 = new string[]
		{
			"Adaradle", "Cimaclidor", "Gertur", "Maldorink", "Robois", "Angunald", "Aade", "Mabias",
			"Adkyna", "Cinghin", "Gladh", "Marmosi", "Rondona", "Aurging", "Ddald", "Liding",
			"Aeferryfela", "Cinthereban", "Glasar", "Marrese", "Rothiw", "Cala", "Dhonl", "Madon",
			"Aehes", "Cirth", "Glassa", "Marri", "Ruadhad", "Cangla", "Dorome", "Maelowr",
			"Aemathan", "Gobar", "Marus", "Ruamnan", "Chan", "Eesa", "Maeluinus",
			"Aenchoba", "Cognvareding", "Gobharlas", "Marvaldiri", "Ruditherming", "Chuthmmi", "Eiba", "Lifferg",
			"Acwulf", "Colairion", "Gimran", "Marry", "Selin", "Dorthu", "Farine", "Litiny",
			"Adakonico", "Colan", "Ginou", "Marus", "Selrontus", "Falla", "Fkisl", "Llice",
			"Aeter", "Collsbe", "Godrica", "Marzhaetod", "Ryannen", "Fauauthencaro", "Gerrut", "Lonan",
			"Adegen", "Collullen", "Glederile", "Marvegis", "Senstarlough", "Gorgortuna", "Higafi", "Magda",
			"Agraien", "Colmgristrow", "Goibhinnach", "Matha", "Sabhrewi", "Hallalur", "Kalir", "Loron",
			"Agrimona", "Colphe", "Gollewyn", "Mattancheude", "Sadyn", "Haulo", "Kesig", "Maglor",
			"Adulf", "Comgalant", "Goibhin", "Mathere", "Seporvi", "Hmoturol", "Moc", "Malog",
			"Agulac", "Conandinas", "Gouthos", "Maxina", "Sadyn", "Kalli", "Oige", "Malpaigley",
			"Aieth", "Concouise", "Grevon", "Melain", "Saegalach", "Lalarornargw", "Rer", "Luches",
			"Aimboruin", "Concs", "Gunory", "Melan", "Saemglas", "Langororobat", "Ritdar", "Maagdvar",
			"Aesci", "Concuxoman", "Goibhir", "Mefferog", "Sequa", "Lkatugin", "Rud", "Macka",
			"Aesiry", "Conner", "Gomeri", "Meladrywood", "Serne", "Lungoth", "Rufar", "Malrickannie",
			"Aitharrely", "Connlugon", "Gwain", "Melathe", "Saiuche", "Miantu", "Sbit", "Malronna",
			"Aethennye", "Conovan", "Gondorcan", "Metta", "Seven", "Moguglk", "Sirtuf", "Maddi",
			"Alennifiel", "Coolis", "Gwalloyd", "Meldurie", "Sammarth", "Sair", "Suro", "Maolinn",
			"Aethosgorla", "Corbjalas", "Gorbis", "Milimbron", "Shaug", "Ugurirt", "Teddot", "Maeret",
			"Agius", "Cordberthyn", "Gorsaihirri", "Moire", "Sheelana", "Vangulurc", "Tedula", "Maglach",
			"Agnat", "Corflaneu", "Grachansien", "Molum", "Shenargharth", "Weglauli", "Tirik", "Maiet",
			"Aiblaithet", "Corina", "Granwy", "Monesthet", "Shenn", "Welrot", "Vada", "Malcolum",
			"Aille", "Coryssa", "Guolde", "Morga", "Shermund", "Alagula", "Aere", "Marcanovane",
			"Alfil", "Covedu", "Gwazeldan", "Meline", "Sandoralith", "Baimmagild", "Dalbal", "Salther",
			"Alherson", "Cregodsunnye", "Gwensin", "Melinneth", "Santhinter", "Caualorirchal", "Delga", "Praergaed",
			"Ailpin", "Creicath", "Gwene", "Morgan", "Shouenollus", "Chaldu", "Dir", "Samaneelene",
			"Alescforther", "Cucus", "Gwith", "Mortiphri", "Sibyll", "Gitulinaethegu", "Dired", "Samith",
			"Alewenna", "Cuintraya", "Haaric", "Moynteinvain", "Sibyrnach", "Hengona", "Fohor", "Sammen",
			"Alingvar", "Culbury", "Gwirkitta", "Meluf", "Saoignolf", "Kaert", "Garit", "Priel",
			"Alien", "Culleofer", "Hadrifiel", "Muirchar", "Signachne", "Kamothuro", "Grocr", "Priveliann",
			"Aliquesnan", "Cunedo", "Hariomhnaid", "Mylee", "Sigurbertur", "Kauiaz", "Hidte", "Prossoke",
			"Allan", "Cunomen", "Hayla", "Naiglossi", "Sipithne", "Korndur", "Hokede", "Pwyllgever",
			"Altheired", "Cunos", "Heiliach", "Nathianus", "Sirardolen", "Lkortul", "Kgedn", "Samporotic",
			"Alungoth", "Cyhilgaleth", "Helmhearlo", "Nealemberhae", "Smiss", "Lungodu", "Lgob", "Quienere",
			"Alvarth", "Cymox", "Helmott", "Nelan", "Solia", "Mamelaz", "Meti", "Sanben",
			"Amaerell", "Cynhel", "Helwyn", "Netheilos", "Sorod", "Nathimingorm", "Nbadr", "Sanberin",
			"Alpho", "Cynlo", "Gwynthian", "Mengar", "Saxus", "Nchammaglkaug", "Nir", "Santheiua",
			"Amalphyth", "Cynraenta", "Halann", "Merdan", "Scait", "Odulurchirordr", "Nondeb", "Quier",
			"Amervis", "Cyrus", "Hazra", "Mersides", "Schaed", "Rilalavangon", "Radavi", "Ragorten",
			"Ancelyndy", "Daalneylyn", "Hearbathekin", "Meryl", "Schondorn", "Rothi", "Rakrit", "Readwennor",
			"Andalf", "Daghallfreda", "Heatosus", "Mewanezel", "Scipirene", "Rthaetulauglu", "Rocnot", "Saury",
			"Anskarim", "Daille", "Hedden", "Millon", "Serezik", "Sakaugitur", "Rohag", "Seenad",
			"Aodre", "Dalbeth", "Helhervi", "Mindon", "Seuma", "Shmmelarirot", "Rreht", "Reosa",
			"Ammeidoc", "Dalie", "Hered", "Nicathrennur", "Spier", "Vandrthuindu", "Ruhdo", "Segast",
			"Amsyndser", "Dalwing", "Hervyn", "Niccus", "Sripthien", "Vaturirthm", "Rumibe", "Reose",
			"Apias", "Damalongan", "Hered", "Minmo", "Shaus", "Vaugberugia", "Simuyo", "Reosef",
			"Andabanberth", "Davetus", "Hildiwa", "Nijenn", "Steald", "Wergor", "Tarkor", "Rhybranen",
			"Anfast", "Deaga", "Hilgriwa", "Nimide", "Steine", "Baingurchul", "Agluah", "Ribus",
			"Aracbergtun", "Decyvedu", "Heredergana", "Moglos", "Sigdis", "Cathiaullrthmm", "Alruur", "Seibhing",
			"Anfela", "Dedegant", "Hires", "Norix", "Steinzena", "Chimak", "Arucraen", "Cathes",
			"Anfridge", "Degil", "Huntresa", "Nuarnoth", "Stwine", "Drndethmatugl", "Athotd", "Carnach",
			"Angborn", "Deiristo", "Hyldristick", "Ogmael", "Suald", "Dronaurndurdug", "Bariag", "Catinethiana",
			"Araddox", "Delany", "Hirmelis", "Moinsanth", "Siger", "Durorind", "Cahubraig", "Celotherich",
			"Aragorn", "Dener", "Hithur", "Molfrikinoth", "Sillovigfus", "Eglrorgundrugo", "Cihededt", "Certan",
			"Anjarmoth", "Denkth", "Iardall", "Oilbhe", "Sulish", "Encatu", "Dogihsosh", "Cassavusime",
			"Annal", "Derriht", "Iatachtach", "Oilfhil", "Svafnkell", "Fando", "Edatibulh", "Cassimurcia",
			"Arberyd", "Derufineryn", "Holas", "Molueli", "Slefsunsha", "Gurinca", "Habsaul", "Catisoth",
			"Ansellryeter", "Desth", "Ideardin", "Oranithos", "Swald", "Gwegorodulam", "Haglitr", "Cativ",
			"Ap-Owerkhes", "Devan", "Idwene", "Orcailifeth", "Sybet", "Ilkarirthar", "Hircalh", "Changaniam",
			"Amlait", "D'Evre", "Heofsa", "Nevettele", "Spant", "Ituth", "Huhahn", "Ceighterri",
			"Archite", "Diand'", "Hosbeth", "Morgar", "Sodenevrina", "Korchaug", "Ibnotn", "Chani",
			"Aptidh", "Dician", "Ilford", "Ordys", "Taigardus", "Korilo", "Ilheab", "Chastinia",
			"Argond", "Dilarion", "Hracye", "Morgonan", "Solas", "Kormiaturgo", "Inorelinl", "Chlinethian",
			"Ariwen", "Dimus", "Hrotheker", "Morix", "Sontz", "Ngogiak", "Itolraab", "Celeribe",
			"Aradys", "Dockermas", "Imchad", "Orefrfast", "Talig", "Ntulrolo", "Lalihg", "Celeved",
			"Arald", "Doireth", "Imlad", "Orody", "Tegar", "Ogoturdugldr", "Loceraoh", "Celte",
			"Arlethawluig", "Doirss", "Hroxie", "Morna", "Sorry", "Olar", "Nennuan", "Ciach",
			"Arhtshallmo", "Domnallys", "Ininna", "Oroke", "Tegarmail", "Orgo", "Olalehaad", "Cibus",
			"Arley", "Donath", "Inionna", "Orrianaid", "Telizez", "Rururch", "Sicatatr", "Celyan",
			"Arlygan", "Donchorundon", "Huailindrog", "Morwe", "Spaisia", "Sharua", "Tinariin", "Cinunnse",
			"Arlos", "Donnie", "Intha", "Osbeorsatus", "Telrichardo", "Vakazgl", "Ubidnert", "Cercnain",
			"Arnagh", "Driathink", "Hunter", "Morwenn", "Stebbi", "Vamimm", "Ulerteeh", "Chrono",
			"Arnethire", "Drovath", "Hwambrevonn", "Mothe", "Straid", "Akoduti", "Ablagr", "Ciacion",
			"Asberg", "Dryrymon", "Idelos", "Mouthiana", "Strogiel", "Asudad", "Adattang", "Ciarion",
			"Artuir", "Dubhe", "Ioardoc", "Otbond", "Tendiegisan", "Atadedak", "Adenalinl", "Clemma",
			"Athallyr", "Dubiggur", "Iehmassi", "Muadamulakk", "Surre", "Axeer", "Aralcail", "Fugeiria",
			"Avien", "Duilen", "Iseas", "Ottiriel", "Terfula", "Binye", "Aribihd", "Gablez",
			"Aylina", "Dumnagh", "Iethilo", "Muiriehmarus", "Sveinestel", "Edeline", "Atedetiet", "Gaess",
			"Avituc", "Dumnochobb", "Isotharic", "Overninus", "Terot", "Etifaca", "Bogatbius", "Gaillughaill",
			"Aylinsonse", "Eadan", "Ilchoard", "Mulnus", "Svert", "Lelhi", "Casbuit", "Galadrime",
			"Baishian", "Eafrikinn", "Imloth", "Murry", "Sween", "Mutydare", "Dunaet", "Galak",
			"Barladucus", "Eaneidh", "Imrilla", "Myles", "Swine", "Niyni", "Edelocoel", "Galduit",
			"Beardolde", "Earcorodrich", "Indingaer", "Naogurm", "Tadhagol", "Odadinir", "Ehalonoad", "Garrinaic",
			"Ayleribesta", "Ebemma", "Issch", "Palman", "Teseibhne", "Ohibarat", "Erdaud", "Garry",
			"Baith", "Eburn", "Jartman", "Paraps", "Teway", "Raruced", "Godreub", "Gatiarth",
			"Becca", "Edrich", "Inmouthor", "Naueritta", "Tallaith", "Rexaxrem", "Hatalitt", "Gauros",
			"Balyesmourn", "Edynoreth", "Jarvelaed", "Pawlynn", "Thats", "Roletizi", "Isniah", "Gavin",
			"Barrim", "Effroyd", "Jazma", "Peole", "Therly", "Royazar", "Lahhoas", "Gavinas",
			"Bedwick", "Eitin", "Intheodar", "Naugh", "Tanton", "Serez", "Lediag", "Gaylesh",
			"Bauisligal", "Eksiprepri", "Jokulan", "Perond", "Therto", "Tarar", "Naderbaah", "Geathet",
			"Bearus", "Eksisbury", "Kabristofa", "Pesifalas", "Thian", "Toxva", "Ogelath", "Geirdarnan",
			"Beleg", "Ekwesth", "Ioete", "Nealys", "Tanyalek", "Utaen", "Onarareab", "Gelege",
			"Belvana", "Ekwiremia", "Ismeneouuin", "Nechuff", "Tasses", "Vosa", "Ralcaal", "Gematurg",
			"Beley", "Elagus", "Kaithe", "Peutoust", "Thidric", "Xetaroka", "Rehatb", "Genildis",
			"Benciann", "Elbius", "Kaluth", "Phana", "Thiet", "Xira", "Renenirl", "Geofgivye",
			"Benou", "Elborn", "Istacheidir", "Neile", "Tefalaf", "Zetomyhe", "Rughend", "Geofraelisa",
			"Beornorth", "Elduin", "Ivalee", "Neldur", "TerSeekonny", "Zilohona", "Teluhoer", "Georht",
			"Benji", "Elian", "Keelyne", "Pothswineron", "Thikaden", "Adinikox", "Batemz", "Gerechula",
			"Bennonne", "Elich", "Kenear", "Prak-Zig", "Thryth", "Adivema", "Bed",
			"Bethimen", "Elimagus", "Jacloves", "Nemilinny", "Teriana", "Anami", "Besotg", "Gerrim",
			"Beribenzel", "Elissire", "Kenni", "Presaric", "Thuiley", "Arira", "Bmon", "Gilos",
			"Berthause", "Elricherick", "Kennie", "Priel", "Tianarus", "Azekis", "Bokesm", "Xabilie",
			"Bethe", "Elrong", "Kennock", "Purtlan", "Tigeri", "Cihal", "Bzagn", "Woodwyn",
			"Bjarma", "Elsecgren", "Jakebing", "Netta", "Thargoll", "Cizavix", "Dob", "Xandur",
			"Blathea", "Elstanly", "Jayden", "Nevalinnyn", "Thella", "Dasibmir", "Duterm", "Xaphorster",
			"Bitane", "Emmena", "Kevyn", "Qilla", "Tillentius", "Dirob", "Gabm", "Xelan",
			"Blatell", "Emyndenelda", "Kibes", "Quaethenzio", "Tilloc", "Elofivi", "Gmerx", "Wrough",
			"Blayne", "Emynyr", "Jayne", "Nicolm", "Theod", "Ferirded", "Gobadk", "Wulla",
			"Blina", "Endrai", "Jazliko", "Nides", "Think", "Hafde", "Gox", "Wynwoioi",
			"Bolbjora", "Eneelesonket", "Knutiltito", "Quebrand", "Tirina", "Ireet", "Gud", "Xabus",
			"Bowdyn", "Enoulheirc", "Kordurh", "Ragluman", "Tiriok", "Mirital", "Kaz", "Xiommish",
			"Boanna", "Entink", "Jenncho", "Nomond", "Thoces", "Nexerek", "Kusz", "Xavio",
			"Boriya", "Eochearnir", "Jesmonan", "Norody", "Thoren", "Nyse", "Mdos", "Xippeleg",
			"Bozef", "Eochuter", "Joakhanezah", "Norysset", "Thosgarenn", "Ratsa", "Mubazs", "Yarmotherl",
			"Brach", "Eogentyne", "Joevicca", "O'Neid", "Thowella", "Renyvar", "Nazogr", "Yourn",
			"Breas", "Eosarath", "Kriseaghy", "Raskars", "Toenbert", "Ridan", "Nes", "Xydrie",
			"Bradowfax", "Eosina", "Jonya", "Odhred", "Thrabent", "Rokan", "Rabx", "Xylan",
			"Brenzander", "Epilla", "Kyantesso", "Ravaciacus", "Torchon", "Sazikak", "Rgemad", "Yeers",
			"Brespal", "Erachlo", "Laistjane", "Refil", "Trach", "Suros", "Temg", "Zahna",
			"Breth", "Eride", "Lardullian", "Reidhg", "Trecumharust", "Tehidora", "Xanozt", "Zaximo",
			"Branabell", "Ermournen", "Joscale", "Olvagosa", "Tikingal", "Todxo", "Zagumx", "Yenoli",
			"Branden", "Erricia", "Josse", "Omnaltgaliam", "Tipher", "Uvalalil", "Zdusb", "Zepheron",
			"Brethur", "Esairfydd", "Laugurespath", "Rekwe", "Tresink", "Afazer", "Brog", "Ysfall",
			"Bridra", "Estendir", "Josuuarn", "Orgeralassa", "Tissia", "Ahoni", "Bxorem", "Zabel",
			"Brilionn", "Esubsiorsa", "Jowan", "Orielsecha", "Toduned", "Ateloba", "Gebakx", "Zabeni",
			"Bring", "Etarva", "Judigbryht", "Orret", "Tothryth", "Avkov", "Gox", "Zebin ",
			"Brochebig", "Etgera", "Kaellecton", "Osbeothe", "Traem", "Dixutir", "Gxomub", "Zozzo ",
			"Brockmarille", "Ethelmotor", "Karkus", "Osbertur", "Treen", "Donorauk", "Kmot", "Boh",
			"Brythaid", "Etrick", "Katanyalin", "Osripi", "Trilynton", "Hara", "Kunedb", "Dehoro",
			"Bretta", "Eultutney", "Lawaithburga", "Relar", "Treva", "Ihizan", "Med", "Del",
			"Burthgham", "Eurieth", "Katyrus", "Ossedelle", "Tuorne", "Iroharim", "Munads", "Detro",
			"Byrtelena", "Evernen", "Kavanora", "Osvinaugh", "Tutel", "Isubasak", "Nmabg", "Fadher",
			"Cadfang", "Evrenth", "Keena", "Othryth", "Twichos", "Itoso", "Ntuser", "Fircis",
			"Briatha", "Excolma", "Leach", "Remay", "Tronellen", "Ivirikor", "Nuxr", "Fsah",
			"Brichadha", "Exinan", "Leanna", "Rendore", "Truin", "Kohurbar", "Nzork", "Garde",
			"Brith", "Faireth", "Leide", "Reote", "Tuarad", "Lidebam", "Rmukg", "Gelnar",
			"Brocht", "Falmtyrnetta", "Leigh", "Reprick", "Tudfran", "Mitik", "Rtegob", "Geri",
			"Brook", "Falyndzai", "Lenoel", "Reprost", "Tyrnaster", "Ranereku", "Suk", "Hvad",
			"Brossies", "Fanghus", "Lerice", "Rettan", "Uilief", "Ronelioc", "Tax", "Iama",
			"Caela", "Fanus", "Kende", "Ottinus", "Uaignobe", "Roxalah", "Tmazx", "Ieva",
			"Bryales", "Fareth", "Lestril", "Rhyas", "Uiran", "Ryxox", "Toxb", "Iiro",
			"Caili", "Faria", "Kennos", "Owaynegalim", "Unpith", "Tayator", "Tzasb", "Kehmul",
			"Bryanna", "Farma", "Leuff", "Rikur", "Uriarmosink", "Teri", "Tzax", "Liler",
			"Caina", "Fenectus", "Kevin", "Padsto", "Unter", "Ulari", "Xbased", "Mnavb",
			"Cairex", "Feras", "Kinburh", "Palurial", "Vadrickael", "Uridol", "Xdukez", "Nrir",
			"Caith", "Ferrick", "Kiorte", "Pants", "Valinick", "Xarirab", "Xem", "Resrol",
			"Caithren", "Ferth", "Knutun", "Parkathant", "Varingwayn", "Ymasul", "Zrebom", "Rregr",
			"Calde", "Fevard", "Kolbye", "Pearus", "Vellw", "Aafi", "Baz", "Rudik",
			"Bryth", "Filbh", "Lilion", "Rivan", "Valan", "Aeyi", "Bmetog", "Rvas",
			"Budouth", "Filies", "Linnda", "Robert", "Vauberth", "Dlarr", "Bsadon", "Tat",
			"Bygyn", "Seekirianor", "Lithlea", "Roche", "Vaunt", "Fufiri", "Bsutax", "Uavi",
			"Byrhthe", "Finshep", "Livarzegin", "Rolyn", "Veneve", "Hide", "Daxonr", "Vidra",
			"Bytzer", "Firil", "Lizola", "Romar", "Waldor", "Hihe", "Der", "Zxotug",
			"Calduil", "Firth", "Kongalangwin", "Pelegoronth", "Vidan", "Hmuht", "Derg", "Videh",
			"Calhoun", "Fithur", "Koule", "Perann", "Vorwin", "Horden", "Dezt", "Zmedr",
			"Calion", "Flaenguallyn", "Krise", "Perman", "Vuall", "Iada", "Dox", "Zrak",
			"Calura", "Flaine", "Laimithifiet", "Persidius", "Waithe", "Kcah", "Gtekus", "Ttem",
			"Camremar", "Fobble", "Latiabair", "Pirogaidach", "Waken", "Kfabt", "Magebs", "Uote",
			"Carapharyn", "Forop", "Lebahar", "Pitinis", "Weidlyn", "Ldor", "Mkanog", "Wihen",
			"Cadha", "Fough", "Lliez", "Ronni", "Wartmanna", "Ledsat", "Mtog", "Wynfreen",
			"Calla", "Fowelinder", "Lobrandiern", "Ronya", "Wathfais", "Lodar", "Mxerb", "Possimpre",
			"Camrod", "Franie", "Lomerveinse", "Rorianton", "Weies", "Mis", "Ndet", "Saebertui",
			"Camrotmather", "Frarellastie", "Losinedyth", "Rorystach", "Wightiua", "Nnukn", "Nuz", "Leoses",
			"Caperes", "Fravela", "Lothiuture", "Roscollus", "Wmffricus", "Rarif", "Ran", "Lynne",
			"Caravis", "Freth", "Lebee", "Pleri", "Werni", "Reb", "Sbaxun", "Froighe",
			"Cardon", "Friennain", "Lotrissimac", "Roway", "Worth", "Rtak", "Sgor", "Froth",
			"Carrannin", "Frienny", "Lucham", "Runstina", "Wrainne", "Safo", "Snar", "Carin",
			"Casin", "Frietga", "Lugalle", "Rutha", "Wulffre", "Sarda", "Xgemz", "Casye",
			"Carilann", "Frith", "Leffer", "Ploughne", "Wichalish", "Tadri", "Xzeb",
			"Lord Blackthorn", "Mondain", "Conan", "Elric", "Merlin", "King Arthur", "Gandalf",
			"Bilbo", "Frodo", "Elrond", "Aragorn", "Lord British", "Thulsa Doom", "King Osric",
			"Valeria", "Zeddicus", "Rahl", "Venger", "Tiamat", "Aphrodite", "Ares", "Hades",
			"Hermes", "Zeus", "Poseidon", "Elminster", "Drizzt", "Yyrkoon", "Moonglum", "Djeryv",
			"Lord Blackthorn", "Mondain", "Conan", "Elric", "Merlin", "King Arthur", "Gandalf",
			"Bilbo", "Frodo", "Elrond", "Aragorn", "Lord British", "Thulsa Doom", "King Osric",
			"Valeria", "Zeddicus", "Rahl", "Venger", "Tiamat", "Aphrodite", "Ares", "Hades",
			"Hermes", "Zeus", "Poseidon", "Elminster", "Drizzt", "Yyrkoon", "Moonglum", "Djeryv",
			"Lord Blackthorn", "Mondain", "Conan", "Elric", "Merlin", "King Arthur", "Gandalf",
			"Bilbo", "Frodo", "Elrond", "Aragorn", "Lord British", "Thulsa Doom", "King Osric",
			"Valeria", "Zeddicus", "Rahl", "Venger", "Tiamat", "Aphrodite", "Ares", "Hades",
			"Hermes", "Zeus", "Poseidon", "Elminster", "Drizzt", "Yyrkoon", "Moonglum",
			"Agtatt", "Erlitl", "Irennaet", "Tararculc", "Asilnaor", "Denead", "Nonanlohl",
			"Ahadnebh", "Gorriin", "Lohiteir", "Toriah", "Astiib", "Dohalosb", "Nublaar",
			"Ahuhcods", "Helirars", "Ogriut", "Udgial", "Badgeal", "Enehtiln", "Onadaog",
			"Anelraul", "Horihitr", "Radatluor", "Uhatens", "Banideor", "Enolsast", "Ruganish",
			"Anrorc", "Ihatigeag", "Raliradr", "Adutenael", "Decollenb", "Etohaat", "Sacunsabn",
			"Atilcesn", "Ihsuhg", "Recergatl", "Alerrohr", "Direrl", "Galnaag", "Todtuhb",
			"Cunaht", "Ledlaub", "Tahedanb", "Anniut", "Edduhr", "Hanenteon", "Udahoic",
			"Deltagt", "Necnilr", "Tararculc", "Asilnaor", "Ernaan", "Hotoog", "Adaraniin",
			"Deneeg", "Nugancues", "Toriah", "Astiib", "Gatair", "Ihediit", "Adrosc",
			"Endelh", "Nuthoed", "Udgial", "Badgeal", "Henisabt", "Ilecurugg", "Agalihenr",
			"Erasaal", "Odirnuag", "Uhatens", "Banideor", "Ilrelt", "Larahn", "Ageneol",
			"Ergiet", "Raganebs", "Ahudieh", "Decollenb", "Irennaet", "Leluul", "Agluun",
			"Etnond", "Raraan", "Anohirirn", "Direrl", "Lohiteir", "Lerhind", "Agutasihh",
			"Gadalaug", "Relicenh", "Cehlaog", "Edduhr", "Ogriut", "Ohadaer", "Aletaab",
			"Habiot", "Silutiin", "Ecehnien", "Ernaan", "Radatluor", "Reroteuh", "Alsoab",
			"Hirern", "Sohbaln", "Eceluel", "Gatair", "Raliradr", "Adutenael", "Ataneon",
			"Isuteeb", "Tarahisd", "Ecurinaln", "Henisabt", "Recergatl", "Alerrohr", "Caralbenl",
			"Nitebecg", "Tinhaed", "Elaceah", "Ilrelt", "Tahedanb", "Anniut",
			"Ath", "Balmonth", "Baranth", "Bralmuth", "Briarananth", "Bucarth", "Bullinth", "Camalanth",
			"Carmath", "Caroth", "Cath", "Chaneth", "Charanuth", "Chatianth", "Colareth", "Coliath",
			"Craillanth", "Craimath", "Crairenarth", "Duvenath", "Emaleth", "Esianth", "Fith", "Galzieth",
			"Gamiath", "Gatianth", "Gebeth", "Gith", "Giyeth", "Glanarth", "Glath", "Gorianth", "Hallath",
			"Halzanth", "Iacanth", "Iasiluth", "Jemiath", "Jenith", "Jeranth", "Jeranuth", "Jeth", "Laneth",
			"Malanenth", "Mallieth", "Mareninth", "Menianth", "Meranuth", "Meth", "Mikenth", "Mikieth",
			"Mileth", "Mneniath", "Mneth", "Mogonth", "Morenelth", "Moth", "Naranoth", "Nenith", "Palararth",
			"Paloth", "Perinth", "Plath", "Polaneth", "Polarinth", "Polzeth", "Poraneth", "Porenorth",
			"Povarith", "Quessith", "Quevenanth", "Rolieth", "Rossoth", "Roth", "Sagrianth", "Samaloth",
			"Shayenth", "Sholzianth", "Shonniath", "Shoth", "Sidieth", "Spamath", "Spamuth", "Spath", "Speth",
			"Tagmuth", "Tarmuth", "Tiagorth", "Tiakoth", "Tiarath", "Trabieth", "Trananth", "Traranath",
			"Tueth", "Weth", "Wielgianth", "Wieneth", "Wienieth", "Wirmarth", "Lunamon", "Ioneron", "Reanthasala",
			"Tiamar", "Olrion", "Lunorx", "Jonin", "Olthonis", "Taleron", "Abraanthasala", "Lunadine", "Olthas",
			"Tiansa", "Valorx", "Talrion", "Caradine", "Shinikon", "Ti", "Takhansa", "Ollev", "Sirorx", "Abrae",
			"Riisis", "Lauralare", "Darrion", "Juderon", "Tieth", "Ri", "Takhorx", "Raist", "Riorx", "Shinadine",
			"Rias", "Nophean", "Amonter", "Antarahi", "Danyan", "Darangan", "Erantog", "Gandong", "Gioga", "Iguga",
			"Jimondan", "Jioga", "Jirondra", "Loga", "Ngran", "Ptangra", "Ptorag", "Rahiosar", "Ranttira", "Rosegh",
			"Sanyarar", "Sptor", "Varon", "Ytaro", "Ytingahi", "Zigodare", "Zilahed", "Bahiga", "Cora", "Egog",
			"Eragontt", "Gospt", "Hirah", "Hratapru", "Hrontr", "Jiran", "Jisa", "Larange", "Llan", "Ndrant", "Ngat",
			"Ntar", "Odzonte", "Onya", "Pigaheey", "Pimahrao", "Ragugieg", "Thraheg", "Uira", "Ulloda", "Vanyanga",
			"Yttongal", "Abotoram", "Bimo", "Cogo", "Contod", "Conya", "Daralora", "Dzig", "Erot", "Ghilong", "Guig",
			"Imegig", "Iong", "Londod", "Mispra", "Pisa", "Pronga", "Ptorahee", "Randrag", "Tahra", "Tong",
			"Ttilo", "Ugago", "Ugaraga", "Zontt", "Arangama", "Bigama", "Dongio", "Espispto", "Gimiosp", "Ilabimo",
			"Irall", "Irgui", "Jilarara", "Lant", "Lararo", "Largimol", "Mand", "Marab", "Ndron", "Ntorahra", "Onthi",
			"Ragrara", "Santega", "Segiong", "Ugra", "Ulor", "Vama", "Yarameg", "Yttigal", "Rieron", "Olrion",
			"Shinadine", "Chislev", "Caras",
			"Mulpelpe", "Ermohup", "Hehsiel", "Nahehoehsas", "Oratinael", "Eriel", "Pocaspomon", "Sorubiseriel",
			"Aropet", "Zases", "Tpahihuz", "Pael", "Erorasl", "Aknrar", "Ahelas", "Pontael", "Asiel", "Nopoz",
			"Rneenuziel", "Namolon", "Tuep", "Henbolaron", "Zedeson", "Assoaz", "Sotas", "Tadal", "Huslcir",
			"Rutselomiel", "Solael", "Saez", "Bettael", "Ampahoel", "Zatar", "Osaselael", "Irpsan", "Alael",
			"Pdutozab", "Luziel", "Tadon", "Asramel", "Aknoan", "Ahnet", "Unonom", "Xuksetpo", "Pemcapso", "Osapon",
			"Pimunael", "Esulamon", "Tuhkaraip", "Menepruron", "Ranaron", "Allaten", "Asoreb", "Razeniel",
			"Sapanolr", "Ticos", "Ussaxot", "Hahlraz", "Tedrahamael", "Apcbun", "Bahmensu", "Oron", "Xatnpih",
			"Opael", "Ipamorz", "Sanopars", "Exroh", "Lurtapios", "Tocpertaniel", "Islopaar", "Nizlpad", "Umlaboor",
			"Dalisatosiel", "Erammozal", "Amsaset", "Anhozal", "Irlap", "Nour", "Etnoxaad", "Imubenn", "Ezipexon",
			"Sihunosl", "Ehohit", "Zatbuhsatiel", "Sazsutpe", "Hipdiel", "Honed", "Unikesm", "Udazbanoe",
			"Zotipeposael", "Halmaneop", "Semnsat", "Itakup", "Ehhbes", "Araraz", "Halbasoon", "Nahahetael",
			"Ubeezh", "Ilpbeh", "Hilopael", "Esboot", "Usoparb", "Hotesiatrem", "Epnanaet", "Lehael", "Lapael",
			"Urapes", "Obalasc", "Pandael", "Damaz", "Ehnnat", "Pnecamob", "Ethahoat", "Kuhretieh", "Ekanzapis",
			"Pundohien", "Honolens", "Ahtuxies", "Asidodiel", "Laripael", "Bortarsariel", "Hirular", "Dunaneboriel",
			"Ensadaap", "Usraat", "Hahuhnerael", "Bolenoz", "Larhepeis", "Irasteheh", "Rarahaimzah", "Alahotael",
			"Litedabh", "Lasnuhtasael", "Unamah", "Topriraiz", "Rsaset", "Ansaap", "Opoasl", "Hetet", "Sotsopsehiel",
			"Kopozel", "Xartatesael", "Amosun", "Perensahael", "Chatomep", "Adiarh", "Loos", "Hador", "Sbeihapiel",
			"Odipin", "Tramater", "Mephotasael", "Mozed", "Depar", "Taron", "Anaobm", "Larpusmason", "Essaheah",
			"Suel", "Sahaminapiel", "Dinatoh", "Anrac", "Rasuniolpas", "Isoteciel", "Atcis", "Hatazeh", "Anhoor",
			"Samnerra", "Snilot", "Esasitm", "Arhel", "Rkesitas", "Hexpemsazon", "Izatap", "Ezon", "Koit", "Obasahiel",
			"Cerneplihael", "Husmled", "Hisipon", "Anolahon", "Ppaironael", "Lanetsosiel", "Olon", "Arpzih",
			"Ekarnahox", "Ibmnat", "Ipon", "Sutlanasael", "Srosirad", "Apomael", "Asemet", "Uknatias", "Nohon",
			"Maholab", "Sacuhatakael", "Orpir", "Bessipcopiel", "Isnal", "Elxar",
			"Aetrtevikr", "Airnucaha", "Alesei", "Amaniuevia", "Anendese", "Anentend", "Aneseicon", "Anonish",
			"Ardelica", "Argaluse", "Astharn", "Astiule", "Avacen", "Baliere", "Balucic", "Bardo", "Beserdei", "Bravamos",
			"Brliamide", "Cacosa", "Caleoleb", "Candrnial", "Cebalel", "Celele", "Chareviu", "Chenus", "Civon", "Danani",
			"Dandelene", "Davismarir", "Delel", "Deliusa", "Desalie", "Dethebe", "Doreti", "Drcatrda", "Dricor", "Ebali",
			"Ebelelu", "Einuce", "Elenenga", "Elestrn", "Elianam", "Elustule", "Ercikrl", "Erdor", "Erelucende", "Esanae",
			"Eseishan", "Esetucae", "Etralan", "Ezanelar", "Febrth", "Felele", "Felerelia", "Felidrane", "Femiard",
			"Fentrdes", "Feost", "Festhar", "Fezan", "Galelile", "Ganacas", "Gardet", "Gargda", "Garoneva", "Gdanue",
			"Gdestia", "Gdonisat", "Gdridordro", "Gebriustr", "Genebe", "Genievel", "Getich", "Gileli", "Hacelu", "Hachie",
			"Haleshi", "Hamazalesa", "Hangde", "Haret", "Havarl", "Helica", "Helichas", "Hetal", "Hianar", "Hicielen",
			"Hinasa", "Holusel", "Hononus", "Ialenanete", "Ianenel", "Ianthian", "Icamalius", "Ienag", "Ienive", "Ikriena",
			"Ilialerl", "Iuceosani", "Iulen", "Jercos", "Jucanin", "Juleva", "Juretri", "Jusara", "Jusaranus", "Justhieole",
			"Kraneb", "Kranie", "Krdrethag", "Kreoles", "Krevia", "Krgdei", "Krgdr", "Kriaster", "Krirni", "Krleleni",
			"Krlenagia", "Krneleba", "Krneleneb", "Lananar", "Lartius", "Laselilaza", "Lebrop", "Lelusaror", "Leorn",
			"Leramaman", "Letrtesend", "Libelem", "Liesm", "Lurneseler", "Luson", "Mabanteno", "Macaralen", "Maneleli",
			"Mantha", "Micon", "Miesth", "Miuli", "Moshardosh", "Nanar", "Neleiuc", "Ntholich", "Oleluc", "Olerusa", "Olusar",
			"Onanist", "Onaret", "Onetial", "Ongdastho", "Onicerus", "Orisel", "Orler", "Orosal", "Osaev", "Osazarna", "Phaletesma",
			"Phard", "Pheni", "Phesel", "Phesor", "Phienuebeb", "Phonu", "Ralus", "Rcanoluele", "Rcivamah", "Rdenaro", "Rdeth",
			"Relelete", "Rnara", "Rores", "Rorngane", "Saiabelel", "Sameti", "Santise", "Sareva", "Sasai", "Serantivet",
			"Sezarnti", "Smanda", "Smelieli", "Smetenelul", "Stiulenet", "Taleluso", "Tenda", "Tharanasan", "Thararin",
			"Thialardrd", "Tiusa", "Ucalelen", "Uchenter", "Uetrdrd", "Ulelen", "Ulerosh", "Uleseles", "Urndern", "Usant",
			"Usavamam", "Usonicar", "Usoris", "Vamorchine", "Vanebr", "Vargatu", "Vatielucac", "Velurelu", "Venez", "Vicav",
			"Viust", "Vondr", "Vorntes", "Vosthel", "Xacagalela", "Xaiel", "Xanismis", "Xarlic", "Xarnieta", "Zalebesan",
			"Zalemeta", "Zamint", "Zanabal", "Zanamai", "Zandoni", "Zaraluc", "Zaror", "Zatesor"
		};

		///// CONFIGURE THE FIRST PART OF A SPELL NAME HERE /////////////////////////////////////////////////////
		public static string[] Items5 { get { return m_Items5; } }

		private static string[] m_Items5 = new string[]
		{
			"Clyz", "Achug", "Theram", "Quale", "Lutin", "Gad",
			"Croeq", "Achund", "Therrisi", "Qualorm", "Lyeit", "Garaso",
			"Crul", "Ackhine", "Thritai", "Quaso", "Lyetonu", "Garck",
			"Cuina", "Ackult", "Tig", "Quealt", "Moin", "Garund",
			"Daror", "Aeny", "Tinalt", "Rador", "Moragh", "Ghagha",
			"Deet", "Aeru", "Tinkima", "Rakeld", "Morir", "Ghatas",
			"Deldrad", "Ageick", "Tinut", "Rancwor", "Morosy", "Gosul",
			"Deldrae", "Agemor", "Tonk", "Ranildu", "Mosat", "Hatalt",
			"Delz", "Aghai", "Tonolde", "Ranot", "Mosd", "Hatash",
			"Denad", "Ahiny", "Tonper", "Ranper", "Mosrt", "Hatque",
			"Denold", "Aldkely", "Torint", "Ransayi", "Mosyl", "Hatskel",
			"Denyl", "Aleler", "Trooph", "Ranzmor", "Moszight", "Hattia",
			"Drahono", "Anagh", "Turbelm", "Raydan", "Naldely", "Hiert",
			"Draold", "Anclor", "Uighta", "Rayxwor", "Nalusk", "Hinalde",
			"Dynal", "Anl", "Uinga", "Rhit", "Nalwar", "Hinall",
			"Dyndray", "Antack", "Umnt", "Risormy", "Nas", "Hindend",
			"Eacki", "Ardburo", "Undaughe", "Risshy", "Nat", "Iade",
			"Earda", "Ardmose", "Untdran", "Rodiz", "Nator", "Iaper",
			"Echal", "Ardurne", "Untld", "Rodkali", "Nayth", "Iass",
			"Echind", "Ardyn", "Uoso", "Rodrado", "Neil", "Iawy",
			"Echwaro", "Ashaugha", "Urnroth", "Roort", "Nenal", "Iechi",
			"Eeni", "Ashdend", "Urode", "Ruina", "Nowy", "Ightult",
			"Einea", "Ashye", "Uskdar", "Rynm", "Nia", "Ildaw",
			"Eldsera", "Asim", "Uskmdan", "Rynryna", "Nikim", "Ildoq",
			"Eldwen", "Athdra", "Usksough", "Ryns", "Nof", "Inabel",
			"Eldyril", "Athskel", "Usktoro", "Rynut", "Nook", "Inaony",
			"Elmkach", "Atkin", "Ustagee", "Samgha", "Nybage", "Inease",
			"Elmll", "Aughint", "Ustld", "Samnche", "Nyiy", "Ineegh",
			"Emath", "Aughthere", "Ustton", "Samssam", "Nyseld", "Ineiti",
			"Emengi", "Avery", "Verporm", "Sawor", "Nysklye", "Ineun",
			"Emild", "Awch", "Vesrade", "Sayimo", "Nyw", "Ingr",
			"Emmend", "Banend", "Voraughe", "Sayn", "Oasho", "Isbaugh",
			"Emnden", "Beac", "Vorril", "Sayskelu", "Oendy", "Islyei",
			"Endvelm", "Belan", "Vorunt", "Scheach", "Oenthi", "Issy",
			"Endych", "Beloz", "Whedan", "Scheyer", "Ohato", "Istin",
			"Engeh", "Beltiai", "Whisam", "Serat", "Oldack", "Iumo",
			"Engen", "Bliorm", "Whok", "Sernd", "Oldar", "Jyhin",
			"Engh", "Burold", "Worath", "Skell", "Oldr", "Jyon",
			"Engraki", "Buror", "Worav", "Skelser", "Oldtar", "Kalov",
			"Engroth", "Byt", "Worina", "Slim", "Omdser", "Kelol",
			"Engum", "Cakal", "Worryno", "Snaest", "Ond", "Kinser",
			"Enhech", "Carr", "Worunty", "Sniund", "Oron", "Koor",
			"Enina", "Cayld", "Worwaw", "Sosam", "Orrbel", "Lear",
			"Enk", "Cerar", "Yary", "Stayl", "Osnt", "Leert",
			"Enlald", "Cerl", "Yawi", "Stol", "Peright", "Legar",
			"Enskele", "Cerv", "Yena", "Strever", "Perpban", "Lerev",
			"Eoru", "Chaur", "Yero", "Swaih", "Phiunt", "Lerzshy",
			"Ernysi", "Chayn", "Yerrves", "Tagar", "Poll", "Llash",
			"Erque", "Cheimo", "Yhone", "Taienn", "Polrad", "Llotor",
			"Errusk", "Chekim", "Yradi", "Taiyild", "Polsera", "Loem",
			"Ervory", "Chreusk", "Zhugar", "Tanen", "Puon", "Loing",
			"Essisi", "Chrir", "Zirt", "Tasaf", "Quaev", "Lorelmo",
			"Essnd", "Chroelt", "Zoine", "Tasrr", "Quahang", "Lorud",
			"Estech", "Cloran", "Zotin", "Thaeng", "Qual", "Lour",
			"Estkunt", "Etoth", "Esule", "Estnight"
		};

		///// CONFIGURE THE SECOND PART OF A SPELL NAME HERE /////////////////////////////////////////////////////
		public static string[] Items6 { get { return m_Items6; } }

		private static string[] m_Items6 = new string[]
		{
			"Kwasowy", "Przywolanie", "Niejasny", "Zelazny", "Oslabiajacy",
			"Zmieniony", "Sekretny",  "Powiekszony", "Bezpieczny", "Staly", "Chetny", "Blyszczacy", "Przezroczysty", "Kontaktujacy",
			"Zwierzecy", "Telekinetyczny", "Prawy", "Zly", "Poruszajacy", "Niewidzialny", "Duchowy", "Energetyczny", "Kolorowy", "Prawdziwy", "Falszywy"
		};

		///// CONFIGURE THE THIRD PART OF A SPELL NAME HERE /////////////////////////////////////////////////////
		public static string[] Items7 { get { return m_Items7; } }

		private static string[] m_Items7 = new string[]
		{
			"Kwas", "Macki", "Sigil", "Zwyczajny", "Legenda", "Grawitacja", "Emocja", "Skrzynia",
			"Alarm", "Teren", "Trucizna", "Piorun", "Tluszcz", "Wytrzymalosc", "Krag",
			"Kotwica", "Mysli", "Skora", "Polimorfia", "Swiatla", "Wzrost", "Unerwienie", "Jasnowidzenie",
			"Zwierze", "Czas", "Sen", "Kuglarstwo", "Lokacja", "Straze", "Osłabienie", "Klon",
			"Antypatia", "Jezyki", "Dusza", "Projekcja", "Zamek", "Reka", "Wzmacniacz", "Chmura",
			"Arkana", "Dotyk", "Dzwiek", "Wiedza", "Pospiech", "Wiecznosc", "Zimno",
			"Zbroja", "Transformacja", "Zaklecia", "Schronienie", "Kapelusz", "Zlo", "Kolor",
			"Strzaly", "Pulapka", "Sfera", "Odrzut", "Magia", "Ogar", "Przywolywanie", "Zamet",
			"Aura", "Sztuczka", "Pajak", "Odpornosc"
		};

		// m_Places1 = LEVEL 1 AREA
		// m_Places2 = LEVEL 2 AREA
		// m_Places3 = LEVEL 3 AREA
		// m_Places4 = LEVEL 4 AREA
		// m_Places5 = LEVEL 5 AREA
		// m_Places6 = LEVEL 6 AREA
		// m_Monster1 = LEVEL 1 MONSTER
		// m_Monster2 = LEVEL 2 MONSTER
		// m_Monster3 = LEVEL 3 MONSTER
		// m_Monster4 = LEVEL 4 MONSTER
		// m_Monster5 = LEVEL 5 MONSTER
		// m_Monster6 = LEVEL 6 MONSTER
		// m_Items1 = ITEM NAME
		// m_Items2 = ENDING ADJECTIVES
		// m_Items3 = STARTING ADJECTIVES
		// m_Items4 = ITEM OWNERS
		// m_Items5 = SPELL NAME - 1ST
		// m_Items6 = SPELL NAME - 2ND
		// m_Items7 = SPELL NAME - 3RD

		public int mNNeed = 0;			// HOW MANY ARE NEEDED
		public int mNGot = 0;			// HOW MANY THE PLAYER GOT
		public int mNChance = 0;		// CHANCE TO GET WHAT IS NEEDED
		public int mNType = 0;			// TYPE OF QUEST 1=SLAY 2=SEEK
		public int mNLevel = 0;			// LEVEL
		public string mNItemName = "";		// NAME OF ITEM NEEDED
		public string mNMonsterType = "";	// TYPE OF MONSTER THAT NEEDS TO BE KILLED
		public string mNLocation = "";		// NAME OF DUNGEON THAT HAS THE ITEM
		public string mNStory = "";		// A SHORT STORY OF THE ITEM

		[CommandProperty(AccessLevel.GameMaster)]
		public int NNeed { get { return mNNeed; } set { mNNeed = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int NGot { get { return mNGot; } set { mNGot = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int NChance { get { return mNChance; } set { mNChance = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int NType { get { return mNType; } set { mNType = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int NLevel { get { return mNLevel; } set { mNLevel = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public string NItemName { get { return mNItemName; } set { mNItemName = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public string NMonsterType { get { return mNMonsterType; } set { mNMonsterType = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public string NLocation { get { return mNLocation; } set { mNLocation = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public string NStory { get { return mNStory; } set { mNStory = value; } }

		[Constructable]
		public QuestScroll( int level ) : base( 5360 )
		{
			//LootType = LootType.Blessed;
			Hue = 511;

			NType = Utility.RandomMinMax( 1/*, 2 */, 1); // TYPE OF QUEST

			if ( level > 0 )
			{
				NLevel = level; // QUEST LEVEL

				int tName = Utility.RandomMinMax( 1, 100 );

				if ( tName > 90 )
				{
					NItemName = m_Items5[Utility.Random(m_Items5.Length)] + "'zwoj " + m_Items6[Utility.Random(m_Items6.Length)] + " " + m_Items7[Utility.Random(m_Items7.Length)];
				}
				else if ( tName > 45 )
				{
					NItemName = " " + m_Items3[Utility.Random(m_Items3.Length)] + " " + m_Items1[Utility.Random(m_Items1.Length)] + " z " + m_Items4[Utility.Random(m_Items4.Length)];
				}
				else
				{
					NItemName = " " + m_Items1[Utility.Random(m_Items1.Length)] + " " + m_Items2[Utility.Random(m_Items2.Length)];
				}
			}

			if ( level == 1 )
			{
				NLocation = m_Places1[Utility.Random(m_Places1.Length)];
				NMonsterType = m_Monster1[Utility.Random(m_Monster1.Length)];
				NChance = Utility.RandomMinMax( 5, 30 );
				NNeed = Utility.RandomMinMax( 1, 25 );
			}

			else if ( level == 2 )
			{
				NLocation = m_Places2[Utility.Random(m_Places2.Length)];
				NMonsterType = m_Monster2[Utility.Random(m_Monster2.Length)];
				NChance = Utility.RandomMinMax( 10, 35 );
				NNeed = Utility.RandomMinMax( 1, 20 );
			}

			else if ( level == 3 )
			{
				NLocation = m_Places3[Utility.Random(m_Places3.Length)];
				NMonsterType = m_Monster3[Utility.Random(m_Monster3.Length)];
				NChance = Utility.RandomMinMax( 15, 40 );
				NNeed = Utility.RandomMinMax( 1, 15 );
			}

			else if ( level == 4 )
			{
				NLocation = m_Places4[Utility.Random(m_Places4.Length)];
				NMonsterType = m_Monster4[Utility.Random(m_Monster4.Length)];
				NChance = Utility.RandomMinMax( 20, 45 );
				NNeed = Utility.RandomMinMax( 1, 10 );
			}

			else if ( level == 5 )
			{
				NLocation = m_Places5[Utility.Random(m_Places5.Length)];
				NMonsterType = m_Monster5[Utility.Random(m_Monster5.Length)];
				NChance = Utility.RandomMinMax( 25, 55 );
				NNeed = Utility.RandomMinMax( 1, 5 );
			}

			else if ( level == 6 )
			{
				NLocation = m_Places6[Utility.Random(m_Places6.Length)];
				NMonsterType = m_Monster6[Utility.Random(m_Monster6.Length)];
				NChance = Utility.RandomMinMax( 30, 60 );
				NNeed = 1;
			}

			if ( NType == 1 )
			{
				Name = "Zgladz " + NMonsterType + " (" + NGot.ToString() + " " + NNeed.ToString() + ")";

				string sPerson = "";

				if ( Utility.RandomMinMax( 1, 2 ) == 1 )
				{
					sPerson = NameList.RandomName( "female" );
				}
				else
				{
					sPerson = NameList.RandomName( "male" );
				}

				if ( Utility.RandomMinMax( 1, 3 ) > 1 )
				{
					NStory = sPerson + " " + m_Story2[Utility.Random(m_Story2.Length)];
				}
				else
				{
					NStory = m_Story3[Utility.Random(m_Story3.Length)] + " " + sPerson;
				}
			}
			/*else
			{
				NNeed = 1;
				Name = "Odnajdz " + NItemName;
				NStory = m_Story1[Utility.Random(m_Story1.Length)] + " " + NLocation;
			}*/
		}

		public QuestScroll( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( NGot >= NNeed )
			{
				from.SendMessage( "To zadanie zostalo juz wypelnione!" );
			}
			else if ( NType == 1 )
			{
				from.SendMessage( "Wskaz cialo, ktore jest przedmiotem zadania." );
				from.SendMessage( "Cialo zniknie, gdy zostanie powiazane z zadaniem." );
				from.Target = new CorpseTarget( this );
			}
			else
			{
				from.SendMessage( "Ktore cialo chcialbys wskazac?" );
				from.SendMessage( "Cialo zniknie, gdy zostanie powiazane z zadaniem." );
				from.Target = new CorpseTarget( this );
			}
		}

		private class CorpseTarget : Target
		{
			private QuestScroll m_Quest;

			public CorpseTarget( QuestScroll quest ) : base( 3, false, TargetFlags.None )
			{
				m_Quest = quest;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Quest.Deleted )
					return;

				if (!(targeted is Corpse))
				{
					from.SendLocalizedMessage( 1042600 ); // That is not a corpse!
				}
				else if ( !m_Quest.IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				}
				else
				{
					object obj = targeted;

					Corpse c = (Corpse)targeted;

					Type M_Type = typeof(Orc);

					int nSpot = 0;



					// SECTION - MNTP1 ////////////////////////////////////////////////////////////////////////

					// THIS SECTION FINDS THE TYPES OF MONSTERS...SINCE I CANNOT SEEM TO CONVERT THE NAMES

					if ( m_Quest.NMonsterType == "Diamentowy Smok" ) { M_Type = typeof(DiamondDragon); }
					else if ( m_Quest.NMonsterType == "Mlody rubinowy smok" ) { M_Type = typeof(RubyDrake); }
					else if ( m_Quest.NMonsterType == "Gnaw" ) { M_Type = typeof(Gnaw); }
					else if ( m_Quest.NMonsterType == "Spaczona wrozka" ) { M_Type = typeof(Changeling); }
					else if ( m_Quest.NMonsterType == "Zloty Kolos" ) { M_Type = typeof(GoldenColossus); }
					else if ( m_Quest.NMonsterType == "Zywiolak Brazu" ) { M_Type = typeof(BronzeElemental); }
					else if ( m_Quest.NMonsterType == "Ghul" ) { M_Type = typeof(Ghoul); }
					else if ( m_Quest.NMonsterType == "Szkielet" ) { M_Type = typeof(Skeleton); }
					else if ( m_Quest.NMonsterType == "Zly Mag" ) { M_Type = typeof(EvilMage); }
					else if ( m_Quest.NMonsterType == "Zombie" ) { M_Type = typeof(Zombie); }
					else if ( m_Quest.NMonsterType == "Imp" ) { M_Type = typeof(Imp); }
					else if ( m_Quest.NMonsterType == "Zywiolak Ziemi" ) { M_Type = typeof(EarthElemental); }
					else if ( m_Quest.NMonsterType == "Ettin" ) { M_Type = typeof(Ettin); }
					else if ( m_Quest.NMonsterType == "Mlody Gazer" ) { M_Type = typeof(GazerLarva); }
					else if ( m_Quest.NMonsterType == "Zjawa" ) { M_Type = typeof(Wraith); }
					else if ( m_Quest.NMonsterType == "Ogromny Pajak" ) { M_Type = typeof(GiantSpider); }
					else if ( m_Quest.NMonsterType == "Cyklop" ) { M_Type = typeof(Cyclops); }
					else if ( m_Quest.NMonsterType == "Pani Sniegu" ) { M_Type = typeof(LadyOfTheSnow); }
					else if ( m_Quest.NMonsterType == "Mlody Lodowy Smok" ) { M_Type = typeof(MlodyLodowySmok); }
					else if ( m_Quest.NMonsterType == "Zywiolak Lodu" ) { M_Type = typeof(IceElemental); }
					else if ( m_Quest.NMonsterType == "Zywiolak Ognia" ) { M_Type = typeof(FireElemental); }
					else if ( m_Quest.NMonsterType == "Ognisty Gargulec" ) { M_Type = typeof(FireGargoyle); }
					else if ( m_Quest.NMonsterType == "Ognisty Rumak" ) { M_Type = typeof(FireSteed); }
					else if ( m_Quest.NMonsterType == "Wojownik Yomotsu" ) { M_Type = typeof(YomotsuWarrior); }
					else if ( m_Quest.NMonsterType == "Zywiolak Agapitu" ) { M_Type = typeof(AgapiteElemental); }
					else if ( m_Quest.NMonsterType == "Zywiolak Powietrza" ) { M_Type = typeof(AirElemental); }
					else if ( m_Quest.NMonsterType == "Banita" ) { M_Type = typeof(Brigand); }
					else if ( m_Quest.NMonsterType == "Mag Morrlokow" ) { M_Type = typeof(MagMorrlok); }
					else if ( m_Quest.NMonsterType == "Morderca Morrlokow" ) { M_Type = typeof(MordercaMorrlok); }
					else if ( m_Quest.NMonsterType == "Trzesawisko" ) { M_Type = typeof(Quagmire); }
					else if ( m_Quest.NMonsterType == "Prastary Gazer" ) { M_Type = typeof(ElderGazer); }
					else if ( m_Quest.NMonsterType == "Wojownik Juka" ) { M_Type = typeof(JukaWarrior); }
					else if ( m_Quest.NMonsterType == "Przeklety Rycerz" ) { M_Type = typeof(KhaldunZealot); }
					else if ( m_Quest.NMonsterType == "Padliniak" ) { M_Type = typeof(Corpser); }
					else if ( m_Quest.NMonsterType == "Zywiolak Krysztalu" ) { M_Type = typeof(CrystalElemental); }
					else if ( m_Quest.NMonsterType == "Przeklety Mag" ) { M_Type = typeof(KhaldunSummoner); }
					else if ( m_Quest.NMonsterType == "Zywiolak Matowej Miedzi" ) { M_Type = typeof(DullCopperElemental); }
					else if ( m_Quest.NMonsterType == "Ognisty Zuk" ) { M_Type = typeof(FireBeetle); }
					else if ( m_Quest.NMonsterType == "Upadly Jednorozec" ) { M_Type = typeof(UpadlyJednorozec); }
					else if ( m_Quest.NMonsterType == "Smuga Cienia" ) { M_Type = typeof(ShadowWisp); }
					else if ( m_Quest.NMonsterType == "Lodowy Sluz" ) { M_Type = typeof(FrostOoze); }
					else if ( m_Quest.NMonsterType == "Lodowy Pajak" ) { M_Type = typeof(FrostSpider); }
					else if ( m_Quest.NMonsterType == "Lodowy Troll" ) { M_Type = typeof(FrostTroll); }
					else if ( m_Quest.NMonsterType == "Gargulec" ) { M_Type = typeof(Gargoyle); }
					else if ( m_Quest.NMonsterType == "Gazer" ) { M_Type = typeof(Gazer); }
					else if ( m_Quest.NMonsterType == "Ogromny Waz" ) { M_Type = typeof(GiantSerpent); }
					else if ( m_Quest.NMonsterType == "Zywiolak Zlota" ) { M_Type = typeof(GoldenElemental); }
					else if ( m_Quest.NMonsterType == "Rozpruwacz" ) { M_Type = typeof(GoreFiend); }
					else if ( m_Quest.NMonsterType == "Piekielny Ogar" ) { M_Type = typeof(HellHound); }
					else if ( m_Quest.NMonsterType == "Upadly Kirin" ) { M_Type = typeof(UpadlyKirin); }
					else if ( m_Quest.NMonsterType == "Lodowy Waz" ) { M_Type = typeof(IceSnake); }
					else if ( m_Quest.NMonsterType == "Zdrajca" ) { M_Type = typeof(Betrayer); }
					else if ( m_Quest.NMonsterType == "Ognisty Waz" ) { M_Type = typeof(LavaSnake); }
					else if ( m_Quest.NMonsterType == "Minotaur" ) { M_Type = typeof(Minotaur); }
					else if ( m_Quest.NMonsterType == "Ogr" ) { M_Type = typeof(Ogre); }
					else if ( m_Quest.NMonsterType == "Rycerz Ophidian" ) { M_Type = typeof(OphidianKnight); }
					else if ( m_Quest.NMonsterType == "Wojownik Ophidian" ) { M_Type = typeof(OphidianWarrior); }
					else if ( m_Quest.NMonsterType == "Ork" ) { M_Type = typeof(Orc); }
					else if ( m_Quest.NMonsterType == "Kapitan Orkow" ) { M_Type = typeof(OrcCaptain); }
					else if ( m_Quest.NMonsterType == "Lord Orkow" ) { M_Type = typeof(OrcishLord); }
					else if ( m_Quest.NMonsterType == "Mag Orkow" ) { M_Type = typeof(OrcishMage); }
					else if ( m_Quest.NMonsterType == "Pozszywany Szkielet" ) { M_Type = typeof(PatchworkSkeleton); }
					else if ( m_Quest.NMonsterType == "Lucznik Szczuroczlekow" ) { M_Type = typeof(RatmanArcher); }
					else if ( m_Quest.NMonsterType == "Mag Szczuroczlekow" ) { M_Type = typeof(RatmanMage); }
					else if ( m_Quest.NMonsterType == "Wielki Demon Chaosu" ) { M_Type = typeof(GreaterChaosDaemon); }
					else if ( m_Quest.NMonsterType == "Piaskowy WIr" ) { M_Type = typeof(SandVortex); }
					else if ( m_Quest.NMonsterType == "Wojownik Dzikusow" ) { M_Type = typeof(Savage); }
					else if ( m_Quest.NMonsterType == "Jezdziec Dzikusow" ) { M_Type = typeof(SavageRider); }
					else if ( m_Quest.NMonsterType == "Szaman Dzikusow" ) { M_Type = typeof(SavageShaman); }
					else if ( m_Quest.NMonsterType == "Zywiolak Mrocznego Metalu" ) { M_Type = typeof(ShadowIronElemental); }
					else if ( m_Quest.NMonsterType == "Kosciany Rycerz" ) { M_Type = typeof(BoneKnight); }
					else if ( m_Quest.NMonsterType == "Kosciany Mag" ) { M_Type = typeof(BoneMagi); }
					else if ( m_Quest.NMonsterType == "Zywiolak Sniegu" ) { M_Type = typeof(SnowElemental); }
					else if ( m_Quest.NMonsterType == "Kamienny Gargulec" ) { M_Type = typeof(StoneGargoyle); }
					else if ( m_Quest.NMonsterType == "Kamienna Harpia" ) { M_Type = typeof(StoneHarpy); }
					else if ( m_Quest.NMonsterType == "Truten Terathan" ) { M_Type = typeof(TerathanDrone); }
					else if ( m_Quest.NMonsterType == "Wojownik Terathan" ) { M_Type = typeof(TerathanWarrior); }
					else if ( m_Quest.NMonsterType == "Troll" ) { M_Type = typeof(Troll); }
					else if ( m_Quest.NMonsterType == "Zywiolak Valorytu" ) { M_Type = typeof(ValoriteElemental); }
					else if ( m_Quest.NMonsterType == "Nietoperz Wampir" ) { M_Type = typeof(VampireBat); }
					else if ( m_Quest.NMonsterType == "Zywiolak Verytu" ) { M_Type = typeof(VeriteElemental); }
					else if ( m_Quest.NMonsterType == "Juggernaut" ) { M_Type = typeof(Juggernaut); }
					else if ( m_Quest.NMonsterType == "Mechaniczny Obserwator" ) { M_Type = typeof(ExodusOverseer); }
					else if ( m_Quest.NMonsterType == "Moczarniak" ) { M_Type = typeof(BogThing); }
					else if ( m_Quest.NMonsterType == "Centaur" ) { M_Type = typeof(Centaur); }
					else if ( m_Quest.NMonsterType == "Smocze Piskle" ) { M_Type = typeof(Drake); }
					else if ( m_Quest.NMonsterType == "Tarantula" ) { M_Type = typeof(DreadSpider); }
					else if ( m_Quest.NMonsterType == "Mechaniczny Straznik" ) { M_Type = typeof(ExodusMinion); }
					else if ( m_Quest.NMonsterType == "Gargulec Niszczyciel" ) { M_Type = typeof(GargoyleDestroyer); }
					else if ( m_Quest.NMonsterType == "Gargoyle Msciciel" ) { M_Type = typeof(GargoyleEnforcer); }
					else if ( m_Quest.NMonsterType == "Czarna wdowa" ) { M_Type = typeof(GiantBlackWidow); }
					else if ( m_Quest.NMonsterType == "Golem" ) { M_Type = typeof(Golem); }
					else if ( m_Quest.NMonsterType == "Nadzorca Golemow" ) { M_Type = typeof(GolemController); }
					else if ( m_Quest.NMonsterType == "Ogromny Lodowy Waz" ) { M_Type = typeof(IceSerpent); }
					else if ( m_Quest.NMonsterType == "Ogisty Jaszczur" ) { M_Type = typeof(LavaLizard); }
					else if ( m_Quest.NMonsterType == "Ogromny Ognisty Waz" ) { M_Type = typeof(LavaSerpent); }
					else if ( m_Quest.NMonsterType == "Licz" ) { M_Type = typeof(Lich); }
					else if ( m_Quest.NMonsterType == "Zwiadowca Minotaurow" ) { M_Type = typeof(MinotaurScout); }
					else if ( m_Quest.NMonsterType == "Mumia" ) { M_Type = typeof(Mummy); }
					else if ( m_Quest.NMonsterType == "Lord Ogrow" ) { M_Type = typeof(OgreLord); }
					else if ( m_Quest.NMonsterType == "Starszy Mag Ophidian" ) { M_Type = typeof(OphidianArchmage); }
					else if ( m_Quest.NMonsterType == "Mag Ophidian" ) { M_Type = typeof(OphidianMage); }
					else if ( m_Quest.NMonsterType == "Matriarchini Ophidian" ) { M_Type = typeof(OphidianMatriarch); }
					else if ( m_Quest.NMonsterType == "Trzesawisko" ) { M_Type = typeof(Quagmire); }
					else if ( m_Quest.NMonsterType == "Zuk Runiczny" ) { M_Type = typeof(RuneBeetle); }
					else if ( m_Quest.NMonsterType == "Smok Bagienny" ) { M_Type = typeof(SwampDragon); }
					else if ( m_Quest.NMonsterType == "bagienna Macka" ) { M_Type = typeof(SwampTentacle); }
					else if ( m_Quest.NMonsterType == "Msciciel Terathan" ) { M_Type = typeof(TerathanAvenger); }
					else if ( m_Quest.NMonsterType == "Matriarchini Terathan" ) { M_Type = typeof(TerathanMatriarch); }
					else if ( m_Quest.NMonsterType == "Tytan" ) { M_Type = typeof(Titan); }
					else if ( m_Quest.NMonsterType == "Wywerna" ) { M_Type = typeof(Wyvern); }
					else if ( m_Quest.NMonsterType == "Lord Snieznych Ogrow" ) { M_Type = typeof(ArcticOgreLord); }
					else if ( m_Quest.NMonsterType == "Zywiolak Krwii" ) { M_Type = typeof(BloodElemental); }
					else if ( m_Quest.NMonsterType == "Wielki Demon Cienia" ) { M_Type = typeof(GreaterArcaneDaemon); }
					else if ( m_Quest.NMonsterType == "Ifryt" ) { M_Type = typeof(Efreet); }
					else if ( m_Quest.NMonsterType == "Lord Liczy" ) { M_Type = typeof(LichLord); }
					else if ( m_Quest.NMonsterType == "Kapitan Minotaurow" ) { M_Type = typeof(MinotaurCaptain); }
					else if ( m_Quest.NMonsterType == "Koszmar" ) { M_Type = typeof(Nightmare); }
					else if ( m_Quest.NMonsterType == "Bestia Plagi" ) { M_Type = typeof(PlagueBeast); }
					else if ( m_Quest.NMonsterType == "Zywiolak Trucizny" ) { M_Type = typeof(PoisonElemental); }
					else if ( m_Quest.NMonsterType == "Gnijace Zwloki" ) { M_Type = typeof(RottingCorpse); }
					else if ( m_Quest.NMonsterType == "Ogromny Srebrny Waz" ) { M_Type = typeof(SilverSerpent); }
					else if ( m_Quest.NMonsterType == "Zywiolak trucizny" ) { M_Type = typeof(ToxicElemental); }
					else if ( m_Quest.NMonsterType == "Smok" ) { M_Type = typeof(Dragon); }


					// END OF SECTION - MNTP1 /////////////////////////////////////////////////////////////////



					// SECTION - LCXY1 ////////////////////////////////////////////////////////////////////////

					// THIS SECTION DEFINES MY LOCATIONS INTO MAP AND XY COORDINATES

					if (
					( m_Quest.NLocation == "Leze Krysztalowych Smokow" && from.Map == Map.Felucca && from.X >= 5149 && from.Y >= 2147 && from.X <= 5475 && from.Y <= 2290 ) ||
					( m_Quest.NLocation == "Tyr Reviaren" && from.Map == Map.Felucca && from.X >= 5327 && from.Y >= 1707 && from.X <= 5127 && from.Y <= 1869 ) ||
					( m_Quest.NLocation == "Jaskinie Blyskow (LV 1)" && from.Map == Map.Malas && from.X >= 113 && from.Y >= 688 && from.X <= 242 && from.Y <= 848 ) ||
					( m_Quest.NLocation == "Jaskinie Blyskow (LV 2)" && from.Map == Map.Malas && from.X >= 242 && from.Y >= 848 && from.X <= 385 && from.Y <= 989 ) ||
					( m_Quest.NLocation == "Jaskinia Krolewej Wrozek" && from.Map == Map.Malas && from.X >= 18 && from.Y >= 877 && from.X <= 62 && from.Y <= 720 ) ||
					( m_Quest.NLocation == "Krolewskie Krypty" && from.Map == Map.Felucca && from.X >= 5384 && from.Y >= 903 && from.X <= 5526 && from.Y <= 765 ) ||
					( m_Quest.NLocation == "Saew (LV 1 i 2)" && from.Map == Map.Felucca && from.X >= 5363 && from.Y >= 1717 && from.X <= 5298 && from.Y <= 1591 ) ||
					( m_Quest.NLocation == "Jaskinia Lodowych Smokow" && from.Map == Map.Felucca && from.X >= 5766 && from.Y >= 2244 && from.X <= 5812 && from.Y <= 1893 ) ||
					( m_Quest.NLocation == "Wulkan (LV 1)" && from.Map == Map.Felucca && from.X >= 5980 && from.Y >= 614 && from.X <= 6060 && from.Y <= 504 ) ||
					( m_Quest.NLocation == "Wulkan (LV 2)" && from.Map == Map.Felucca && from.X >= 5141 && from.Y >= 1506 && from.X <= 5275 && from.Y <= 1188 ) ||
					( m_Quest.NLocation == "Wulkan (LV 3)" && from.Map == Map.Felucca && from.X >= 5451 && from.Y >= 1392 && from.X <= 5305 && from.Y <= 1127 ) ||
					( m_Quest.NLocation == "Wulkan (LV 4)" && from.Map == Map.Felucca && from.X >= 5692 && from.Y >= 525 && from.X <= 5597 && from.Y <= 602 ) ||
					( m_Quest.NLocation == "Ruiny Elbrind" && from.Map == Map.Felucca && from.X >= 1524 && from.Y >= 780 && from.X <= 1713 && from.Y <= 1039 ) ||
					( m_Quest.NLocation == "Alcala" && from.Map == Map.Felucca && from.X >= 629 && from.Y >= 1631 && from.X <= 755 && from.Y <= 1414 ) ||
					( m_Quest.NLocation == "Jaskinia Banitow)" && from.Map == Map.Felucca && from.X >= 1074 && from.Y >= 1958 && from.X <= 1034 && from.Y <= 1919 ) ||
					( m_Quest.NLocation == "Zwiedla Roza" && from.Map == Map.Felucca && from.X >= 5950 && from.Y >= 637 && from.X <= 5781 && from.Y <= 971 ) ||
					( m_Quest.NLocation == "Fort Orkow" && from.Map == Map.Felucca && from.X >= 939 && from.Y >= 2520 && from.X <= 1007 && from.Y <= 2467 ) ||
					( m_Quest.NLocation == "Swiatynia Hurengrav" && from.Map == Map.Felucca && from.X >= 5252 && from.Y >= 916 && from.X <= 5365 && from.Y <= 798 ) ||
					( m_Quest.NLocation == "Garth" && from.Map == Map.Felucca && from.X >= 1215 && from.Y >= 2961 && from.X <= 983 && from.Y <= 2961 ) ||
					( m_Quest.NLocation == "Gath (LV 1)" && from.Map == Map.Felucca && from.X >= 5417 && from.Y >= 1030 && from.X <= 5143 && from.Y <= 937 ) ||
					( m_Quest.NLocation == "Garth (LV 2)" && from.Map == Map.Felucca && from.X >= 6142 && from.Y >= 287 && from.X <= 5904 && from.Y <= 13 ) ||
					( m_Quest.NLocation == "Ninatyl" && from.Map == Map.Felucca && from.X >= 1984 && from.Y >= 3169 && from.X <= 1857 && from.Y <= 3110 ) ||
					( m_Quest.NLocation == "Posterunek Ninatyl" && from.Map == Map.Felucca && from.X >= 1881 && from.Y >= 3529 && from.X <= 1920 && from.Y <= 3466 ) ||
					( m_Quest.NLocation == "Ulnhyr Orben (LV 1)" && from.Map == Map.Felucca && from.X >= 5753 && from.Y >= 342 && from.X <= 5646 && from.Y <= 293 ) ||
					( m_Quest.NLocation == "Ulnhyr Orben (LV 2)" && from.Map == Map.Felucca && from.X >= 5741 && from.Y >= 148 && from.X <= 5866 && from.Y <= 271 ) ||
					( m_Quest.NLocation == "Piaskowe Krypty" && from.Map == Map.Felucca && from.X >= 5471 && from.Y >= 369 && from.X <= 5397 && from.Y <= 220 ) ||
					( m_Quest.NLocation == "Mechaniczna Krypta" && from.Map == Map.Felucca && from.X >= 5486 && from.Y >= 269 && from.X <= 5554 && from.Y <= 365 ) ||
					( m_Quest.NLocation == "Labirynt" && from.Map == Map.Felucca && from.X >= 2769 && from.Y >= 2027 && from.X <= 2651 && from.Y <= 1897 ) ||
					( m_Quest.NLocation == "Leze Ognistych Smokow (LV 1)" && from.Map == Map.Felucca && from.X >= 5744 && from.Y >= 1933 && from.X <= 5586 && from.Y <= 2199 ) ||
					( m_Quest.NLocation == "Leze Ognistych Smokow (LV 2)" && from.Map == Map.Felucca && from.X >= 5634 && from.Y >= 2275 && from.X <= 5440 && from.Y <= 1939 ) ||
					( m_Quest.NLocation == "Krysztalowy Loch" && from.Map == Map.Felucca && from.X >= 5608 && from.Y >= 191 && from.X <= 5876 && from.Y <= 62 ) ||
					( m_Quest.NLocation == "Loen Torech" && from.Map == Map.Felucca && from.X >= 5582 && from.Y >= 1911 && from.X <= 5384 && from.Y <= 1759 ) ||
					( m_Quest.NLocation == "Hsll Torech" && from.Map == Map.Felucca && from.X >= 5807 && from.Y >= 1868 && from.X <= 5663 && from.Y <= 1681 ) ||
					( m_Quest.NLocation == "Lochy Ophidian" && from.Map == Map.Felucca && from.X >= 5582 && from.Y >= 671 && from.X <= 5728 && from.Y <= 840 ) ||
					( m_Quest.NLocation == "Tasandorskie Kanaly" && from.Map == Map.Felucca && from.X >= 5373 && from.Y >= 691 && from.X <= 5499 && from.Y <= 418) 
					/*( m_Quest.NLocation == "the Solen Hive" && from.Map == Map.Trammel && from.X >= 5639 && from.Y >= 1780 && from.X <= 5934 && from.Y <= 2037 ) ||
					( m_Quest.NLocation == "Dungeon Covetous (Level 1)" && from.Map == Map.Trammel && from.X >= 5373 && from.Y >= 1843 && from.X <= 5508 && from.Y <= 1942 ) ||
					( m_Quest.NLocation == "Dungeon Covetous (Level 2)" && from.Map == Map.Trammel && from.X >= 5374 && from.Y >= 1951 && from.X <= 5622 && from.Y <= 2045 ) ||
					( m_Quest.NLocation == "Dungeon Covetous (Level 3)" && from.Map == Map.Trammel && from.X >= 5533 && from.Y >= 1821 && from.X <= 5630 && from.Y <= 1937 ) ||
					( m_Quest.NLocation == "Dungeon Covetous (Jail Cells)" && from.Map == Map.Trammel && from.X >= 5491 && from.Y >= 1791 && from.X <= 5556 && from.Y <= 1821 ) ||
					( m_Quest.NLocation == "Dungeon Covetous (Lake Cave)" && from.Map == Map.Trammel && from.X >= 5390 && from.Y >= 1780 && from.X <= 5486 && from.Y <= 1838 ) ||
					( m_Quest.NLocation == "Dungeon Doom" && from.Map == Map.Malas && from.X >= 249 && from.Y >= 0 && from.X <= 515 && from.Y <= 257 ) ||
					( m_Quest.NLocation == "Bedlam" && from.Map == Map.Malas && from.X >= 71 && from.Y >= 1564 && from.X <= 211 && from.Y <= 1690 ) ||
					( m_Quest.NLocation == "the Sorcerer`s Dungeon (Level 1)" && from.Map == Map.Ilshenar && from.X >= 365 && from.Y >= 0 && from.X <= 483 && from.Y <= 116 ) ||
					( m_Quest.NLocation == "the Sorcerer`s Dungeon (Level 2)" && from.Map == Map.Ilshenar && from.X >= 196 && from.Y >= 0 && from.X <= 363 && from.Y <= 101 ) ||
					( m_Quest.NLocation == "the Sorcerer`s Dungeon (Level 3)" && from.Map == Map.Ilshenar && from.X >= 52 && from.Y >= 0 && from.X <= 185 && from.Y <= 134 ) ||
					( m_Quest.NLocation == "the Sorcerer`s Dungeon (Jail Cells)" && from.Map == Map.Ilshenar && from.X >= 218 && from.Y >= 104 && from.X <= 251 && from.Y <= 147 ) ||
					( m_Quest.NLocation == "the Ancient Cave" && from.Map == Map.Ilshenar && from.X >= 13 && from.Y >= 658 && from.X <= 134 && from.Y <= 760 ) ||
					( m_Quest.NLocation == "the Kirin Passage" && from.Map == Map.Ilshenar && from.X >= 0 && from.Y >= 805 && from.X <= 187 && from.Y <= 1198 ) ||
					( m_Quest.NLocation == "Dungeon Ankh" && from.Map == Map.Ilshenar && from.X >= 0 && from.Y >= 1247 && from.X <= 183 && from.Y <= 1584 ) ||
					( m_Quest.NLocation == "the Serpentine Passage" && from.Map == Map.Ilshenar && from.X >= 382 && from.Y >= 1497 && from.X <= 542 && from.Y <= 1596 ) ||
					( m_Quest.NLocation == "the Wisp Dungeon (Level 3)" && from.Map == Map.Ilshenar && from.X >= 815 && from.Y >= 1446 && from.X <= 913 && from.Y <= 1584 ) ||
					( m_Quest.NLocation == "the Wisp Dungeon (Level 5)" && from.Map == Map.Ilshenar && from.X >= 917 && from.Y >= 1456 && from.X <= 1017 && from.Y <= 1578 ) ||
					( m_Quest.NLocation == "the Wisp Dungeon (Level 7)" && from.Map == Map.Ilshenar && from.X >= 740 && from.Y >= 1509 && from.X <= 815 && from.Y <= 1585 ) ||
					( m_Quest.NLocation == "the Wisp Dungeon (Level 8)" && from.Map == Map.Ilshenar && from.X >= 746 && from.Y >= 1460 && from.X <= 792 && from.Y <= 1495 ) ||
					( m_Quest.NLocation == "the Ratman Mines (Level 1)" && from.Map == Map.Ilshenar && from.X >= 1263 && from.Y >= 1460 && from.X <= 1355 && from.Y <= 1574 ) ||
					( m_Quest.NLocation == "the Ratman Mines (Level 2)" && from.Map == Map.Ilshenar && from.X >= 1151 && from.Y >= 1460 && from.X <= 1259 && from.Y <= 1558 ) ||
					( m_Quest.NLocation == "the Spider Cave" && from.Map == Map.Ilshenar && from.X >= 1749 && from.Y >= 941 && from.X <= 1870 && from.Y <= 1003 ) ||
					( m_Quest.NLocation == "the Spectre Dungeon" && from.Map == Map.Ilshenar && from.X >= 1940 && from.Y >= 1006 && from.X <= 2022 && from.Y <= 1113 ) ||
					( m_Quest.NLocation == "Dungeon Blood" && from.Map == Map.Ilshenar && from.X >= 2048 && from.Y >= 825 && from.X <= 2195 && from.Y <= 1060 ) ||
					( m_Quest.NLocation == "the Rock Dungeon" && from.Map == Map.Ilshenar && from.X >= 2084 && from.Y >= 0 && from.X <= 2244 && from.Y <= 183 ) ||
					( m_Quest.NLocation == "Dungeon Exodus" && from.Map == Map.Ilshenar && from.X >= 1836 && from.Y >= 9 && from.X <= 2082 && from.Y <= 210 )*/
					)
					{
						nSpot = 1;
					}

					// END OF SECTION - LCXY1 /////////////////////////////////////////////////////////////////



					if ( c.Owner == null )
					{
						if ( m_Quest.NType == 1 )
						{
							from.SendMessage( "Za pozno bys przypisal sobie zaslugi za to!" );
						}
						else
						{
							from.SendMessage( "Twe poszukiwania okazaly sie pojsc na marne!" );
						}

						return;
					}

					if ( obj is Corpse )
					{
						obj = ((Corpse)obj).Owner;

						if ( ( M_Type == obj.GetType() ) && ( m_Quest.NType == 1 ) )
						{
							from.SendMessage( "You claim this toward your quest." );

							c.Delete();

							m_Quest.NGot = m_Quest.NGot + 1;

							if ( m_Quest.NGot >= m_Quest.NNeed )
							{
								from.PrivateOverheadMessage(MessageType.Regular, 0x44, false, "Zadanie wykonane!", from.NetState);
								m_Quest.Name = "Wykonano - Zgladz " + m_Quest.NMonsterType + " (" + m_Quest.NGot.ToString() + " z " + m_Quest.NNeed.ToString() + ")";
								m_Quest.Hue = 1258;
							}
							else
							{
								m_Quest.Name = "Zgladz " + m_Quest.NMonsterType + " (" + m_Quest.NGot.ToString() + " z " + m_Quest.NNeed.ToString() + ")";
							}
						}

						else if ( ( nSpot == 1 ) && ( m_Quest.NType == 2 ) )
						{
							if ( m_Quest.NChance > Utility.Random( 100 ) )
							{
								from.PrivateOverheadMessage(MessageType.Regular, 0x44, false, "Zanalazlem " + m_Quest.NItemName + "!", from.NetState);
								from.SendMessage( "ZNALZALES " + m_Quest.NItemName + "!" );
								c.Delete();
								m_Quest.NGot = m_Quest.NGot + 1;
								m_Quest.Name = "WYKONANE - ODSZUKAJ " + m_Quest.NItemName;
								m_Quest.Hue = 1258;
							}
							else
							{
								from.SendMessage( "Nie znalazles przedmiotu, ktorego szukales!" );
								c.Delete();
							}
						}
						else
						{
							from.SendMessage( "To nie ma nic wspolnego z zadaniem!" );
						}

						return;
					}

					from.SendMessage( "To nie ma nic wspolnego z zadaniem!" );
				}
			}
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			string sStatus = "Poziom " + NLevel.ToString() + " Zadania";
			string sStory = "";

			base.AddNameProperties( list );
			list.Add( 1070722, NStory );
			list.Add( 1049644, sStatus );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write(mNNeed);
			writer.Write(mNGot);
			writer.Write(mNChance);
			writer.Write(mNLevel);
			writer.Write(mNType);
			writer.Write(mNItemName);
			writer.Write(mNMonsterType);
			writer.Write(mNLocation);
			writer.Write(mNStory);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			mNNeed = reader.ReadInt();
			mNGot = reader.ReadInt();
			mNChance = reader.ReadInt();
			mNLevel = reader.ReadInt();
			mNType = reader.ReadInt();
			mNItemName = reader.ReadString();
			mNMonsterType = reader.ReadString();
			mNLocation = reader.ReadString();
			mNStory = reader.ReadString();

			string sCOMPLETE = "";

			if ( NGot >= NNeed )
			{
				sCOMPLETE = "WYKONANE - ";
				Hue = 1258;
			}

			if ( NType == 1 )
			{
				Name = sCOMPLETE + "Zgladz " + NMonsterType + " (" + NGot.ToString() + "" + NNeed.ToString() + ")";
			}
			else
			{
				Name = sCOMPLETE + "Poszukaj " + NItemName;
			}
		}
	}
}