using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBScribe : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo;
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBScribe(Mobile m)
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
				Add(new GenericBuyInfo(typeof(ScribesPen), 30, 50, 0xFBF, 0));
				Add(new GenericBuyInfo(typeof(BlankScroll), 10, 50, 0x0E34, 0));
				Add(new GenericBuyInfo(typeof(BrownBook), 15, 50, 0xFEF, 0));
				Add(new GenericBuyInfo(typeof(TanBook), 15, 50, 0xFF0, 0));
				Add(new GenericBuyInfo(typeof(BlueBook), 15, 50, 0xFF2, 0));
				Add(new GenericBuyInfo("1041267", typeof(Runebook), 35000, 10, 0xEFA, 0x461));

				AddRange(SBMage.MageryScrolls);
				AddRange(SBNecromancer.NecromancerSpells);
				AddRange(SBKeeperOfChivalry.ChivalryScrolls);
				AddRange(SBKeeperOfBushido.BushidoScrolls);
				AddRange(SBKeeperOfNinjitsu.NinjitsuScrolls);
				AddRange(SBSpellWeaver.SpellweaverSpells);
				//!Mistyk!
				// AddRange(SBMystic.MysticSpells);

				Add(new GenericBuyInfo(typeof(Spellbook), 200, 20, 0x23A0, 0));
				Add(new GenericBuyInfo(typeof(NecromancerSpellbook), 300, 20, 0x23A0, 0));
				Add(new GenericBuyInfo(typeof(BookOfChivalry), 350, 20, 0x23A0, 0));
				Add(new GenericBuyInfo(typeof(BookOfBushido), 350, 20, 0x238C, 0));
				Add(new GenericBuyInfo(typeof(BookOfNinjitsu), 350, 20, 0x23A0, 0));
				Add(new GenericBuyInfo(typeof(SpellweavingBook), 400, 20, 0x2D50, 0));
				//!Mistyk!
				// Add(new GenericBuyInfo(typeof(MysticBook), 400, 20, 0x2D9D, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(ScribesPen), 8);
				Add(typeof(BrownBook), 7);
				Add(typeof(TanBook), 7);
				Add(typeof(BlueBook), 7);
				Add(typeof(BlankScroll), 2);
				Add(typeof(Spellbook), 50);
			}
		}
	}
}
