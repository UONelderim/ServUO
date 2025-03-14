using Server.Items;
using Server.Multis;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBShipwright : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo;
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBShipwright(Mobile m)
		{
			if (m != null)
			{
				m_BuyInfo = new InternalBuyInfo(m);
			}
		}

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo(Mobile m)
			{
				//statki 
				//Add(new GenericBuyInfo("1116491", typeof(RowBoatDeed), 30000, 5, 0x14F2, 0)); TODO: fix it - does not destroy the deed
				Add(new GenericBuyInfo("1041205", typeof(SmallBoatDeed), 70000, 5, 0x14F2, 0));
				Add(new GenericBuyInfo("1041206", typeof(SmallDragonBoatDeed), 80000, 5, 0x14F2, 0));
				Add(new GenericBuyInfo("1041207", typeof(MediumBoatDeed), 90000, 5, 0x14F2, 0));
				Add(new GenericBuyInfo("1041208", typeof(MediumDragonBoatDeed), 100000, 5, 0x14F2, 0));
				Add(new GenericBuyInfo("1041209", typeof(LargeBoatDeed), 110000, 5, 0x14F2, 0));
				Add(new GenericBuyInfo("1041210", typeof(LargeDragonBoatDeed), 120000, 5, 0x14F2, 0));

				Add(new GenericBuyInfo("1116740", typeof(TokunoGalleonDeed), 2000000, 5, 0x14F2, 0));
				Add(new GenericBuyInfo("1116739", typeof(GargishGalleonDeed), 2500000, 5, 0x14F2, 0));
				//Add(new GenericBuyInfo("1116738", typeof(OrcishGalleonDeed), 2700000, 5, 0x14F2, 0)); Orczy galeon dostępny jest z questa. Można dodać kiedyś.
				Add(new GenericBuyInfo("1150017", typeof(BritannianShipDeed), 3000000, 5, 0x14F2, 0));
				
				Add(new GenericBuyInfo(typeof(Spyglass), 3, 20, 0x14F5, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				//You technically CAN sell them back, *BUT* the vendors do not carry enough money to buy with
				Add(typeof(Spyglass), 1);
			}
		}
	}
}
