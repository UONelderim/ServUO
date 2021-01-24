using System;
using System.Collections.Generic;
using Server.Engines.Craft;
using Server.Items;

namespace Server.Mobiles
{
    // Klasa automatycznie generujaca liste itemkow, ktore mozna sprzedac do NPC
    // na podstawie wszystkiego, co mozna wyprodukowac danym skillem rzemieslniczym.
    public class CraftSB : SBInfo
    {
        // Ponizsze obiekty mozna uzywac jako SBInfo w definicjach klas NPC:
        public static SBInfo CraftSellWeaponsmith = CraftSB.CraftItemsExcluding( DefBlacksmithy.CraftSystem, new Type[] { typeof( BaseWeapon ), typeof( DragonBardingDeed ), typeof( Shuriken ) } );
        public static SBInfo CraftSellBlacksmith = CraftSB.CraftItemsExcluding( DefBlacksmithy.CraftSystem, new Type[] { typeof( BaseArmor ), typeof( DragonBardingDeed ) } );
        public static SBInfo CraftSellTinker = CraftSB.CraftItemsExcluding( DefTinkering.CraftSystem, new Type[] { typeof( BaseJewel ) } );
        public static SBInfo CraftSellCarpenter = CraftSB.CraftItemsExcluding( DefCarpentry.CraftSystem, new Type[] { typeof( BaseInstrument ) } );
        public static SBInfo CraftSellTailor = CraftSB.CraftItemsExcluding( DefTailoring.CraftSystem, new Type[] { typeof( BaseArmor ) } );
        public static SBInfo CraftSellLeatherWorker = CraftSB.CraftItemsExcluding( DefTailoring.CraftSystem, new Type[] { typeof( BaseClothing ) } );
        public static SBInfo CraftSellFletcher = CraftSB.CraftItemsIncluding( DefBowFletching.CraftSystem, new Type[] { typeof( BaseWeapon ) } );

        private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
        private GenericSellInfo m_SellInfo;

        private CraftSystem m_CraftSystem;
        private List<Type> m_Exclude;   // If the list is not empty, then NPC will not buy these items
        private List<Type> m_Include;   // If the list is not empty, then NPC will only buy these items (m_Exclude becomes irrelevant)

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo; 

        private CraftSB( CraftSystem system, Type[] exclude, Type[] include )
        {
            m_Exclude = new List<Type>();
            m_Include = new List<Type>();

            m_CraftSystem = system;

            if ( exclude != null && exclude.Length > 0 )
                m_Exclude.AddRange( exclude );

            if ( include != null && include.Length > 0 )
                m_Include.AddRange( include );

            SellInit();
        }

        protected static SBInfo CraftItemsIncluding( CraftSystem system, Type[] list )
        {
            return new CraftSB( system, null, list );
        }

        protected static SBInfo CraftItemsExcluding( CraftSystem system, Type[] list )
        {
            return new CraftSB( system, list, null );
        }

        // Ta metoda oblicza wartosc produktu na podstawie ilosci potrzebnego do produkcji surowca.
        // Dodatkowo mozna tez uwzglednic rodzaj samego rzemiosla.
        private int CalculatePrice( CraftItem item )
        {
            CraftRes res;

            if ( item.UseSubRes2 && item.Resources.Count > 1 )
                res = item.Resources.GetAt( 1 );
            else
                res = item.Resources.GetAt( 0 );

            int amount = res.Amount;
            double unitPrice = 1.0;

            if ( res.ItemType == typeof( Log ) ) // DREWNO
            {
                if ( m_CraftSystem == DefCarpentry.CraftSystem )  // SeedBox: 50 klod
                    unitPrice = 2.50;
                else if ( m_CraftSystem == DefBowFletching.CraftSystem ) // crossbow: 7 klod
                    unitPrice = 3.50;
                else if ( m_CraftSystem == DefTinkering.CraftSystem ) // ClockFrame: 6 klod
                    unitPrice = 1.50; // majster ma zarabiac metalem :P
                else
                    unitPrice = 2.00;
            }
            else if ( res.ItemType == typeof( IronIngot ) ) // SZTABY
            {
                if ( m_CraftSystem == DefTinkering.CraftSystem )  // kula do boli: 10 sztab
                    unitPrice = 3.33;
                else if ( m_CraftSystem == DefBlacksmithy.CraftSystem ) // 20 lub 750*95% (smocza zbroja)
                    unitPrice = 3.33;
                else
                    unitPrice = 2.00;
            }
            else if ( res.ItemType == typeof( Leather ) ) // SKORY
            {
                if ( m_CraftSystem == DefTailoring.CraftSystem )  // StuddedXxxx: 14 skor
                    unitPrice = 4.75;
                else if ( m_CraftSystem == DefBowFletching.CraftSystem ) // BowstringLeather: masowo
                    unitPrice = 2.00;
                else
                    unitPrice = 2.00;
            }
            else if ( res.ItemType == typeof( Cloth ) ) // MATERIAL
            {
                unitPrice = 1.50;
            }

            return (int)((double)amount * unitPrice);
        }

        private void SellInit()
        {
            m_SellInfo = new GenericSellInfo();

            if ( m_Exclude != null && m_Exclude.Count > 0 )
            {
                // We add everything from craft list except some type of items

                foreach ( CraftItem item in m_CraftSystem.CraftItems )
                {
                    foreach ( Type exclude in m_Exclude )
                    {
                        if ( item.ItemType.IsSubclassOf( exclude ) || item.ItemType == exclude )
                            break;
                        m_SellInfo.Add( item.ItemType, CalculatePrice( item ) );

                        //Console.WriteLine("{0} {1}  {2} {3}", ex, item.ItemType, exclude, m_CraftSystem);
                    }
                }
            }
            else if ( m_Include != null && m_Include.Count > 0 )
            {
                // We only add specified type of items from craft list

                foreach ( CraftItem item in m_CraftSystem.CraftItems )
                {
                    foreach ( Type include in m_Include )
                    {
                        if ( item.ItemType.IsSubclassOf( include ) || item.ItemType == include )
                        {
                            m_SellInfo.Add( item.ItemType, CalculatePrice( item ) );
                            break;
                        }
                    }
                }
            }
            else
            {
                // We add everything from craft list

                foreach ( CraftItem item in m_CraftSystem.CraftItems )
                {
                    m_SellInfo.Add( item.ItemType, CalculatePrice( item ) );
                }
            }
        }
    }

}
