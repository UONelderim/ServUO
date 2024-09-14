using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using Server.ACC.CSS.Systems.Cleric;

namespace Server.Engines.Quests
{
	public class ClericQuest : BaseSpecialSkillQuest
	{
		public ClericQuest()
		{
			AddObjective(new SlayObjective(typeof(VampireBat), "nietoperz wampir", 20));

			AddReward(new BaseReward(3060192)); // Krok blizej od poznania tajemnic Herdeizmu.
		}

		public override QuestChain ChainID => QuestChain.Cleric;

		public override Type NextQuest => typeof(ClericPhase2Quest);

		/* Herdeizm */
		public override object Title => 33060183;

		/* Umiejetne leczenie ran to jedno, ale by byc prawdziwie przygotowanym do boju i walczyc w imie Pana,
		 nalezy wiernie podazac za jego wartosciami, a wtedy on zesle na nas wiedze potrzebna, by poslac Mlot Pana wprost na ich glupie lby!.  */
		public override object Description => 3060184;

		/* No wiesz... juz myslalem, ze mi pomozesz. */
		public override object Refuse => 3060185;

		/* Nie trac mego czasu. Zadanie jest proste - masz zabic 20 nietoperzy wampirow */
		public override object Uncomplete => 3060186;

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

	public class ClericPhase2Quest : BaseQuest
	{
		public ClericPhase2Quest()
		{
			AddObjective(new ObtainObjective(typeof(PetrafiedWood), "spetryfikowane drzewo", 50, 0x97A));
			AddObjective(new ObtainObjective(typeof(SpringWater), "wiosenna woda", 50, 0xE24));

			AddReward(new BaseReward(3060192)); // Krok blizej od poznania tajemnic Herdeizmu
		}

		public override QuestChain ChainID => QuestChain.Cleric;

		public override Type NextQuest => typeof(ClericPhase3Quest);

		/* Misja Kaplana Cumberlanda */
		public override object Title => 3060187;

		/* Kaplan Cumberliand nakazal opatrzyc rany lezacych w izbie. Jedna z rannych osob, pomimo iz wydawala sie byc jedynie lekko ranna, jej rany zamienialy sie w czarna jak smola maz.
		 Owa maz nie byla czyms co w tych stronach byloby znane uzdrowicielom. Jedynie wyposazony w odpowiednia Herdeista mogl temu zaradzic. J
		 ednakze, mlodzi adepci tej sztuki widzac to pierwszy raz w swym, miejmy nadzieje, dlugim zyciu, nie wiedza jakie remedium moze tutaj pomoc. Stad,
		 Cumberliand wyslal ich po zebranie kilku skladnikow, by takie remedium sporzadzic. Potrzebne sa: woda wiosenna, sztuk 50, spetryfikowane drzewo, tyle samo. */
		public override object Description => 3060188;

		/* Nie chcesz pomoc, to nie zawracaj mi glowy. Potrzebujacy czekaja... */
		public override object Refuse => 3060189;

		/* No dalej,pomoz nam pomoc innym. */
		public override object Uncomplete => 3060190;

		/* I jak? Masz pierwsza transze skladnikow? */
		public override object Complete => 3060191;

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

	public class ClericPhase3Quest : BaseQuest
	{
		public ClericPhase3Quest()
		{
			AddObjective(new ObtainObjective(typeof(ZoogiFungus), "grzyby zoogi", 50, 0x26B7));
			AddObjective(new ObtainObjective(typeof(SulfurousAsh), "siarka", 200, 0xF8C));
			AddObjective(new ObtainObjective(typeof(Pitcher), "dzban", 1, 0x9A7));
			AddObjective(new ClericPhase5Quest.SayObjective());

			AddReward(new BaseReward(3060192)); // Krok blizej od poznania tajemnic Herdeizmu.
		}

		public override QuestChain ChainID => QuestChain.Cleric;

		public override Type NextQuest => typeof(ClericPhase4Quest);

		/* Chwalmy Pana */
		public override object Title => 3060193;

