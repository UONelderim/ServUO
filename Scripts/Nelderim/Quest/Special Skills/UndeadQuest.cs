using Server.Items;
using Server.Mobiles;
using System;
using Server.ACC.CSS.Systems.Undead;
using Server.Regions;

namespace Server.Engines.Quests
{
	public class UndeadQuest : BaseSpecialSkillQuest
	{
		public UndeadQuest()
		{
			AddObjective(new SlayObjective(typeof(Sheep), "owca", 20));

			AddReward(new BaseReward(3060307)); // Coraz blizej wejscia w posiadanie umiejetnosci okultystycznych
		}

		public override QuestChain ChainID => QuestChain.Undead;

		public override Type NextQuest => typeof(UndeadPhase2Quest);

		/* Inna droga */
		public override object Title => 3060305;

		/*  Oklultyzm jest wiedza, ktora nie kazdy Nekromanta posiada. Jest ona bowiem tylko dla elitarnych czlonkow Wyznawcow Smierci. Tych najbardziej zagorzalych wyznawcow, ktorzy potrafia oddac swoja dusze, serce i cialo w rece Smierci.
Kazdy nekromanta ma do czynienia z pewna wersja wiary w Smierc, ale tylko nieliczni sa godni dostapienia prawdziwej laski.
Jak wyroznic sie wsrod zwyczajnych wiernych, zapytasz? Otoz, najpierw trzeba podarowac Naszej wspanialej Smierci martwa owce.

  */
		public override object Description => 3060306;

		/* No wiesz... juz myslalem, ze mi pomozesz. */
		public override object Refuse => 3060185;

		/* A co to? Spekales sie?! Hahaha, no popatrzcie... */
		public override object Uncomplete => 3060297;
		
		public override bool CanOffer()
		{
			if (Owner.SpecialSkills.Undead)
				return false;
			
			return base.CanOffer();
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

	public class UndeadPhase2Quest : BaseQuest
	{
		public UndeadPhase2Quest()
		{
			AddObjective(
				new ObtainObjective(typeof(SpidersSilk), "Pajecza siec", 100, 0xF8D));
			AddObjective(new ObtainObjective(typeof(DestroyingAngel), "Niszczejacy aniol", 100, 0xE1F));

			AddReward(new BaseReward(3060307)); // Coraz blizej wejscia w posiadanie umiejetnosci okultystycznych
		}

		public override QuestChain ChainID => QuestChain.Undead;

		public override Type NextQuest => typeof(UndeadPhase3Quest);

		/* Co dalej? */
		public override object Title => 3060308;

		/*  Co dalej pytasz? Nastepnie zebrac 100 pajeczej sieci i 100 niszczejacych aniolow. Te skladniki oddac nalezy kaplanowi Smierci w Tasandorze.  */
		public override object Description => 3060309;

		/* A co to? Spekales sie?! Hahaha, no popatrzcie... */
		public override object Refuse => 3060297;

		/* I jak ida zbiory? */
		public override object Uncomplete => 3060310;

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

	public class UndeadPhase3Quest : BaseQuest
	{
		public UndeadPhase3Quest()
		{
			AddObjective(new SlayObjective(typeof(Lich), "licz", 20));
			AddObjective(new SlayObjective(typeof(SkeletalDragon), "kosciany smok", 1));

			AddReward(new BaseReward(3060307)); // Coraz blizej wejscia w posiadanie umiejetnosci okultystycznych
		}

		public override QuestChain ChainID => QuestChain.Undead;

		public override Type NextQuest => typeof(UndeadPhase4Quest);

		/* Powazny Okultyzm */
		public override object Title => 3060312;

		/*  No dobra. Widze, ze na powaznie bierzesz to zadanie. Okultysci opanowali wladze na trupiszczami i czarna magia w stopniu tak zaawansowanym, ze malo kto to pojmie. Jednakze, nie obedzie sie bez zaplacenia ceny za te wiedze. Smierc musi otrzymac swoja czesc. Zatem wysylam Cie w jej imieniu bys zgladzil 20 liczow i 10 koscianych smokow */
		public override object Description => 3060311;

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

	public class UndeadPhase4Quest : BaseQuest
	{
		public UndeadPhase4Quest()
		{
			AddObjective(
				new ObtainObjective(typeof(BonerPowder), "Proch kościeja", 1, 0xF8F));


			AddReward(new BaseReward(3060307)); // Coraz blizej wejscia w posiadanie umiejetnosci okultystycznych
		}

		public override QuestChain ChainID => QuestChain.Undead;

		public override Type NextQuest => typeof(UndeadPhase5Quest);

		/* Jeszcze jedna dostawa */
		public override object Title => 3060289;

		/* Wysmienicie Ci idzie. Tak dobrze, ze poprosze Cie o jedna dostawe. Nekromanci potrafia tworzyc potezne ozywience, ktore beda bronic nas, niczym zwierze swego oswajacza... czy jakos tak. Jeno, trza nam pylu. Przynies proch koscieja do Tasandory, a dzieki temu zdobedziemy przewage nad tymi pomiotami Naneth */
		public override object Description => 3060313;

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

	public class UndeadPhase5Quest : BaseQuest
	{
		public UndeadPhase5Quest()
		{
			AddObjective(new SlayObjective(typeof(EtherealWarrior), "eteryczny wojownik", 15));
			AddObjective(new SlayObjective(typeof(Pixie), "wrozka", 15));


			AddReward(new BaseReward(typeof(UndeadHammerOfFaithScroll), "Sierp Wiary Smierci"));
			AddReward(new BaseReward("Księga Okultyzmu"));
		}

		public override QuestChain ChainID => QuestChain.Undead;

		/* Proba sil */
		public override object Title => 3060291;

		/* Teraz dopiero sprawdzimy czy nadajesz sie Okultyste. Zmiazdz slugi Matki. Zabij 15 wrozek i 15 eterycznych wojownikow. Pokaz, ze masz klejnoty!  */
		public override object Description => 3060314;

		/* I teraz chcesz mnie opusicic?!. */
		public override object Refuse => 3060180;

		/* I jak Ci idzie? */
		public override object Uncomplete => 3060303;

		/* No i kurrrwancka, o to chodzi! */
		public override object Complete => 3060304;

		public override void GiveRewards()
		{
			Owner.AddToBackpack(new UndeadSpellbook() { BlessedFor = Owner });
			Owner.SpecialSkills.Undead = true;

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
