using Server.Items;

using System;
using System.Linq;

namespace Server.Engines.Craft
{
    #region Mondain's Legacy
    public enum CarpRecipes
    {
        // stuff
        WarriorStatueSouth = 100,
        WarriorStatueEast = 101,
        SquirrelStatueSouth = 102,
        SquirrelStatueEast = 103,
        AcidProofRope = 104,
        OrnateElvenChair = 105,
        ArcaneBookshelfSouth = 106,
        ArcaneBookshelfEast = 107,
        OrnateElvenChestSouth = 108,
        ElvenDresserSouth = 109,
        ElvenDresserEast = 110,
        FancyElvenArmoire = 111,
        ArcanistsWildStaff = 112,
        AncientWildStaff = 113,
        ThornedWildStaff = 114,
        HardenedWildStaff = 115,
        TallElvenBedSouth = 116,
        TallElvenBedEast = 117,
        StoneAnvilSouth = 118,
        StoneAnvilEast = 119,
        OrnateElvenChestEast = 120,

        // arties
        PhantomStaff = 150,
        IronwoodCrown = 151,
        BrambleCoat = 152,

        SmallElegantAquarium = 153,
        WallMountedAquarium = 154,
        LargeElegantAquarium = 155,

        KotlBlackRod = 170,
        KotlAutomaton = 171,
    }
    #endregion

    public class DefCarpentry : CraftSystem
    {
        public override SkillName MainSkill => SkillName.Carpentry;

        public override int GumpTitleNumber => 1044004;

        public override CraftECA ECA => CraftECA.ChanceMinusSixtyToFourtyFive;

        private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem
        {
            get
            {
                if (m_CraftSystem == null)
                    m_CraftSystem = new DefCarpentry();

                return m_CraftSystem;
            }
        }

        public override double GetChanceAtMin(CraftItem item)
        {
            return 0.5; // 50%
        }

        private DefCarpentry()
            : base(1, 1, 1.25)// base( 1, 1, 3.0 )
        {
        }

        public override int CanCraft(Mobile from, ITool tool, Type itemType)
        {
            int num = 0;

            if (tool == null || tool.Deleted || tool.UsesRemaining <= 0)
                return 1044038; // You have worn out your tool!
            else if (!tool.CheckAccessible(from, ref num))
                return num; // The tool must be on your person to use.

            return 0;
        }

        private readonly Type[] _RetainsColor = new[]
        {
            typeof(BasePlayerBB)
        };

        public override bool RetainsColorFrom(CraftItem item, Type type)
        {
            var itemType = item.ItemType;

            if (_RetainsColor.Any(t => t == itemType || itemType.IsSubclassOf(t)))
            {
                return true;
            }

            return base.RetainsColorFrom(item, type);
        }

