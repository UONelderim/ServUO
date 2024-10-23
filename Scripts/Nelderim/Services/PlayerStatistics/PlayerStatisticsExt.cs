using System;
using System.Collections.Generic;
using Nelderim;
using Server.Commands;
using Server.Engines.BulkOrders;
using Server.Items;
using Server.Nelderim;
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
						from.SendMessage("Not implemented");
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
					killer.Statistics.PlayerKillsFaction.Increment(killed.Faction);
					killer.Statistics.PlayerKillsRace.Increment(killed.Race);
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
			EventSink.SkillGain += e =>
			{
				if (e.From is PlayerMobile pm)
				{
					if (pm.Statistics.MaxSkillGained.TryGetValue(e.Skill.SkillName, out var value))
					{
						if (e.Skill.BaseFixedPoint > value) ;
						{
							pm.Statistics.MaxSkillGained[e.Skill.SkillName] = e.Skill.BaseFixedPoint;
						}
					}
					else
					{
						pm.Statistics.MaxSkillGained[e.Skill.SkillName] = e.Skill.BaseFixedPoint;
					}
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
		public Dictionary<Faction, long> PlayerKillsFaction = new();
		public Dictionary<Race, long> PlayerKillsRace = new();

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
		public Dictionary<SkillName, long> MaxSkillGained = new();
		public long BandagesUsed;

		//Other
		public Dictionary<Type, long> DungeonTreasureChestsOpened;
		public Dictionary<int, long> TreasureMapChestsDigged = new();
		public Dictionary<int, long> SOSChestsFished;
		public long ParagonChestsLooted;
		public long GoldLooted;
		public long StepsTakenTotal;
		public long DamageDealtTotal;
		public long DamageTakenTotal;

		public override void Serialize(GenericWriter writer)
		{
			writer.Write((int)0); //version
			writer.WriteTypeLongDict(CreaturesKilled);
			writer.WriteTypeLongDict(ParagonsKilled);
			writer.Write(PlayerKillsFaction.Count);
			foreach (var pair in PlayerKillsFaction)
			{
				writer.Write(pair.Key);
				writer.Write(pair.Value);
			}
			writer.Write(PlayerKillsRace.Count);
			foreach (var pair in PlayerKillsRace)
			{
				writer.Write(pair.Key);
				writer.Write(pair.Value);
			}
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
			writer.Write(MaxSkillGained.Count);
			foreach (var pair in MaxSkillGained)
			{
				writer.Write((int)pair.Key);
				writer.Write(pair.Value);
			}
			writer.Write(BandagesUsed);
			writer.WriteTypeLongDict(DungeonTreasureChestsOpened);
			writer.WriteIntLongDict(TreasureMapChestsDigged);
			writer.WriteIntLongDict(SOSChestsFished);
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
			var count = reader.ReadInt();
			for (var i = 0; i < count; i++)
			{
				var faction = reader.ReadFaction();
				var value = reader.ReadLong();
				PlayerKillsFaction.Add(faction, value);
			}
			count = reader.ReadInt();
			for (var i = 0; i < count; i++)
			{
				var faction = reader.ReadRace();
				var value = reader.ReadLong();
				PlayerKillsRace.Add(faction, value);
			}
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
			count = reader.ReadInt();
			for (var i = 0; i < count; i++)
			{
				var skillName = (SkillName)reader.ReadInt();
				var value = reader.ReadLong();
				MaxSkillGained.Add(skillName, value);
			}
			BandagesUsed = reader.ReadLong();
			DungeonTreasureChestsOpened = reader.ReadTypeLongDict();
			TreasureMapChestsDigged = reader.ReadIntLongDict();
			SOSChestsFished = reader.ReadIntLongDict();
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

		public static void WriteIntLongDict(this GenericWriter writer, Dictionary<int, long> dict)
		{
			writer.Write(dict.Count);
			foreach (var pair in dict)
			{
				writer.Write(pair.Key);
				writer.Write(pair.Value);
			}
		}
		
		public static Dictionary<int, long> ReadIntLongDict(this GenericReader reader)
		{
			var dict = new Dictionary<int, long>();
			var count = reader.ReadInt();
			for (var i = 0; i < count; i++)
			{
				var type = reader.ReadInt();
				var value = reader.ReadLong();
				dict.Add(type, value);
			}

			return dict;
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
