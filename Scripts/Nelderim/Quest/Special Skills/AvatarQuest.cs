using Server.Items;
using Server.Mobiles;
using System;
using Server.ACC.CSS.Systems.Avatar;
using Server.Regions;

namespace Server.Engines.Quests
{
	public class AvatarQuest : BaseSpecialSkillQuest
	{
		public AvatarQuest()
		{
			AddObjective(new SlayObjective(typeof(VampireBat), "nietoperz wampir", 20));

			AddReward(new BaseReward(3060225)); // Droga do poznania magii natury
		}

		public override QuestChain ChainID => QuestChain.Avatar;

		public override Type NextQuest => typeof(AvatarPhase2Quest);

		/* Droga Mnicha */
		public override object Title => 3060255;

		/*  Zanim Pan poblogoslawi Cie bys wyruszyl na sciezke jego walki o porzadek, musisz cos dla niego zrobic, by pokazac, iz godzien jego wiedzy... trzeba wyciac pare slug Smierci. Zabij 20 nietoperzy Wampirow.  */
		public override object Description => 3060256;

		/* No wiesz... juz myslalem, ze mi pomozesz. */
		public override object Refuse => 3060185;

		/* Nie trac mego czasu. Zadanie jest proste - masz zabic 20 nietoperzy wampirow */
		public override object Uncomplete => 3060186;
		
		public override bool CanOffer()
		{
			if (Owner.SpecialSkills.Avatar)
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

	public class AvatarPhase2Quest : BaseQuest
	{
		public AvatarPhase2Quest()
		{
			AddObjective(new ObtainObjective(typeof(PowerCrystal), "Krysztal mocy", 10,
				0x1F49));

			AddReward(new BaseReward(3060259)); // Krok blizej od poznania tajemnic Mnicha
		}

		public override QuestChain ChainID => QuestChain.Avatar;

		public override Type NextQuest => typeof(AvatarPhase3Quest);

		/* Moc Pana */
		public override object Title => 3060258;

		/* Mnisi od zawsze uwazani byli za wiernych Panu i tak to wygladalo przez wieki. Pan wymaga od nas
cierpliwosci i darow, wiec i od Ciebie, jako przyszlego mnicha, wymagac tego bedziemy. Musisz udac sie do
Swiatyni Stworzenia w Tasandorze i dostarczyc tam 10 krysztalow mocy.. */
		public override object Description => 3060257;

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

	public class AvatarPhase3Quest : BaseQuest
	{
		public AvatarPhase3Quest()
		{
			AddObjective(new ObtainObjective(typeof(Gold), "zloto", 30000, 3821));

			AddReward(new BaseReward(3060259)); // Krok blizej od poznania tajemnic Mnicha
		}

		public override QuestChain ChainID => QuestChain.Avatar;

		public override Type NextQuest => typeof(AvatarPhase4Quest);

		/* Datki na swiatynie */
		public override object Title => 3060233;

		/* Kolejnym krokiem na drodze do oswiecenia, bedzie zebranie majatku na rzecz naszego bractwa, bowiem Pan sprzyja praworzadnym, ktorzy majatek zdobywaja cierpliwa praca. Zbierz 30,000 sztuk zlota i dostarcz je do mnie.. */
		public override object Description => 3060260;

		/* No nie. Teraz rezygnujesz?! */
		public override object Refuse => 3060195;

		/* I jak Ci idzie?. */
		public override object Uncomplete => 3060261;

		/* Pan bedzie z Ciebie dumny mlody mnichu */
		public override object Complete => 3060262;

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

	public class AvatarPhase4Quest : BaseQuest
	{
		public AvatarPhase4Quest()
		{
			AddObjective(new SlayObjective(typeof(Dragon), "smok", 10));
			AddObjective(new SlayObjective(typeof(Succubus), "sukkub", 10));
			AddObjective(new SlayObjective(typeof(Imp), "imp", 10));
			AddObjective(new SlayObjective(typeof(Betrayer), "zdrajca", 10));


			AddReward(new BaseReward(3060259)); // Krok blizej od poznania tajemnic Mnicha
		}

		public override QuestChain ChainID => QuestChain.Avatar;

		public override Type NextQuest => typeof(AvatarPhase5Quest);

		/* Na uslugach Pana */
		public override object Title => 3060263;

		/* Pan sprzyja tez tepicielom zla - kazdy kto zgladzi kreature w jego imieniu, poczuje spoczywajacy na swoich plecach wzrok Pana, ktory z usmiechem spoglada na swych heroldow. Z tego wzgledu, musimy sprawdzic czy jestes godzien jego spojrzenia. Musisz wytropic i zabic: 10 sukkubow, 10 smokow, 10 zdrajcow, 10 impow */
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

	public class AvatarPhase5Quest : BaseQuest
	{
		public AvatarPhase5Quest()
		{
			AddObjective(new ObtainObjective(typeof(GreaterHealScroll), "Zwoj Wiekszego Leczenia (Greater Heal)", 30,
				0x1F49));


			AddReward(new BaseReward(typeof(AvatarCurseRemovalScroll), "Reka Mnicha"));
			AddReward(new BaseReward("Księga Zaklęć Mnicha"));
		}

		public override QuestChain ChainID => QuestChain.Avatar;

		/* Ostatnie zamowienie */
		public override object Title => 3060238;

		/* Uzdrowiciel poprosil o kolejna dostawe. Poprosil o zebranie  30 zwojow wiekszego leczenia. Musimy pomoc tym biednym ludziom. */
		public override object Description => 3060265;

		/* I teraz chcesz mnie opusicic?!. */
		public override object Refuse => 3060180;

		/* I jak ida zbiory? */
		public override object Uncomplete => 3060240;

		/* *usmiecha sie* Swietene! Mozemy pomoc tym biednym ludziom. */
		public override object Complete => 3060241;

		public override void GiveRewards()
		{
			Owner.AddToBackpack(new AvatarSpellbook() { BlessedFor = Owner });
			Owner.SpecialSkills.Avatar = true;

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