        public override void PlayCraftEffect(Mobile from)
        {
	        if ( from.Body.Type == BodyType.Human && !from.Mounted )
		        from.Animate( 33, 5, 1, true, false, 0 );
            from.PlaySound(0x23D);
            from.Emote("*wyrabia przedmiot*");
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
            int index = -1;

            #region Siedzenia 1044294
            AddCraft(typeof(FootStool), 1044294, 1022910, 11.0, 36.0, typeof(Board), 1044041, 9, 1044351);//Maly Taboret
            AddCraft(typeof(Stool), 1044294, 1022602, 11.0, 36.0, typeof(Board), 1044041, 9, 1044351);//Taboret
            AddCraft(typeof(BambooChair), 1044294, 1044300, 21.0, 46.0, typeof(Board), 1044041, 13, 1044351);//Krzeslo
            AddCraft(typeof(WoodenChair), 1044294, 1044301, 21.0, 46.0, typeof(Board), 1044041, 13, 1044351);//Proste Krzeslo
            AddCraft(typeof(WoodenChairCushion), 1044294, 1044303, 42.1, 67.1, typeof(Board), 1044041, 13, 1044351);//Tapicerowane Krzeslo
            AddCraft(typeof(WoodenThrone), 1044294, 1044304, 52.6, 77.6, typeof(Board), 1044041, 17, 1044351);//Drewniane Krzeslo
            AddCraft(typeof(BigElvenChair), 1044294, 1072872, 85.0, 110.0, typeof(Board), 1044041, 40, 1044351);//Zdobione Krzeslo
            AddCraft(typeof(ElvenReadingChair), 1044294, 1072873, 80.0, 105.0, typeof(Board), 1044041, 30, 1044351);//Niskie Krzeslo

            //index = AddCraft(typeof(OrnateElvenChair), 1044294, 1072870, 80.0, 105.0, typeof(Board), 1044041, 30, 1044351);//Ornamentowane Krzeslo
           // AddRecipe(index, (int)CarpRecipes.OrnateElvenChair);

            AddCraft(typeof(Throne), 1044294, 1044305, 73.6, 98.6, typeof(Board), 1044041, 19, 1044351);//Tron
            AddCraft(typeof(TerMurStyleChair), 1044294, 1095291, 85.0, 110.0, typeof(Board), 1044041, 40, 1044351);//Wielki Tron
            
            index = AddCraft(typeof(UpholsteredChairDeed), 1044294, 1154173, 70.0, 110.0, typeof(Board), 1044041, 40, 1044351);//Fotel
            AddSkill(index, SkillName.Tailoring, 55.0, 60.0);
            AddRes(index, typeof(Cloth), 1044286, 12, 1044287);

            AddCraft(typeof(WoodenBench), 1044294, 1022860, 52.6, 77.6, typeof(Board), 1044041, 17, 1044351);//Drewniana Laweczka

            AddCraft(typeof(RusticBenchSouthDeed), 1044294, 1150593, 94.7, 119.8, typeof(Board), 1044041, 35, 1044351);//Rustykalna Lawka (S)
            AddCraft(typeof(RusticBenchEastDeed), 1044294, 1150594, 94.7, 119.8, typeof(Board), 1044041, 35, 1044351);//Rustykalna Lawka (E)
            
            index = AddCraft(typeof(ElvenLoveseatSouthDeed), 1044294, 1072867, 80.0, 105.0, typeof(Board), 1044041, 50, 1044351);//Wytworna Lawka (S)
            SetDisplayID(index, 0x2DDF);
            ForceNonExceptional(index);

            index = AddCraft(typeof(ElvenLoveseatEastDeed), 1044294, 1073372, 80.0, 105.0, typeof(Board), 1044041, 50, 1044351);//Wytworna Lawka (E)
            SetDisplayID(index, 0x2DE0);
            ForceNonExceptional(index);

            index = AddCraft(typeof(FancyLoveseatDeed), 1044294, 1098462, 70.0, 120.0, typeof(Board), 1044041, 80, 1044351);//Wytworna Mala Sofa (S)
            AddSkill(index, SkillName.Tailoring, 55.0, 60.0);
            AddRes(index, typeof(Cloth), 1044286, 24, 1044287);

            index = AddCraft(typeof(FancyCouchSouthDeed), 1044294, 1154139, 70.0, 120.0, typeof(Board), 1044041, 80, 1044351);//Wytowrna Sofa (S)
            AddSkill(index, SkillName.Tailoring, 55.0, 60.0);
            AddRes(index, typeof(Cloth), 1044286, 48, 1044287);

            index = AddCraft(typeof(FancyCouchEastDeed), 1044294, 1154140, 70.0, 120.0, typeof(Board), 1044041, 80, 1044351);//Wytowrna Sofa (E)
            AddSkill(index, SkillName.Tailoring, 55.0, 60.0);
            AddRes(index, typeof(Cloth), 1044286, 48, 1044287);

            index = AddCraft(typeof(FancyCouchNorthDeed), 1044294, 1156582, 70.0, 120.0, typeof(Board), 1044041, 80, 1044351);//Wytowrna Sofa (N)
            AddSkill(index, SkillName.Tailoring, 55.0, 60.0);
            AddRes(index, typeof(Cloth), 1044286, 48, 1044287);

            index = AddCraft(typeof(FancyCouchWestDeed), 1044294, 1156583, 70.0, 120.0, typeof(Board), 1044041, 80, 1044351);//Wytowrna Sofa (W)
            AddSkill(index, SkillName.Tailoring, 55.0, 60.0);
            AddRes(index, typeof(Cloth), 1044286, 48, 1044287);

            index = AddCraft(typeof(PlushLoveseatSouthDeed), 1044294, 1154135, 70.0, 120.0, typeof(Board), 1044041, 80, 1044351);//Kanapa (S)
            AddSkill(index, SkillName.Tailoring, 55.0, 60.0);
            AddRes(index, typeof(Cloth), 1044286, 24, 1044287);

            index = AddCraft(typeof(PlushLoveseatEastDeed), 1044294, 1154136, 70.0, 120.0, typeof(Board), 1044041, 80, 1044351);//Kanapa (W)
            AddSkill(index, SkillName.Tailoring, 55.0, 60.0);
            AddRes(index, typeof(Cloth), 1044286, 24, 1044287);

            AddCraft(typeof(GargishCouchEastDeed), 1044294, 1111776, 90.0, 115.0, typeof(Board), 1044041, 75, 1044351);//Ornamentowana Kanapa (E)
            AddCraft(typeof(GargishCouchSouthDeed), 1044294, 1111775, 90.0, 115.0, typeof(Board), 1044041, 75, 1044351);//Ornamentowana Kanapa (S)
            #endregion
            
            #region Szafy i Pojemniki 1044291
            AddCraft(typeof(WoodenBox), 1044291, 1023709, 21.0, 46.0, typeof(Board), 1044041, 10, 1044351);//Skrzynka
            AddCraft(typeof(SmallCrate), 1044291, 1044309, 10.0, 35.0, typeof(Board), 1044041, 8, 1044351);//Mala Skrzynka
            AddCraft(typeof(MediumCrate), 1044291, 1044310, 31.0, 56.0, typeof(Board), 1044041, 15, 1044351);//Srednia Skrzynka
            AddCraft(typeof(LargeCrate), 1044291, 1044311, 47.3, 72.3, typeof(Board), 1044041, 18, 1044351);//Duza Skrzynka
            AddCraft(typeof(WoodenChest), 1044291, 1023650, 73.6, 98.6, typeof(Board), 1044041, 20, 1044351);//Drewniana Skrzynia
            AddCraft(typeof(RarewoodChest), 1044291, 1073402, 80.0, 105.0, typeof(Board), 1044041, 30, 1044351);//Ozdobna Skrzynia
            AddCraft(typeof(GargishChest), 1044291, 1095293, 80.0, 105.0, typeof(Board), 1044041, 30, 1044351);//Ornamentowana Skrzynia
            AddCraft(typeof(DecorativeBox), 1044291, 1073403, 80.0, 105.0, typeof(Board), 1044041, 25, 1044351);//Pudelko Dekoracyjne
            AddCraft(typeof(PlainWoodenChest), 1044291, 1030251, 90.0, 115.0, typeof(Board), 1044041, 30, 1044351);//Prosta Drewniania Skrzynia
            AddCraft(typeof(OrnateWoodenChest), 1044291, 1030253, 90.0, 115.0, typeof(Board), 1044041, 30, 1044351);//Zdobiona Drewniania Skrzynia
            AddCraft(typeof(GildedWoodenChest), 1044291, 1030255, 90.0, 115.0, typeof(Board), 1044041, 30, 1044351);//Pozlacana Drewniana Skrzynia
            AddCraft(typeof(WoodenFootLocker), 1044291, 1030257, 90.0, 115.0, typeof(Board), 1044041, 30, 1044351);//Drewniany Kufer
            AddCraft(typeof(FinishedWoodenChest), 1044291, 1030259, 90.0, 115.0, typeof(Board), 1044041, 30, 1044351);//Okuta Drewniana Skrzynia
            AddCraft(typeof(TallCabinet), 1044291, 1030261, 90.0, 115.0, typeof(Board), 1044041, 35, 1044351);//Wysoka Komoda
            AddCraft(typeof(ShortCabinet), 1044291, 1030263, 90.0, 115.0, typeof(Board), 1044041, 35, 1044351);//Niska Komoda
            AddCraft(typeof(ElegantArmoire), 1044291, 1030330, 90.0, 115.0, typeof(Board), 1044041, 40, 1044351);//Elegancka Mala Komoda
            AddCraft(typeof(MapleArmoire), 1044291, 1030332, 90.0, 115.0, typeof(Board), 1044041, 40, 1044351);//Zdobiona Mala Komoda
            AddCraft(typeof(CherryArmoire), 1044291, 1030334, 90.0, 115.0, typeof(Board), 1044041, 40, 1044351);//Mala Komoda
            
            // AddCraft(typeof(RedArmoire), 1044291, 1030328, 90.0, 115.0, typeof(Board), 1044041, 40, 1044351); Dziwny Kolor

            /*index = AddCraft(typeof(ArcaneBookShelfDeedSouth), 1044291, 1072871, 94.7, 119.7, typeof(Board), 1044041, 80, 1044351);//Ogromna Komoda (S)
            AddRecipe(index, (int)CarpRecipes.ArcaneBookshelfSouth);
            ForceNonExceptional(index);

            index = AddCraft(typeof(ArcaneBookShelfDeedEast), 1044291, 1073371, 94.7, 119.7, typeof(Board), 1044041, 80, 1044351);//Ogromna Komoda (E)
            AddRecipe(index, (int)CarpRecipes.ArcaneBookshelfEast);
            ForceNonExceptional(index);

            index = AddCraft(typeof(OrnateElvenChestSouthDeed), 1044291, 1072862, 94.7, 119.7, typeof(Board), 1044041, 40, 1044351);//Ornamentowana Niska Komoda (S)
            AddRecipe(index, (int)CarpRecipes.OrnateElvenChestSouth);
            ForceNonExceptional(index);

            index = AddCraft(typeof(OrnateElvenChestEastDeed), 1044291, 1073383, 94.7, 119.7, typeof(Board), 1044041, 40, 1044351);//Ornamentowana Niska Komoda (E)
            AddRecipe(index, (int)CarpRecipes.OrnateElvenChestEast);
            ForceNonExceptional(index);

            index = AddCraft(typeof(ElvenDresserDeedSouth), 1044291, 1072864, 75.0, 100.0, typeof(Board), 1044041, 45, 1044351);//Szeroka Komoda (S)
            AddRecipe(index, (int)CarpRecipes.ElvenDresserSouth);
            ForceNonExceptional(index);

            index = AddCraft(typeof(ElvenDresserDeedEast), 1044291, 1073388, 75.0, 100.0, typeof(Board), 1044041, 45, 1044351);//Szeroka Komoda (E)
            AddRecipe(index, (int)CarpRecipes.ElvenDresserEast);
            ForceNonExceptional(index);*/
            
            AddCraft(typeof(FancyArmoire), 1044291, 1044312, 84.2, 109.2, typeof(Board), 1044041, 35, 1044351);//Szafa
            AddCraft(typeof(Armoire), 1044291, 1022643, 84.2, 109.2, typeof(Board), 1044041, 35, 1044351);//Prosta Szafa
            
            
            /*index = AddCraft(typeof(FancyElvenArmoire), 1044291, 1072866, 80.0, 105.0, typeof(Board), 1044041, 60, 1044351);//Ornamentowana Szafa
            AddRecipe(index, (int)CarpRecipes.FancyElvenArmoire);
            ForceNonExceptional(index);*/

            index = AddCraft(typeof(SimpleElvenArmoire), 1044291, 1073401, 80.0, 105.0, typeof(Board), 1044041, 60, 1044351);//Duza Szafa
            ForceNonExceptional(index);

            AddCraft(typeof(EmptyBookcase), 1044291, 1022718, 31.5, 56.5, typeof(Board), 1044041, 25, 1044351);//Regal
            AddCraft(typeof(AcademicBookCase), 1044291, 1071213, 60.0, 85.0, typeof(Board), 1044041, 25, 1044351);//Regal z Ksiazkami
            #endregion
            
            #region Stoly 1044292
            AddCraft(typeof(Nightstand), 1044292, 1044306, 42.1, 67.1, typeof(Board), 1044041, 17, 1044351);//Stolik
            AddCraft(typeof(LargeTable), 1044292, 1044308, 84.2, 109.2, typeof(Board), 1044041, 27, 1044351);//Stol
            AddCraft(typeof(YewWoodTable), 1044292, 1044307, 63.1, 88.1, typeof(Board), 1044041, 23, 1044351);//Maly Stol
            AddCraft(typeof(ElegantLowTable), 1044292, 1030265, 80.0, 105.0, typeof(Board), 1044041, 35, 1044351);//Kwadratowy Niski Stol
            AddCraft(typeof(PlainLowTable), 1044292, 1030266, 80.0, 105.0, typeof(Board), 1044041, 35, 1044351);//Prosty Niski Stol
            AddCraft(typeof(TerMurStyleTable), 1044292, 1095321, 75.0, 100.0, typeof(Board), 1044041, 25, 1044351);//Zdobiony Stolik

            index = AddCraft(typeof(OrnateElvenTableSouthDeed), 1044292, 1072869, 85.0, 110.0, typeof(Board), 1044041, 60, 1044351);//Zdobiony Zaokraglony Stol (S)
            ForceNonExceptional(index);

            index = AddCraft(typeof(OrnateElvenTableEastDeed), 1044292, 1073384, 85.0, 110.0, typeof(Board), 1044041, 60, 1044351);//Zdobiony Zaokraglony Stol (E)
            ForceNonExceptional(index);

            index = AddCraft(typeof(FancyElvenTableSouthDeed), 1044292, 1073385, 80.0, 105.0, typeof(Board), 1044041, 50, 1044351);//Zdobiony Prostokatny Stol (S)
            ForceNonExceptional(index);

            index = AddCraft(typeof(FancyElvenTableEastDeed), 1044292, 1073386, 80.0, 105.0, typeof(Board), 1044041, 50, 1044351);//Zdobiony Prostokatny Stol (E)
            ForceNonExceptional(index);
            
            AddCraft(typeof(LongTableSouthDeed), 1044292, 1111781, 90.0, 115.0, typeof(Board), 1044041, 80, 1044351);//Ornamentowany Stol (S)
            AddCraft(typeof(LongTableEastDeed), 1044292, 1111782, 90.0, 115.0, typeof(Board), 1044041, 80, 1044351);//Ornamentowany Stol (E)
            
            index = AddCraft(typeof(MetalTableSouthDeed), 1044292, 1154154, 80.0, 105.0, typeof(Board), 1044041, 15, 1044351);//Metalowy Stol (S)
            AddSkill(index, SkillName.Tinkering, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 40, 1044037);

            index = AddCraft(typeof(MetalTableEastDeed), 1044292, 1154155, 80.0, 105.0, typeof(Board), 1044041, 15, 1044351);//Metalowy Stol (E)
            AddSkill(index, SkillName.Tinkering, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 40, 1044037);

            index = AddCraft(typeof(LongMetalTableSouthDeed), 1044292, 1154164, 80.0, 105.0, typeof(Board), 1044041, 40, 1044351);//Dlugi Metalowy Stol (S)
            AddSkill(index, SkillName.Tinkering, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 30, 1044037);

            index = AddCraft(typeof(LongMetalTableEastDeed), 1044292, 1154165, 80.0, 105.0, typeof(Board), 1044041, 40, 1044351);//Dlugi Metalowy Stol (E)
            AddSkill(index, SkillName.Tinkering, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 30, 1044037);

            AddCraft(typeof(WoodenTableSouthDeed), 1044292, 1154156, 80.0, 105.0, typeof(Board), 1044041, 20, 1044351);//Drewniany Stol (S)
            AddCraft(typeof(WoodenTableEastDeed), 1044292, 1154157, 80.0, 105.0, typeof(Board), 1044041, 20, 1044351);//Drewniany Stol (E)
            AddCraft(typeof(LongWoodenTableSouthDeed), 1044292, 1154166, 80.0, 105.0, typeof(Board), 1044041, 80, 1044351);//Dlugi Drewniany Stol (S)
            AddCraft(typeof(LongWoodenTableEastDeed), 1044292, 1154167, 80.0, 105.0, typeof(Board), 1044041, 80, 1044351);//Dlugi Drewniany Stol (E)
            AddCraft(typeof(WritingTable), 1044292, 1022890, 63.1, 88.1, typeof(Board), 1044041, 17, 1044351);//Skryptorium
            
            index = AddCraft(typeof(AlchemistTableSouthDeed), 1044292, 1073396, 85.0, 110.0, typeof(Board), 1044041, 70, 1044351);//Stol Alchemika (S)
            SetDisplayID(index, 0x2DD4);
            ForceNonExceptional(index);

            index = AddCraft(typeof(AlchemistTableEastDeed), 1044292, 1073397, 85.0, 110.0, typeof(Board), 1044041, 70, 1044351);//Stol Alchemika (E)
            SetDisplayID(index, 0x2DD3);
            ForceNonExceptional(index);
            #endregion
            
            #region Oswietlenie 1044566
            index = AddCraft(typeof(RedHangingLantern), 1044566, 1029412, 65.0, 90.0, typeof(Board), 1044041, 5, 1044351);//Czerwona Wiszaca Lampa
            AddRes(index, typeof(BlankScroll), 1044377, 10, 1044378);

            index = AddCraft(typeof(WhiteHangingLantern), 1044566, 1029416, 65.0, 90.0, typeof(Board), 1044041, 5, 1044351);//Biala Wiszaca Lampa
            AddRes(index, typeof(BlankScroll), 1044377, 10, 1044378);
	        #endregion
	        
	        #region Ozdoby 1062760
	        
	        AddCraft(typeof(TerMurDresserEastDeed), 1062760, 1111784, 90.0, 115.0, typeof(Board), 1044041, 60, 1044351);//Toaletka z Lustrem (E)
	        AddCraft(typeof(TerMurDresserSouthDeed), 1062760, 1111783, 90.0, 115.0, typeof(Board), 1044041, 60, 1044351);//Toaletka z Lustrem (S)
	        
	        index = AddCraft(typeof(ElvenWashBasinSouthWithDrawerDeed), 1062760, 1072865, 70.0, 95.0, typeof(Board), 1044041, 40, 1044351);//Umywalka (S)
	        ForceNonExceptional(index);

	        index = AddCraft(typeof(ElvenWashBasinEastWithDrawerDeed), 1062760, 1073387, 70.0, 95.0, typeof(Board), 1044041, 40, 1044351);//Umywalka (E)
	        ForceNonExceptional(index);
	        
	        index = AddCraft(typeof(ShojiScreen), 1062760, 1029423, 80.0, 105.0, typeof(Board), 1044041, 75, 1044351);//Parawan w Krate
	        AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
	        AddRes(index, typeof(Cloth), 1044286, 60, 1044287);

	        index = AddCraft(typeof(BambooScreen), 1062760, 1029428, 80.0, 105.0, typeof(Board), 1044041, 75, 1044351);//Parawan
	        AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
	        AddRes(index, typeof(Cloth), 1044286, 60, 1044287);
	        
	        AddCraft(typeof(PlainWoodenShelfSouthDeed), 1062760, 1154160, 40.0, 90.0, typeof(Board), 1044041, 15, 1044351);//Polka Scienna (S)
	        AddCraft(typeof(PlainWoodenShelfEastDeed), 1062760, 1154161, 40.0, 90.0, typeof(Board), 1044041, 15, 1044351);//Polka Scienna (E)
	        AddCraft(typeof(FancyWoodenShelfSouthDeed), 1062760, 1154158, 40.0, 90.0, typeof(Board), 1044041, 15, 1044351);//Zdobiona Polka Scienna (S)
	        AddCraft(typeof(FancyWoodenShelfEastDeed), 1062760, 1154159, 40.0, 90.0, typeof(Board), 1044041, 15, 1044351);//Zdobiona Polka Scienna (E)
	        AddCraft(typeof(ElvenPodium), 1062760, 1073399, 80.0, 105.0, typeof(Board), 1044041, 20, 1044351);//Stojak

           /* index = AddCraft(typeof(TallElvenBedSouthDeed), 1062760, 1072858, 94.7, 119.7, typeof(Board), 1044041, 200, 1044351);//Zdobione Dwuosobowe Lozko (S)
            AddSkill(index, SkillName.Tailoring, 75.0, 80.0);
            AddRes(index, typeof(Cloth), 1044286, 100, 1044287);
            AddRecipe(index, (int)CarpRecipes.TallElvenBedSouth);
            ForceNonExceptional(index);

            index = AddCraft(typeof(TallElvenBedEastDeed), 1062760, 1072859, 94.7, 119.7, typeof(Board), 1044041, 200, 1044351);//Zdobione Dwuosobowe Lozko (E)
            AddSkill(index, SkillName.Tailoring, 75.0, 80.0);
            AddRes(index, typeof(Cloth), 1044286, 100, 1044287);
            AddRecipe(index, (int)CarpRecipes.TallElvenBedEast);
            ForceNonExceptional(index);*/

            index = AddCraft(typeof(ElvenBedSouthDeed), 1062760, 1072860, 94.7, 119.7, typeof(Board), 1044041, 100, 1044351);//Zdobione Lozko (S)
            AddRes(index, typeof(Cloth), 1044286, 100, 1044287);
            ForceNonExceptional(index);

            index = AddCraft(typeof(ElvenBedEastDeed), 1062760, 1072861, 94.7, 119.7, typeof(Board), 1044041, 100, 1044351);//Zdobione Lozko (E)
            AddRes(index, typeof(Cloth), 1044286, 100, 1044287);
            ForceNonExceptional(index);

            index = AddCraft(typeof(SmallBedSouthDeed), 1062760, 1044321, 94.7, 119.8, typeof(Board), 1044041, 100, 1044351);//Proste Lozko (S)
            AddSkill(index, SkillName.Tailoring, 75.0, 80.0);
            AddRes(index, typeof(Cloth), 1044286, 100, 1044287);

            index = AddCraft(typeof(SmallBedEastDeed), 1062760, 1044322, 94.7, 119.8, typeof(Board), 1044041, 100, 1044351);//Proste Lozko (E)
            AddSkill(index, SkillName.Tailoring, 75.0, 80.0);
            AddRes(index, typeof(Cloth), 1044286, 100, 1044287);

            index = AddCraft(typeof(LargeBedSouthDeed), 1062760, 1044323, 94.7, 119.8, typeof(Board), 1044041, 150, 1044351);//Wytworne Dwuosobowe Lozko (S)
            AddSkill(index, SkillName.Tailoring, 75.0, 80.0);
            AddRes(index, typeof(Cloth), 1044286, 150, 1044287);

            index = AddCraft(typeof(LargeBedEastDeed), 1062760, 1044324, 94.7, 119.8, typeof(Board), 1044041, 150, 1044351);//Wytworne Dwuosobowe Lozko (E)
            AddSkill(index, SkillName.Tailoring, 75.0, 80.0);
            AddRes(index, typeof(Cloth), 1044286, 150, 1044287);
            
            AddCraft(typeof(DartBoardSouthDeed), 1062760, 1044325, 15.7, 40.7, typeof(Board), 1044041, 5, 1044351);//Tablica do Gry w Rzutki (S)
            AddCraft(typeof(DartBoardEastDeed), 1062760, 1044326, 15.7, 40.7, typeof(Board), 1044041, 5, 1044351);//Tablica do Gry w Rzutki (E)
            
            index = AddCraft(typeof(ArcaneCircleDeed), 1062760, 1072703, 94.7, 119.7, typeof(Board), 1044041, 100, 1044351);//Arkanistyczny Krag
            AddSkill(index, SkillName.Magery, 75.0, 80.0);
            AddRes(index, typeof(BlueDiamond), 1026255, 1, 1053098);
            AddRes(index, typeof(PerfectEmerald), 1026251, 1, 1053098);
            AddRes(index, typeof(FireRuby), 1026254, 1, 1053098);
            ForceNonExceptional(index);
            
            index = AddCraft(typeof(PentagramDeed), 1062760, 1044328, 100.0, 125.0, typeof(Board), 1044041, 100, 1044351);//Pentagram
            AddSkill(index, SkillName.Magery, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 40, 1044037);
            ForceNonExceptional(index);

            index = AddCraft(typeof(AbbatoirDeed), 1062760, 1044329, 100.0, 125.0, typeof(Board), 1044041, 100, 1044351);//Oltarz
            AddSkill(index, SkillName.Magery, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 40, 1044037);
            ForceNonExceptional(index);
            
            AddCraft(typeof(BallotBoxDeed), 1062760, 1044327, 47.3, 72.3, typeof(Board), 1044041, 5, 1044351);//Urna do Glosowania
            AddCraft(typeof(ParrotPerchAddonDeed), 1062760, 1072617, 50.0, 85.0, typeof(Board), 1044041, 100, 1044351);//Zerdz Dla Papugi
	        AddCraft(typeof(EasleSouth), 1062760, 1044317, 86.8, 111.8, typeof(Board), 1044041, 20, 1044351);//Sztaluga z Plotnem (S)
	        AddCraft(typeof(EasleEast), 1062760, 1044318, 86.8, 111.8, typeof(Board), 1044041, 20, 1044351);//Sztaluga z Plotnem (E)
	        AddCraft(typeof(EasleNorth), 1062760, 1044319, 86.8, 111.8, typeof(Board), 1044041, 20, 1044351);//Sztaluga z Plotnem (W)
	        
	        index = AddCraft(typeof(PlantTapestrySouthDeed), 1062760, 1154146, 85.0, 110.0, typeof(Board), 1044041, 12, 1044351);//Kwiatowy Gobelin (S)
            AddSkill(index, SkillName.Tailoring, 75.0, 80.0);
            AddRes(index, typeof(Cloth), 1044286, 50, 1044287);

            index = AddCraft(typeof(PlantTapestryEastDeed), 1062760, 1154147, 85.0, 110.0, typeof(Board), 1044041, 12, 1044351);//Kwiatowy Gobelin (E)
            AddSkill(index, SkillName.Tailoring, 75.0, 80.0);
            AddRes(index, typeof(Cloth), 1044286, 50, 1044287);
            
            index = AddCraft(typeof(SmallDisplayCaseSouthDeed), 1062760, 1155842, 95.0, 120.0, typeof(Board), 1044041, 40, 1044351);//Mala Witryna (S)
            AddSkill(index, SkillName.Tinkering, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 10, 1044037);

            index = AddCraft(typeof(SmallDisplayCaseEastDeed), 1062760, 1155843, 95.0, 120.0, typeof(Board), 1044041, 40, 1044351);//Mala Witryna (E)
            AddSkill(index, SkillName.Tinkering, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 10, 1044037);
            
           /* index = AddCraft(typeof(SmallElegantAquariumDeed), 1062760, 1159134, 100.0, 160.0, typeof(Board), 1044041, 20, 1044351);//Male Akwarium
            AddRes(index, typeof(WorkableGlass), 1154170, 2, 1154171);
            AddRes(index, typeof(Sand), 1044625, 5, 1044627);
            AddRes(index, typeof(LiveRock), 1159133, 1, 1159132);
            AddRecipe(index, (int)CarpRecipes.SmallElegantAquarium);

            index = AddCraft(typeof(WallMountedAquariumDeed), 1062760, 1159135, 100.0, 160.0, typeof(Board), 1044041, 50, 1044351);//Scienne Akwarium
            AddRes(index, typeof(WorkableGlass), 1154170, 4, 1154171);
            AddRes(index, typeof(Sand), 1044625, 10, 1044627);
            AddRes(index, typeof(LiveRock), 1159133, 3, 1159132);
            AddRecipe(index, (int)CarpRecipes.WallMountedAquarium);

            index = AddCraft(typeof(LargeElegantAquariumDeed), 1062760, 1159136, 100.0, 160.0, typeof(Board), 1044041, 100, 1044351);//Duze Akwarium
            AddRes(index, typeof(WorkableGlass), 1154170, 8, 1154171);
            AddRes(index, typeof(Sand), 1044625, 20, 1044627);
            AddRes(index, typeof(LiveRock), 1159133, 5, 1159132);
            AddRecipe(index, (int)CarpRecipes.LargeElegantAquarium);*/
	        
	        AddCraft(typeof(GiantReplicaAcorn), 1062760, 1072889, 80.0, 105.0, typeof(Board), 1044041, 35, 1044351);//Gigantyczna Replika Zoledzia
	        
	        index = AddCraft(typeof(ArcanistStatueSouthDeed), 1062760, 1072885, 0.0, 35.0, typeof(Board), 1044041, 250, 1044351);//Statua Arkanisty (S)
	        ForceNonExceptional(index);

	        index = AddCraft(typeof(ArcanistStatueEastDeed), 1062760, 1072886, 0.0, 35.0, typeof(Board), 1044041, 250, 1044351);//Statua Arkanisty (E)
	        ForceNonExceptional(index);

	        /*index = AddCraft(typeof(WarriorStatueSouthDeed), 1062760, 1072887, 0.0, 35.0, typeof(Board), 1044041, 250, 1044351);//Statua Wojownika (S)
	        AddRecipe(index, (int)CarpRecipes.WarriorStatueSouth);
	        ForceNonExceptional(index);

	        index = AddCraft(typeof(WarriorStatueEastDeed), 1062760, 1072888, 0.0, 35.0, typeof(Board), 1044041, 250, 1044351);//Statua Wojownika (E)
	        AddRecipe(index, (int)CarpRecipes.WarriorStatueEast);
	        ForceNonExceptional(index);

	        index = AddCraft(typeof(SquirrelStatueSouthDeed), 1062760, 1072884, 0.0, 35.0, typeof(Board), 1044041, 250, 1044351);//Statua Wiewiorki (S)
	        AddRecipe(index, (int)CarpRecipes.SquirrelStatueSouth);
	        ForceNonExceptional(index);

	        index = AddCraft(typeof(SquirrelStatueEastDeed), 1062760, 1073398, 0.0, 35.0, typeof(Board), 1044041, 250, 1044351);//Statua Wiewiorki (E)
	        AddRecipe(index, (int)CarpRecipes.SquirrelStatueEast);
	        ForceNonExceptional(index);*/
	        
	        /*index = AddCraft(typeof(GargishBanner), 1062760, 1095312, 94.7, 115.0, typeof(Board), 1044041, 10, 1044351);//Sztandar
	        AddSkill(index, SkillName.Tailoring, 75.0, 105.0);
	        AddRes(index, typeof(Cloth), 1044286, 20, 1044287);*/
	        
	        AddCraft(typeof(ChickenCoop), 1062760, 1112570, 90.0, 115.0, typeof(Board), 1044041, 150, 1044351);//Kurnik
	        AddCraft(typeof(Incubator), 1062760, 1112479, 90.0, 115.0, typeof(Board), 1044041, 100, 1044351);//Inkubator
	        #endregion

	        #region Zbroje 1044293
	        AddCraft(typeof(WoodenShield), 1044293, 1027034, 52.6, 77.6, typeof(Board), 1044041, 9, 1044351);

            index = AddCraft(typeof(WoodlandChest), 1044293, 1031111, 90.0, 115.0, typeof(Board), 1044041, 20, 1044351);
            AddRes(index, typeof(BarkFragment), 1032687, 6, 1053098);

            index = AddCraft(typeof(WoodlandArms), 1044293, 1031116, 80.0, 105.0, typeof(Board), 1044041, 15, 1044351);
            AddRes(index, typeof(BarkFragment), 1032687, 4, 1053098);

            index = AddCraft(typeof(WoodlandGloves), 1044293, 1031114, 85.0, 110.0, typeof(Board), 1044041, 15, 1044351);
            AddRes(index, typeof(BarkFragment), 1032687, 4, 1053098);

            index = AddCraft(typeof(WoodlandLegs), 1044293, 1031115, 85.0, 110.0, typeof(Board), 1044041, 15, 1044351);
            AddRes(index, typeof(BarkFragment), 1032687, 4, 1053098);

            index = AddCraft(typeof(WoodlandGorget), 1044293, 1031113, 85.0, 110.0, typeof(Board), 1044041, 15, 1044351);
            AddRes(index, typeof(BarkFragment), 1032687, 4, 1053098);

            index = AddCraft(typeof(RavenHelm), 1044293, 1031121, 65.0, 115.0, typeof(Board), 1044041, 10, 1044351);
            AddRes(index, typeof(BarkFragment), 1032687, 4, 1053098);
            AddRes(index, typeof(Feather), 1027123, 25, 1053098);

            index = AddCraft(typeof(VultureHelm), 1044293, 1031122, 63.9, 113.9, typeof(Board), 1044041, 10, 1044351);
            AddRes(index, typeof(BarkFragment), 1032687, 4, 1053098);
            AddRes(index, typeof(Feather), 1027123, 25, 1053098);

            index = AddCraft(typeof(WingedHelm), 1044293, 1031123, 58.4, 108.4, typeof(Board), 1044041, 10, 1044351);
            AddRes(index, typeof(BarkFragment), 1032687, 4, 1053098);
            AddRes(index, typeof(Feather), 1027123, 60, 1053098);

           /* index = AddCraft(typeof(IronwoodCrown), 1044293, 1072924, 85.0, 120.0, typeof(Board), 1044041, 10, 1044351);
            AddRes(index, typeof(DiseasedBark), 1032683, 1, 1053098);
            AddRes(index, typeof(Corruption), 1032676, 10, 1053098);
            AddRes(index, typeof(Putrefaction), 1032678, 10, 1053098);
            AddRecipe(index, (int)CarpRecipes.IronwoodCrown);
            ForceNonExceptional(index);

            index = AddCraft(typeof(BrambleCoat), 1044293, 1072925, 85.0, 120.0, typeof(Board), 1044041, 10, 1044351);
            AddRes(index, typeof(DiseasedBark), 1032683, 1, 1053098);
            AddRes(index, typeof(Taint), 1032679, 10, 1053098);
            AddRes(index, typeof(Scourge), 1032677, 10, 1053098);
            AddRecipe(index, (int)CarpRecipes.BrambleCoat);
            ForceNonExceptional(index);*/

            index = AddCraft(typeof(DarkwoodCrown), 1044293, 1073481, 85.0, 120.0, typeof(Board), 1044041, 10, 1044351);
            AddRes(index, typeof(LardOfParoxysmus), 1032681, 1, 1053098);
            AddRes(index, typeof(Blight), 1032675, 10, 1053098);
            AddRes(index, typeof(Taint), 1032679, 10, 1053098);
            ForceNonExceptional(index);

            index = AddCraft(typeof(DarkwoodChest), 1044293, 1073482, 85.0, 120.0, typeof(Board), 1044041, 20, 1044351);
            AddRes(index, typeof(DreadHornMane), 1032682, 1, 1053098);
            AddRes(index, typeof(Corruption), 1032676, 10, 1053098);
            AddRes(index, typeof(Muculent), 1032680, 10, 1053098);
            ForceNonExceptional(index);

            index = AddCraft(typeof(DarkwoodGorget), 1044293, 1073483, 85.0, 120.0, typeof(Board), 1044041, 15, 1044351);
            AddRes(index, typeof(DiseasedBark), 1032683, 1, 1053098);
            AddRes(index, typeof(Blight), 1032675, 10, 1053098);
            AddRes(index, typeof(Scourge), 1032677, 10, 1053098);
            ForceNonExceptional(index);

            index = AddCraft(typeof(DarkwoodLegs), 1044293, 1073484, 85.0, 120.0, typeof(Board), 1044041, 15, 1044351);
            AddRes(index, typeof(GrizzledBones), 1032684, 1, 1053098);
            AddRes(index, typeof(Corruption), 1032676, 10, 1053098);
            AddRes(index, typeof(Putrefaction), 1072137, 10, 1053098);
            ForceNonExceptional(index);

            index = AddCraft(typeof(DarkwoodPauldrons), 1044293, 1073485, 85.0, 120.0, typeof(Board), 1044041, 15, 1044351);
            AddRes(index, typeof(EyeOfTheTravesty), 1032685, 1, 1053098);
            AddRes(index, typeof(Scourge), 1032677, 10, 1053098);
            AddRes(index, typeof(Taint), 1032679, 10, 1053098);
            ForceNonExceptional(index);

            index = AddCraft(typeof(DarkwoodGloves), 1044293, 1073486, 85.0, 120.0, typeof(Board), 1044041, 15, 1044351);
            AddRes(index, typeof(CapturedEssence), 1032686, 1, 1053098);
            AddRes(index, typeof(Putrefaction), 1032678, 10, 1053098);
            AddRes(index, typeof(Muculent), 1032680, 10, 1053098);
            ForceNonExceptional(index);

            //AddCraft(typeof(GargishWoodenShield), 1044293, 1095768, 52.6, 77.6, typeof(Board), 1044041, 9, 1044351);
	        #endregion

	        #region Bron 1062759
	        AddCraft(typeof(ShepherdsCrook), 1062759, 1023713, 78.9, 103.9, typeof(Board), 1044041, 7, 1044351);
            AddCraft(typeof(QuarterStaff), 1062759, 1023721, 73.6, 98.6, typeof(Board), 1044041, 6, 1044351);
            AddCraft(typeof(GnarledStaff), 1062759, 1025112, 78.9, 103.9, typeof(Board), 1044041, 7, 1044351);
            AddCraft(typeof(Bokuto), 1062759, 1030227, 70.0, 95.0, typeof(Board), 1044041, 6, 1044351);
            AddCraft(typeof(Fukiya), 1062759, 1030229, 60.0, 85.0, typeof(Board), 1044041, 6, 1044351);
            AddCraft(typeof(Tetsubo), 1062759, 1030225, 80.0, 105.0, typeof(Board), 1044041, 10, 1044351);
            AddCraft(typeof(WildStaff), 1062759, 1031557, 63.8, 113.8, typeof(Board), 1044041, 16, 1044351);
            AddCraft(typeof(Club), 1062759, 1025043, 65.0, 90.0, typeof(Board), 1044041, 9, 1044351);
            AddCraft(typeof(BlackStaff), 1062759, 1023568, 81.5, 106.5, typeof(Board), 1044041, 9, 1044351);

           /* index = AddCraft(typeof(KotlBlackRod), 1062759, 1156990, 100.0, 160.0, typeof(Board), 1044041, 20, 1044351);
            AddRes(index, typeof(BlackrockMoonstone), 1156993, 1, 1156992);
            AddRes(index, typeof(StaffOfTheMagi), 1061600, 1, 1044253);
            AddRecipe(index, (int)CarpRecipes.KotlBlackRod);
            ForceNonExceptional(index);
            
            index = AddCraft(typeof(PhantomStaff), 1062759, 1072919, 90.0, 130.0, typeof(Board), 1044041, 16, 1044351);
            AddRes(index, typeof(DiseasedBark), 1032683, 1, 1053098);
            AddRes(index, typeof(Putrefaction), 1032678, 10, 1053098);
            AddRes(index, typeof(Taint), 1032679, 10, 1053098);
            AddRecipe(index, (int)CarpRecipes.PhantomStaff);
            ForceNonExceptional(index);

            index = AddCraft(typeof(ArcanistsWildStaff), 1062759, 1073549, 63.8, 113.8, typeof(Board), 1044041, 16, 1044351);
            AddRes(index, typeof(WhitePearl), 1026253, 1, 1053098);
            AddRecipe(index, (int)CarpRecipes.ArcanistsWildStaff);

            index = AddCraft(typeof(AncientWildStaff), 1062759, 1073550, 63.8, 113.8, typeof(Board), 1044041, 16, 1044351);
            AddRes(index, typeof(PerfectEmerald), 1026251, 1, 1053098);
            AddRecipe(index, (int)CarpRecipes.AncientWildStaff);

            index = AddCraft(typeof(ThornedWildStaff), 1062759, 1073551, 63.8, 113.8, typeof(Board), 1044041, 16, 1044351);
            AddRes(index, typeof(FireRuby), 1026254, 1, 1053098);
            AddRecipe(index, (int)CarpRecipes.ThornedWildStaff);

            index = AddCraft(typeof(HardenedWildStaff), 1062759, 1073552, 63.8, 113.8, typeof(Board), 1044041, 16, 1044351);
            AddRes(index, typeof(Turquoise), 1026250, 1, 1053098);
            AddRecipe(index, (int)CarpRecipes.HardenedWildStaff);*/
            
            //index = AddCraft(typeof(SerpentStoneStaff), 1062759, 1095367, 63.8, 113.8, typeof(Board), 1044041, 16, 1044351);
            //AddRes(index, typeof(EcruCitrine), 1026252, 1, 1053098);

            //index = AddCraft(typeof(GargishGnarledStaff), 1062759, 1097488, 78.9, 128.9, typeof(Board), 1044041, 16, 1044351);
            //AddRes(index, typeof(EcruCitrine), 1026252, 1, 1053098);
            
            #endregion

            #region Instrumenty 1044298
            index = AddCraft(typeof(LapHarp), 1044298, 1023762, 63.1, 88.1, typeof(Board), 1044041, 20, 1044351);
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(Harp), 1044298, 1023761, 78.9, 103.9, typeof(Board), 1044041, 35, 1044351);
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(Cloth), 1044286, 15, 1044287);

