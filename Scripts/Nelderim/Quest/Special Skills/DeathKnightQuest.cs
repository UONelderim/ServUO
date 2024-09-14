using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Engines.Quests
{
	public class DeathKnightQuest : BaseSpecialSkillQuest
	{
		public DeathKnightQuest()
		{
			AddObjective(new SlayObjective(typeof(Pixie), "wrozka", 20));

			AddReward(new BaseReward(3060205)); // Krok blizej od poznania tajemnic Mroczego Rycerza.
			AddReward(new BaseReward(typeof(SoulLantern), "Latarnia Dusz")); // Latarnia Dusz
		}

		public override QuestChain ChainID => QuestChain.DeathKnight;

		public override Type NextQuest => typeof(DeathKnightPhase2Quest);

		/* Mroczny Rycerz nachodzi */
		public override object Title => 3060206;

		/* Rycerz zawsze ma 2 ścieżki: Pierwsza z nich jest Ścieżką Światła i Ci, którzy nią podążają, są prawi, lub Ścieżka Mroku, którą podążają nieliczni, wykolejeńcy…,
		 Ci, którzy zawarli pakt ze Śmiercią i pozyskują dlań dusze w zamian za moc wprost z czarnego jej serca. Najsampierw, zamrodowac ze szczegolnym okrucienstwem musisz slugi matki. Wrozki, mowiac konkretnie. 20 sztuk. Aby przypodobac sie naszej Pani.  */
		public override object Description => 3060207;

		/* No wiesz... juz myslalem, ze mi pomozesz. */
		public override object Refuse => 3060185;

		/* Nie trac mego czasu. Zadanie jest proste - masz zabic 20 wrozek */
		public override object Uncomplete => 3060208;

		public override void GiveRewards()
		{
			base.GiveRewards();

			var soulLanterns = Owner.Backpack.FindItemsByType<SoulLantern>();
			var soulLantern = soulLanterns.FirstOrDefault(s => s.Owner == null);
			if (soulLantern != null)
			{
				soulLantern.Owner = Owner;
			}
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

	public class DeathKnightPhase2Quest : BaseQuest
	{
		public DeathKnightPhase2Quest()
		{
			AddObjective(new SoulsObjective());

			AddReward(new BaseReward(3060205)); // Krok blizej od poznania tajemnic Mrocznego Rycerza
		}

		public override QuestChain ChainID => QuestChain.DeathKnight;

		public override Type NextQuest => typeof(DeathKnightPhase3Quest);

		/* Dusze zaginione */
		public override object Title => 3060210;

		/*Kazdy upadly paladyn kieruje sie w strone przyblizajaca go do Smierci. Smierc nie odtraca tych, ktorzy poprzednio wyznawali inne bostwa. Smierc jest laskawa...jednakze, jej laska ma swa cene.
		 Otrzymasz bowiem Latarnie Dusz, za pomoca ktorej musisz pochwycic 8000 dusz. Kiedy te dusze pochwycisz, wroc do mnie.  */
		public override object Description => 3060188;

		/* Nie chcesz pomoc, to nie zawracaj mi glowy. Potrzebujacy czekaja... */
		public override object Refuse => 3060189;

		/* Ty smieciu! Pani nasza czeka na te dusze! */
		public override object Uncomplete => 3060212;

		/* Te dusze na pewno zostana docenione przez Smierc */
		public override object Complete => 3060211;

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

	public class DeathKnightPhase3Quest : BaseQuest
	{
		public DeathKnightPhase3Quest()
		{
			AddObjective(new ObtainObjective(typeof(Gold), "zloto", 5000, 3821));


			AddReward(new BaseReward(3060205)); // Krok blizej od poznania tajemnic Mrocznego Rycerza.
		}

		public override QuestChain ChainID => QuestChain.DeathKnight;

		public override Type NextQuest => typeof(DeathKnightPhase4Quest);

		/* Poswiecenie godne Smierci */
		public override object Title => 3060213;

		/* Nastepnym krokiem bedzie Twe poswiecenie, w zlocie. Smierc, tak jak i Pan, wymagaja ofiar w zlocie. Wplac tam 5000 sztuk zlota na rzecz Smierci. */
		public override object Description => 3060214;

		/* No nie. Teraz rezygnujesz?! */
		public override object Refuse => 3060195;

		/* No cos Ty... To tylko 5000 centarow.... */
		public override object Uncomplete => 3060215;

		/* *klaszcze entuzjastycznie* */
		public override object Complete => 3060216;

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

	public class DeathKnightPhase4Quest : BaseQuest
	{
		public DeathKnightPhase4Quest()
		{
			AddObjective(new SlayObjective(typeof(Unicorn), "jednorozec", 10));

			AddReward(new BaseReward(3060205)); // Krok blizej od poznania tajemnic Mrocznego Rycerza.
		}

		public override QuestChain ChainID => QuestChain.DeathKnight;

		public override Type NextQuest => typeof(DeathKnightPhase5Quest);

		/* Swieta Misja */
		public override object Title => 3060217;

		/* Kolejne zadanie, moj adepcie. Jednorozce, mawia sie, ze posiadaja sekret wiecznego zywota.
		 Trucizna sie ich nie ima, dusze maja najczystrze. Zabij ich 10 sztuk, aby Matka poczula uklucie smierci! */
		public override object Description => 3060218;

		/* Ahh, tracisz moj czas, czy cos?. */
		public override object Refuse => 3060200;

		/* I jak idzie Ci mordowanie niewinnych stworzen? *cieszy sie zlowieszczo**/
		public override object Uncomplete => 3060219;

		/* Brawo, brawo. Jestesmy juz prawie na koncu drogi. */
		public override object Complete => 3060220;

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

	public class DeathKnightPhase5Quest : BaseQuest
	{
		public DeathKnightPhase5Quest()
		{
			AddObjective(new ObtainObjective(typeof(VolcanicAsh), "pyl wulkaniczny", 20, 3983));


			AddReward(new BaseReward(typeof(DemonicTouchSkull), "Dotyk Demona")); // Dotyk Demona
			AddReward(new BaseReward(typeof(DeathKnightSpellbook), "Ksiega Mrocznego Rycerza")); // Ksiega 
		}

		public override QuestChain ChainID => QuestChain.DeathKnight;

		/*Z pylu powstales*/
		public override object Title => 3060221;

		/* Aby dolaczyc do waskiego grona Rycerzy Mroku, nalezy zdobyc pyl wulkaniczny potrzebny do przeprowadzenia rytualu przywolania demona,
		 ktory naznaczy Ciebie jako swego awatara. Zdobadz 20 szuk pylu wulkanicznego. */
		public override object Description => 3060222;

		/* To tylko glupi pyl wulkaniczny. Nie dasz rady? */
		public override object Refuse => 3060223;

		/* Smierc bedzie rozczarowana, jesli Ci sie nie uda. */
		public override object Uncomplete => 3060224;

		/* *klaszcze entuzjastycznie* */
		public override object Complete => 3060216;

		public override void GiveRewards()
		{
			Owner.SpecialSkills.DeathKnight = true;

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

	public class SoulsObjective : SimpleObjective
	{
		private readonly List<string> m_Descr = new List<string>();
		public override List<string> Descriptions => m_Descr;

		public SoulsObjective()
			: base(8000, -1)
		{
			m_Descr.Add("Pochwyc 8000 dusz zabijajac potwory trzymajac latarnie w rece.");
		}

		public override bool Update(object obj)
		{
			if (obj is int soulsAdded)
			{
				CurProgress += soulsAdded;

				if (Completed)
					Quest.OnCompleted();
				else
				{
					Quest.Owner.SendMessage($"Pozostalo dusz do zebrania: {MaxProgress - CurProgress}");
					Quest.Owner.PlaySound(Quest.UpdateSound);
				}

				return true;
			}

			return false;
		}
	}
}
