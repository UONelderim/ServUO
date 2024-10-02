using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBBlacksmith : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(IronIngot), 5, 16, 0x1BF2, 0, true));
				Add(new GenericBuyInfo(typeof(Tongs), 15, 50, 0xFBB, 0));
				Add(new GenericBuyInfo(typeof(SmithHammer), 30, 50, 0x13E3, 0));
				Add(new GenericBuyInfo(typeof(Pickaxe), 30, 50, 0xE86, 0));

				Add(new GenericBuyInfo(typeof(MetalShield), 121, 50, 0x1B7B, 0));

				Add(new GenericBuyInfo(typeof(WoodenShield), 30, 50, 0x1B7A, 0));

				Add(new GenericBuyInfo(typeof(PlateGorget), 104, 50, 0x1413, 0));
				Add(new GenericBuyInfo(typeof(PlateChest), 243, 50, 0x1415, 0));
				Add(new GenericBuyInfo(typeof(PlateLegs), 218, 50, 0x1411, 0));
				Add(new GenericBuyInfo(typeof(PlateArms), 188, 50, 0x1410, 0));
				Add(new GenericBuyInfo(typeof(PlateGloves), 155, 50, 0x1414, 0));

				Add(new GenericBuyInfo(typeof(PlateHelm), 53, 50, 0x1412, 0));
				Add(new GenericBuyInfo(typeof(CloseHelm), 51, 50, 0x1408, 0));
				Add(new GenericBuyInfo(typeof(CloseHelm), 51, 50, 0x1409, 0));
				Add(new GenericBuyInfo(typeof(Helmet), 51, 50, 0x140A, 0));
				Add(new GenericBuyInfo(typeof(Helmet), 51, 50, 0x140B, 0));
				Add(new GenericBuyInfo(typeof(NorseHelm), 51, 50, 0x140E, 0));
				Add(new GenericBuyInfo(typeof(NorseHelm), 51, 50, 0x140F, 0));
				Add(new GenericBuyInfo(typeof(Bascinet), 51, 50, 0x140C, 0));
				Add(new GenericBuyInfo(typeof(PlateHelm), 53, 50, 0x1419, 0));

				Add(new GenericBuyInfo(typeof(ChainCoif), 35, 50, 0x13BB, 0));
				Add(new GenericBuyInfo(typeof(ChainChest), 143, 50, 0x13BF, 0));
				Add(new GenericBuyInfo(typeof(ChainLegs), 149, 50, 0x13BE, 0));

				Add(new GenericBuyInfo(typeof(RingmailChest), 121, 50, 0x13ec, 0));
				Add(new GenericBuyInfo(typeof(RingmailLegs), 90, 50, 0x13F0, 0));
				Add(new GenericBuyInfo(typeof(RingmailArms), 85, 50, 0x13EE, 0));
				Add(new GenericBuyInfo(typeof(RingmailGloves), 93, 50, 0x13eb, 0));
				Add(new GenericBuyInfo(typeof(DragonBardingDeed), 30000, 20, 0x14F0, 0));

				Add(new GenericBuyInfo(typeof(ExecutionersAxe), 48, 50, 0xF45, 0));
				Add(new GenericBuyInfo(typeof(Bardiche), 60, 50, 0xF4D, 0));
				Add(new GenericBuyInfo(typeof(BattleAxe), 48, 50, 0xF47, 0));
				Add(new GenericBuyInfo(typeof(TwoHandedAxe), 55, 50, 0x1443, 0));
				Add(new GenericBuyInfo(typeof(Bow), 35, 50, 0x13B2, 0));
				Add(new GenericBuyInfo(typeof(ButcherKnife), 14, 50, 0x13F6, 0));
				Add(new GenericBuyInfo(typeof(Cutlass), 28, 50, 0x1441, 0));
				Add(new GenericBuyInfo(typeof(Dagger), 21, 50, 0xF52, 0));
				Add(new GenericBuyInfo(typeof(Halberd), 68, 50, 0x143E, 0));
				Add(new GenericBuyInfo(typeof(HammerPick), 55, 50, 0x143D, 0));
				Add(new GenericBuyInfo(typeof(Katana), 33, 50, 0x13FF, 0));
				Add(new GenericBuyInfo(typeof(Kryss), 32, 50, 0x1401, 0));
				Add(new GenericBuyInfo(typeof(Broadsword), 35, 50, 0xF5E, 0));
				Add(new GenericBuyInfo(typeof(Longsword), 55, 50, 0xF61, 0));
				Add(new GenericBuyInfo(typeof(ThinLongsword), 27, 50, 0x13B8, 0));
				Add(new GenericBuyInfo(typeof(VikingSword), 55, 50, 0x13B9, 0));
				Add(new GenericBuyInfo(typeof(Cleaver), 15, 50, 0xEC3, 0));
				Add(new GenericBuyInfo(typeof(Axe), 48, 50, 0xF49, 0));
				Add(new GenericBuyInfo(typeof(DoubleAxe), 52, 50, 0xF4B, 0));

				Add(new GenericBuyInfo(typeof(Pitchfork), 19, 50, 0xE87, 0));
				Add(new GenericBuyInfo(typeof(Scimitar), 36, 50, 0x13B6, 0));
				Add(new GenericBuyInfo(typeof(SkinningKnife), 14, 50, 0xEC4, 0));
				Add(new GenericBuyInfo(typeof(LargeBattleAxe), 41, 50, 0x13FB, 0));
				Add(new GenericBuyInfo(typeof(WarAxe), 55, 50, 0x13B0, 0));

				Add(new GenericBuyInfo(typeof(BoneHarvester), 35, 50, 0x26BB, 0));
				Add(new GenericBuyInfo(typeof(CrescentBlade), 48, 50, 0x26C1, 0));
				Add(new GenericBuyInfo(typeof(DoubleBladedStaff), 55, 50, 0x26BF, 0));
				Add(new GenericBuyInfo(typeof(Lance), 68, 50, 0x26C0, 0));
				Add(new GenericBuyInfo(typeof(Pike), 39, 50, 0x26BE, 0));
				Add(new GenericBuyInfo(typeof(Scythe), 48, 50, 0x26BA, 0));
				Add(new GenericBuyInfo(typeof(CompositeBow), 50, 50, 0x26C2, 0));
				Add(new GenericBuyInfo(typeof(RepeatingCrossbow), 57, 50, 0x26C3, 0));

				Add(new GenericBuyInfo(typeof(BlackStaff), 22, 50, 0xDF1, 0));
				Add(new GenericBuyInfo(typeof(Club), 16, 50, 0x13B4, 0));
				Add(new GenericBuyInfo(typeof(GnarledStaff), 20, 50, 0x13F8, 0));
				Add(new GenericBuyInfo(typeof(Mace), 28, 50, 0xF5C, 0));
				Add(new GenericBuyInfo(typeof(Maul), 35, 50, 0x143B, 0));
				Add(new GenericBuyInfo(typeof(QuarterStaff), 19, 50, 0xE89, 0));
				Add(new GenericBuyInfo(typeof(ShepherdsCrook), 20, 50, 0xE81, 0));

				Add(new GenericBuyInfo(typeof(ShortSpear), 23, 50, 0x1403, 0));
				Add(new GenericBuyInfo(typeof(Spear), 41, 50, 0xF62, 0));
				Add(new GenericBuyInfo(typeof(WarHammer), 55, 50, 0x1439, 0));
				Add(new GenericBuyInfo(typeof(WarMace), 48, 50, 0x1407, 0));


				Add(new GenericBuyInfo(typeof(Scepter), 39, 50, 0x26BC, 0));
				Add(new GenericBuyInfo(typeof(BladedStaff), 40, 50, 0x26BD, 0));

				Add(new GenericBuyInfo("1154005", typeof(MalleableAlloy), 50, 500, 7139, 2949, true));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Tongs), 4);
				Add(typeof(SmithHammer), 8);
				Add(typeof(Pickaxe), 8);

				Add(typeof(IronIngot), 1);
				Add(typeof(BattleAxe), 15);
				Add(typeof(DoubleAxe), 18);
				Add(typeof(ExecutionersAxe), 18);
				Add(typeof(LargeBattleAxe), 19);
				Add(typeof(TwoHandedAxe), 17);
				Add(typeof(WarAxe), 15);
				Add(typeof(Axe), 19);
				Add(typeof(Bardiche), 30);
				Add(typeof(Halberd), 21);
				Add(typeof(ButcherKnife), 6);
				Add(typeof(Cleaver), 7);
				Add(typeof(Dagger), 10);
				Add(typeof(SkinningKnife), 6);
				Add(typeof(Club), 8);
				Add(typeof(HammerPick), 13);
				Add(typeof(Mace), 14);
				Add(typeof(Maul), 10);
				Add(typeof(WarHammer), 12);
				Add(typeof(WarMace), 15);

				Add(typeof(Scepter), 20);
				Add(typeof(BladedStaff), 20);
				Add(typeof(Scythe), 19);
				Add(typeof(BoneHarvester), 17);
				Add(typeof(Scepter), 18);
				Add(typeof(BladedStaff), 16);
				Add(typeof(Pike), 19);
				Add(typeof(DoubleBladedStaff), 17);
				Add(typeof(Lance), 17);
				Add(typeof(CrescentBlade), 18);

				Add(typeof(Spear), 15);
				Add(typeof(Pitchfork), 9);
				Add(typeof(WarFork), 10);
				Add(typeof(ShortSpear), 11);
				Add(typeof(BlackStaff), 11);
				Add(typeof(GnarledStaff), 8);
				Add(typeof(QuarterStaff), 9);
				Add(typeof(ShepherdsCrook), 10);
				Add(typeof(Broadsword), 17);
				Add(typeof(Cutlass), 12);
				Add(typeof(Katana), 16);
				Add(typeof(Kryss), 16);
				Add(typeof(Longsword), 18);
				Add(typeof(Scimitar), 18);
				Add(typeof(ThinLongsword), 13);
				Add(typeof(VikingSword), 18);
			}
		}
	}
}