		/* To jeszcze nie wszystko. Potrzebne jeszcze beda siarka w ilości 200, dzban wody i 50 grzybów zoogi. Podane składniki należy oddać Kapłanowi, który remedium sporządzi na oczach adeptów,
		 by Ci mogli uczyć się od mistrza.
		 Następnie, tuż przy samym okaleczonym, nakładając remedium na rany, mają wyrzec trzykrotnie „Chwalmy Pana”. . */
		public override object Description => 3060194;

		/* No nie. Teraz rezygnujesz?! */
		public override object Refuse => 3060195;

		/* Jak Ci idzie z tymi skladnikami?. */
		public override object Uncomplete => 3060196;

		/* No swietnie, mamy wszystko co trzeba! *podskakuje z radosci* */
		public override object Complete => 3060197;

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

	public class ClericPhase4Quest : BaseQuest
	{
		public ClericPhase4Quest()
		{
			AddObjective(new SlayObjective(typeof(GargoyleDestroyer), "gargulec niszczyciel", 10,
				"Hurengrav_VeryDifficult"));

			AddReward(new BaseReward(3060192)); // Szansa poznania drogi Herdeisty.
		}

		public override QuestChain ChainID => QuestChain.Cleric;

		public override Type NextQuest => typeof(ClericPhase5Quest);

		/* Wycieczka do Huerngrav */
		public override object Title => 3060198;

		/* Kolejnym krokiem bedzie udanie sie do Hurengrav i wytepienie tam poplecznikow Smierci. Zabicie 10 gargulcow niszczycieli pokaze Panu, iz nie tylko wiedza,
		 ale i sila przepelnia mlodych Herdeistow. Musza one zginac na samym koncu dawnej swiatyni. Przy stolach, nieopodal pentagramu. Tylko tak przypodobasz sie Panu. */
		public override object Description => 3060199;

		/* Ahh, tracisz moj czas, czy cos?. */
		public override object Refuse => 3060200;

		/* I jak Ci idzie gromienie gargulcow? Chyba slabo.... */
		public override object Uncomplete => 3060176;

		/* Oh! Swietnie! Kolejny krok blizej utworzenia Twej ksiegi! */
		public override object Complete => 3060177;

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

	public class ClericPhase5Quest : BaseQuest
	{
		public ClericPhase5Quest()
		{
			AddObjective(new ObtainObjective(typeof(Gold), "zloto", 20000, 3821));


			AddReward(new BaseReward(typeof(ClericSacrificeScroll), "Poswiecenie")); // Poswiecenie
			AddReward(new BaseReward(typeof(ClericSpellbook), "Ksiega Herdeizmu")); // Ksiega Herdeizmu
		}

		public override QuestChain ChainID => QuestChain.Cleric;

		/*Zlote a skromne*/
		public override object Title => 3060201;

		/* Poswiecenie, to najwieksza cnota Herdeisty. No, moze z wyjatkiem zlota poswiecanego na swiatynie. Aby zostac przyjety do tego waskiego grona wybranych, nalezy zlozyc ofiare na Pana w wysokosci 20000 centarow */
		public override object Description => 3060202;

		/* I teraz chcesz mnie opusicic?!. */
		public override object Refuse => 3060180;

		/* Laskawco! Badz zasz drobym Herdeista i daj na Pana choc centara */
		public override object Uncomplete => 3060203;

		/* *wyciagnal reke po mieszki zlota* Nooo... i to sie nazywa ofiara *usmiecha sie* Witamy w gronie Herdeistow */
		public override object Complete => 3060204;

		public override void GiveRewards()
		{
			Owner.SpecialSkills.Cleric = true;

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

		public class SayObjective : SimpleObjective
		{
			private readonly List<string> m_Descr = new List<string>();
			public override List<string> Descriptions => m_Descr;

			public SayObjective()
				: base(1, -1)
			{
				m_Descr.Add("Wypowiedz \"Chwalmy Pana\" w poblizu zwlok.");
			}

			public override bool Update(object obj)
			{
				if (obj is string text && text.Trim().Equals("Chwalmy pana"))
				{
					CurProgress++;

					if (Completed)
						Quest.OnCompleted();
					else
					{
						Quest.Owner.PlaySound(Quest.UpdateSound);
					}

					return true;
				}

				return false;
			}
		}
	}
}
