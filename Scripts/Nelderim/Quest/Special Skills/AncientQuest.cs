using Server.Items;
using Server.Mobiles;
using System;
using Server.ACC.CSS.Systems.Ancient;
using Server.Regions;

namespace Server.Engines.Quests
{
	public class AncientQuest : BaseSpecialSkillQuest
	{
		public AncientQuest()
		{
			AddObjective(new SlayObjective(typeof(Imp), "Imp", 15));

			AddReward(new BaseReward(3060243)); // Krok blizej do poznania tajnikow starozytnej magii.
		}

		public override QuestChain ChainID => QuestChain.Ancient;

		public override Type NextQuest => typeof(AncientPhase2Quest);

		/* Starozytna Magyja */
		public override object Title => 3060242;

		/* Starozytna Magia jest zarezerwowana dla waskiej grupy smialkow gotowych poswiecic swe zycie na zglebianie tajnikow magii. Niewielu dostapilo zaszczytu przegladania Ksiag Moreny. Zanim zwrocila sie do Smierci, zglebiala bowiem Starozytna Magie dniami i nocami. Aby odblokowac potencjal Twego umyslu, sama wola nie wystarczy. Bedziesz musial odszukac magiczna kostke, ktora zdejmie kajdany z Twego umyslu i pozwoli Ci pojac misterium Starozytnych. Magie, ktora ongis jedynie Bogowie wladali. Najpierw trzeba bedzie zabic 15 impow, by zyskac wystarczajaco duzo esencji magicznej, aby wykonac rytulal  */
		public override object Description => 3060244;

		/* No wiesz... juz myslalem, ze mi pomozesz. */
		public override object Refuse => 3060161;

		/* No wiesz? Juz rezygnujesz... */
		public override object Uncomplete => 3060245;

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

	public class AncientPhase2Quest : BaseQuest
	{
		public AncientPhase2Quest()
		{
			AddObjective(new DeliverObjective(typeof(KostkaAncientQuestItem), "Pierwsza Czesc Starozytnej Kosci", 1,
				typeof(DungeonRegion), "Labitynt"));

			AddReward(new BaseReward(3060243)); // Krok blizej do poznania tajnikow starozytnej magii
		}

		public override QuestChain ChainID => QuestChain.Ancient;

		public override Type NextQuest => typeof(AncientPhase3Quest);

		/* Wazny skladnik */
		public override object Title => 3060163;

		/* Doniesiono mi, że kostki można szukać w kilku miejscach, lecz nie wiadomo, która z nich jest tą
właściwą: W Labiryncie Minotaurów ponoc slyszano o tym, ze owa kostka moze byc. Sprawdz to miejsce. */
		public override object Description => 3060246;

		/* No wiesz? Juz rezygnujesz... */
		public override object Refuse => 3060245;

		/* Ahh... no pospiesz sie.... */
		public override object Uncomplete => 3060247;

		/* *entuzjastycznie bije brawo* */
		public override object Complete => 3060248;

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

	public class AncientPhase3Quest : BaseQuest
	{
		public AncientPhase3Quest()
		{
			AddObjective(new DeliverObjective(typeof(KostkaAncientQuestItem2), "Druga Czesc Starozytnej Kosci", 1,
				typeof(DungeonRegion), "Labitynt"));

			AddReward(new BaseReward(3060243)); // Krok blizej do poznania tajnikow starozytnej magii
		}

		public override QuestChain ChainID => QuestChain.Ancient;

		public override Type NextQuest => typeof(AncientPhase4Quest);

		/* Kolejny krok */
		public override object Title => 3060249;

		/* Slyszalem, ze druga czesc powinna byc w Wulkanie na samym jego koncu, tuz przy wielkim imiennym smoku. Warto zerknac. Tam kosc powinna sie polaczyc. */
		public override object Description => 3060250;

		/* No wiesz? Juz rezygnujesz... */
		public override object Refuse => 3060245;

		/* I jak Ci idzie? */
		public override object Uncomplete => 3060251;

		/* BRAWO!! UDALO CI SIE! */
		public override object Complete => 3060252;

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

	public class AncientPhase4Quest : BaseQuest
	{
		public AncientPhase4Quest()
		{
			AddObjective(new ObtainObjective(typeof(ArtefaktowyPyl), "pyl artefaktowy", 1, 0x26B8));
			AddObjective(new ObtainObjective(typeof(ScribesPen), "pioro skryby", 1, 0x0FBF));
			AddObjective(new ObtainObjective(typeof(BlankScroll), "czysty zwoj", 20, 0xEF3));

			AddReward(new BaseReward(3060243)); // Krok blizej do poznania tajnikow starozytnej magii
		}

		public override QuestChain ChainID => QuestChain.Ancient;

		public override Type NextQuest => typeof(AncientPhase5Quest);

		/* Wiecej zapasow */
		public override object Title => 3060173;

		/* Aby napisac dla Ciebie ksiege, potrzebuje duzo wiecej, niz tylko zwoje. Ostantie zapasy zostaly zrabowane przez rzezimieszkow,
		 wiec trzeba zdobyc kilka rzeczy. Potrzebujemy 1 sztuke pylu artefaktowego, 20 winogron na tusz i 1 pioro skryby. */
		public override object Description => 3060253;

		/* Ahh, tracisz moj czas, czy cos?. */
		public override object Refuse => 3060175;

		/* Dlugo mam jeszcze czekac na te zapasy? */
		public override object Uncomplete => 3060176;

		/* *entuzjastycznie bije brawo*! */
		public override object Complete => 3060248;

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

	public class AncientPhase5Quest : BaseQuest
	{
		public AncientPhase5Quest()
		{
			AddObjective(new ObtainObjective(typeof(ArcaneGem), "tajemniczy kamien", 30, 0x1EA7));

			AddReward(new BaseReward(typeof(AncientIgniteScroll), "Zwój podpalenia"));		
			AddReward(new BaseReward("Ksiega Starozytnej Magii"));
		}

		public override QuestChain ChainID => QuestChain.Ancient;

		/* Tajemnice*/
		public override object Title => 3060178;

		/* Aby odkryc tajemnice starozytnej magii, musimy zebrac kilka tajemniczych kamieni, ktore pozniej roztrzaskamy, by ich tajemnice zostaly przed nami odkryte.  */
		public override object Description => 3060254;

		/* I teraz chcesz mnie opusicic?!. */
		public override object Refuse => 3060180;

		/* Te tajemnice musza byc odkryte! */
		public override object Uncomplete => 3060181;

		/* O, widze, ze przyniosles te tajemnicze kamienie, o ktore prosilem. */
		public override object Complete => 3060182;

		public override void GiveRewards()
		{
			Owner.AddToBackpack(new AncientSpellbook { BlessedFor = Owner });
			Owner.SpecialSkills.Ancient = true;

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
