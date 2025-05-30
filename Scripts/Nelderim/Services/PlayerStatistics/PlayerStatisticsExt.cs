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

	public class PlayerStatistics() : NExtension<PlayerStatisticsInfo>("PlayerStatistics")
	{
		public static void Configure()
		{
			Register(new PlayerStatistics());
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
				//Ore,Wood,Fish,Clams,Milk
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
				if (e.Mobile is PlayerMobile pm && 
				    e.Looted != null && 
				    e.Corpse is Corpse corpse &&
				    corpse.Owner is BaseCreature) // Only count loot from creatures
				{
					if (e.Looted is Gold)
					{
						pm.Statistics.GoldLooted += e.Looted.Amount;
					}

					if (e.Looted is ParagonChest)
					{
						pm.Statistics.ParagonChestsLooted++;
					}
					
					var craftResource = CraftResources.GetFromType(e.Looted.GetType());
					if(craftResource != CraftResource.None)
					{
						var resourceType = CraftResources.GetType(craftResource) switch
						{
							//Scales have only one resources, all others have raw resource at index 1
							CraftResourceType.Scales => CraftResources.GetInfo(craftResource).ResourceTypes[0],
							_ => CraftResources.GetInfo(craftResource).ResourceTypes[1],
						};
						pm.Statistics.ResourceHarvested.Increment(resourceType, e.Looted.Amount);
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
			EventSink.BolaThrown += e =>
			{
				if (e.From is PlayerMobile pm)
				{
					pm.Statistics.BolasThrown++;
				}
			};
			EventSink.NecromancySummonCrafted += e =>
			{
				if (e.Crafter is PlayerMobile pm)
				{
					pm.Statistics.NecromancySummonsCrafted.Increment(e.Summon.GetType());
				}
			};
			EventSink.QuestComplete += e =>
			{
				if (e.Mobile is PlayerMobile pm)
				{
					pm.Statistics.QuestsCompleted.Increment(e.QuestType);
				}
			};
			EventSink.PaintingCreated += e =>
			{
				if (e.Artist is PlayerMobile pm)
				{
					pm.Statistics.PaintingsCreated.Increment(e.Painting.GetType());
				}
			};
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
		public Dictionary<Type, long> DungeonTreasureChestsOpened = new();
		public Dictionary<int, long> TreasureMapChestsDigged = new();
		public Dictionary<int, long> SOSChestsFished = new();
		public long ParagonChestsLooted;
		public long GoldLooted;
		public long StepsTakenTotal;
		public long DamageDealtTotal;
		public long DamageTakenTotal;
		public Dictionary<Type, long> QuestsCompleted = new();
		public Dictionary<Type, long> PaintingsCreated = new();

		public override void Serialize(GenericWriter writer)
		{
			writer.Write((int)0); //version
			writer.Write(CreaturesKilled);
			writer.Write(ParagonsKilled);
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
			writer.Write(SpellsCast);
			writer.Write(CreaturesSummoned);
			writer.Write(ItemsCrafted);
			writer.Write(ItemsEnhanced);
			writer.Write(ItemsRepaired);
			writer.Write(BulkOrderDeedsCompleted);
			writer.Write(ResourceHarvested);
			writer.Write(CreaturesCarved);
			writer.Write(GravesDigged);
			writer.Write(AnimalsTamed);
			writer.Write(AnimalsBonded);
			writer.Write(NecromancySummonsCrafted);
			writer.Write(PotionsUsed);
			writer.Write(BolasThrown);
			writer.Write(FoodConsumed);
			writer.Write(TobaccoSmoked);
			writer.Write(MaxSkillGained.Count);
			foreach (var pair in MaxSkillGained)
			{
				writer.Write((int)pair.Key);
				writer.Write(pair.Value);
			}
			writer.Write(BandagesUsed);
			writer.Write(DungeonTreasureChestsOpened);
			writer.Write(TreasureMapChestsDigged);
			writer.Write(SOSChestsFished);
			writer.Write(ParagonChestsLooted);
			writer.Write(GoldLooted);
			writer.Write(StepsTakenTotal);
			writer.Write(DamageDealtTotal);
			writer.Write(DamageTakenTotal);
			writer.Write(QuestsCompleted);
			writer.Write(PaintingsCreated);
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
			QuestsCompleted = reader.ReadTypeLongDict();
			PaintingsCreated = reader.ReadTypeLongDict();
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

		public static void Write(this GenericWriter writer, Dictionary<int, long> dict)
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
		
		public static void Write(this GenericWriter writer, Dictionary<Type, long> dict)
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
