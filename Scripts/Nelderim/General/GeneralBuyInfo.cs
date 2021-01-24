using Server.Helpers;
using System;

namespace Server.Mobiles
{
    class GeneralBuyInfo : GenericBuyInfo
    {
        public GeneralBuyInfo( Type type, int price, int amount )
            : base(type, price, amount, Default.Item[type].ItemID, Default.Item[type].Hue)
        {
        }
    }
}
