using Server.Engines.Craft;
using Server.Engines.PartySystem;
using Server.Mobiles;
using Server.SkillHandlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Items
{
	public enum TreasureLevel
	{
		Stash,
		Supply,
		Cache,
		Hoard,
		Trove
	}

	public enum TreasurePackage
	{
		Artisan,
		Assassin,
		Mage,
		Ranger,
		Warrior
	}

	public enum TreasureFacet
	{
		Trammel,
		Felucca,
		Ilshenar,
		Malas,
		Tokuno,
		TerMur,
		Eodon
	}

	public enum ChestQuality
	{
		None,
		Rusty,
		Standard,
		Gold
	}

	public static class TreasureMapInfo
	{
		/// <summary>
		/// This is called from BaseCreature. Instead of editing EVERY creature that drops a map, we'll simply convert it here.
		/// </summary>
		/// <param name="level"></param>
		public static int ConvertLevel(int level)
		{
			if (level == -1)
				return level;

			switch (level)
			{
				default: return (int)TreasureLevel.Stash;
				case 2:
				case 3: return (int)TreasureLevel.Supply;
				case 4:
				case 5: return (int)TreasureLevel.Cache;
				case 6: return (int)TreasureLevel.Hoard;
				case 7: return (int)TreasureLevel.Trove;
			}
		}

		public static int PackageLocalization(TreasurePackage package)
		{
			switch (package)
			{
				case TreasurePackage.Artisan: return 1158989;
				case TreasurePackage.Assassin: return 1158987;
				case TreasurePackage.Mage: return 1158986;
				case TreasurePackage.Ranger: return 1158990;
				case TreasurePackage.Warrior: return 1158988;
			}

			return 0;
		}

		public static TreasureFacet GetFacet(IPoint2D p, Map map)
		{
			return TreasureFacet.Felucca;
		}

		public static IEnumerable<Type> GetRandomEquipment(TreasureLevel level, TreasurePackage package, int amount)
		{
			Type[] weapons = GetWeaponList(level, package);
			Type[] armor = GetArmorList(level, package);
			Type[] jewels = GetJewelList(level, package);
			Type[] list;

			for (int i = 0; i < amount; i++)
			{
				switch (Utility.Random(5))
				{
					default:
					case 0: list = weapons; break;
					case 1:
					case 2: list = armor; break;
					case 3:
					case 4: list = jewels; break;
				}

				yield return list[Utility.Random(list.Length)];
			}
		}

		public static Type[] GetWeaponList(TreasureLevel level, TreasurePackage package)
		{
			return _WeaponTable[(int)package];
		}

		public static Type[] GetArmorList(TreasureLevel level, TreasurePackage package)
		{
			return _ArmorTable[(int)package];
		}

		public static Type[] GetJewelList(TreasureLevel level, TreasurePackage package)
		{
			return _JewelTable;
		}

		public static SkillName[] GetTranscendenceList(TreasureLevel level, TreasurePackage package)
		{
			if (level == TreasureLevel.Supply || level == TreasureLevel.Cache)
			{
				return null;
			}

			return _TranscendenceTable[(int)package];
		}

		/* public static SkillName[] GetAlacrityList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
		 {
		     if (level == TreasureLevel.Stash || (facet == TreasureFacet.Felucca && level == TreasureLevel.Cache))
		     {
		         return null;
		     }

		     return _AlacrityTable[(int)package];
		 }

		 public static SkillName[] GetPowerScrollList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
		 {
		     if (facet != TreasureFacet.Felucca)
		         return null;

		     if (level >= TreasureLevel.Cache)
		     {
		         return _PowerscrollTable[(int)package];
		     }

		     return null;
		 }*/

		public static Type[] GetCraftingMaterials(TreasureLevel level, TreasurePackage package, ChestQuality quality)
		{
			if (package == TreasurePackage.Artisan && level <= TreasureLevel.Supply && quality != ChestQuality.None)
			{
				return _MaterialTable[(int)quality - 1];
			}

			return null;
		}

		public static Type[] GetSpecialMaterials(TreasureLevel level, TreasurePackage package)
		{
			if (package == TreasurePackage.Artisan && level == TreasureLevel.Supply)
			{
				return _SpecialMaterialTable;
			}

			return null;
		}

		public static Type[] GetDecorativeList(TreasureLevel level, TreasurePackage package)
		{
			Type[] list = null;

			if (level >= TreasureLevel.Cache)
			{
				list = _DecorativeTable[(int)package];
			}
			else if (level == TreasureLevel.Supply)
			{
				list = _DecorativeMinorArtifacts;
			}

			return list;
		}

		public static Type[] GetReagentList(TreasureLevel level, TreasurePackage package)
		{
			if (level != TreasureLevel.Stash || package != TreasurePackage.Mage)
				return null;

			return Utility.Random(3) switch
			{
				1 => Loot.NecroRegTypes,
				2 => Loot.MysticRegTypes,
				_ => Loot.RegTypes
			};
		}

		public static Recipe[] GetRecipeList(TreasureLevel level, TreasurePackage package)
		{
			if (package == TreasurePackage.Artisan && level == TreasureLevel.Supply)
			{
				return Recipe.Recipes.Values.ToArray();
			}

			return null;
		}

		public static Type[] GetSpecialLootList(TreasureLevel level, TreasurePackage package)
		{
			if (level == TreasureLevel.Stash)
				return null;

			Type[] list;

			if (level == TreasureLevel.Supply)
			{
				list = _SpecialSupplyLoot;
			}
			else
			{
				list = _SpecialCacheHordeAndTrove;
			}

			if (package > TreasurePackage.Artisan)
			{
				list = list.Concat(ArtifactHelper.ArtifactsCurrentSeason(ArtGroup.Cartography)).ToArray();
			}

			return list;
		}

		public static int GetGemCount(ChestQuality quality, TreasureLevel level)
		{
			int baseAmount = 0;

			switch (quality)
			{
				case ChestQuality.Rusty: baseAmount = 1; break; //bylo 7
				case ChestQuality.Standard: baseAmount = Utility.RandomBool() ? 1 : 2; break; // bylo 7 : 9
				case ChestQuality.Gold: baseAmount = Utility.RandomList(1, 2, 3); break; //bylo (7, 9, 11)
			}

			return baseAmount + ((int)level * 1); //bylo 5
		}

		public static int GetGoldCount(TreasureLevel level)
		{
			switch (level)
			{
				default:
				case TreasureLevel.Stash: return Utility.RandomMinMax(1000, 2000); //było (1000, 4000);
				case TreasureLevel.Supply: return Utility.RandomMinMax(2000, 3000); // było (2000, 5000);
				case TreasureLevel.Cache: return Utility.RandomMinMax(3000, 4000); //było (3000, 6000)
				case TreasureLevel.Hoard: return Utility.RandomMinMax(4000, 5000); // było (4000, 7000)
				case TreasureLevel.Trove: return Utility.RandomMinMax(5000, 6000); // było (5000, 7000)
			}
		}

		public static int GetRefinementRolls(ChestQuality quality)
		{
			switch (quality)
			{
				default:
				case ChestQuality.Rusty: return 2;
				case ChestQuality.Standard: return 4;
				case ChestQuality.Gold: return 6;
			}
		}

		public static int GetResourceAmount(TreasureLevel level)
		{
			switch (level)
			{
				case TreasureLevel.Stash: return 25; //bylo 50
				case TreasureLevel.Supply: return 50; //bylo 100
			}

			return 0;
		}

		public static int GetRegAmount(ChestQuality quality)
		{
			switch (quality)
			{
				default:
				case ChestQuality.Rusty: return 5; //bylo 20
				case ChestQuality.Standard: return 10; //bylo 40
				case ChestQuality.Gold: return 20; //bylo 60
			}
		}

		public static int GetSpecialResourceAmount(ChestQuality quality)
		{
			switch (quality)
			{
				default:
				case ChestQuality.Rusty: return 1;
				case ChestQuality.Standard: return 2;
				case ChestQuality.Gold: return 3;
			}
		}

		public static int GetEquipmentAmount(Mobile from, TreasureLevel level, TreasurePackage package)
		{
			int amount = 0;

			switch (level)
			{
				default:
				case TreasureLevel.Stash: amount = 6; break;
				case TreasureLevel.Supply: amount = 8; break;
				case TreasureLevel.Cache: amount = package == TreasurePackage.Assassin ? 24 : 12; break;
				case TreasureLevel.Hoard: amount = 18; break;
				case TreasureLevel.Trove: amount = 36; break;
			}

			Party p = Party.Get(from);

			if (p != null && p.Count > 1)
			{
				for (int i = 0; i < p.Count - 1; i++)
				{
					if (Utility.RandomBool())
					{
						amount++;
					}
				}
			}

			return amount;
		}

		public static void GetMinMaxBudget(TreasureLevel level, Item item, out int min, out int max)
		{
			int preArtifact = Imbuing.GetMaxWeight(item) + 100;
			min = max = 0;

			switch (level)
			{
				default:
				case TreasureLevel.Stash:
				case TreasureLevel.Supply:
					min = 250;
					max = preArtifact;
					break;
				case TreasureLevel.Cache:
				case TreasureLevel.Hoard:
				case TreasureLevel.Trove:
					min = 500;
					max = 1300;
					break;
			}
		}

		private static readonly Type[][] _WeaponTable =
		[
			[
				typeof(HammerPick), typeof(SledgeHammerWeapon), typeof(SmithyHammer), typeof(WarAxe), typeof(WarHammer),
				typeof(Axe), typeof(BattleAxe), typeof(DoubleAxe), typeof(ExecutionersAxe), typeof(Hatchet),
				typeof(LargeBattleAxe), typeof(OrnateAxe), typeof(TwoHandedAxe), typeof(Pickaxe)
			],
			[
				typeof(Dagger), typeof(Kryss), typeof(Cleaver), typeof(Cutlass), typeof(ElvenMachete)
			],
			[
				typeof(BlackStaff), typeof(ShepherdsCrook), typeof(GnarledStaff), typeof(QuarterStaff)
			],
			[
				typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow), typeof(CompositeBow), typeof(ButcherKnife),
				typeof(SkinningKnife)
			],
			[
				typeof(Lance), typeof(Pike), typeof(Pitchfork), typeof(ShortSpear), typeof(WarFork), typeof(Club),
				typeof(Mace), typeof(Maul), typeof(WarAxe), typeof(Bardiche), typeof(Broadsword), typeof(CrescentBlade),
				typeof(Halberd), typeof(Longsword), typeof(Scimitar), typeof(VikingSword)
			]
		];

		private static readonly Type[][] _ArmorTable =
		[
			[
				typeof(Bonnet), typeof(Cap), typeof(Circlet), typeof(FeatheredHat), typeof(FlowerGarland),
				typeof(JesterHat), typeof(SkullCap), typeof(StrawHat), typeof(TallStrawHat), typeof(WideBrimHat)
			],
			[
				typeof(ChainLegs), typeof(ChainCoif), typeof(ChainChest), typeof(RingmailLegs), typeof(RingmailGloves),
				typeof(RingmailChest), typeof(RingmailArms), typeof(Bandana)
			],
			[
				typeof(LeafGloves), typeof(LeafLegs), typeof(LeafTonlet), typeof(LeafGorget), typeof(LeafArms),
				typeof(LeafChest), typeof(LeatherArms), typeof(LeatherChest), typeof(LeatherLegs),
				typeof(LeatherGloves), typeof(LeatherGorget), typeof(WizardsHat)
			],
			[
				typeof(HidePants), typeof(HidePauldrons), typeof(HideGorget), typeof(HideFemaleChest),
				typeof(HideChest), typeof(HideGloves), typeof(StuddedLegs), typeof(StuddedGorget),
				typeof(StuddedGloves), typeof(StuddedChest), typeof(StuddedBustierArms), typeof(StuddedArms),
				typeof(RavenHelm), typeof(VultureHelm), typeof(WingedHelm)
			],
			[
				typeof(PlateLegs), typeof(PlateHelm), typeof(PlateGorget), typeof(PlateGloves), typeof(PlateChest),
				typeof(PlateArms), typeof(Bascinet), typeof(CloseHelm), typeof(Helmet), typeof(LeatherCap),
				typeof(NorseHelm), typeof(TricorneHat), typeof(BronzeShield), typeof(Buckler), typeof(ChaosShield),
				typeof(HeaterShield), typeof(MetalKiteShield), typeof(MetalShield), typeof(OrderShield),
				typeof(WoodenKiteShield)
			]
		];

		public static Type[][] _MaterialTable =
		[
			[
				typeof(SpinedLeather), typeof(OakBoard), typeof(AshBoard), typeof(DullCopperIngot),
				typeof(ShadowIronIngot), typeof(CopperIngot)
			],
			[
				typeof(HornedLeather), typeof(YewBoard), typeof(HeartwoodBoard), typeof(BronzeIngot), typeof(GoldIngot),
				typeof(AgapiteIngot)
			],
			[
				typeof(BarbedLeather), typeof(BloodwoodBoard), typeof(FrostwoodBoard), typeof(ValoriteIngot),
				typeof(VeriteIngot)
			]
		];

		public static Type[] _JewelTable =
		[
			typeof(GoldRing), typeof(GoldBracelet), typeof(SilverRing), typeof(SilverBracelet)
		];

		public static Type[][] _DecorativeTable =
		[
			[typeof(carpet11mDeed)], //bylo SkullTiledFloorAddonDee
			[typeof(LuminescentFungi)], //bylo AncientWeapon3
			[typeof(DecorativeHourglass)],
			[typeof(BarkFragment), typeof(CreepingVine)], //bylo AncientWeapon1
			[typeof(Corruption)], //bylo typeof(AncientWeapon2)
			[typeof(CoffinPiece)]
		];

		public static Type[] _SpecialMaterialTable =
		[
			typeof(LuminescentFungi), typeof(BarkFragment), typeof(Blight), typeof(Corruption), typeof(Muculent),
			typeof(Putrefaction), typeof(Scourge), typeof(Taint)
		];

		public static Type[] _SpecialSupplyLoot =
			[
				typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding),
				typeof(ShieldEngravingTool), null
			]
			//new Type[] { typeof(ForgedPardon), typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding), typeof(Skeletonkey), typeof(MasterSkeletonKey), typeof(SurgeShield) },
			// new Type[] { typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding) },
			// new Type[] { typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding), typeof(TastyTreat) },
			//new Type[] { typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding) },
			;

		public static Type[] _SpecialCacheHordeAndTrove =
		[
			typeof(OctopusNecklace) /*, typeof(SkullGnarledStaff), typeof(SkullLongsword)*/
		];

		public static Type[] _DecorativeMinorArtifacts =
		[
			typeof(CandelabraOfSouls), typeof(GoldBricks), typeof(PhillipsWoodenSteed),
			typeof(AncientShipModelOfTheHMSCape), typeof(AdmiralsHeartyRum)
		];

		public static SkillName[][] _TranscendenceTable =
		[
			[
				SkillName.ArmsLore, SkillName.Blacksmith, SkillName.Carpentry, SkillName.Cartography, SkillName.Cooking,
				SkillName.Cooking, SkillName.Fletching, SkillName.Mining, SkillName.Tailoring
			],
			[
				SkillName.Anatomy, SkillName.DetectHidden, SkillName.Fencing, SkillName.Poisoning, SkillName.RemoveTrap,
				SkillName.Snooping, SkillName.Stealth
			],
			[SkillName.Magery, SkillName.Meditation, SkillName.MagicResist, SkillName.Spellweaving],
			[SkillName.Alchemy, SkillName.AnimalLore, SkillName.AnimalTaming, SkillName.Archery],
			[
				SkillName.Chivalry, SkillName.Focus, SkillName.Parry, SkillName.Swords, SkillName.Tactics,
				SkillName.Wrestling
			]
		];

		public static SkillName[][] _AlacrityTable =
		[
			[
				SkillName.ArmsLore, SkillName.Blacksmith, SkillName.Carpentry, SkillName.Cartography, SkillName.Cooking,
				SkillName.Cooking, SkillName.Fletching, SkillName.Mining, SkillName.Tailoring, SkillName.Lumberjacking
			],
			[
				SkillName.DetectHidden, SkillName.Fencing, SkillName.Hiding, SkillName.Lockpicking, SkillName.Poisoning,
				SkillName.RemoveTrap, SkillName.Snooping, SkillName.Stealing, SkillName.Stealth
			],
			[
				SkillName.Alchemy, SkillName.EvalInt, SkillName.Inscribe, SkillName.Magery, SkillName.Meditation,
				SkillName.Spellweaving, SkillName.SpiritSpeak
			],
			[
				SkillName.AnimalLore, SkillName.AnimalTaming, SkillName.Archery, SkillName.Musicianship,
				SkillName.Peacemaking, SkillName.Provocation, SkillName.Tinkering, SkillName.Tracking,
				SkillName.Veterinary
			],
			[
				SkillName.Chivalry, SkillName.Focus, SkillName.Macing, SkillName.Parry, SkillName.Swords,
				SkillName.Wrestling
			]
		];

		public static SkillName[][] _PowerscrollTable =
		[
			null,
			[SkillName.Ninjitsu],
			[
				SkillName.Magery, SkillName.Meditation, SkillName.Mysticism, SkillName.Spellweaving,
				SkillName.SpiritSpeak
			],
			[SkillName.AnimalTaming, SkillName.Discordance, SkillName.Provocation, SkillName.Veterinary],
			[
				SkillName.Bushido, SkillName.Chivalry, SkillName.Focus, SkillName.Healing, SkillName.Parry,
				SkillName.Swords, SkillName.Tactics
			]
		];

		public static void Fill(Mobile from, TreasureMapChest chest, TreasureMap tMap)
		{
			TreasureLevel level = tMap.TreasureLevel;
			TreasurePackage package = tMap.Package;
			ChestQuality quality = chest.ChestQuality;

			chest.Movable = false;
			chest.Locked = true;

			chest.TrapType = TrapType.ExplosionTrap;

			switch ((int)level)
			{
				default:
				case 0:
					chest.RequiredSkill = 5;
					chest.TrapPower = 25;
					chest.TrapLevel = 1;
					break;
				case 1:
					chest.RequiredSkill = 45;
					chest.TrapPower = 75;
					chest.TrapLevel = 3;
					break;
				case 2:
					chest.RequiredSkill = 75;
					chest.TrapPower = 125;
					chest.TrapLevel = 5;
					break;
				case 3:
					chest.RequiredSkill = 80;
					chest.TrapPower = 150;
					chest.TrapLevel = 6;
					break;
				case 4:
					chest.RequiredSkill = 80;
					chest.TrapPower = 170;
					chest.TrapLevel = 7;
					break;
			}

			chest.LockLevel = chest.RequiredSkill - 10;
			chest.MaxLockLevel = chest.RequiredSkill + 40;

			if (Engines.JollyRoger.JollyRogerEvent.Instance.Running && 0.10 > Utility.RandomDouble())
			{
				chest.DropItem(new MysteriousFragment());
			}

			#region Refinements

			if (level == TreasureLevel.Stash)
			{
				RefinementComponent.Roll(chest, GetRefinementRolls(quality), 0.9);
			}

			#endregion

			#region TMaps

			bool dropMap = false;
			if (level < TreasureLevel.Trove && 0.1 > Utility.RandomDouble())
			{
				chest.DropItem(new TreasureMap(tMap.Level + 1, chest.Map));
				dropMap = true;
			}

			#endregion

			Type[] list = null;
			int amount = 0;
			double dropChance = 0.0;

			#region Gold

			int goldAmount = GetGoldCount(level);
			Bag lootBag = new BagOfGold();

			while (goldAmount > 0)
			{
				if (goldAmount <= 20000)
				{
					lootBag.DropItem(new Gold(goldAmount));
					goldAmount = 0;
				}
				else
				{
					lootBag.DropItem(new Gold(20000));
					goldAmount -= 20000;
				}

				chest.DropItem(lootBag);
			}

			#endregion

			#region Regs

			list = GetReagentList(level, package);

			if (list != null)
			{
				amount = GetRegAmount(quality);
				lootBag = new BagOfRegs();

				for (int i = 0; i < amount; i++)
				{
					lootBag.DropItemStacked(Loot.Construct(list));
				}

				chest.DropItem(lootBag);
				list = null;
			}

			#endregion

			#region Gems

			amount = GetGemCount(quality, level);

			if (amount > 0)
			{
				lootBag = new BagOfGems();

				foreach (Type gemType in Loot.GemTypes)
				{
					Item gem = Loot.Construct(gemType);
					gem.Amount = amount;

					lootBag.DropItem(gem);
				}

				chest.DropItem(lootBag);
			}

			#endregion

			#region Crafting Resources

			// TODO: DO each drop, or do only 1 drop?
			list = GetCraftingMaterials(level, package, quality);

			if (list != null)
			{
				amount = GetResourceAmount(level);

				foreach (Type type in list)
				{
					Item craft = Loot.Construct(type);
					craft.Amount = amount;

					chest.DropItem(craft);
				}

				list = null;
			}

			#endregion

			#region Special Resources

			// TODO: DO each drop, or do only 1 drop?
			list = GetSpecialMaterials(level, package);

			if (list != null)
			{
				amount = GetSpecialResourceAmount(quality);

				foreach (Type type in list)
				{
					Item specialCraft = Loot.Construct(type);
					specialCraft.Amount = amount;

					chest.DropItem(specialCraft);
				}

				list = null;
			}

			#endregion

			#region Special Scrolls

			amount = (int)level + 1;

			if (dropMap)
			{
				amount--;
			}

			if (amount > 0)
			{
				SkillName[] transList = GetTranscendenceList(level, package);
				//SkillName[] alacList = GetAlacrityList(level, package, facet);
				// SkillName[] pscrollList = GetPowerScrollList(level, package, facet);

				List<Tuple<int, SkillName>> scrollList = new List<Tuple<int, SkillName>>();

				if (transList != null)
				{
					foreach (SkillName sk in transList)
					{
						scrollList.Add(new Tuple<int, SkillName>(1, sk));
					}
				}

				/*   if (alacList != null)
				   {
				       foreach (SkillName sk in alacList)
				       {
				           scrollList.Add(new Tuple<int, SkillName>(2, sk));
				       }
				   }

				   if (pscrollList != null)
				   {
				       foreach (SkillName sk in pscrollList)
				       {
				           scrollList.Add(new Tuple<int, SkillName>(3, sk));
				       }
				   }*/

				if (scrollList.Count > 0)
				{
					for (int i = 0; i < amount; i++)
					{
						Tuple<int, SkillName> random = scrollList[Utility.Random(scrollList.Count)];

						switch (random.Item1)
						{
							case 1:
								chest.DropItem(new ScrollOfTranscendence(random.Item2,
									Utility.RandomMinMax(1.0, chest.Map == Map.Felucca ? 1.0 : 3.0) / 10));
								break; //bylo 7-10
							// case 2: chest.DropItem(new ScrollOfAlacrity(random.Item2)); break;
							// case 2: chest.DropItem(new PowerScroll(random.Item2, 110.0)); break;
						}
					}
				}
			}

			#endregion

			#region Decorations

			switch (level)
			{
				case TreasureLevel.Stash: dropChance = 0.00; break;
				case TreasureLevel.Supply: dropChance = 0.10; break;
				case TreasureLevel.Cache: dropChance = 0.20; break;
				case TreasureLevel.Hoard: dropChance = 0.40; break;
				case TreasureLevel.Trove: dropChance = 0.50; break;
			}

			if (Utility.RandomDouble() < dropChance)
			{
				list = GetDecorativeList(level, package);

				if (list != null)
				{
					if (list.Length > 0)
					{
						Item deco = Loot.Construct(list[Utility.Random(list.Length)]);

						if (_DecorativeMinorArtifacts.Any(t => t == deco.GetType()))
						{
							Container pack = new Backpack { Hue = 1278 };

							pack.DropItem(deco);
							chest.DropItem(pack);
						}
						else
						{
							chest.DropItem(deco);
						}
					}

					list = null;
				}
			}

			switch (level)
			{
				case TreasureLevel.Stash: dropChance = 0.00; break;
				case TreasureLevel.Supply: dropChance = 0.10; break;
				case TreasureLevel.Cache: dropChance = 0.20; break;
				case TreasureLevel.Hoard: dropChance = 0.50; break;
				case TreasureLevel.Trove: dropChance = 0.75; break;
			}

			if (Utility.RandomDouble() < dropChance)
			{
				list = GetSpecialLootList(level, package);

				if (list != null)
				{
					if (list.Length > 0)
					{
						Type type = list[Utility.Random(list.Length)];
						Item deco;

						if (type == null)
						{
							deco = GetRandomRecipe();
						}
						else
						{
							deco = Loot.Construct(type);
						}

						if (deco is SkullGnarledStaff || deco is SkullLongsword)
						{
							if (package == TreasurePackage.Artisan)
							{
								((IQuality)deco).Quality = ItemQuality.Exceptional;
							}
							else
							{
								int min, max;
								GetMinMaxBudget(level, deco, out min, out max);
								RunicReforging.GenerateRandomItem(deco,
									from is PlayerMobile ? ((PlayerMobile)from).RealLuck : from.Luck,
									min,
									max,
									chest.Map);
							}
						}

						chest.DropItem(deco);
					}

					list = null;
				}
			}

			#endregion

			#region Magic Equipment

			amount = GetEquipmentAmount(from, level, package);

			foreach (Type type in GetRandomEquipment(level, package, amount))
			{
				Item item = Loot.Construct(type);
				int min, max;
				GetMinMaxBudget(level, item, out min, out max);

				if (item != null)
				{
					RunicReforging.GenerateRandomItem(item,
						from is PlayerMobile ? ((PlayerMobile)from).RealLuck : from.Luck,
						min,
						max,
						chest.Map);
					chest.DropItem(item);
				}
			}

			list = null;

			#endregion
		}
		
		
		public static Item GetRandomRecipe()
		{
			var recipes = new List<Recipe>(Recipe.Recipes.Values);

			return new RecipeScroll(recipes[Utility.Random(recipes.Count)]);
		}
	}
}
