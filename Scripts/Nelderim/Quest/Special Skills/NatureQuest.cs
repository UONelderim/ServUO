using Server.Items;
using Server.Mobiles;
using System;
using Server.ACC.CSS.Systems.Druid;
using Server.Items.Crops;

namespace Server.Engines.Quests
{
	public class NatureQuest : BaseSpecialSkillQuest
	{
		public NatureQuest()
		{
			AddObjective(new SlayObjective(typeof(VampireBat), "nietoperz wampir", 20));

			AddReward(new BaseReward(3060225)); // Droga do poznania magii natury
		}

		public override QuestChain ChainID => QuestChain.Nature;

		public override Type NextQuest => typeof(NaturePhase2Quest);

		/* Magia Natury*/
		public override object Title => 3060226;

		/* Magia natury to starozytne polaczenie Zielarstwa oraz Magii. Ta kombinacja powoduje, iz Ci, ktorzy wierza w sile Matki,
		 potrafia sprawic, ze sama natura stanie w ich obronie i bedzie sluchac slow wladajacego ta magia.
		 Zanim zas do zbierania ziolek i nauki o magii sie zabierzemy... trzeba wyciac pare slug Smierci. Zabij 20 nietoperzy Wampirow.  */
		public override object Description => 3060227;

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

	public class NaturePhase2Quest : BaseQuest
	{
		public NaturePhase2Quest()
		{
			AddObjective(new ObtainObjective(typeof(PetrafiedWood), "spetryfikowane drzewo", 50, 0x97A));
			AddObjective(new ObtainObjective(typeof(SpringWater), "wiosenna woda", 50, 0xE24));

			AddReward(new BaseReward(3060228)); // Krok blizej od poznania tajemnic Magii Natury
		}

		public override QuestChain ChainID => QuestChain.Nature;

		public override Type NextQuest => typeof(NaturePhase3Quest);

		/* Prawdziwie wyleczony */
		public override object Title => 3060230;

		/* Zielarz dal mi te liste. Potrzeba kilku rzeczy by sporzadzic mikstury dla chorych. Potrzebne sa: woda wiosenna, sztuk 50, spetryfikowane drzewo, tyle samo. */
		public override object Description => 3060229;

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

	public class NaturePhase3Quest : BaseQuest
	{
		public NaturePhase3Quest()
		{
			AddObjective(new ObtainObjective(typeof(SzczepkaCzosnek), "szczepka czosnku", 10, 0x18E3));
			AddObjective(new ObtainObjective(typeof(Pitcher), "dzban", 1, 0x9A7));


			AddReward(new BaseReward(3060228)); // Krok blizej od poznania tajemnic Magii Natury
		}

		public override QuestChain ChainID => QuestChain.Nature;

		public override Type NextQuest => typeof(NaturePhase4Quest);

		/* Datki na swiatynie */
		public override object Title => 3060233;

		/* Ah, nie zapomnij zabrac ze soba dzbanka oraz szczepki czosnku, sztuk 10. Tym czynem nie tylko przyslugujesz sie ochronie naszego zielonego swiata Nelderim, ale i kregowi
		 Druidow, ktory tak jak i Ty, mlody adepcie sztuk Magii Natury, chcesz chronic Naneth i wielbic ja po kres swych dni. */
		public override object Description => 3060234;

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

	public class NaturePhase4Quest : BaseQuest
	{
		public NaturePhase4Quest()
		{
			AddObjective(new SlayObjective(typeof(GargoyleEnforcer), "gargulec msciciel", 10));
			AddObjective(new ObtainObjective(typeof(Gold), "zloto", 4000, 3821));

			AddReward(new BaseReward(3060228)); // Krok blizej od poznania tajemnic Magii Natury
		}

		public override QuestChain ChainID => QuestChain.Nature;

		public override Type NextQuest => typeof(NaturePhase5Quest);

		/* Wycieczka do Velkyn Ato */
		public override object Title => 3060235;

		/* Kolejnym krokiem bedzie udanie sie do Podmroku, a, by byc dokladnym do Velkyn Ato i wytepienie tam poplecznikow Smierci. Zabicie 10 gargulcow niewolnikow pokaze Naneth, iz nie tylko wiedza o ziolach,
		 ale i sila przepelnia mlodych Magow. Musza one zginac. Pozwoli Ci to tez zebrac fundusze na datek dla Naneth - 4000 centarow */
		public override object Description => 3060236;

		/* Ahh, tracisz moj czas, czy cos?. */
		public override object Refuse => 3060200;

		/* I jak Ci idzie gromienie gargulcow? Chyba slabo.... */
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

	public class NaturePhase5Quest : BaseQuest
	{
		public NaturePhase5Quest()
		{
			AddObjective(new ObtainObjective(typeof(GreaterHealScroll), "Zwoj Wiekszego Leczenia (Greater Heal)", 20,
				0x1F49));
			AddObjective(new ObtainObjective(typeof(SzczepkaBoczniak), "Zarodniki boczniaka", 10, 0x0F23));


			AddReward(new BaseReward(typeof(DruidHollowReedScroll), "Sila Natury"));
			AddReward(new BaseReward("Ksiega Magii Natury"));
		}

		public override QuestChain ChainID => QuestChain.Nature;

		/*Ostatnie zamowienie*/
		public override object Title => 3060238;

		/* Uzdrowiciel poprosil o kolejna dostawe. Poprosil o zebranie 10 zarodnikow boczniakow i 20 zwojow wiekszego leczenia. Musimy pomoc tym biednym ludziom. */
		public override object Description => 3060239;

		/* I teraz chcesz mnie opusicic?!. */
		public override object Refuse => 3060180;

		/* I jak ida zbiory? */
		public override object Uncomplete => 3060240;

		/* *usmiecha sie* Swietene! Mozemy pomoc tym biednym ludziom. */
		public override object Complete => 3060241;

		public override void GiveRewards()
		{
			Owner.AddToBackpack(new DruidSpellbook() { BlessedFor = Owner });
			Owner.SpecialSkills.Nature = true;

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
