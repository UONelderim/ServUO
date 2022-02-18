#region References

using System;
using Server.Helpers;

#endregion

namespace Server.Mobiles
{
	class GeneralBuyInfo : GenericBuyInfo
	{
		public GeneralBuyInfo(Type type, int price, int amount)
			: base(type, price, amount, Default.Item[type].ItemID, Default.Item[type].Hue)
		{
		}
	}
}
