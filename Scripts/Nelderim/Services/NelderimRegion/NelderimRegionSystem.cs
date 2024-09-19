using System;
using System.Collections.Generic;
using System.IO;
using Server.Mobiles;
using System.Text.Json;
using System.Text.Json.Serialization;
using Server.Commands;

namespace Server.Nelderim
{
    public class NelderimRegionSystem
    {
        internal static string BaseDir = Path.Combine(Core.BaseDirectory, "Data", "NelderimRegions");
        private static string JsonPath = Path.Combine(BaseDir, "NelderimRegions.json");

        internal static Dictionary<string, NelderimRegion> NelderimRegions = new();
        internal static Dictionary<string, NelderimGuardProfile> GuardProfiles = new();
        
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
	        WriteIndented = true,
	        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        };

        public static void Configure()
        {
	        Load();
        }

        public static void Initialize()
        {
	        CommandSystem.Register("NRSLoad", AccessLevel.Administrator, e => Load());
            RumorsSystem.Load();
        }

        private static void Load()
        {
            NelderimRegions.Clear();
            var region = JsonSerializer.Deserialize<NelderimRegion>(File.ReadAllText(JsonPath), SerializerOptions);
            Add(region);
            Console.WriteLine("NelderimRegions: Loaded.");
        }

        private static void Add(NelderimRegion region)
        {
	        if (region.Regions != null)
	        {
		        region.Regions.Sort();
		        foreach (var subRegion in region.Regions)
		        {
			        subRegion.Parent = region;
			        Add(subRegion);
		        }
	        }
	        region.Validate();
	        NelderimRegions.Add(region.Name, region);
        }

        private static void Save()
        {
            File.WriteAllText(JsonPath, JsonSerializer.Serialize(NelderimRegions["Default"], SerializerOptions));
            Console.WriteLine("NelderimRegions: Saved!");
        }
        
        public static void OnRegionChange(Mobile m, Region Old, Region New)
        {
	        if (New == null || New.Map == Map.Internal) return;
	        //Use configured Race as flag if mobile was already initialized. Is it good enough?
	        if (m.Race != Race.DefaultRace) return;
	        
	        InitMobile(m);
        }
        
        public static NelderimRegion GetRegion(string regionName)
        {
	        if(regionName == null)
		        return NelderimRegions["Default"];
	        
            if (NelderimRegions.TryGetValue(regionName, out var result))
            {
                return result;
            }
            return NelderimRegions["Default"]; //Fallback to default for non specified regions
        }

        public static void InitMobile(Mobile m)
        {
	        if (m.Deleted) return;
	        
	        var region = GetRegion(m.Region.Name);
	        
	        m.Female = region.RollFemale();
	        m.BodyValue = m.Female ? 0x191 : 0x190;

		    m.Race = region.RandomRace();
	        
	        m.Faction = region.GetFaction();

	        if(String.IsNullOrEmpty(m.Name))
		        m.Name = NameList.RandomName(m.Race.Name.ToLower() + "_" + (m.Female ? "female" : "male"));
        }

        internal static NelderimGuardProfile GetGuardProfile(string name)
        {
            if (!GuardProfiles.ContainsKey(name))
            {
                var newProfile = new NelderimGuardProfile(name);
                GuardProfiles.Add(name, newProfile);
            }
            return GuardProfiles[name];
        }
        
        private static Func<Race, String>[] IntoleranceEmote =
        {
	        r => $"*mierzy zlowrogim spojrzeniem {r.GetName(Cases.Biernik)}*",
	        r => $"*z odraza sledzi wzrokiem {r.GetName(Cases.Biernik)}*",
	        _ => "*spluwa*!",
	        _ => "*prycha*",
        };
        
        private static Func<Race, String>[] IntoleranceSaying =
		{
			r => $"Co za czasy! Wszedzie {r.GetPluralName(Cases.Mianownik)}",
			r => $"Zejdz mi z drogi {r.GetName(Cases.Wolacz)}!",
			r => $"Lepiej opusc ta okolice {r.GetName(Cases.Wolacz)}! Moze Cie spotkac krzywda!",
			r => $"{r.GetName(Cases.Wolacz)}! Psie jeden! Wynos sie z mego rewiru!",
			r => $"Nie chce Cie tu widziec {r.GetName(Cases.Wolacz)}!",
			r => $"{r.GetName(Cases.Wolacz)}! Psie jeden! To nie jest miejsce dla takich jak TY!",
			r => $"Co za czasy! Wszedzie {r.GetPluralName(Cases.Mianownik)}!"
		};
		
		private static void MentionIntolerance(Mobile source, Mobile target)
		{
			source.Emote(Utility.RandomList(IntoleranceEmote).Invoke(target.Race));
			source.Yell(Utility.RandomList(IntoleranceSaying).Invoke(target.Race));
			target.SendMessage($"Zdaje sie, ze w tej okolicy nie lubi sie {target.Race.GetPluralName(Cases.Dopelniacz)}!",0x25);
		}

		public static bool ActIntolerativeHarmful(Mobile source, Mobile target, bool msg = true)
		{
			if(source == null || target == null || !source.InLOS(target))
				return false;
			try
			{
				var intolerance = GetRegion(source.Region.Name).GetIntolerance(target.Race);
				if (intolerance >= 0.3)
				{
					if (msg)
					{
						MentionIntolerance(source, target);
					}

					// szansa na crim
					if (intolerance > 0.5)
					{
						double distMod = 0;

						if (source is BaseNelderimGuard)
						{
							var distance = source.GetDistanceToSqrt(target);

							if (distance <= 3)
								distMod = 0.1;
							else if (distance >= 7)
								distMod = -0.1;
						}

						double minVal = source is BaseVendor ? 0.3 : 0.5;
						var chance = (intolerance - minVal) * 2 + distMod;
						return chance >= Utility.RandomDouble();
					}
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
				return false;
			}

			return false;
		}
    }
}
