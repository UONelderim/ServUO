using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBPlateArmor : SBInfo
    {
        private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(PlateGorget), 104, 20, 0x1413, 0));
                Add(new GenericBuyInfo(typeof(PlateChest), 243, 20, 0x1415, 0));
                Add(new GenericBuyInfo(typeof(PlateLegs), 218, 20, 0x1411, 0));
                Add(new GenericBuyInfo(typeof(PlateArms), 188, 20, 0x1410, 0));
                Add(new GenericBuyInfo(typeof(PlateGloves), 155, 20, 0x1414, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(PlateArms), 94);
                Add(typeof(PlateChest), 121);
                Add(typeof(PlateGloves), 72);
                Add(typeof(PlateGorget), 52);
                Add(typeof(PlateLegs), 109);

                Add(typeof(FemalePlateChest), 113);
            }
        }
    }
}
