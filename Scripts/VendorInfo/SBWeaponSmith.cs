using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBWeaponSmith : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(BronzeShield), 66, 50, 0x1B72, 0));
				Add(new GenericBuyInfo(typeof(Buckler), 50, 50, 0x1B73, 0));
				Add(new GenericBuyInfo(typeof(MetalKiteShield), 123, 50, 0x1B74, 0));
				Add(new GenericBuyInfo(typeof(HeaterShield), 231, 50, 0x1B76, 0));
				Add(new GenericBuyInfo(typeof(WoodenKiteShield), 70, 50, 0x1B78, 0));
				Add(new GenericBuyInfo(typeof(MetalShield), 121, 50, 0x1B7B, 0));

				Add(new GenericBuyInfo(typeof(BlackStaff), 22, 50, 0xDF1, 0));
				Add(new GenericBuyInfo(typeof(Club), 16, 50, 0x13B4, 0));
				Add(new GenericBuyInfo(typeof(GnarledStaff), 19, 50, 0x13F8, 0));
				Add(new GenericBuyInfo(typeof(Mace), 28, 50, 0xF5C, 0));
				Add(new GenericBuyInfo(typeof(Maul), 35, 50, 0x143B, 0));
				Add(new GenericBuyInfo(typeof(QuarterStaff), 19, 50, 0xE89, 0));
				Add(new GenericBuyInfo(typeof(ShepherdsCrook), 20, 50, 0xE81, 0));
				Add(new GenericBuyInfo(typeof(SmithHammer), 21, 50, 0x13E3, 0));
				Add(new GenericBuyInfo(typeof(ShortSpear), 23, 50, 0x1403, 0));
				Add(new GenericBuyInfo(typeof(Spear), 41, 50, 0xF62, 0));
				Add(new GenericBuyInfo(typeof(WarHammer), 55, 50, 0x1439, 0));
				Add(new GenericBuyInfo(typeof(WarMace), 48, 50, 0x1407, 0));

				Add(new GenericBuyInfo(typeof(PlateGorget), 104, 50, 0x1413, 0));
				Add(new GenericBuyInfo(typeof(PlateChest), 243, 50, 0x1415, 0));
				Add(new GenericBuyInfo(typeof(PlateLegs), 218, 50, 0x1411, 0));
				Add(new GenericBuyInfo(typeof(PlateArms), 188, 50, 0x1410, 0));
				Add(new GenericBuyInfo(typeof(PlateGloves), 155, 50, 0x1414, 0));

				Add(new GenericBuyInfo(typeof(Scepter), 39, 50, 0x26BC, 0));
				Add(new GenericBuyInfo(typeof(BladedStaff), 40, 50, 0x26BD, 0));

				Add(new GenericBuyInfo(typeof(Hatchet), 30, 50, 0xF44, 0));
				Add(new GenericBuyInfo(typeof(Hatchet), 30, 50, 0xF43, 0));
				Add(new GenericBuyInfo(typeof(WarFork), 41, 50, 0x1405, 0));

				switch (Utility.Random(3))
				{
					case 0:
					{
						Add(new GenericBuyInfo(typeof(ExecutionersAxe), 48, 50, 0xF45, 0));
						Add(new GenericBuyInfo(typeof(Bardiche), 60, 50, 0xF4D, 0));
						Add(new GenericBuyInfo(typeof(BattleAxe), 48, 50, 0xF47, 0));
						Add(new GenericBuyInfo(typeof(TwoHandedAxe), 55, 50, 0x1443, 0));

						Add(new GenericBuyInfo(typeof(Bow), 35, 50, 0x13B2, 0));

						Add(new GenericBuyInfo(typeof(ButcherKnife), 14, 50, 0x13F6, 0));

						Add(new GenericBuyInfo(typeof(Crossbow), 46, 50, 0xF50, 0));
						Add(new GenericBuyInfo(typeof(HeavyCrossbow), 55, 50, 0x13FD, 0));

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
						Add(new GenericBuyInfo(typeof(Pickaxe), 22, 50, 0xE86, 0));

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

						break;
					}
				}
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Buckler), 18);
				Add(typeof(BronzeShield), 21);
				Add(typeof(MetalShield), 24);
				Add(typeof(MetalKiteShield), 28);
				Add(typeof(HeaterShield), 36);
				Add(typeof(WoodenKiteShield), 14);
				Add(typeof(WoodenShield), 15);

				Add(typeof(PlateArms), 40);
				Add(typeof(PlateChest), 58);
				Add(typeof(PlateGloves), 26);
				Add(typeof(PlateGorget), 20);
				Add(typeof(PlateLegs), 45);

				Add(typeof(FemalePlateChest), 44);

				Add(typeof(Bascinet), 9);
				Add(typeof(CloseHelm), 9);
				Add(typeof(Helmet), 9);
				Add(typeof(NorseHelm), 9);
				Add(typeof(PlateHelm), 10);

				Add(typeof(ChainCoif), 17);
				Add(typeof(ChainChest), 46);
				Add(typeof(ChainLegs), 38);

				Add(typeof(RingmailArms), 30);
				Add(typeof(RingmailChest), 39);
				Add(typeof(RingmailGloves), 19);
				Add(typeof(RingmailLegs), 38);
			}
		}
	}
}
