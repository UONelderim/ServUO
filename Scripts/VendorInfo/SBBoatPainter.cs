using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBBoatPainter : SBInfo
    {
        private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<IBuyItemInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo("Farba do statku", typeof(BoatPaint), 25000, 20, 4011, 276, new object[] { 276 }));
                Add(new GenericBuyInfo("Farba do statku", typeof(BoatPaint), 25000, 20, 4011, 396, new object[] { 396 }));
                Add(new GenericBuyInfo("Farba do statku", typeof(BoatPaint), 25000, 20, 4011, 516, new object[] { 516 }));
                Add(new GenericBuyInfo("Farba do statku", typeof(BoatPaint), 25000, 20, 4011, 1900, new object[] { 1900 }));
                Add(new GenericBuyInfo("Farba do statku", typeof(BoatPaint), 25000, 20, 4011, 251, new object[] { 251 }));
                Add(new GenericBuyInfo("Farba do statku", typeof(BoatPaint), 25000, 20, 4011, 246, new object[] { 246 }));
                Add(new GenericBuyInfo("Farba do statku", typeof(BoatPaint), 25000, 20, 4011, 2213, new object[] { 2213 }));
                Add(new GenericBuyInfo("Farba do statku", typeof(BoatPaint), 25000, 20, 4011, 36, new object[] { 36 }));
                Add(new GenericBuyInfo("Srodek do usuwania farby do statku", typeof(BoatPaintRemover), 10000, 20, 4011, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(LobsterTrap), 10);
            }
        }
    }
}
