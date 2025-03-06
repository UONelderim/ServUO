using Server.Items;
using Server.Mobiles;
using System;
using Server.ACC.CSS.Systems.Ranger;
using Server.Regions;

namespace Server.Engines.Quests
{
	public class RangerQuest : BaseSpecialSkillQuest
	{
		public RangerQuest()
		{
			AddObjective(new ObtainObjective(typeof(Arrow), "strzaly", 200, 0xF3F));

			AddReward(new BaseReward(3060283)); // Coraz blizej opanowania sztuczek Straznika Lesnego
		}

		public override QuestChain ChainID => QuestChain.Ranger;

		public override Type NextQuest => typeof(RangerPhase2Quest);

		/* Strzez sie Strazniku */
		public override object Title => 3060278;

		/*  Tarak poprawil uprzaz u swojego konia. Szybkim spojrzeniem strzaly przeliczyl w kolczanie. Wszak nie
moglo ich zabraknac. Zwlaszcza dzisiaj. Dzisiaj pierwszy raz z nowym kandydatem do lesnej sluzby
udawal sie na rekonesans w okolice ruin zamieszkanych przez Ophidian. Niebezpieczne to miejsce,
nie tylko z ich powodu. Niedaleko bowiem widywano mroczne elfy, ktore niespecjalnie milo witaly
napotkanych wedrowcow. Dobrze, ze zarowno on jak i mlody Maethor potrafili szybko przemieszczac
sie, nie robic zbytniego halasu a nawet wsrod kepek trawy czarne i zamaskowane szpiczaste uszy
drowow wypatrywac. Nieraz zdazyli nawet jedna, czy dwie zblakane strzaly poslac w ich kierunku.
Wszak kazdy mroczny elf zycie i przyrode mial gleboko w d..uzym powazaniu. No moze poza
pajakami, ale na ten rodzaj chyba zboczenia Tarak nie mial wplywu. Prznies mi prosze 200 strzal.  */
		public override object Description => 3060279;

		/* No wiesz... juz myslalem, ze mi pomozesz. */
		public override object Refuse => 3060185;

		/* Noo, dalej. Przynies mi te strzaly... */
		public override object Uncomplete => 3060280;

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

	public class RangerPhase2Quest : BaseQuest
	{
		public RangerPhase2Quest()
		{
			AddObjective(new SlayObjective(typeof(Ettin), "ettin", 10));
			AddObjective(new SlayObjective(typeof(GiantSpider), "ogromny pajak", 20));
			AddObjective(new SlayObjective(typeof(OrcishLord), "lord ork", 10));

			AddReward(new BaseReward(3060283)); // Coraz blizej opanowania sztuczek Straznika Lesnego
		}

		public override QuestChain ChainID => QuestChain.Ranger;

		public override Type NextQuest => typeof(RangerPhase3Quest);

		/* Droga Eliminacji */
		public override object Title => 3060282;

		/* Akurat pajakow nie specjalnie
lubil, ale jego obowiazki nakladaly na niego opieke rowniez nad nimi. Najwieksze baczenie Straznicy
Lesni mieli na Swiatynie Matki i jej okolice, jednak czasami jakies miejsce nie bylo oznaczone moca
cudownej Matki, czy jak powiadal Maethor elfem bedacy - cudownej Naneth - pomimo tego
obowiazek oraz jakies nie sprecyzowane cele nakazywaly opieke. Jak to mowia Lesnym Straznikiem
jest sie wszedzie i zawsze. Teraz przed Tarakiem stalo wazne zadanie. Kandydata przygotowac na tyle
dobrze, by swoja przydatnosc w sluzbie pokazal i na ksiege zasluzyl. Najpierw potrzeba bylo wykazac
sie umiejetnoscia strzelania z luku. Z ta umiejetnoscia wiekszosc elfow rodzi sie , jak zartobliwie
wyolbrzymiajac opowiadano. Nastepnie w Swiatyni Matki nalezalo stawic sie o swicie i po krotkich
modlach potwierdzic gotowosc zostania Straznikiem. To tez nie bylo problem. Nastepnie Kandydat
musi udowodnic swoje umiejetnosci. Powinien bez problemu oczyscic dowolny las z pewnej ilosci
szkodnikow - tu prym wiodly orki i gobliny, jako bezmyslni klusownicy i podpalacze oraz Ogry i
Ettiny, ktore pol lasu dla jednej maczugi gotowe zniszczyc, nie martwiac sie o nic. Dopiero wtedy
ksiege kandydat otrzyma. Zabij 20 pajakow i 10 ettinow, do tego 10 lordow orkow
 */ //
		public override object Description => 3060281;