            index = AddCraft(typeof(Drums), 1044298, 1023740, 57.8, 82.8, typeof(Board), 1044041, 20, 1044351);
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(Lute), 1044298, 1023763, 68.4, 93.4, typeof(Board), 1044041, 25, 1044351);
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(Tambourine), 1044298, 1023741, 57.8, 82.8, typeof(Board), 1044041, 15, 1044351);
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(TambourineTassel), 1044298, 1044320, 57.8, 82.8, typeof(Board), 1044041, 15, 1044351); //Tamburyn ze Wstazka
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(Cloth), 1044286, 15, 1044287);

            index = AddCraft(typeof(BambooFlute), 1044298, 1030247, 80.0, 105.0, typeof(Board), 1044041, 15, 1044351);//Flet
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);

            index = AddCraft(typeof(AudChar), 1044298, 1095315, 78.9, 103.9, typeof(Board), 1044041, 35, 1044351);//Basetla
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(Granite), 1044514, 3, 1044513);

            index = AddCraft(typeof(CelloDeed), 1044298, 1098390, 75.0, 105.0, typeof(Board), 1044041, 15, 1044351);
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(Cloth), 1044286, 5, 1044287);

            index = AddCraft(typeof(WallMountedBellSouthDeed), 1044298, 1154162, 75.0, 105.0, typeof(Board), 1044041, 50, 1044351);//Dzwon Scienny (S)
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(IronIngot), 1044036, 50, 1044037);

            index = AddCraft(typeof(WallMountedBellEastDeed), 1044298, 1154163, 75.0, 105.0, typeof(Board), 1044041, 50, 1044351);//Dzwon Scienny (E)
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(IronIngot), 1044036, 50, 1044037);

            index = AddCraft(typeof(TrumpetDeed), 1044298, 1098388, 85.0, 105.0, typeof(Board), 1044041, 10, 1044351);
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(IronIngot), 1044036, 15, 1044037);

            index = AddCraft(typeof(CowBellDeed), 1044298, 1098418, 85.0, 105.0, typeof(Board), 1044041, 10, 1044351);
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(IronIngot), 1044036, 15, 1044037);
            
            index = AddCraft(typeof(SnakeCharmerFlute), 1044298, 1112174, 80.0, 105.0, typeof(Board), 1044041, 15, 1044351);
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(LuminescentFungi), 1073475, 3, 1044253);
            
            AddCraft(typeof(ShortMusicStandLeft), 1044298, 1044313, 78.9, 103.9, typeof(Board), 1044041, 15, 1044351);
            AddCraft(typeof(ShortMusicStandRight), 1044298, 1044314, 78.9, 103.9, typeof(Board), 1044041, 15, 1044351);
	        AddCraft(typeof(TallMusicStandLeft), 1044298, 1044315, 81.5, 106.5, typeof(Board), 1044041, 20, 1044351);
	        AddCraft(typeof(TallMusicStandRight), 1044298, 1044316, 81.5, 106.5, typeof(Board), 1044041, 20, 1044351);
	        #endregion
	        
	        #region Rzemieslnicze
	        // Tailoring and Cooking
            index = AddCraft(typeof(DressformFront), 1111809, 1044339, 63.1, 88.1, typeof(Board), 1044041, 25, 1044351);
            AddSkill(index, SkillName.Tailoring, 65.0, 70.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(DressformSide), 1111809, 1044340, 63.1, 88.1, typeof(Board), 1044041, 25, 1044351);
            AddSkill(index, SkillName.Tailoring, 65.0, 70.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(ElvenSpinningwheelEastDeed), 1111809, 1073393, 75.0, 100.0, typeof(Board), 1044041, 60, 1044351);//Male Kolo Przedzalnicze (E)
            AddSkill(index, SkillName.Tailoring, 65.0, 85.0);
            AddRes(index, typeof(Cloth), 1044286, 40, 1044287);
            ForceNonExceptional(index);

            index = AddCraft(typeof(ElvenSpinningwheelSouthDeed), 1111809, 1072878, 75.0, 100.0, typeof(Board), 1044041, 60, 1044351);//Male Kolo Przedzalnicze (S)
            AddSkill(index, SkillName.Tailoring, 65.0, 85.0);
            AddRes(index, typeof(Cloth), 1044286, 40, 1044287);
            ForceNonExceptional(index);

            index = AddCraft(typeof(ElvenStoveSouthDeed), 1111809, 1073394, 85.0, 110.0, typeof(Board), 1044041, 80, 1044351);//Piec Wolnostojacy (S)
            ForceNonExceptional(index);

            index = AddCraft(typeof(ElvenStoveEastDeed), 1111809, 1073395, 85.0, 110.0, typeof(Board), 1044041, 80, 1044351);//Piec Wolnostojacy (E)
            ForceNonExceptional(index);
            
            index = AddCraft(typeof(SpinningwheelSouthDeed), 1111809, 1044342, 73.6, 98.6, typeof(Board), 1044041, 75, 1044351);//Kolo Przedzalnicze (S)
            AddSkill(index, SkillName.Tailoring, 65.0, 70.0);
            AddRes(index, typeof(Cloth), 1044286, 25, 1044287);

            index = AddCraft(typeof(SpinningwheelEastDeed), 1111809, 1044341, 73.6, 98.6, typeof(Board), 1044041, 75, 1044351);//Kolo Przedzalnicze (E)
            AddSkill(index, SkillName.Tailoring, 65.0, 70.0);
            AddRes(index, typeof(Cloth), 1044286, 25, 1044287);


            index = AddCraft(typeof(LoomEastDeed), 1111809, 1044343, 84.2, 109.2, typeof(Board), 1044041, 85, 1044351);
            AddSkill(index, SkillName.Tailoring, 65.0, 70.0);
            AddRes(index, typeof(Cloth), 1044286, 25, 1044287);

            index = AddCraft(typeof(LoomSouthDeed), 1111809, 1044344, 84.2, 109.2, typeof(Board), 1044041, 85, 1044351);
            AddSkill(index, SkillName.Tailoring, 65.0, 70.0);
            AddRes(index, typeof(Cloth), 1044286, 25, 1044287);

            index = AddCraft(typeof(StoneOvenEastDeed), 1111809, 1044345, 68.4, 93.4, typeof(Board), 1044041, 85, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 125, 1044037);

            index = AddCraft(typeof(StoneOvenSouthDeed), 1111809, 1044346, 68.4, 93.4, typeof(Board), 1044041, 85, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 125, 1044037);

            index = AddCraft(typeof(FlourMillEastDeed), 1111809, 1044347, 94.7, 119.7, typeof(Board), 1044041, 100, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 50, 1044037);

            index = AddCraft(typeof(FlourMillSouthDeed), 1111809, 1044348, 94.7, 119.7, typeof(Board), 1044041, 100, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 50, 1044037);

            AddCraft(typeof(WaterTroughEastDeed), 1111809, 1044349, 94.7, 119.7, typeof(Board), 1044041, 150, 1044351);
            AddCraft(typeof(WaterTroughSouthDeed), 1111809, 1044350, 94.7, 119.7, typeof(Board), 1044041, 150, 1044351);

            // Anvils and Forges
            index = AddCraft(typeof(ElvenForgeDeed), 1111809, 1072875, 94.7, 119.7, typeof(Board), 1044041, 200, 1044351);//Runiczne Palenisko
            ForceNonExceptional(index);

            index = AddCraft(typeof(SoulForgeDeed), 1111809, 1095827, 100.0, 200.0, typeof(Board), 1044041, 150, 1044351);
            AddSkill(index, SkillName.Imbuing, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 150, 1044037);
            AddRes(index, typeof(RelicFragment), 1031699, 1, 1044253);
            ForceNonExceptional(index);

            index = AddCraft(typeof(SmallForgeDeed), 1111809, 1044330, 73.6, 98.6, typeof(Board), 1044041, 5, 1044351);
            AddSkill(index, SkillName.Blacksmith, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 75, 1044037);

            index = AddCraft(typeof(LargeForgeEastDeed), 1111809, 1044331, 78.9, 103.9, typeof(Board), 1044041, 5, 1044351);
            AddSkill(index, SkillName.Blacksmith, 80.0, 85.0);
            AddRes(index, typeof(IronIngot), 1044036, 100, 1044037);

            index = AddCraft(typeof(LargeForgeSouthDeed), 1111809, 1044332, 78.9, 103.9, typeof(Board), 1044041, 5, 1044351);
            AddSkill(index, SkillName.Blacksmith, 80.0, 85.0);
            AddRes(index, typeof(IronIngot), 1044036, 100, 1044037);

            index = AddCraft(typeof(AnvilEastDeed), 1111809, 1044333, 73.6, 98.6, typeof(Board), 1044041, 5, 1044351);
            AddSkill(index, SkillName.Blacksmith, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 150, 1044037);

            index = AddCraft(typeof(AnvilSouthDeed), 1111809, 1044334, 73.6, 98.6, typeof(Board), 1044041, 5, 1044351);
            AddSkill(index, SkillName.Blacksmith, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 150, 1044037);

            // Training
            index = AddCraft(typeof(TrainingDummyEastDeed), 1111809, 1044335, 68.4, 93.4, typeof(Board), 1044041, 55, 1044351);//Kukla Treningowa (E)
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);

            index = AddCraft(typeof(TrainingDummySouthDeed), 1111809, 1044336, 68.4, 93.4, typeof(Board), 1044041, 55, 1044351);//Kukla Treningowa (S)
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);

            index = AddCraft(typeof(PickpocketDipEastDeed), 1111809, 1044337, 73.6, 98.6, typeof(Board), 1044041, 65, 1044351);//Kukla do Okradania (E)
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);

            index = AddCraft(typeof(PickpocketDipSouthDeed), 1111809, 1044338, 73.6, 98.6, typeof(Board), 1044041, 65, 1044351);//Kukla do Okradania (S)
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);
	        #endregion

	        #region Inne 1044297
	        // Other
	        index = AddCraft(typeof(WoodenContainerEngraver), 1044297, 1072153, 75.0, 100.0, typeof(Board), 1044041, 4, 1044351);
	        AddRes(index, typeof(IronIngot), 1044036, 2, 1044037);
            
            AddCraft(typeof(BarrelStaves), 1044297, 1027857, 00.0, 25.0, typeof(Board), 1044041, 5, 1044351);
            AddCraft(typeof(BarrelLid), 1044297, 1027608, 11.0, 36.0, typeof(Board), 1044041, 4, 1044351);
            
            index = AddCraft(typeof(FishingPole), 1044297, 1023519, 68.4, 93.4, typeof(Board), 1044041, 5, 1044351); //This is in the categor of Other during AoS
            AddSkill(index, SkillName.Tailoring, 40.0, 45.0);
            AddRes(index, typeof(Cloth), 1044286, 5, 1044287);
            
            index = AddCraft(typeof(RunedSwitch), 1044297, 1072896, 70.0, 120.0, typeof(Board), 1044041, 2, 1044351);
            AddRes(index, typeof(EnchantedSwitch), 1072893, 1, 1053098);
            AddRes(index, typeof(RunedPrism), 1073465, 1, 1053098);
            AddRes(index, typeof(JeweledFiligree), 1072894, 1, 1053098);
            
            /*index = AddCraft(typeof(MountedDreadHorn), 1044297, 1032632, 90.0, 115.0, typeof(Board), 1044041, 50, 1044351);
            AddRes(index, typeof(PristineDreadHorn), 1032634, 1, 1053098); Przeniesc do trofeów
            ForceNonExceptional(index);*///Przeniesc do trofeów

            /*index = AddCraft(typeof(AcidProofRope), 1044297, 1074886, 80, 130.0, typeof(GreaterStrengthPotion), 1073466, 2, 1044253);
            AddRes(index, typeof(ProtectionScroll), 1044395, 1, 1053098);
            AddRes(index, typeof(SwitchItem), 1032127, 1, 1053098);
            AddRecipe(index, (int)CarpRecipes.AcidProofRope);
            ForceNonExceptional(index);*/
            
            index = AddCraft(typeof(ExodusSummoningAlter), 1044297, 1153502, 95.0, 120.0, typeof(Board), 1044041, 100, 1044351);
            AddSkill(index, SkillName.Magery, 75.0, 120.0);
            AddRes(index, typeof(Granite), 1044607, 10, 1044253);
            AddRes(index, typeof(SmallPieceofBlackrock), 1150016, 10, 1044253);
            AddRes(index, typeof(NexusCore), 1153501, 1, 1044253);
            
            /*index = AddCraft(typeof(CraftableHouseItem), 1044297, 1155849, 42.1, 77.7, typeof(Board), 1044041, 5, 1044351);
            SetData(index, CraftableItemType.DarkWoodenSignHanger);
            SetDisplayID(index, 2967);

            index = AddCraft(typeof(CraftableHouseItem), 1044297, 1155850, 42.1, 77.7, typeof(Board), 1044041, 5, 1044351);
            SetData(index, CraftableItemType.LightWoodenSignHanger);
            SetDisplayID(index, 2969);*/
            
            //AddCraft(typeof(FancyWoodenChairCushion), 1044297, 1044302, 42.1, 67.1, typeof(Board), 1044041, 15, 1044351); to samo co WoodenChairCushion
         
            index = AddCraft(typeof(Keg), 1044297, 1023711, 57.8, 82.8, typeof(BarrelStaves), 1044288, 3, 1044253);
            AddRes(index, typeof(BarrelHoops), 1044289, 1, 1044253);
            AddRes(index, typeof(BarrelLid), 1044251, 1, 1044253);
            ForceNonExceptional(index);
            
            AddCraft(typeof(LiquorBarrel), 1044297, 1150816, 60.0, 90.0, typeof(Board), 1044041, 50, 1044351);
            AddCraft(typeof(PlayerBBEast), 1044297, 1062420, 85.0, 110.0, typeof(Board), 1044041, 50, 1044351);
            AddCraft(typeof(PlayerBBSouth), 1044297, 1062421, 85.0, 110.0, typeof(Board), 1044041, 50, 1044351);

            //index = AddCraft(typeof(ParrotPerchAddonDeed), 1044297, 1072617, 50.0, 85.0, typeof(Board), 1044041, 100, 1044351);
           // ForceNonExceptional(index);
           
            AddCraft(typeof(SmokingPipe), 1044297, 1025700, 11.7, 30.1, typeof(Board), 1044041, 1, 1044351);
            #endregion
            
            MarkOption = true;
            Repair = true;
            CanEnhance = true;
            CanAlter = false;

            SetSubRes(typeof(Board), 1072643);

            // Add every material you want the player to be able to choose from
            // This will override the overridable material
            AddSubRes(typeof(Board), 1072643, 0.0, 1044041, 1072653);
            AddSubRes(typeof(OakBoard), 1072644, 65.0, 1044041, 1072653);
            AddSubRes(typeof(AshBoard), 1072645, 75.0, 1044041, 1072653);
            AddSubRes(typeof(YewBoard), 1072646, 85.0, 1044041, 1072653);
            AddSubRes(typeof(HeartwoodBoard), 1072647, 95.0, 1044041, 1072653);
            AddSubRes(typeof(BloodwoodBoard), 1072648, 95.0, 1044041, 1072653);
            AddSubRes(typeof(FrostwoodBoard), 1072649, 95.0, 1044041, 1072653);
        }
    }
}
