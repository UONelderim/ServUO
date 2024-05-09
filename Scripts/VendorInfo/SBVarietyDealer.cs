using Server.Items;
using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class SBVarietyDealer : SBInfo
	{
		private readonly List<IBuyItemInfo> m_BuyInfo = new InternalBuyInfo();
		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;
		public override List<IBuyItemInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<IBuyItemInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo("1060834", typeof(Engines.Plants.PlantBowl), 50, 5, 0x15FD, 0));

				Add(new GenericBuyInfo(typeof(SkinningKnife), 70, 5, 0xEC4, 0));
				Add(new GenericBuyInfo(typeof(Club), 70, 5, 0x13B4, 0));
				Add(new GenericBuyInfo(typeof(Bow), 70, 5, 0x13B2, 0));
				Add(new GenericBuyInfo(typeof(Dagger), 70, 5, 0xF52, 0));

				Add(new GenericBuyInfo(typeof(Lute), 300, 2, 0x0EB3, 0));

				Add(new GenericBuyInfo(typeof(Bolt), 8, 20, 0x1BFB, 0));
				Add(new GenericBuyInfo(typeof(Arrow), 6, 25, 0xF3F, 0));

				Add(new GenericBuyInfo(typeof(BlackPearl), 13, 25, 0xF7A, 0));
				Add(new GenericBuyInfo(typeof(Bloodmoss), 13, 25, 0xF7B, 0));
				Add(new GenericBuyInfo(typeof(Garlic), 10, 25, 0xF84, 0));
				Add(new GenericBuyInfo(typeof(Ginseng), 10, 25, 0xF85, 0));
				Add(new GenericBuyInfo(typeof(MandrakeRoot), 10, 25, 0xF86, 0));
				Add(new GenericBuyInfo(typeof(Nightshade), 10, 25, 0xF88, 0));
				Add(new GenericBuyInfo(typeof(SpidersSilk), 10, 25, 0xF8D, 0));
				Add(new GenericBuyInfo(typeof(SulfurousAsh), 10, 25, 0xF8C, 0));

				Add(new GenericBuyInfo(typeof(TinkerTools), 70, 1, 0x1EB8, 0));
				Add(new GenericBuyInfo(typeof(Shovel), 120, 1, 0xF39, 0));
				Add(new GenericBuyInfo(typeof(Scissors), 55, 10, 0xF9F, 0));
				Add(new GenericBuyInfo(typeof(BivalviaNet), 60, 8, 0x0DD2, 0));

				Add(new GenericBuyInfo(typeof(Bottle), 10, 20, 0xF0E, 0));
				Add(new GenericBuyInfo(typeof(Lockpick), 20, 10, 0x14FC, 0));

				Add(new GenericBuyInfo(typeof(RedBook), 100, 1, 0xFF1, 0));
				Add(new GenericBuyInfo(typeof(BlueBook), 100, 1, 0xFF2, 0));
				Add(new GenericBuyInfo(typeof(TanBook), 100, 1, 0xFF0, 0));

				Add(new GenericBuyInfo(typeof(Key), 10, 5, 0x100E, 0));

				Add(new GenericBuyInfo(typeof(Bedroll), 20, 5, 0xA59, 0));
				Add(new GenericBuyInfo(typeof(Kindling), 10, 5, 0xDE1, 0));

				Add(new GenericBuyInfo(typeof(Backpack), 100, 1, 0x9B2, 0));
				Add(new GenericBuyInfo(typeof(Pouch), 100, 1, 0xE79, 0));
				Add(new GenericBuyInfo(typeof(Bag), 100, 1, 0xE76, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add(typeof(Bandage), 1);

				Add(typeof(BlankScroll), 1);


				Add(typeof(NightSightPotion), 2);
				Add(typeof(AgilityPotion), 2);
				Add(typeof(StrengthPotion), 2);
				Add(typeof(RefreshPotion), 2);
				Add(typeof(LesserCurePotion), 2);
				Add(typeof(LesserHealPotion), 2);
				Add(typeof(LesserPoisonPotion), 2);
				Add(typeof(LesserExplosionPotion), 2);

				Add(typeof(Bolt), 1);
				Add(typeof(Arrow), 1);

				Add(typeof(Log), 1);
				Add(typeof(Board), 1);

				Add(typeof(BlackPearl), 1);
				Add(typeof(Bloodmoss), 1);
				Add(typeof(MandrakeRoot), 1);
				Add(typeof(Garlic), 1);
				Add(typeof(Ginseng), 1);
				Add(typeof(Nightshade), 1);
				Add(typeof(SpidersSilk), 1);
				Add(typeof(SulfurousAsh), 1);

				Add(typeof(BreadLoaf), 1);
				Add(typeof(Backpack), 1);
				Add(typeof(RecallRune), 2);
				Add(typeof(Spellbook), 2);
				Add(typeof(BlankScroll), 1);

				Add(typeof(BreadLoaf), 1);
				Add(typeof(FrenchBread), 1);
				Add(typeof(Cake), 1);
				Add(typeof(Cookies), 1);
				Add(typeof(Muffins), 1);
				Add(typeof(CheesePizza), 1);
				Add(typeof(ApplePie), 1);
				Add(typeof(PeachCobbler), 1);
				Add(typeof(Quiche), 1);
				Add(typeof(Dough), 1);
				Add(typeof(JarHoney), 1);
				Add(typeof(Pitcher), 1);
				Add(typeof(SackFlour), 1);
				Add(typeof(Eggs), 1);

				Add(typeof(LapHarp), 2);
				Add(typeof(Lute), 2);
				Add(typeof(Drums), 2);
				Add(typeof(Harp), 2);
				Add(typeof(Tambourine), 2);

				Add(typeof(BatWing), 1);
				Add(typeof(GraveDust), 1);
				Add(typeof(DaemonBlood), 1);
				Add(typeof(NoxCrystal), 1);
				Add(typeof(PigIron), 1);

				Add(typeof(Tongs), 1);
				Add(typeof(IronIngot), 1);

				Add(typeof(Buckler), 3);
				Add(typeof(BronzeShield), 3);
				Add(typeof(MetalShield), 3);
				Add(typeof(MetalKiteShield), 3);
				Add(typeof(HeaterShield), 3);
				Add(typeof(WoodenKiteShield), 3);

				Add(typeof(WoodenShield), 2);

				Add(typeof(PlateArms), 5);
				Add(typeof(PlateChest), 5);
				Add(typeof(PlateGloves), 5);
				Add(typeof(PlateGorget), 5);
				Add(typeof(PlateLegs), 5);

				Add(typeof(FemalePlateChest), 5);
				Add(typeof(FemaleLeatherChest), 5);
				Add(typeof(FemaleStuddedChest), 3);
				Add(typeof(LeatherShorts), 3);
				Add(typeof(LeatherSkirt), 3);
				Add(typeof(LeatherBustierArms), 3);
				Add(typeof(StuddedBustierArms), 3);

				Add(typeof(Bascinet), 2);
				Add(typeof(CloseHelm), 2);
				Add(typeof(Helmet), 2);
				Add(typeof(NorseHelm), 2);
				Add(typeof(PlateHelm), 2);

				Add(typeof(ChainCoif), 3);
				Add(typeof(ChainChest), 3);
				Add(typeof(ChainLegs), 3);

				Add(typeof(RingmailArms), 2);
				Add(typeof(RingmailChest), 2);
				Add(typeof(RingmailGloves), 2);
				Add(typeof(RingmailLegs), 2);

				Add(typeof(BattleAxe), 2);
				Add(typeof(DoubleAxe), 2);
				Add(typeof(ExecutionersAxe), 2);
				Add(typeof(LargeBattleAxe), 2);
				Add(typeof(Pickaxe), 2);
				Add(typeof(TwoHandedAxe), 2);
				Add(typeof(WarAxe), 2);
				Add(typeof(Axe), 2);

				Add(typeof(Bardiche), 2);
				Add(typeof(Halberd), 2);

				Add(typeof(Cleaver), 1);
				Add(typeof(Dagger), 1);
				Add(typeof(SkinningKnife), 1);

				Add(typeof(Club), 1);
				Add(typeof(HammerPick), 1);
				Add(typeof(Mace), 1);
				Add(typeof(Maul), 1);
				Add(typeof(WarHammer), 1);
				Add(typeof(WarMace), 1);

				Add(typeof(HeavyCrossbow), 3);
				Add(typeof(Bow), 3);
				Add(typeof(Crossbow), 3);

				Add(typeof(CompositeBow), 3);
				Add(typeof(RepeatingCrossbow), 3);
				Add(typeof(Scepter), 2);
				Add(typeof(BladedStaff), 2);
				Add(typeof(Scythe), 2);
				Add(typeof(BoneHarvester), 2);
				Add(typeof(Scepter), 2);
				Add(typeof(BladedStaff), 2);
				Add(typeof(Pike), 2);
				Add(typeof(DoubleBladedStaff), 2);
				Add(typeof(Lance), 2);
				Add(typeof(CrescentBlade), 2);

				Add(typeof(Spear), 2);
				Add(typeof(Pitchfork), 2);
				Add(typeof(WarFork), 2);
				Add(typeof(ShortSpear), 2);

				Add(typeof(BlackStaff), 2);
				Add(typeof(GnarledStaff), 2);
				Add(typeof(QuarterStaff), 2);
				Add(typeof(ShepherdsCrook), 2);

				Add(typeof(SmithHammer), 1);

				Add(typeof(Broadsword), 2);
				Add(typeof(Cutlass), 2);
				Add(typeof(Katana), 2);
				Add(typeof(Kryss), 2);
				Add(typeof(Longsword), 2);
				Add(typeof(Scimitar), 2);
				Add(typeof(ThinLongsword), 2);
				Add(typeof(VikingSword), 2);

				Add(typeof(SewingKit), 1);
				Add(typeof(Dyes), 1);
				Add(typeof(DyeTub), 1);

				Add(typeof(BoltOfCloth), 3);

				Add(typeof(FancyShirt), 1);
				Add(typeof(Shirt), 1);

				Add(typeof(ShortPants), 1);
				Add(typeof(LongPants), 1);

				Add(typeof(Cloak), 1);
				Add(typeof(FancyDress), 1);
				Add(typeof(Robe), 1);
				Add(typeof(PlainDress), 1);

				Add(typeof(Skirt), 1);
				Add(typeof(Kilt), 1);

				Add(typeof(Doublet), 1);
				Add(typeof(Tunic), 1);
				Add(typeof(JesterSuit), 1);

				Add(typeof(FullApron), 1);
				Add(typeof(HalfApron), 1);

				Add(typeof(JesterHat), 1);
				Add(typeof(FloppyHat), 1);
				Add(typeof(WideBrimHat), 1);
				Add(typeof(Cap), 1);
				Add(typeof(SkullCap), 1);
				Add(typeof(Bandana), 1);
				Add(typeof(TallStrawHat), 1);
				Add(typeof(StrawHat), 1);
				Add(typeof(WizardsHat), 1);
				Add(typeof(Bonnet), 1);
				Add(typeof(FeatheredHat), 1);
				Add(typeof(TricorneHat), 1);

				Add(typeof(SpoolOfThread), 1);

				Add(typeof(Flax), 1);
				Add(typeof(Cotton), 1);
				Add(typeof(Wool), 1);

				Add(typeof(LeatherArms), 2);
				Add(typeof(LeatherChest), 2);
				Add(typeof(LeatherGloves), 2);
				Add(typeof(LeatherGorget), 2);
				Add(typeof(LeatherLegs), 2);
				Add(typeof(LeatherCap), 1);

				Add(typeof(StuddedArms), 3);
				Add(typeof(StuddedChest), 3);
				Add(typeof(StuddedGloves), 3);
				Add(typeof(StuddedGorget), 3);
				Add(typeof(StuddedLegs), 3);

				/*
				Type[] types = Loot.RegularScrollTypes;

				for ( int i = 0; i < types.Length; ++i )
					Add( types[i], ((i / 8) + 2) * 5 );
				*/

				Add(typeof(RawRibs), 1);
				Add(typeof(RawLambLeg), 1);
				Add(typeof(RawChickenLeg), 1);
				Add(typeof(RawBird), 1);
				Add(typeof(Bacon), 1);
				Add(typeof(Sausage), 1);
				Add(typeof(Ham), 1);

				Add(typeof(Amber), 10);
				Add(typeof(Amethyst), 10);
				Add(typeof(Citrine), 10);
				Add(typeof(Diamond), 20);
				Add(typeof(Emerald), 15);
				Add(typeof(Ruby), 15);
				Add(typeof(Sapphire), 15);
				Add(typeof(StarSapphire), 20);
				Add(typeof(Tourmaline), 15);
				Add(typeof(GoldRing), 5);
				Add(typeof(SilverRing), 5);
				Add(typeof(Necklace), 5);
				Add(typeof(GoldNecklace), 5);
				Add(typeof(GoldBeadNecklace), 5);
				Add(typeof(SilverNecklace), 5);
				Add(typeof(SilverBeadNecklace), 5);
				Add(typeof(Beads), 5);
				Add(typeof(GoldBracelet), 5);
				Add(typeof(SilverBracelet), 5);
				Add(typeof(GoldEarrings), 5);
				Add(typeof(SilverEarrings), 5);


				Add(typeof(WoodenBox), 1);
				Add(typeof(SmallCrate), 1);
				Add(typeof(MediumCrate), 1);
				Add(typeof(LargeCrate), 1);
				Add(typeof(WoodenChest), 2);

				Add(typeof(LargeTable), 2);
				Add(typeof(Nightstand), 1);
				Add(typeof(YewWoodTable), 1);

				Add(typeof(Throne), 2);
				Add(typeof(WoodenThrone), 1);
				Add(typeof(Stool), 1);
				Add(typeof(FootStool), 1);

				Add(typeof(FancyWoodenChairCushion), 1);
				Add(typeof(WoodenChairCushion), 1);
				Add(typeof(WoodenChair), 1);
				Add(typeof(BambooChair), 1);
				Add(typeof(WoodenBench), 1);

				Add(typeof(Saw), 1);
				Add(typeof(Scorp), 1);
				Add(typeof(SmoothingPlane), 1);
				Add(typeof(DrawKnife), 1);
				Add(typeof(Froe), 1);
				Add(typeof(Hammer), 1);
				Add(typeof(Inshave), 1);
				Add(typeof(JointingPlane), 1);
				Add(typeof(MouldingPlane), 1);
				Add(typeof(DovetailSaw), 1);
				Add(typeof(BallotBoxDeed), 1);

				Add(typeof(Shovel), 1);
				Add(typeof(SewingKit), 1);

				Add(typeof(Nails), 1);

				Add(typeof(ClockParts), 1);
				Add(typeof(AxleGears), 1);
				Add(typeof(Hinge), 1);
				Add(typeof(Sextant), 2);
				Add(typeof(SextantParts), 1);
				Add(typeof(Springs), 1);

				Add(typeof(Lockpick), 1);
				Add(typeof(TinkerTools), 1);

				Add(typeof(ButcherKnife), 1);
			}
		}
	}
}
