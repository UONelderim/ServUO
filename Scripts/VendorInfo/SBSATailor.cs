using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBSATailor : SBInfo
    {
        private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(Cotton), 102, 20, 0xDF9, 0, true));
                Add(new GenericBuyInfo(typeof(Wool), 62, 20, 0xDF8, 0, true));
                Add(new GenericBuyInfo(typeof(Flax), 102, 20, 0x1A9C, 0, true));
                Add(new GenericBuyInfo(typeof(SpoolOfThread), 18, 20, 0xFA0, 0, true));
                Add(new GenericBuyInfo(typeof(SewingKit), 15, 20, 0xF9D, 0));
                Add(new GenericBuyInfo(typeof(Scissors), 11, 20, 0xF9F, 0));
                Add(new GenericBuyInfo(typeof(DyeTub), 8, 20, 0xFAB, 0));
                Add(new GenericBuyInfo(typeof(Dyes), 8, 20, 0xFA9, 0));

                Add(new GenericBuyInfo(typeof(GargishRobe), 32, 20, 0x4000, 0));
                Add(new GenericBuyInfo(typeof(GargishFancyRobe), 46, 20, 0x4002, 0));

                Add(new GenericBuyInfo(typeof(FemaleGargishClothArmsArmor), 62, 20, 0x403, 0));
                Add(new GenericBuyInfo(typeof(GargishClothArmsArmor), 61, 20, 0x404, 0));
                Add(new GenericBuyInfo(typeof(FemaleGargishClothChestArmor), 83, 20, 0x405, 0));
                Add(new GenericBuyInfo(typeof(GargishClothChestArmor), 78, 20, 0x406, 0));
                Add(new GenericBuyInfo(typeof(FemaleGargishClothLegsArmor), 71, 20, 0x409, 0));
                Add(new GenericBuyInfo(typeof(GargishClothLegsArmor), 66, 20, 0x40A, 0));
                Add(new GenericBuyInfo(typeof(FemaleGargishClothKiltArmor), 57, 20, 0x407, 0));
                Add(new GenericBuyInfo(typeof(GargishClothKiltArmor), 56, 20, 0x408, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(Cotton), 51);
                Add(typeof(Wool), 31);
                Add(typeof(Flax), 51);
                Add(typeof(SpoolOfThread), 9);
                Add(typeof(SewingKit), 1);
                Add(typeof(Scissors), 6);
                Add(typeof(DyeTub), 4);
                Add(typeof(Dyes), 4);

                Add(typeof(GargishRobe), 16);
                Add(typeof(GargishFancyRobe), 23);
                Add(typeof(FemaleGargishClothArmsArmor), 30);
                Add(typeof(GargishClothArmsArmor), 30);
                Add(typeof(FemaleGargishClothChestArmor), 40);
                Add(typeof(GargishClothChestArmor), 42);
                Add(typeof(FemaleGargishClothLegsArmor), 30);
                Add(typeof(GargishClothLegsArmor), 32);
                Add(typeof(FemaleGargishClothKiltArmor), 30);
                Add(typeof(GargishClothKiltArmor), 32);
            }
        }
    }
}
