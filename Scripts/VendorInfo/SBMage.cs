using Server.Items;
using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBMage : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Spellbook), 100, 50, 0xEFA, 0));
				Add(new GenericBuyInfo(typeof(ScribesPen), 30, 50, 0xFBF, 0));
				Add(new GenericBuyInfo(typeof(BlankScroll), 10, 50, 0x0E34, 0));
				Add(new GenericBuyInfo(typeof(MagicWizardsHat), 11, 50, 0x1718, Utility.RandomDyedHue()));
				Add(new GenericBuyInfo(typeof(RecallRune), 2000, 10, 0x1F14, 0));
				
				Add(new GenericBuyInfo(typeof(RefreshPotion), 15, 50, 0xF0B, 0));
				Add(new GenericBuyInfo(typeof(AgilityPotion), 15, 50, 0xF08, 0));
				Add(new GenericBuyInfo(typeof(NightSightPotion), 15, 50, 0xF06, 0));
				Add(new GenericBuyInfo(typeof(LesserHealPotion), 15, 50, 0xF0C, 0));
				Add(new GenericBuyInfo(typeof(StrengthPotion), 15, 50, 0xF09, 0));
				Add(new GenericBuyInfo(typeof(LesserPoisonPotion), 15, 50, 0xF0A, 0));
				Add(new GenericBuyInfo(typeof(LesserCurePotion), 15, 50, 0xF07, 0));
				Add(new GenericBuyInfo(typeof(LesserExplosionPotion), 21, 50, 0xF0D, 0));

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

				Add(new GenericBuyInfo(typeof(BatWing), 6, 50, 0xF78, 0));
				Add(new GenericBuyInfo(typeof(DaemonBlood), 8, 50, 0xF7D, 0));
				Add(new GenericBuyInfo(typeof(PigIron), 7, 50, 0xF8A, 0));
				Add(new GenericBuyInfo(typeof(NoxCrystal), 8, 50, 0xF8E, 0));
				Add(new GenericBuyInfo(typeof(GraveDust), 6, 50, 0xF8F, 0));

				AddRange(MageryScrolls);
			}
		}

		public static List<IBuyItemInfo> MageryScrolls
		{
			get
			{
				var result = new List<IBuyItemInfo>();
				Type[] types = Loot.MageryScrollTypes;
				int circles = 5;
				for (int i = 0; i < circles * 8 && i < types.Length; ++i)
				{
					if (types[i] == typeof(RecallScroll)) continue; //We don't want recalls from NPCs
					int itemID = 0x1F2E + i;

					if (i == 6)
						itemID = 0x1F2D;
					else if (i > 6)
						--itemID;

					result.Add(new GenericBuyInfo(types[i], 12 + ((i / 8) * 10), 20, itemID, 0, true));
				}

				return result;
			}
		}

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

				Type[] types = Loot.MageryScrollTypes;

				for (int i = 0; i < types.Length; ++i)
					Add(types[i], ((i / 8) + 2) * 2);

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
			}
		}
	}
}
