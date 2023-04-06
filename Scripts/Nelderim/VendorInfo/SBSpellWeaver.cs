using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBSpellWeaver : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(SpellweavingBook), 500, 20, 0x23A0, 0));
				AddRange(SpellweaverSpells);
			}
		}
        
		public static List<IBuyItemInfo> SpellweaverSpells = new List<IBuyItemInfo>(
			new []{
				new GenericBuyInfo(typeof(ArcaneCircleScroll), 8, 20, 0x2D9F, 0),
				new GenericBuyInfo(typeof(AttuneWeaponScroll), 8, 20, 0x2D9E, 0),
				new GenericBuyInfo(typeof(GiftOfRenewalScroll), 8, 10, 0x2DA1, 0, true),
				new GenericBuyInfo(typeof(NatureFuryScroll), 8, 10, 0x2DA0, 0, true),
				new GenericBuyInfo(typeof(ImmolatingWeaponScroll), 18, 10, 0x2DA3, 0, true),
				new GenericBuyInfo(typeof(ThunderstormScroll), 18, 10, 0x2DA2, 0, true),
				new GenericBuyInfo(typeof(ArcaneEmpowermentScroll), 38, 10, 0x2DA4, 0, true),
				new GenericBuyInfo(typeof(EtherealVoyageScroll), 38, 10, 0x2DA5, 0, true),
				new GenericBuyInfo(typeof(ReaperFormScroll), 38, 10, 0x2DA5, 0, true)
			});


		public class InternalSellInfo : GenericSellInfo
		{
		}
	}
}
