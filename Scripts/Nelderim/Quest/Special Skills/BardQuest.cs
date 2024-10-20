using Server.Items;
using Server.Mobiles;
using System;
using Server.ACC.CSS.Systems.Bard;
using Server.Regions;

namespace Server.Engines.Quests
{
	public class BardQuest : BaseSpecialSkillQuest
	{
		public BardQuest()
		{
			
			AddObjective(new ObtainObjective(typeof(AdmiralsHeartyRum), "krasnoludzki rum", 10, 0xE9C));

			AddReward(new BaseReward(3060225)); // Coraz blizej wielkich piesni bardowskich
		}

		public override QuestChain ChainID => QuestChain.Bard;

		public override Type NextQuest => typeof(BardPhase2Quest);

		/* Bardo dobrze */
		public override object Title => 3060268;

		/*  *ledwo trzyma sie na nogach* Kuuuuuuuuurrrwaaanckaa...., ale mnie leb boli. Cholera... *spoglada kaprawymi oczetami na postac przed soba* ....przynies mi troche krasnoludzkiego rumu, prosze... inaczej nie dam rady prowadzic rozmowy *czka*  */
		public override object Description => 3060267;

		/* No wiesz... juz myslalem, ze mi pomozesz. */
		public override object Refuse => 3060185;

		/* Kras-noo-luuuudzkiiii rum. Ile razy mam mowic?*/
		public override object Uncomplete => 3060269;

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

	public class BardPhase2Quest : BaseQuest
	{
		public BardPhase2Quest()
		{
			AddObjective(new ObtainObjective(typeof(Gold), "Zloto", 10, 3821));
			AddObjective(new ObtainObjective(typeof(ZapomnianaPiesn), "Zapomniana piesn", 10, 5358));

			AddReward(new BaseReward(3060225)); // Coraz blizej wielkich piesni bardowskich
		}

		public override QuestChain ChainID => QuestChain.Bard;

		public override Type NextQuest => typeof(BardPhase3Quest);

		/* Moc Pana */
		public override object Title => 3060258;

		/* Chcesz wiedziec wiecej na temat tych slawnych piesi... TO ja ich nie znam. Znal je Odon.. ehh... Odon jak zwykle wieczor spedzal w Karczmie.
		 Nie to ze pijanstwo, chociaz zdarzalo sie i to... Zarechotal na samo wspomnienie. Lazil do Karczmy, bo graniem na zycie zarabial.
		 Wlasciwie przypadkiem kiedys wzial do reki lutnie i dalej juz poszlo. Slowa wymyslane na poczekaniu, zartobliwe uwagi, dramatyczne pozy. Niezbyt wyszukana publika karczmy od razu to pokochala.
		 Wczoraj jednak trafila mu sie okazja. Zarobek wiekszy niz przy spiewach. Ale od poczatku zacznijmy. Kiedy Karczma powoli pustoszala, a przy barze dosypialo kilku gosci, zas barman scyzorykiem dlubal
		 w zebie otworzyly sie drzwi. Barczysta postac, ledwie mieszczaca sie w framudze. Jakis cien ledwie widoczny w miejscu twarzy. Rosly czlowiek, jak okazalo sie po zdjeciu kaptura ledwie mrugnal do barmana,
		 ktory przerwal swoje czynnosci i cichutko pogwizdujac odwrocil sie do okna. Od razu podszedl do Odona. Witaj grajku, nie bede czasu tracil - powiedzial basowym glosem -
		 Chcesz zarobic wiecej jak w tej zapomnianej przez Bogow dziurze? Odon zgodzil sie wysluchac jego propozycji, przez glowe przelecialy mu bowiem wyobrazenia o worach pelnych zlota i kosztownosci.
		 Nieznajomy zaczal opowiadac. W miare opowiesci oczy Odona najpierw robily sie wielkie, potem ich ksztalt przypominal waskie szczeliny - nieznajomy bowiem przeszedl do czesci wynagrodzenia.
		 Zatem zadanie bylo nastepujace - mial odzyskac nuty pewnej piesni, ktora w posiadaniu rodziny nieznajomego byla.
		 Podobno gdzies na bagnach zlodzieje ukryli skradzione zloto, oraz ostatni egzemplarz piesni. Ponoc tam bylo 5000 centarow. */
		//TODO: Na bagnach kolo Ophi dac spawn piesni
		public override object Description => 3060270;

