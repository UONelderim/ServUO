using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Engines.Quests
{
	public class MysticQuest : BaseSpecialSkillQuest
	{
		public MysticQuest()
		{
			AddObjective(new SlayObjective(typeof(Beetle), "Ogromny zuk", 20));

			AddReward(new BaseReward(3060158)); // Szansa poznania drogi Mistyka.
		}

		public override QuestChain ChainID => QuestChain.Mystic;

		public override Type NextQuest => typeof(MysticPhase2Quest);

		/* Mystic */
		public override object Title => 3060159;

		/* Nauka zaklec to jedno. Aby zostac prawdziwym mistykiem, nalezy wykazac sie prawdziwa wiedza i madroscia.
		 Nie nauczamy tych, ktorzy sa niegodnymi wiedzy mistycznej. Twym pierwszym zadaniem bedzie zidentyfikowanie
		 w ziemi Nelderim ogromnych zukow i pozbycie sie 20 sztuk owej bestyji.  */
		public override object Description => 3060160;

		/* No wiesz... juz myslalem, ze mi pomozesz. */
		public override object Refuse => 3060161;

		/* You waste my time.  The task is simple. Kill 50 rats in an hour. */
		public override object Uncomplete => 3060162;

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

	public class MysticPhase2Quest : BaseQuest
	{
		public MysticPhase2Quest()
		{
			AddObjective(new ObtainObjective(typeof(DragonsHeart), "serce smoka", 1, 3985));

			AddReward(new BaseReward(3060158)); // Szansa poznania drogi Mistyka.
		}

		public override QuestChain ChainID => QuestChain.Mystic;

		public override Type NextQuest => typeof(MysticPhase3Quest);

		/* Wazny skladnik */
		public override object Title => 3060163;

		/* Mistycyzm to nie czary z powietrza. Aby wyczarowac potezne zaklecia mistyczne, potrzebne sa pewne skladniki.
		 Aby rozpoczac sciezke mistyka, bedziesz musial zdobyc dla mnie 1 serce smoka, ktore pozwoli mi rzucic zaklecie, ktore otworzy Twoj umysl i ukryty potencjal. */
		public override object Description => 3060164;

		/* Zagrazasz sciezce rozwoju Mistykow. Precz... */
		public override object Refuse => 3060165;

		/* No dalej, przyniesc to serce smoka. */
		public override object Uncomplete => 3060166;

		/* Well, where are the cotton bales? */
		public override object Complete => 3060167;

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

	public class MysticPhase3Quest : BaseQuest
	{
		public MysticPhase3Quest()
		{
			AddObjective(new ObtainObjective(typeof(BlankScroll), "czysty zwoj", 250, 0x1BD7));

			AddReward(new BaseReward(3060158)); // Szansa poznania drogi Mistyka.
		}

		public override QuestChain ChainID => QuestChain.Mystic;

		public override Type NextQuest => typeof(MysticPhase4Quest);

		/* Ksiega Mistyka */
		public override object Title => 3060168;

		/* We must look to the defense of our people! Bring boards for new arrows. */
		public override object Description => 3060169;

		/* Teraz musimy stworzyc Twoja ksiege. Niestety, zuzylem ostatnie zwoje na kilka zaklec dla moich uczniow. Czy mozesz zdbobyc kilka sztuk? */
		public override object Refuse => 3060170;

		/* Jak Ci idzie z tymi zwojami?. */
		public override object Uncomplete => 3060171;

		/* Well, where are the boards? */
		public override object Complete => 3060172;

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

	public class MysticPhase4Quest : BaseQuest
	{
		public MysticPhase4Quest()
		{
			AddObjective(new ObtainObjective(typeof(ArtefaktowyPyl), "pyl artefaktowy", 1, 0x26B8));
			AddObjective(new ObtainObjective(typeof(ScribesPen), "pioro skryby", 1, 0x0FBF));
			AddObjective(new ObtainObjective(typeof(Grapes), "winogrono", 20, 0x9D1));

			AddReward(new BaseReward(3060158)); // Szansa poznania drogi Mistyka.
		}

		public override QuestChain ChainID => QuestChain.Mystic;

		public override Type NextQuest => typeof(MysticPhase5Quest);

		/* Wiecej zapasow */
		public override object Title => 3060173;

		/* Aby napisac dla Ciebie ksiege, potrzebuje duzo wiecej, niz tylko zwoje. Ostantie zapasy zostaly zrabowane przez rzezimieszkow,
		 wiec trzeba zdobyc kilka rzeczy. Potrzebujemy 1 sztuke pylu artefaktowego, 20 winogron na tusz i 1 pioro skryby. */
		public override object Description => 3060174;

		/* Ahh, tracisz moj czas, czy cos?. */
		public override object Refuse => 3060175;

		/* Dlugo mam jeszcze czekac na te zapasy? */
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

	public class MysticPhase5Quest : BaseQuest
	{
		public MysticPhase5Quest()
		{
			AddObjective(new ObtainObjective(typeof(ArcaneGem), "tajemniczy kamien", 30, 0x1EA7));


			AddReward(new BaseReward(typeof(NetherBoltScroll), 1031678));
			AddReward(new BaseReward(1031677)); // MysticBook
		}

		public override QuestChain ChainID => QuestChain.Mystic;

		/* Tajemnice*/
		public override object Title => 3060178;

		/* Mistyk nie tylko tworzy ksiegi czy zabija stwory, ktore sluza Smierci. Mistyk, przede wszystkim, zglebia swa wiedze. W zwiazku z tym, poprosze Cie o zbadanie tajemniczych kamieni.
		 Najczesciej posiadaja je jukowie. W szczegvolnosci Lordowie Juka.
		 Kazdy kamien kryje w sobie pewna tajemnice zakleta w sobie. Warto je poznac. Zdobac 30 sztuk takich kamieni i przynies je tu.  */
		public override object Description => 3060179;

		/* I teraz chcesz mnie opusicic?!. */
		public override object Refuse => 3060180;

		/* Te tajemnice musza byc odkryte! */
		public override object Uncomplete => 3060181;

		/* O, widze, ze przyniosles te tajemnicze kamienie, o ktore prosilem. */
		public override object Complete => 3060182;

		public override void GiveRewards()
		{
			Owner.AddToBackpack(new MysticBook() { BlessedFor = Owner });
			Owner.SpecialSkills.Mysticism = true;

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