		/* Nie chcesz pomoc, to nie zawracaj mi glowy. Potrzebujacy czekaja... */
		public override object Refuse => 3060189;

		/* I jak? Udalo Ci sie zgladzic te monstra?. */
		public override object Uncomplete => 3060284;

		/* Oj, beda z Ciebie ludzie! */
		public override object Complete => 3060285;

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

	public class RangerPhase3Quest : BaseQuest
	{
		public RangerPhase3Quest()
		{
			AddObjective(new ObtainObjective(typeof(PetrafiedWood), "spetryfikowane drzewo", 200, 0x97A));

			AddReward(new BaseReward(3060283)); // Coraz blizej opanowania sztuczek Straznika Lesnego
		}

		public override QuestChain ChainID => QuestChain.Ranger;

		public override Type NextQuest => typeof(RangerPhase4Quest);

		/* Petryfikacja */
		public override object Title => 3060286;

		/*  Spetryfikowane drzewo - to skladnik kluczowy naszych sztuczek. Aby bronic sie przed Mrocznymi Elfami, potrzebujemy zrobic ich zapas, a nacieraja tu czesto. Przynies 200 sztuk, a pokaze Ci kilk sztuczek *zarechotal* */
		public override object Description => 3060287;

		/* No nie. Teraz rezygnujesz?! */
		public override object Refuse => 3060195;

		/* I jak Ci idzie?. */
		public override object Uncomplete => 3060261;

		/* *pokiwal glowa* No, no, dobrze rokujesz. */
		public override object Complete => 3060288;

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

	public class RangerPhase4Quest : BaseQuest
	{
		public RangerPhase4Quest()
		{
			AddObjective(
				new ObtainObjective(typeof(ZoogiFungus), "grzyby zoogi", 200, 0x26B7));

			AddReward(new BaseReward(3060283)); // Coraz blizej opanowania sztuczek Straznika Lesnego
		}

		public override QuestChain ChainID => QuestChain.Ranger;

		public override Type NextQuest => typeof(RangerPhase5Quest);

		/* Jeszcze jedna dostawa */
		public override object Title => 3060289;

		/* Aby wzmocnic nasze szyki, potrzeba byloby nam jeszcze jednej rzeczy, mianowicie, grzyby zoogi. Szuk 200. Prosze pomoz nam i przynies tyle sztuk wlasnie. */
		public override object Description => 3060290;

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

	public class RangerPhase5Quest : BaseQuest
	{
		public RangerPhase5Quest()
		{
			AddObjective(new SlayObjective(typeof(RagingGrizzlyBear), "wsciekly niedzwiedz grizzly", 3));

			AddReward(new BaseReward(typeof(RangerFireBowScroll), "Ognisty Łuk"));
			AddReward(new BaseReward("Poradnik Strażnika Leśnego"));
		}

		public override QuestChain ChainID => QuestChain.Ranger;

		/* Proba sil */
		public override object Title => 3060291;

		/* Teraz dopiero sprawdzimy czy nadajesz sie na Straznika Lesnego, mlokosie. Trzeba spacyfikowac kilka stworzen, ktore pod wplywem zlej magii zaczely szalec w lasach. Wsciekly niedzwiedz grizzly, szutk 30.  */
		public override object Description => 3060292;

		/* I teraz chcesz mnie opusicic?!. */
		public override object Refuse => 3060180;

		/* I jak? Udalo Ci sie zgladzic te monstra?*/
		public override object Uncomplete => 3060284;

		/* Jestes prawdziwym darem Naneth. */
		public override object Complete => 3060293;

		public override void GiveRewards()
		{
			Owner.AddToBackpack(new RangerSpellbook() { BlessedFor = Owner });
			Owner.SpecialSkills.Ranger = true;

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
