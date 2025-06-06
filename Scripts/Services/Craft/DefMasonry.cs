using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Engines.Craft
{
	public enum MasonryRecipes
	{
		AnniversaryVaseShort = 701,
		AnniversaryVaseTall = 702
	}

	public class DefMasonry : CraftSystem
	{
		public override SkillName MainSkill => SkillName.Carpentry;

		public override int GumpTitleNumber => 1044500;

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if (m_CraftSystem == null)
					m_CraftSystem = new DefMasonry();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin(CraftItem item)
		{
			return 0.0; // 0% 
		}

		private DefMasonry()
			: base(1, 1, 1.25)// base( 1, 2, 1.7 ) 
		{
		}

		public override bool RetainsColorFrom(CraftItem item, Type type)
		{
			return true;
		}

		public override int CanCraft(Mobile from, ITool tool, Type itemType)
		{
			if ( from.Mounted )
			{
				return 1072018;  // Nie mozesz wykonywac tej czynnosci bedac konno!
			}
	        
			int num = 0;

			if (tool == null || tool.Deleted || tool.UsesRemaining <= 0)
				return 1044038; // You have worn out your tool!
			else if (tool is Item && !BaseTool.CheckTool((Item)tool, from))
				return 1048146; // If you have a tool equipped, you must use that tool.
			else if (!(from is PlayerMobile && ((PlayerMobile)from).Masonry && from.Skills[SkillName.Carpentry].Base >= 100.0))
				return 1044633; // You havent learned stonecraft.
			else if (!tool.CheckAccessible(from, ref num))
				return num; // The tool must be on your person to use.

			return 0;
		}

		public override void PlayCraftEffect(Mobile from)
		{
			from.Emote( "*rzezbi*" );
			from.PlaySound(0x13F);
		}

		// Delay to synchronize the sound with the hit on the anvil 
		private class InternalTimer : Timer
		{
			private readonly Mobile m_From;

			public InternalTimer(Mobile from)
				: base(TimeSpan.FromSeconds(0.7))
			{
				m_From = from;
			}

			protected override void OnTick()
			{
				m_From.PlaySound(0x23D);
			}
		}

		public override int PlayEndingEffect(Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item)
		{
			if (toolBroken)
				from.SendLocalizedMessage(1044038); // You have worn out your tool 

			if (failed)
			{
				if (lostMaterial)
					return 1044043; // You failed to create the item, and some of your materials are lost. 
				else
					return 1044157; // You failed to create the item, but no materials were lost. 
			}
			else
			{
				if (quality == 0)
					return 502785; // You were barely able to make this item.  It's quality is below average. 
				else if (makersMark && quality == 2)
					return 1044156; // You create an exceptional quality item and affix your maker's mark. 
				else if (quality == 2)
					return 1044155; // You create an exceptional quality item. 
				else
					return 1044154; // You create the item. 
			}
		}

		public override void InitCraftList()
		{
			//Wazy
			AddCraft(typeof(Vase), 3060332, 1022888, 52.5, 102.5, typeof(Granite), 1044514, 1, 1044513);
			AddCraft(typeof(LargeVase), 3060332, 1022887, 52.5, 102.5, typeof(Granite), 1044514, 3, 1044513);
			int index = AddCraft(typeof(SmallUrn), 3060332, 1029244, 82.0, 132.0, typeof(Granite), 1044514, 1, 1044513);
			AddCraft(typeof(SmallTowerSculpture), 3060332, 1029242, 82.0, 132.0, typeof(Granite), 1044514, 3, 1044513);
			AddCraft(typeof(GargoylePainting), 3060332, 1095317, 83.0, 133.0, typeof(Granite), 1044514, 3, 1044513);//Plaskorzezba
			AddCraft(typeof(GargishSculpture), 3060332, 1095319, 82.0, 132.0, typeof(Granite), 1044514, 3, 1044513);//Pokretna Rzezba
			AddCraft(typeof(GargoyleVase), 3060332, 1095322, 80.0, 126.0, typeof(Granite), 1044514, 4, 1044513);//Zdobiona Waza

			/*index = AddCraft(typeof(AnniversaryVaseTall), 3060332, 1156147, 60.0, 110.0, typeof(Granite), 1044514, 6, 1044513);
			AddRecipe(index, (int)MasonryRecipes.AnniversaryVaseTall); //Wysoka Zdobiona Waza

			index = AddCraft(typeof(AnniversaryVaseShort), 3060332, 1156148, 60.0, 110.0, typeof(Granite), 1044514, 6, 1044513);
			AddRecipe(index, (int)MasonryRecipes.AnniversaryVaseShort);//Szeroka Zdobiona Waza*/

			//Meble
			AddCraft(typeof(StoneChair), 1044502, 1024635, 55.0, 105.0, typeof(Granite), 1044514, 4, 1044513);
			AddCraft(typeof(MediumStoneTableEastDeed), 1044502, 1044508, 65.0, 115.0, typeof(Granite), 1044514, 6, 1044513);
			AddCraft(typeof(MediumStoneTableSouthDeed), 1044502, 1044509, 65.0, 115.0, typeof(Granite), 1044514, 6, 1044513);
			AddCraft(typeof(LargeStoneTableEastDeed), 1044502, 1044511, 75.0, 125.0, typeof(Granite), 1044514, 9, 1044513);
			AddCraft(typeof(LargeStoneTableSouthDeed), 1044502, 1044512, 75.0, 125.0, typeof(Granite), 1044514, 9, 1044513);
			AddCraft(typeof(RitualTableDeed), 1044502, 1097690, 94.7, 103.5, typeof(Granite), 1044514, 8, 1044513);//Rytualny Stol
			AddCraft(typeof(Piedestal), 1044502, 3060333, 90.0, 140.0, typeof(Granite), 1044514, 30, 1044513);
            
			index = AddCraft(typeof(LargeGargoyleBedSouthDeed), 1044502, 1111761, 76.0, 126.0, typeof(Granite), 1044514, 3, 1044513);//Kamienne Loze (S)
			AddSkill(index, SkillName.Tailoring, 70.0, 75.0);
			AddRes(index, typeof(Cloth), 1044286, 100, 1044287);

			index = AddCraft(typeof(LargeGargoyleBedEastDeed), 1044502, 1111762, 76.0, 126.0, typeof(Granite), 1044514, 3, 1044513);//Kamienne Loze (E)
			AddSkill(index, SkillName.Tailoring, 70.0, 75.0);
			AddRes(index, typeof(Cloth), 1044286, 100, 1044287);

			index = AddCraft(typeof(GargishCotEastDeed), 1044502, 1111921, 76.0, 126.0, typeof(Granite), 1044514, 3, 1044513);//Kamienne Lozko (E)
			AddSkill(index, SkillName.Tailoring, 70.0, 75.0);
			AddRes(index, typeof(Cloth), 1044286, 100, 1044287);

			index = AddCraft(typeof(GargishCotSouthDeed), 1044502, 1111920, 76.0, 126.0, typeof(Granite), 1044514, 3, 1044513);//Kamienne Lozko (S)
			AddSkill(index, SkillName.Tailoring, 70.0, 75.0);
			AddRes(index, typeof(Cloth), 1044286, 100, 1044287);
            
			//Kominki
			AddCraft(typeof(StoneFireplaceSouthDeed),3060334, 1061849,94.2, 112.2,typeof(Granite), 3060328, 40, 1044351 );
			AddCraft(typeof(StoneFireplaceEastDeed),	3060334, 1061848,94.2, 112.2,typeof(Granite), 3060328, 40, 1044351 );
			AddCraft(typeof(GrayBrickFireplaceSouthDeed), 3060334, 1061847, 94.2, 112.2, typeof(DullCopperGranite), 3060329, 40, 1044351 );
			AddCraft(typeof(GrayBrickFireplaceEastDeed), 3060334, 1061846, 94.2, 112.2, typeof(DullCopperGranite), 3060329, 40, 1044351 );
			AddCraft(typeof(SandstoneFireplaceSouthDeed), 3060334, 1061845, 94.2, 112.2, typeof(GoldGranite), 3060330, 40, 1044351 );
			AddCraft(typeof(SandstoneFireplaceEastDeed), 3060334, 1061844, 94.2, 112.2, typeof(GoldGranite), 3060330, 40, 1044351 );
	        
			//Pomniki
			AddCraft( typeof( PomnikOrlaS ), 3060331, "Pomnik Orla (S)", 85.0, 135.0, typeof( Granite ), 1044514, 25, 1044513 );
			AddCraft( typeof( PomnikOrlaE ), 3060331, "Pomnik Orla (E)", 85.0, 135.0, typeof( Granite ), 1044514, 25, 1044513 );
			AddCraft( typeof( PomnikFeniksaS ), 3060331, "Pomnik Feniksa (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikFeniksaE ), 3060331, "Pomnik Feniksa (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikDuchaS ), 3060331, "Pomnik Ducha (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikDuchaE ), 3060331, "Pomnik Ducha (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikOrdyjskaS ), 3060331, "Pomnik Ordyjski (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikOrdyjskaE ), 3060331, "Pomnik Ordyjski (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikLahlithS ), 3060331, "Pomnik Lahlith (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikLahlithE ), 3060331, "Pomnik Lahlith (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikLowcyS ), 3060331, "Pomnik Lowcy (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikLowcyE ), 3060331, "Pomnik Lowcy (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikElfaS ), 3060331, "Pomnik Elfa (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikElfaE ), 3060331, "Pomnik Elfa (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikGargulcaF ), 3060331, "Pomnik Gargulca", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikSmokaS ), 3060331, "Pomnik Smoka (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikSmokaE ), 3060331, "Pomnik Smoka (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikGryfaS ), 3060331, "Pomnik Gryfa (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikGryfaE ), 3060331, "Pomnik Gryfa (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikKaplankiF ), 3060331, "Pomnik Kaplanki", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikJezdzcaS ), 3060331, "Pomnik Jezdzca (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikJezdzcaE ), 3060331, "Pomnik Jezdzca (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikPanaKoncaS ), 3060331, "Pomnik Pana Konca (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikPanaKoncaE ), 3060331, "Pomnik Pana Konca (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikLotheS ), 3060331, "Pomnik Loethe (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikLotheE ), 3060331, "Pomnik Loethe (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikMagaS ), 3060331, "Pomnik Maga (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikMagaE ), 3060331, "Pomnik Maga (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikOrbaS ), 3060331, "Pomnik Orba (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikOrbaE ), 3060331, "Pomnik Orba (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikSfinksaS ), 3060331, "Pomnik Sfinksa (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikSfinksaE ), 3060331, "Pomnik Sfinksa (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikStraznikaS ), 3060331, "Pomnik Straznika (S)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( PomnikStraznikaE ), 3060331, "Pomnik Straznika (E)", 90.0, 140.0, typeof( Granite ), 1044514, 30, 1044513 );
			AddCraft( typeof( Piedestal ), "Pomniki", "Granitowy piedestal", 80.0, 120.0, typeof(Granite), 1044514, 30, 1044513) ;
			
			//Oltarze
			AddCraft( typeof( OltarzKoncaF ), "Oltarze", "Oltarz Konca", 80.0, 130.0, typeof( Granite ), 1044514, 15, 1044513 );
			AddCraft( typeof( OltarzMatkiF ), "Oltarze", "Oltarz Matki", 80.0, 130.0, typeof( Granite ), 1044514, 15, 1044513 );
			AddCraft( typeof( OltarzPoczatkuF ), "Oltarze", "Oltarz Poczatku", 80.0, 130.0, typeof( Granite ), 1044514, 15, 1044513 );

			// Statues
			AddCraft(typeof(StatueSouth), 1044503, 1044505, 60.0, 110.0, typeof(Granite), 1044514, 3, 1044513);//Statuetka Kobiety
			AddCraft(typeof(StatueNorth), 1044503, 1044506, 60.0, 110.0, typeof(Granite), 1044514, 3, 1044513);//Statuetka Sukkuba
			AddCraft(typeof(StatueEast), 1044503, 1044507, 60.0, 110.0, typeof(Granite), 1044514, 3, 1044513);//Statuetka Mezczyzny
			AddCraft(typeof(StatuePegasusSouth), 1044503, 1044510, 70.0, 120.0, typeof(Granite), 1044514, 4, 1044513);//Statuetka Pegaza
			AddCraft(typeof(StatueGargoyleEast), 1044503, 1097637, 54.5, 104.5, typeof(Granite), 1044514, 20, 1044513);//Statua Gargulca
			AddCraft(typeof(StatueGryphonEast), 1044503, 1097619, 54.5, 104.5, typeof(Granite), 1044514, 15, 1044513);//Statua Gryfa
			AddCraft( typeof( GargulecStatua ), 1044503, "statuetka gargulca", 60.0, 120.0, typeof( Granite ), 1044514, 7, 1044513 );
			AddCraft( typeof( WojownikStatua ), 1044503, "statuetka wojownika", 60.0, 120.0, typeof( Granite ), 1044514, 7, 1044513 );
			AddCraft( typeof( StatueNorth ), 1044503, 1044506, 50.0, 110.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatueEast ), 1044503, 1044507, 50.0, 110.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaDemonaS ), 1044503, "Statuetka Demona (Poludnie)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaDemonaE ), 1044503, "Statuetka Demona (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaDuchaS ), 1044503, "Statuetka Ducha (Poludnie)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaDuchaE ), 1044503, "Statuetka Ducha (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaOrdyjskaS ), 1044503, "Statuetka Ordyjska (Poludnie)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaOrdyjskaE ), 1044503, "Statuetka Ordyjska (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaLahlithS ), 1044503, "Statuetka Lahlith (Poludnie)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaLahlithE ), 1044503, "Statuetka Lahlith (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaLowcyS ), 1044503, "Statuetka lowcy (Poludnie)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaLowcyE ), 1044503, "Statuetka lowcy (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaElfaS ), 1044503, "Statuetka Elfa (Poludnie)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaElfaE ), 1044503, "Statuetka Elfa (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaGargulcaF ), 1044503, "Statuetka Gargulca", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaSmokaS ), 1044503, "Statuetka Smoka (Poludnie)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaSmokaE ), 1044503, "Statuetka Smoka (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaGryfaS ), 1044503, "Statuetka Gryfa (Poludnie)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaGryfaE ), 1044503, "Statuetka Gryfa (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaKaplankiF ), 1044503, "Statuetka Kaplanki", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaJezdzcaS ), 1044503, "Statuetka Jezdzca (Poludnie)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaJezdzcaE ), 1044503, "Statuetka Jezdzca (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaKrasnoludaS ), 1044503, "Statuetka Krasnoluda (Poludnie)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaKrasnoludaE ), 1044503, "Statuetka Krasnoluda (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaBerserkeraS ), 1044503, "Statuetka Berserkera (Poludnie)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaBerserkeraE ), 1044503, "Statuetka Berserkera (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaLotheS ), 1044503, "Statuetka Lothe (Poludnie)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaLotheE ), 1044503, "Statuetka Lothe (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaPanaF ), 1044503, "Statuetka Pana", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaStraznikaF ), 1044503, "Statuetka Straznika (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaSmierciS ), 1044503, "Statuetka Smierci (Poludnie)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaSmierciE ), 1044503, "Statuetka Smierci (Wschod)", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuetkaDuszka ), 1044503, "Statuetka Duszka", 70.0, 120.0, typeof( Granite ), 1044514, 4, 1044513 );
			AddCraft( typeof( StatuetkaDuszkaF ), 1044503, "Statuetka Duszka", 70.0, 120.0, typeof( Granite ), 1044514, 4, 1044513 );
			AddCraft( typeof( StatuetkaDuszkaaF ), 1044503, "Statuetka Duszka", 70.0, 120.0, typeof( Granite ), 1044514, 4, 1044513 );
			AddCraft( typeof( StatuetkaDuszkafF ), 1044503, "Statuetka Duszka", 70.0, 120.0, typeof( Granite ), 1044514, 4, 1044513 );

			//Wazy

			AddCraft( typeof( WazaMalaF ), "Wazy", "Mala Waza", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( WazaMala1F ), "Wazy", "Mala Waza", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( WazaMala2F ), "Wazy", "Mala Waza", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( WazaMala3F ), "Wazy", "Mala Waza", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( WazaMala4F ), "Wazy", "Mala Waza", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( WazaMala5F ), "Wazy", "Mala Waza", 70.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );

			AddCraft( typeof( Waza1F ), "Wazy", "Waza", 72.6, 122.6, typeof( Granite ), 1044514, 4, 1044513 );
			AddCraft( typeof( Waza2F ), "Wazy", "Waza", 72.6, 122.6, typeof( Granite ), 1044514, 4, 1044513 );
			AddCraft( typeof( Waza3F ), "Wazy", "Waza", 72.6, 122.6, typeof( Granite ), 1044514, 4, 1044513 );
			AddCraft( typeof( Waza4F ), "Wazy", "Waza", 72.6, 122.6, typeof( Granite ), 1044514, 4, 1044513 );
			AddCraft( typeof( Waza5F ), "Wazy", "Waza", 72.6, 122.6, typeof( Granite ), 1044514, 4, 1044513 );

			AddCraft( typeof( WazaDuza1F ), "Wazy", "Dula Waza", 75.0, 125.0, typeof( Granite ), 1044514, 6, 1044513 );
			AddCraft( typeof( WazaDuza2F ), "Wazy", "Dula Waza", 75.0, 125.0, typeof( Granite ), 1044514, 6, 1044513 );
			AddCraft( typeof( WazaDuza3F ), "Wazy", "Dula Waza", 75.0, 125.0, typeof( Granite ), 1044514, 6, 1044513 );
			AddCraft( typeof( WazaDuza4F ), "Wazy", "Dula Waza", 75.0, 125.0, typeof( Granite ), 1044514, 6, 1044513 );
			AddCraft( typeof( WazaDuza5F ), "Wazy", "Dula Waza", 75.0, 125.0, typeof( Granite ), 1044514, 6, 1044513 );
			AddCraft( typeof( WazaDuza6F ), "Wazy", "Dula Waza", 75.0, 125.0, typeof( Granite ), 1044514, 6, 1044513 );

			AddCraft( typeof( WazaOgromna1F ), "Wazy", "B.Dula Waza", 80.0, 130.0, typeof( Granite ), 1044514, 10, 1044513 );
			AddCraft( typeof( WazaOgromna2F ), "Wazy", "B.Dula Waza", 80.0, 130.0, typeof( Granite ), 1044514, 10, 1044513 );
			AddCraft( typeof( WazaOgromna3F ), "Wazy", "B.Dula Waza", 80.0, 130.0, typeof( Granite ), 1044514, 10, 1044513 );
			AddCraft( typeof( WazaOgromna4F ), "Wazy", "B.Dula Waza", 80.0, 130.0, typeof( Granite ), 1044514, 10, 1044513 );
			AddCraft( typeof( WazaOgromna5F ), "Wazy", "B.Dula Waza", 80.0, 130.0, typeof( Granite ), 1044514, 10, 1044513 );
			AddCraft( typeof( WazaOgromna6F ), "Wazy", "B.Dula Waza", 80.0, 130.0, typeof( Granite ), 1044514, 10, 1044513 );

			//Kowadla
			/*index = AddCraft(typeof(StoneAnvilSouthDeed), 1044290, 1072876, 78.0, 128.0, typeof(Granite), 1044514, 20, 1044513);//Kamienne Kowadlo (S)
			AddRecipe(index, (int)CarpRecipes.StoneAnvilSouth);

			index = AddCraft(typeof(StoneAnvilEastDeed), 1044290, 1073392, 78.0, 128.0, typeof(Granite), 1044514, 20, 1044513);//Kamienne Kowadlo (E)
			AddRecipe(index, (int)CarpRecipes.StoneAnvilEast);*/

			/*   // Stone Armor
			   index = AddCraft(typeof(FemaleGargishStoneArms), 1111705, 1020643, 56.3, 106.3, typeof(Granite), 1044514, 8, 1044513);
			   AddCraft(typeof(FemaleGargishStoneChest), 1111705, 1020645, 55.0, 105.0, typeof(Granite), 1044514, 12, 1044513);
			   AddCraft(typeof(FemaleGargishStoneLegs), 1111705, 1020649, 58.8, 108.8, typeof(Granite), 1044514, 10, 1044513);
			   AddCraft(typeof(FemaleGargishStoneKilt), 1111705, 1020647, 48.9, 98.9, typeof(Granite), 1044514, 6, 1044513);
			   AddCraft(typeof(GargishStoneArms), 1111705, 1020643, 56.3, 106.3, typeof(Granite), 1044514, 8, 1044513);
			   AddCraft(typeof(GargishStoneChest), 1111705, 1020645, 65.0, 115.0, typeof(Granite), 1044514, 12, 1044513);
			   AddCraft(typeof(GargishStoneLegs), 1111705, 1020649, 58.8, 108.8, typeof(Granite), 1044514, 10, 1044513);
			   AddCraft(typeof(GargishStoneKilt), 1111705, 1020647, 48.9, 98.9, typeof(Granite), 1044514, 6, 1044513);
			   AddCraft(typeof(LargeStoneShield), 1111705, 1095773, 55.0, 105.0, typeof(Granite), 1044514, 16, 1044513);
			   AddCraft(typeof(GargishStoneAmulet), 1111705, 1098594, 60.0, 110.0, typeof(Granite), 1044514, 3, 1044513);

			   // Stone Weapons
			   AddCraft(typeof(StoneWarSword), 1111719, 1022304, 55.0, 105.0, typeof(Granite), 1044514, 18, 1044513);*/

			// Stone Walls
			/*index = AddCraft(typeof(CraftableHouseItem), 1155792, 1155794, 60.0, 110.0, typeof(Granite), 1044514, 10, 1044513);
			SetData(index, CraftableItemType.RoughWindowless);
			SetDisplayID(index, 464);

			index = AddCraft(typeof(CraftableHouseItem), 1155792, 1155797, 60.0, 110.0, typeof(Granite), 1044514, 10, 1044513);
			SetData(index, CraftableItemType.RoughWindow);
			SetDisplayID(index, 467);

			index = AddCraft(typeof(CraftableHouseItem), 1155792, 1155799, 60.0, 110.0, typeof(Granite), 1044514, 10, 1044513);
			SetData(index, CraftableItemType.RoughArch);
			SetDisplayID(index, 469);

			index = AddCraft(typeof(CraftableHouseItem), 1155792, 1155804, 60.0, 110.0, typeof(Granite), 1044514, 10, 1044513);
			SetData(index, CraftableItemType.RoughPillar);
			SetDisplayID(index, 474);

			index = AddCraft(typeof(CraftableHouseItem), 1155792, 1155805, 60.0, 110.0, typeof(Granite), 1044514, 10, 1044513);
			SetData(index, CraftableItemType.RoughRoundedArch);
			SetDisplayID(index, 475);

			index = AddCraft(typeof(CraftableHouseItem), 1155792, 1155810, 60.0, 110.0, typeof(Granite), 1044514, 10, 1044513);
			SetData(index, CraftableItemType.RoughSmallArch);
			SetDisplayID(index, 480);

			index = AddCraft(typeof(CraftableHouseItem), 1155792, 1155814, 60.0, 110.0, typeof(Granite), 1044514, 10, 1044513);
			SetData(index, CraftableItemType.RoughAngledPillar);
			SetDisplayID(index, 486);

			index = AddCraft(typeof(CraftableHouseItem), 1155792, 1155816, 60.0, 110.0, typeof(Granite), 1044514, 10, 1044513);
			SetData(index, CraftableItemType.ShortRough);
			SetDisplayID(index, 488);

			index = AddCraft(typeof(CraftableStoneHouseDoor), 1155792, 1156078, 60.0, 110.0, typeof(Granite), 1044514, 10, 1044513);
			SetData(index, DoorType.StoneDoor_S_In);
			SetDisplayID(index, 804);
			AddCreateItem(index, CraftableStoneHouseDoor.Create);

			index = AddCraft(typeof(CraftableStoneHouseDoor), 1155792, 1156079, 60.0, 110.0, typeof(Granite), 1044514, 10, 1044513);
			SetData(index, DoorType.StoneDoor_E_Out);
			SetDisplayID(index, 805);
			AddCreateItem(index, CraftableStoneHouseDoor.Create);

			index = AddCraft(typeof(CraftableStoneHouseDoor), 1155792, 1156348, 60.0, 110.0, typeof(Granite), 1044514, 10, 1044513);
			SetData(index, DoorType.StoneDoor_S_Out);
			SetDisplayID(index, 804);
			AddCreateItem(index, CraftableStoneHouseDoor.Create);

			index = AddCraft(typeof(CraftableStoneHouseDoor), 1155792, 1156349, 60.0, 110.0, typeof(Granite), 1044514, 10, 1044513);
			SetData(index, DoorType.StoneDoor_E_In);
			SetDisplayID(index, 805);
			AddCreateItem(index, CraftableStoneHouseDoor.Create);

			// Stone Stairs
			index = AddCraft(typeof(CraftableHouseItem), 1155820, 1155821, 60.0, 110.0, typeof(Granite), 1044514, 5, 1044513);
			SetData(index, CraftableItemType.RoughBlock);
			SetDisplayID(index, 1928);

			index = AddCraft(typeof(CraftableHouseItem), 1155820, 1155822, 60.0, 110.0, typeof(Granite), 1044514, 5, 1044513);
			SetData(index, CraftableItemType.RoughSteps);
			SetDisplayID(index, 1929);

			index = AddCraft(typeof(CraftableHouseItem), 1155820, 1155826, 60.0, 110.0, typeof(Granite), 1044514, 5, 1044513);
			SetData(index, CraftableItemType.RoughCornerSteps);
			SetDisplayID(index, 1934);

			index = AddCraft(typeof(CraftableHouseItem), 1155820, 1155830, 60.0, 110.0, typeof(Granite), 1044514, 5, 1044513);
			SetData(index, CraftableItemType.RoughRoundedCornerSteps);
			SetDisplayID(index, 1938);

			index = AddCraft(typeof(CraftableHouseItem), 1155820, 1155834, 60.0, 110.0, typeof(Granite), 1044514, 5, 1044513);
			SetData(index, CraftableItemType.RoughInsetSteps);
			SetDisplayID(index, 1941);

			index = AddCraft(typeof(CraftableHouseItem), 1155820, 1155838, 60.0, 110.0, typeof(Granite), 1044514, 5, 1044513);
			SetData(index, CraftableItemType.RoughRoundedInsetSteps);
			SetDisplayID(index, 1945);

			// Stone Floors
			index = AddCraft(typeof(CraftableHouseItem), 1155877, 1155878, 60.0, 110.0, typeof(Granite), 1044514, 5, 1044513);
			SetData(index, CraftableItemType.LightPaver);
			SetDisplayID(index, 1305);

			index = AddCraft(typeof(CraftableHouseItem), 1155877, 1155879, 60.0, 110.0, typeof(Granite), 1044514, 5, 1044513);
			SetData(index, CraftableItemType.MediumPaver);
			SetDisplayID(index, 1309);

			index = AddCraft(typeof(CraftableHouseItem), 1155877, 1155880, 60.0, 110.0, typeof(Granite), 1044514, 5, 1044513);
			SetData(index, CraftableItemType.DarkPaver);
			SetDisplayID(index, 1313);*/

			MarkOption = true;
			Repair = true;
			CanEnhance = true;

			SetSubRes(typeof(Granite), 1044525);

			AddSubRes(typeof(Granite), 1044525, 00.0, 1044514, 1044526);
			AddSubRes(typeof(DullCopperGranite), 1044023, 65.0, 1044514, 1044526);
			AddSubRes(typeof(ShadowIronGranite), 1044024, 70.0, 1044514, 1044526);
			AddSubRes(typeof(CopperGranite), 1044025, 75.0, 1044514, 1044526);
			AddSubRes(typeof(BronzeGranite), 1044026, 80.0, 1044514, 1044526);
			AddSubRes(typeof(GoldGranite), 1044027, 85.0, 1044514, 1044526);
			AddSubRes(typeof(AgapiteGranite), 1044028, 90.0, 1044514, 1044526);
			AddSubRes(typeof(VeriteGranite), 1044029, 95.0, 1044514, 1044526);
			AddSubRes(typeof(ValoriteGranite), 1044030, 99.0, 1044514, 1044526);
		}
	}
}