		/* Nie chcesz pomoc, to nie zawracaj mi glowy. Potrzebujacy czekaja... */
		public override object Refuse => 3060189;

		/* I jak CI idzie?. */
		public override object Uncomplete => 3060232;

		/* Swietenie, ze udalo Ci sie je zebrac */
		public override object Complete => 3060231;

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

	public class BardPhase3Quest : BaseQuest
	{
		public BardPhase3Quest()
		{
			AddObjective(new ObtainObjective(typeof(Drums), "bebenek", 10, 0xE9C));
			AddObjective(new ObtainObjective(typeof(Lute), "bebenek", 10, 0xEB3));
			AddObjective(new ObtainObjective(typeof(Tambourine), "tamburyn", 10, 0xE9D));

			AddReward(new BaseReward(3060225)); // Coraz blizej wielkich piesni bardowskich
		}

		public override QuestChain ChainID => QuestChain.Bard;

		public override Type NextQuest => typeof(BardPhase4Quest);

		/* Zbieram zespol */
		public override object Title => 3060271;

		/*  To nie jest najwazniejsze. Najwazniejsze, bys zdobyl tamburyny, lutnie i bebny...*rzekl* W pewnym momencie nieznajomy ponoc dodal, ze sam od siebie
nagrode ufunduje - po czym pokazal ksiege, w ktorej bojowe piesni mozna by umieszczac. To
przewazylo, gdyz Odon od zlota i bogactwa tylko muzyke kochal bardziej. Czy zdolasz powtorzyc jego
wyczyn mlody bardzie? */
		public override object Description => 3060260;

		/* No nie. Teraz rezygnujesz?! */
		public override object Refuse => 3060195;

		/* I jak Ci idzie?. */
		public override object Uncomplete => 3060261;

		/* Brawo, brawo *entuzjastycznie zaklaskal w rytmie muzyki lecacej z budnykow wokol* */
		public override object Complete => 3060273;

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

	public class BardPhase4Quest : BaseQuest
	{
		public BardPhase4Quest()
		{
			AddObjective(new SlayObjective(typeof(Imp), "imp", 10));

			AddReward(new BaseReward(3060225)); // Coraz blizej wielkich piesni bardowskich
		}

		public override QuestChain ChainID => QuestChain.Bard;

		public override Type NextQuest => typeof(BardPhase5Quest);

		/* Na uslugach Barda */
		public override object Title => 3060274;

		/* Dobra, dobra. Nie sama muzyka czlowiek zyje. Zeby zdobyc piesni, to trza tez krwii troche przelac *melodyjnie dodal* a najlepiej tej zlej. Impow zabij ... 20.. no. Wystarczyc powinno. */
		public override object Description => 3060264;

		/* Ahh, tracisz moj czas, czy cos?. */
		public override object Refuse => 3060261;

		/* I jak Ci idzie? */
		public override object Uncomplete => 3060176;

		/* *podskakuje z radosci* */
		public override object Complete => 3060237;

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

	public class BardPhase5Quest : BaseQuest
	{
		public BardPhase5Quest()
		{
			AddObjective(new ObtainObjective(typeof(BlankScroll), "Czysty zwoj", 30, 0xEF3));


			AddReward(new BaseReward(typeof(BardArmysPaeonScroll), "Śpiew Armii"));
			AddReward(new BaseReward("Księga Pieśni Bojowych"));
		}

		public override QuestChain ChainID => QuestChain.Bard;

		/* Ostatnie zamowienie */
		public override object Title => 3060238;

		/* Aby przepisac te piesn i rozszyfrowac jej znaczenie, bede musial miec troche zwojow. Niestety, pieniazki he he... przehulalem. Zdobadz 30 zwojoj i powinnismy byc w stanie zrobic cos takiego ooo. */
		public override object Description => 3060276;

		/* I teraz chcesz mnie opusicic?!. */
		public override object Refuse => 3060180;

		/* I jak ida zbiory? */
		public override object Uncomplete => 3060240;

		/* Dobrze, zabieram sie do pracy!. */
		public override object Complete => 3060277;

		public override void GiveRewards()
		{
			Owner.AddToBackpack(new BardSpellbook() { BlessedFor = Owner });
			Owner.SpecialSkills.Bard = true;

			base.GiveRewards();
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
}
