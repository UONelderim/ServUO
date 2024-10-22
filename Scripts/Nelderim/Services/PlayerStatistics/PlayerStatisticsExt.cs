using System;
using System.Collections.Generic;
using Nelderim;
using Server.Commands;
using Server.Engines.BulkOrders;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public partial class PlayerMobile
	{
		public PlayerStatisticsInfo Statistics
		{
			get => PlayerStatistics.Get(this);
		}
	}

	public class PlayerStatistics : NExtension<PlayerStatisticsInfo>
	{
		public static string ModuleName = "PlayerStatistics";

		public static void Initialize()
		{
			EventSink.WorldSave += Save;
			RegisterMetrics();
			CommandSystem.Register("printStats", AccessLevel.Player, e =>
			{
				e.Mobile.BeginTarget(12,
					false,
					TargetFlags.None,
					(from, targeted) =>
					{
						if (targeted is PlayerMobile pm)
							pm.Statistics.Print();
					});
			});
			Load(ModuleName);
		}

		private static void RegisterMetrics()
		{
			EventSink.CreatureDeath += e =>
			{
				if (e.Killer is PlayerMobile pm && e.Creature is BaseCreature bc)
				{
					pm.Statistics.CreaturesKilled.Increment(bc.GetType());
					if (bc.IsParagon)
						pm.Statistics.ParagonsKilled.Increment(bc.GetType());
				}
			};
			EventSink.PlayerDeath += e =>
			{
				if (e.Mobile is PlayerMobile killed && e.Killer is PlayerMobile killer)
				{
					//Implement death tracker
					killer.Statistics.PlayerKillsFaction.Increment(killed.Faction.GetType());
					killer.Statistics.PlayerKillsRace.Increment(killed.Race.GetType());
				}
			};
			EventSink.SpellCast += e =>
			{
				if (e.Caster is PlayerMobile pm)
				{
					pm.Statistics.SpellsCast.Increment(e.Spell.GetType());
				}
			};
			EventSink.CraftSuccess += e =>
			{
				if (e.Crafter is PlayerMobile pm)
				{
					pm.Statistics.ItemsCrafted.Increment(e.CraftedItem.GetType());
				}
			};
			EventSink.EnhanceSuccess += e =>
			{
				if (e.Crafter is PlayerMobile pm)
				{
					pm.Statistics.ItemsEnhanced.Increment(e.EnhancedItem.GetType());
				}
			};
			EventSink.RepairItem += e =>
			{
				if (e.Mobile is PlayerMobile pm)
				{
					pm.Statistics.ItemsRepaired.Increment(e.Repaired.GetType());
				}
			};
			EventSink.BODCompleted += e =>
			{
				if (e.User is PlayerMobile pm)
				{
					if (e.BOD is IBOD)
					{
						pm.Statistics.BulkOrderDeedsCompleted.Increment(e.BOD.GetType());
					}
				}
			};
			EventSink.ResourceHarvestSuccess += e =>
			{
				//Ore,Wood,Fish,Clams
				if (e.Harvester is PlayerMobile pm)
				{
					pm.Statistics.ResourceHarvested.Increment(e.Resource.GetType(), e.Resource.Amount);
					if (e.BonusResource != null)
					{
						pm.Statistics.ResourceHarvested.Increment(e.BonusResource.GetType(), e.Resource.Amount);
					}
				}
			};
			EventSink.CreatureCarved += e =>
			{
				if (e.Carver is PlayerMobile pm)
				{
					pm.Statistics.CreaturesCarved.Increment(e.Creature.GetType());
				}
			};
			EventSink.OnConsume += e =>
			{
				if (e.Consumer is PlayerMobile pm)
				{
					pm.Statistics.FoodConsumed.Increment(e.Consumed.GetType(), e.Quantity);
				}
			};
			EventSink.CorpseLoot += e =>
			{
				if (e.Mobile is PlayerMobile pm)
				{
					if (e.Looted is Gold)
					{
						pm.Statistics.GoldLooted += e.Looted.Amount;
					}

					if (e.Looted is ParagonChest)
					{
						pm.Statistics.ParagonChestsLooted++;
					}
				}
			};
			EventSink.Movement += e =>
			{
				if (e.Mobile is PlayerMobile pm)
				{
					pm.Statistics.StepsTakenTotal++;
				}
			};
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Save(args, ModuleName);
		}
	}

	public class PlayerStatisticsInfo : NExtensionInfo
	{
		//Kills
		public Dictionary<Type, long> CreaturesKilled = new();
		public Dictionary<Type, long> ParagonsKilled = new();
		public Dictionary<Type, long> PlayerKillsFaction = new();
		public Dictionary<Type, long> PlayerKillsRace = new();

		//Magery
		public Dictionary<Type, long> SpellsCast = new();
		public Dictionary<Type, long> CreaturesSummoned = new();

		//Craft
		public Dictionary<Type, long> ItemsCrafted = new();
		public Dictionary<Type, long> ItemsEnhanced = new();
		public Dictionary<Type, long> ItemsRepaired = new();

		public Dictionary<Type, long> BulkOrderDeedsCompleted = new();

		//Harvest
		public Dictionary<Type, long> ResourceHarvested = new();
		public Dictionary<Type, long> CreaturesCarved = new();
		public long GravesDigged;

		public Dictionary<Type, long> AnimalsTamed = new();
		public Dictionary<Type, long> AnimalsBonded = new();
		public Dictionary<Type, long> NecromancySummonsCrafted = new();

		//Items
		public Dictionary<Type, long> PotionsUsed = new();
		public long BolasThrown;
		public Dictionary<Type, long> FoodConsumed = new();
		public Dictionary<Type, long> TobaccoSmoked = new();
		public HashSet<SkillName> SkillsMastered = new();
		public long BandagesUsed;

		//Other
		public long DungeonTreasureChestsOpened;
		public long TreasureMapChestsOpened;
		public long SOSChestsFound;
		public long ParagonChestsLooted;
		public long GoldLooted;
		public long StepsTakenTotal;
		public long DamageDealtTotal;
		public long DamageTakenTotal;

		public void Print()
		{
			Console.WriteLine("Creatures Killed:");
			CreaturesKilled.Print();
			ParagonsKilled.Print();
			PlayerKillsFaction.Print();
			PlayerKillsRace.Print();
			SpellsCast.Print();
			CreaturesSummoned.Print();
			ItemsCrafted.Print();
			ItemsEnhanced.Print();
			ItemsRepaired.Print();
			BulkOrderDeedsCompleted.Print();
			ResourceHarvested.Print();
			CreaturesCarved.Print();
			Console.WriteLine($"Graves Digged: {GravesDigged}");
			AnimalsTamed.Print();
			AnimalsBonded.Print();
			NecromancySummonsCrafted.Print();
			PotionsUsed.Print();
			Console.WriteLine($"Bolas Thrown: {BolasThrown}");
			FoodConsumed.Print();
			TobaccoSmoked.Print();
			Console.WriteLine("Skills Mastered:");
			foreach (var skill in SkillsMastered)
			{
				Console.WriteLine(skill);
			}
			Console.WriteLine($"Bandages Used: {BandagesUsed}");
			Console.WriteLine($"Dungeon Treasure Chests Opened: {DungeonTreasureChestsOpened}");
			Console.WriteLine($"Treasure Map Chests Opened: {TreasureMapChestsOpened}");
			Console.WriteLine($"SOS Chests Found: {SOSChestsFound}");
			Console.WriteLine($"Paragon Chests Looted: {ParagonChestsLooted}");
			Console.WriteLine($"Gold Looted: {GoldLooted}");
			Console.WriteLine($"Steps Taken Total: {StepsTakenTotal}");
			Console.WriteLine($"Damage Dealt Total: {DamageDealtTotal}");
			Console.WriteLine($"Damage Taken Total: {DamageTakenTotal}");
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write((int)0); //version
			writer.WriteTypeLongDict(CreaturesKilled);
			writer.WriteTypeLongDict(ParagonsKilled);
			writer.WriteTypeLongDict(PlayerKillsFaction);
			writer.WriteTypeLongDict(PlayerKillsRace);
			writer.WriteTypeLongDict(SpellsCast);
			writer.WriteTypeLongDict(CreaturesSummoned);
			writer.WriteTypeLongDict(ItemsCrafted);
			writer.WriteTypeLongDict(ItemsEnhanced);
			writer.WriteTypeLongDict(ItemsRepaired);
			writer.WriteTypeLongDict(BulkOrderDeedsCompleted);
			writer.WriteTypeLongDict(ResourceHarvested);
			writer.WriteTypeLongDict(CreaturesCarved);
			writer.Write(GravesDigged);
			writer.WriteTypeLongDict(AnimalsTamed);
			writer.WriteTypeLongDict(AnimalsBonded);
			writer.WriteTypeLongDict(NecromancySummonsCrafted);
			writer.WriteTypeLongDict(PotionsUsed);
			writer.Write(BolasThrown);
			writer.WriteTypeLongDict(FoodConsumed);
			writer.WriteTypeLongDict(TobaccoSmoked);
			writer.Write(SkillsMastered.Count);
			foreach (var skill in SkillsMastered)
			{
				writer.Write((int)skill);
			}

			writer.Write(BandagesUsed);
			writer.Write(DungeonTreasureChestsOpened);
			writer.Write(TreasureMapChestsOpened);
			writer.Write(SOSChestsFound);
			writer.Write(ParagonChestsLooted);
			writer.Write(GoldLooted);
			writer.Write(StepsTakenTotal);
			writer.Write(DamageDealtTotal);
			writer.Write(DamageTakenTotal);
		}

		public override void Deserialize(GenericReader reader)
		{
			var version = reader.ReadInt();
			CreaturesKilled = reader.ReadTypeLongDict();
			ParagonsKilled = reader.ReadTypeLongDict();
			PlayerKillsFaction = reader.ReadTypeLongDict();
			PlayerKillsRace = reader.ReadTypeLongDict();
			SpellsCast = reader.ReadTypeLongDict();
			CreaturesSummoned = reader.ReadTypeLongDict();
			ItemsCrafted = reader.ReadTypeLongDict();
			ItemsEnhanced = reader.ReadTypeLongDict();
			ItemsRepaired = reader.ReadTypeLongDict();
			BulkOrderDeedsCompleted = reader.ReadTypeLongDict();
			ResourceHarvested = reader.ReadTypeLongDict();
			CreaturesCarved = reader.ReadTypeLongDict();
			GravesDigged = reader.ReadLong();
			AnimalsTamed = reader.ReadTypeLongDict();
			AnimalsBonded = reader.ReadTypeLongDict();
			NecromancySummonsCrafted = reader.ReadTypeLongDict();
			PotionsUsed = reader.ReadTypeLongDict();
			BolasThrown = reader.ReadLong();
			FoodConsumed = reader.ReadTypeLongDict();
			TobaccoSmoked = reader.ReadTypeLongDict();
			var count = reader.ReadInt();
			for (var i = 0; i < count; i++)
			{
				SkillsMastered.Add((SkillName)reader.ReadInt());
			}

			BandagesUsed = reader.ReadLong();
			DungeonTreasureChestsOpened = reader.ReadLong();
			TreasureMapChestsOpened = reader.ReadLong();
			SOSChestsFound = reader.ReadLong();
			ParagonChestsLooted = reader.ReadLong();
			GoldLooted = reader.ReadLong();
			StepsTakenTotal = reader.ReadLong();
			DamageDealtTotal = reader.ReadLong();
			DamageTakenTotal = reader.ReadLong();
		}
	}

	public static class DictionaryExtensions
	{
		public static void Increment<T>(this Dictionary<T, long> dict, T key, long value = 1)
		{
			if (!dict.TryAdd(key, value))
			{
				dict[key] += value;
			}
		}
		
		public static void Print<T>(this Dictionary<T, long> dict)
		{
			foreach (var pair in dict)
			{
				Console.WriteLine($"{pair.Key}: {pair.Value}");
			}
		}

		public static void WriteTypeLongDict(this GenericWriter writer, Dictionary<Type, long> dict)
		{
			writer.Write(dict.Count);
			foreach (var pair in dict)
			{
				writer.WriteObjectType(pair.Key);
				writer.Write(pair.Value);
			}
		}

		public static Dictionary<Type, long> ReadTypeLongDict(this GenericReader reader)
		{
			var dict = new Dictionary<Type, long>();
			var count = reader.ReadInt();
			for (var i = 0; i < count; i++)
			{
				var type = reader.ReadObjectType();
				var value = reader.ReadLong();
				dict.Add(type, value);
			}

			return dict;
		}
	}
}
