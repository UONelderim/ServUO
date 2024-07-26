using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Chivalry;
using Server.Spells.Necromancy;
using Server.Spells.Spellweaving;

namespace Server.Nelderim;

public class NelderimRegion : IComparable<NelderimRegion>
{
    [JsonInclude] public string Name { get; set; }
    [JsonIgnore] public NelderimRegion Parent { get; set; }
    [JsonInclude] public List<NelderimRegion> Regions { get; set; }
    [JsonInclude] private NelderimRegionSchools BannedSchools { get; set; }
    [JsonInclude] private double? Female { get; set; }
    [JsonInclude] private Dictionary<string, double> Population { get; set; }
    [JsonInclude] private Dictionary<string, double> Intolerance { get; set; } 
    [JsonInclude] private Dictionary<GuardType, NelderimRegionGuard> Guards { get; set; }
    [JsonInclude] private Dictionary<CraftResource, double> Resources { get; set; }
    [JsonInclude] private Dictionary<DifficultyLevelValue, double> DifficultyLevel { get; set; }
    [JsonInclude] private string Faction { get; set; }

    public bool Validate()
    {
        if(Population!= null)
        {
            var populationSum = Population.Values.Sum();
            if (Math.Abs(populationSum - 1.0) > 0.001)
            {
                Console.WriteLine(
                    $"Population sum for region {Name} is incorrect. Expected: 1.0. Acutal: {populationSum}");
            }
        }
        if(Guards != null)
        {
            foreach (var kvp in Guards)
            {
                var guardType = kvp.Key;
                var guardDef = kvp.Value;
                if (guardDef.Population == null) continue;
                
                var guardDefPopulationSum = guardDef.Population.Values.Sum();
                if (Math.Abs(guardDefPopulationSum - 1.0) > 0.001)
                {
	                Console.WriteLine(
		                $"Guard population sum for region {Name} for type {guardType} is incorrect. Expected: 1.0. Acutal: {guardDefPopulationSum}");
                }
            }
        }
        //TODO: Check Resources
        return true;
    }

    public double FemaleChance()
    {
        if (Female.HasValue)
        {
            return Female.Value;
        }

        var parentResult = Parent?.FemaleChance();
        if (parentResult.HasValue)
        {
            return parentResult.Value;
        }
        return 0.5;
    }

    public Race RandomRace()
    {
        if (Population is { Count: > 0 })
        {
            return Race.Parse(Utility.RandomWeigthed(Population));
        }

        var parentResult = Parent?.RandomRace();
        if (parentResult != null)
        {
            return parentResult;
        }
        Console.WriteLine($"Unable to get race for region {Name}");
        return Race.None;
    }
    
    public Faction GetFaction()
    {
	    if (Faction != null)
	    {
		    return Server.Nelderim.Faction.Parse(Faction);
	    }

	    var parentResult = Parent?.GetFaction();
	    if (parentResult != null)
	    {
		    return parentResult;
	    }
	    Console.WriteLine($"Unable to get faction for region {Name}");
	    return Server.Nelderim.Faction.None;
    }


    public double GetIntolerance(Race race)
    {
        if (Intolerance != null)
        {
            if (Intolerance.TryGetValue(race.Name, out var result))
            {
                return result;
            }
            return 0;
        }

        var parentResult = Parent?.GetIntolerance(race);
        if (parentResult.HasValue)
        {
            return parentResult.Value;
        }
        Console.WriteLine($"Unable to get intolerance for region {Name}");
        return 0;
    }

    private NelderimRegionGuard GuardDefinition(GuardType guardType)
    {
        if (Guards is { Count: > 0 })
        {
            if(Guards.TryGetValue(guardType, out var profile))
            {
                return profile;
            }
        }
        var parentResult = Parent?.GuardDefinition(guardType);
        if (parentResult != null)
        {
            return parentResult;
        }

        Console.WriteLine($"Unable to get guard profile for {guardType} for region {Name}");
        return default;
    }
    
    public bool CastIsBanned(ISpell spell)
    {
        if (BannedSchools != null)
        {
            return spell switch
            {
                MagerySpell => BannedSchools.Magery,
                NecromancerSpell => BannedSchools.Necromancy,
                PaladinSpell => BannedSchools.Chivalry,
                ArcanistSpell => BannedSchools.Spellweaving,
                _ => false
            };
        }
        var parentResult = Parent?.CastIsBanned(spell);
        if (parentResult.HasValue)
        {
            return parentResult.Value;
        }
        return default;
    }
    
    public bool PetIsBanned(BaseCreature pet)
    {
        if (BannedSchools != null)
        {
            return pet switch
            {
                BaseFamiliar => BannedSchools.Necromancy,
                SummonedAirElemental or SummonedEarthElemental or SummonedFireElemental or SummonedFireElemental or 
                    SummonedPoisonElemental or SummonedDaemon=> BannedSchools.Magery,
                _ => false
            };
        }
        var parentResult = Parent?.PetIsBanned(pet);
        if (parentResult.HasValue)
        {
            return parentResult.Value;
        }
        return default;
    }

    public void MakeGuard(BaseNelderimGuard guard)
    {
        //We need Race and Gender first
        var guardDefinition = GuardDefinition(guard.Type);
        if (guardDefinition != null)
        {
            guard.Race = Race.Parse(Utility.RandomWeigthed(guardDefinition.Population ?? Population));
            guard.Female = Utility.RandomDouble() < guardDefinition.Female;
            NelderimRegionSystem.GetGuardProfile(guardDefinition.Name).Make(guard);
        }
        else
        {
            Console.WriteLine($"Unable to get guard definition for {guard.Type} for region {Name}");
        }
    }

    public Dictionary<CraftResource, double> ResourceVeins()
    {
        if (Resources is { Count: > 0 })
        {
            return Resources;
        }

        var parentResult = Parent?.ResourceVeins();
        if (parentResult != null)
        {
            return parentResult;
        }
        return new Dictionary<CraftResource, double>();
    }
    
    public Dictionary<DifficultyLevelValue, double> DifficultyLevelWeights()
    {
	    if (DifficultyLevel is { Count: > 0 })
	    {
		    return DifficultyLevel;
	    }

	    var parentResult = Parent?.DifficultyLevelWeights();
	    if (parentResult != null)
	    {
		    return parentResult;
	    }
	    return new Dictionary<DifficultyLevelValue, double>();
    }
    
    protected bool Equals(NelderimRegion other)
    {
        return Name == other.Name;
    }

    public override int GetHashCode()
    {
        return Name != null ? Name.GetHashCode() : 0;
    }

    public int CompareTo(NelderimRegion other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return string.Compare(Name, other.Name, StringComparison.Ordinal);
    }
}
