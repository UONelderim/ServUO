using Server.Items;
using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBNecromancer : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Scalpel), 50, 50, 4327, 0));

				Add(new GenericBuyInfo(typeof(BlackPearl), 7, 50, 0xF7A, 0));
				Add(new GenericBuyInfo(typeof(Bloodmoss), SBHerbalist.GlobalHerbsPriceBuy, 50, 0xF7B, 0));
				Add(new GenericBuyInfo(typeof(Garlic), SBHerbalist.GlobalHerbsPriceBuy, 50, 0xF84, 0));
				Add(new GenericBuyInfo(typeof(Ginseng), SBHerbalist.GlobalHerbsPriceBuy, 50, 0xF85, 0));
				Add(new GenericBuyInfo(typeof(MandrakeRoot), SBHerbalist.GlobalHerbsPriceBuy, 50, 0xF86, 0));
				Add(new GenericBuyInfo(typeof(Nightshade), SBHerbalist.GlobalHerbsPriceBuy, 50, 0xF88, 0));
				Add(new GenericBuyInfo(typeof(SpidersSilk), 5, 50, 0xF8D, 0));
				Add(new GenericBuyInfo(typeof(SulfurousAsh), 5, 50, 0xF8C, 0));
				Add(new GenericBuyInfo(typeof(DestroyingAngel), 8, 50, 0xE1F, 0));
				Add(new GenericBuyInfo(typeof(PetrafiedWood), 8, 50, 0x97A, 0));
				Add(new GenericBuyInfo(typeof(SpringWater), 8, 50, 0xE24, 0));

				Add(new GenericBuyInfo(typeof(MortarPestle), 9, 50, 0xE9B, 0));
				Add(new GenericBuyInfo(typeof(Bottle), 5, 50, 0xF0E, 0));

				Add(new GenericBuyInfo(typeof(NecromancerSpellbook), 150, 10, 0x2253, 0));
				
				Add(new GenericBuyInfo(typeof(BatWing), 6, 50, 0xF78, 0));
				Add(new GenericBuyInfo(typeof(DaemonBlood), 8, 50, 0xF7D, 0));
				Add(new GenericBuyInfo(typeof(PigIron), 7, 50, 0xF8A, 0));
				Add(new GenericBuyInfo(typeof(NoxCrystal), 8, 50, 0xF8E, 0));
				Add(new GenericBuyInfo(typeof(GraveDust), 6, 50, 0xF8F, 0));

				Type[] types = Loot.MageryScrollTypes;

				for (int i = 0; i < types.Length && i < 8; ++i)
				{
					int itemID = 0x1F2E + i;

					if (i == 6)
						itemID = 0x1F2D;
					else if (i > 6)
						--itemID;

					Add(new GenericBuyInfo(types[i], 12 + ((i / 8) * 10), 20, itemID, 0, true));
				}
				
				AddRange(NecromancerSpells);
			}
		}

		public static List<IBuyItemInfo> NecromancerSpells = new List<IBuyItemInfo>(
		new []{
			new GenericBuyInfo(typeof(CurseWeaponScroll), 40, 10, 0x2263, 0),
			new GenericBuyInfo(typeof(BloodOathScroll), 40, 10, 0x2261, 0),
			new GenericBuyInfo(typeof(CorpseSkinScroll), 40, 10, 0x2262, 0),
			new GenericBuyInfo(typeof(EvilOmenScroll), 40, 10, 0x2264, 0),
			new GenericBuyInfo(typeof(PainSpikeScroll), 40, 10, 0x2268, 0),
			new GenericBuyInfo(typeof(WraithFormScroll), 40, 10, 0x226F, 0),
			new GenericBuyInfo(typeof(MindRotScroll), 40, 10, 0x2267, 0),
			new GenericBuyInfo(typeof(SummonFamiliarScroll), 40, 105, 0x226B, 0),
			new GenericBuyInfo(typeof(AnimateDeadScroll), 40, 10, 0x2260, 0)
		});

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(WizardsHat), 5);

				Add(typeof(BlackPearl), 2);
				Add(typeof(Bloodmoss), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(MandrakeRoot), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(Garlic), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(Ginseng), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(Nightshade), SBHerbalist.GlobalHerbsPriceSell);
				Add(typeof(SpidersSilk), 2);
				Add(typeof(SulfurousAsh), 2);

				Add(typeof(BatWing), 2);
				Add(typeof(DaemonBlood), 2);
				Add(typeof(PigIron), 2);
				Add(typeof(NoxCrystal), 2);
				Add(typeof(GraveDust), 2);

				Add(typeof(RecallRune), 50);
				Add(typeof(Spellbook), 25);

				Add(typeof(ExorcismScroll), 35);
				Add(typeof(AnimateDeadScroll), 30);
				Add(typeof(BloodOathScroll), 18);
				Add(typeof(CorpseSkinScroll), 24);
				Add(typeof(CurseWeaponScroll), 20);
				Add(typeof(EvilOmenScroll), 20);
				Add(typeof(PainSpikeScroll), 16);
				Add(typeof(SummonFamiliarScroll), 26);
				Add(typeof(HorrificBeastScroll), 22);
				Add(typeof(MindRotScroll), 28);
				Add(typeof(PoisonStrikeScroll), 22);
				Add(typeof(WraithFormScroll), 25);
				Add(typeof(LichFormScroll), 38);
				Add(typeof(StrangleScroll), 33);
				Add(typeof(WitherScroll), 35);
				Add(typeof(VampiricEmbraceScroll), 45);
				Add(typeof(VengefulSpiritScroll), 48);

				Add(typeof(ScribesPen), 3);
				Add(typeof(BrownBook), 4);
				Add(typeof(TanBook), 4);
				Add(typeof(BlueBook), 4);
				Add(typeof(BlankScroll), 2);

				Type[] types = Loot.MageryScrollTypes;

				for (int i = 0; i < types.Length; ++i)
					Add(types[i], ((i / 8) + 2) * 2);
			}
		}
	}
}
