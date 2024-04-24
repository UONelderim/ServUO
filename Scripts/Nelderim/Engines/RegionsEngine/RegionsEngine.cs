using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Nelderim.Races;
using Server.Commands;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Chivalry;
using Server.Spells.Eighth;
using Server.Spells.Necromancy;

namespace Server.Nelderim
{
	public class RegionsEngine
	{
		public static readonly string NelderimRegionsDirectory = Path.Combine(Core.BaseDirectory, "Data", "NelderimRegions");
		public static readonly string NelderimRegionsFile = Path.Combine(NelderimRegionsDirectory, "NelderimRegions.xml");
		public static List<RegionsEngineRegion> NelderimRegions { get; } = new();

		public static void Initialize()
		{
			Load();
			CommandSystem.Register("RELoad", AccessLevel.Administrator, RELoad_OnCommand);
			CommandSystem.Register("RESave", AccessLevel.Administrator, RESave_OnCommand);
			RumorsSystem.Load();
			EventSink.MobileCreated += OnCreate;
			EventSink.OnEnterRegion += OnEnterRegion;
		}

		[Usage("RELoad")]
		[Description("Wgrywa informacje o regionach (RegionsEngine).")]
		public static void RELoad_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendMessage("Wczytuje regiony Nelderim...");

			if (Load())
				e.Mobile.SendMessage("Regiony wczytane!");
			else
				e.Mobile.SendMessage("Blad odczytu regionow!");
		}

		[Usage("RESave")]
		[Description("Zapisuje informacje o regionach (RegionsEngine).")]
		public static void RESave_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendMessage("Zapisuje regiony Nelderim...");

			if (Save())
				e.Mobile.SendMessage("Regiony zapisane!");
			else
				e.Mobile.SendMessage("Blad zapisu regionow!");
		}

		public static bool Load()
		{
			NelderimRegions.Clear();
			Console.Write("NelderimRegions: Wczytywanie...");
			try
			{
				if (!File.Exists(NelderimRegionsFile))
				{
					Console.WriteLine("Error: NelderimRegions.xml does not exist");
					return false;
				}

				var doc = new XmlDocument();
				doc.Load(NelderimRegionsFile);

				var root = doc["NelderimRegions"];

				foreach (XmlElement reg in root.GetElementsByTagName("region"))
				{
					var name = reg.GetAttribute("name");
					var parent = reg.GetAttribute("parent");

					var newRegion = new RegionsEngineRegion(name, parent);

					try
					{
						var nodes = reg.GetElementsByTagName("oreveins");

						if (nodes.Count > 0)
						{
							var pop = nodes.Item(0) as XmlElement;
							var s = new double[9];

							s[0] = XmlConvert.ToDouble(pop.GetAttribute("Iron"));
							s[1] = XmlConvert.ToDouble(pop.GetAttribute("DullCopper"));
							s[2] = XmlConvert.ToDouble(pop.GetAttribute("ShadowIron"));
							s[3] = XmlConvert.ToDouble(pop.GetAttribute("Copper"));
							s[4] = XmlConvert.ToDouble(pop.GetAttribute("Bronze"));
							s[5] = XmlConvert.ToDouble(pop.GetAttribute("Gold"));
							s[6] = XmlConvert.ToDouble(pop.GetAttribute("Agapite"));
							s[7] = XmlConvert.ToDouble(pop.GetAttribute("Verite"));
							s[8] = XmlConvert.ToDouble(pop.GetAttribute("Valorite"));

							newRegion.ResourceVeins = s;
						}
						else
						{
							nodes = reg.GetElementsByTagName("woodveins");
							if (nodes.Count > 0)
							{
								var pop = nodes.Item(0) as XmlElement;
								var s = new double[7];

								s[0] = XmlConvert.ToDouble(pop.GetAttribute("Wood"));
								s[1] = XmlConvert.ToDouble(pop.GetAttribute("Oak"));
								s[2] = XmlConvert.ToDouble(pop.GetAttribute("Ash"));
								s[3] = XmlConvert.ToDouble(pop.GetAttribute("Yew"));
								s[4] = XmlConvert.ToDouble(pop.GetAttribute("Heartwood"));
								s[5] = XmlConvert.ToDouble(pop.GetAttribute("Bloodwood"));
								s[6] = XmlConvert.ToDouble(pop.GetAttribute("Frostwood"));

								newRegion.ResourceVeins = s;
							}
						}
					}
					catch (Exception e)
					{
						Console.WriteLine(e.ToString());
					}

					try
					{
						var nodes = reg.GetElementsByTagName("bannedfollowers");

						if (nodes.Count > 0)
						{
							var pop = nodes.Item(0) as XmlElement;
							var s = new bool[3];

							s[0] = (XmlConvert.ToInt32(pop.GetAttribute("summons")) == 1);
							s[1] = (XmlConvert.ToInt32(pop.GetAttribute("familiars")) == 1);
							s[2] = (XmlConvert.ToInt32(pop.GetAttribute("tammed")) == 1);

							newRegion.TameLimit = XmlConvert.ToInt32(pop.GetAttribute("tammedlimit"));
							newRegion.BannedFollowers = s;
						}
					}
					catch (Exception e)
					{
						Console.WriteLine(e.ToString());
					}

					try
					{
						var nodes = reg.GetElementsByTagName("bannedschools");

						if (nodes.Count > 0)
						{
							var pop = nodes.Item(0) as XmlElement;
							var s = new bool[4];

							s[0] = (XmlConvert.ToInt32(pop.GetAttribute("magery")) == 1);
							s[1] = (XmlConvert.ToInt32(pop.GetAttribute("chivalry")) == 1);
							s[2] = (XmlConvert.ToInt32(pop.GetAttribute("necromancy")) == 1);
							s[3] = (XmlConvert.ToInt32(pop.GetAttribute("druidism")) == 1);

							newRegion.Schools = s;
						}
					}
					catch (Exception e)
					{
						Console.WriteLine(e.ToString());
					}

					try
					{
						var nodes = reg.GetElementsByTagName("intolerance");

						if (nodes.Count > 0)
						{
							var pop = nodes.Item(0) as XmlElement;
							var intolerance = new int[NRace.AllRaces.Count];

							for (var i = 0; i < intolerance.Length; i++)
							{
								var attr = pop.GetAttribute(NRace.AllRaces[i].Name);
								intolerance[i] = XmlConvert.ToInt32(attr == "" ? "0" : attr);
							}

							newRegion.Intolerance = intolerance;
						}
					}
					catch (Exception e)
					{
						Console.WriteLine(e.ToString());
					}

					try
					{
						var nodes = reg.GetElementsByTagName("races");

						if (nodes.Count > 0)
						{
							var pop = nodes.Item(0) as XmlElement;

							var population = new double[NRace.AllRaces.Count];
							double sum = 0;

							for (var i = 0; i < population.Length; i++)
							{
								var attr = pop.GetAttribute(NRace.AllRaces[i].Name);
								population[i] = XmlConvert.ToDouble(attr == "" ? "0" : attr);
								sum += population[i];
							}

							var femaleStr = pop.GetAttribute("Female");
							var female = XmlConvert.ToDouble(femaleStr == "" ? "0.5" : femaleStr);

							if (sum != 100)
								Console.WriteLine(
									"Suma prawdopodobienstw dla regionu {0} jest niepoprawna i wynosi {1}", name, sum);

							newRegion.RegionPopulation = new Population(population, female);
						}
					}
					catch (Exception e)
					{
						Console.WriteLine(e.ToString());
					}

					try
					{
						var g = reg.GetElementsByTagName("guards").Item(0) as XmlElement;

						foreach (XmlElement guard in g.GetElementsByTagName("guard"))
						{
							var type = XmlConvert.ToInt32(guard.GetAttribute("type"));
							var factor = XmlConvert.ToDouble(guard.GetAttribute("factor"));
							var span = XmlConvert.ToDouble(guard.GetAttribute("span"));
							var female = XmlConvert.ToDouble(guard.GetAttribute("female"));
							var file = Path.Combine(NelderimRegionsDirectory, "Profiles", $"{guard.GetAttribute("file")}.xml");
							var guards = new int[NRace.AllRaces.Count];
							var sum = 0;

							var r = guard.GetElementsByTagName("races").Item(0) as XmlElement;

							for (var i = 0; i < guards.Length; i++)
							{
								var attr = r.GetAttribute(NRace.AllRaces[i].Name);
								guards[i] = XmlConvert.ToInt32(attr == "" ? "0" : attr);
								sum += guards[i];
							}

							if (sum != 100)
								Console.WriteLine(
									"[Guards] Suma prawdopodobienstw dla regionu {0} jest niepoprawna i wynosi {1}",
									name, sum);

							newRegion.Guards[type] = new GuardEngine(file, factor, span, female, guards);
						}
					}
					catch
					{
					}

					var difficultyLevelXml = reg["difficultyLevel"];
					if (difficultyLevelXml != null)
					{
						foreach (DifficultyLevelValue difficultyLevel in Enum.GetValues(typeof(DifficultyLevelValue)))
						{
							var value = 0;
							if(Region.ReadInt32(difficultyLevelXml, difficultyLevel.ToString(), ref value, false));
								newRegion.DifficultyLevelWeights[difficultyLevel] = value;
						}

						var sum = newRegion.DifficultyLevelWeights.Values.Sum();
						if (sum != 100)
							Console.WriteLine($"[DifficultyLevel] Suma prawdopodobienstw dla regionu {name} jest niepoprawna i wynosi {sum}");
					}
					
					NelderimRegions.Add(newRegion);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Error");
				Console.WriteLine(e.ToString());
				return false;
			}

			Console.WriteLine("Done");
			return true;
		}

		public static bool Save()
		{
			Console.Write("NelderimRegions: Saving...");

			try
			{
				var xml = new XmlTextWriter(NelderimRegionsFile, Encoding.UTF8);
				xml.Indentation = 1;
				xml.IndentChar = '\t';
				xml.Formatting = Formatting.Indented;

				xml.WriteStartDocument(true);
				xml.WriteStartElement("NelderimRegions");

				foreach (var reg in NelderimRegions)
				{
					xml.WriteStartElement("region");
					xml.WriteAttributeString("name", reg.Name);
					xml.WriteAttributeString("parent", reg.Parent);

					//                try
					// {
					//
					//                    if ( reg.ResourceVeins != null )
					//                     
					// 	xml.WriteStartAttribute( "oreveins" );
					//
					// 	if ( nodes.Count > 0 )
					// 	{
					// 		XmlElement pop = nodes.Item( 0 ) as XmlElement;
					// 		double[] s = new double[9];
					//
					// 		s[0] = XmlConvert.ToDouble( pop.GetAttribute( "Iron" ) );
					// 		s[1] = XmlConvert.ToDouble( pop.GetAttribute( "DullCopper" ) );
					// 		s[2] = XmlConvert.ToDouble( pop.GetAttribute( "ShadowIron" ) );
					// 		s[3] = XmlConvert.ToDouble( pop.GetAttribute( "Copper" ) );
					// 		s[4] = XmlConvert.ToDouble( pop.GetAttribute( "Bronze" ) );
					// 		s[5] = XmlConvert.ToDouble( pop.GetAttribute( "Gold" ) );
					// 		s[6] = XmlConvert.ToDouble( pop.GetAttribute( "Agapite" ) );
					// 		s[7] = XmlConvert.ToDouble( pop.GetAttribute( "Verite" ) );
					// 		s[8] = XmlConvert.ToDouble( pop.GetAttribute( "Valorite" ) );
					//
					//                        newRegion.ResourceVeins = s;
					//                        
					// 	} 
					//                    else
					//                    {
					//                        // sprawdz drewno
					//                        nodes = reg.GetElementsByTagName( "woodveins" );
					//                        if ( nodes.Count > 0 )
					//                        {
					// 		    XmlElement pop = nodes.Item( 0 ) as XmlElement;
					// 		    double[] s = new double[7];
					//
					// 		    s[0] = XmlConvert.ToDouble( pop.GetAttribute( "Wood" ) );
					// 		    s[1] = XmlConvert.ToDouble( pop.GetAttribute( "Oak" ) );
					// 		    s[2] = XmlConvert.ToDouble( pop.GetAttribute( "Ash" ) );
					// 		    s[3] = XmlConvert.ToDouble( pop.GetAttribute( "Yew" ) );
					// 		    s[4] = XmlConvert.ToDouble( pop.GetAttribute( "Heartwood" ) );
					// 		    s[5] = XmlConvert.ToDouble( pop.GetAttribute( "Bloodwood" ) );
					// 		    s[6] = XmlConvert.ToDouble( pop.GetAttribute( "Frostwood" ) );
					//
					//                            newRegion.ResourceVeins = s;
					//                        }
					//                    }
					// }
					// catch ( Exception e )
					// {
					// 	Console.WriteLine( e.ToString() );
					// }

					if (reg.Schools != null)
					{
						xml.WriteStartElement("bannedschools");

						xml.WriteAttributeString("magery", (reg.MageryIsBanned) ? "1" : "0");
						xml.WriteAttributeString("chivalry", (reg.ChivalryIsBanned) ? "1" : "0");
						xml.WriteAttributeString("necromancy", (reg.NecromantionIsBanned) ? "1" : "0");
						xml.WriteAttributeString("druidism", (reg.DruidismIsBanned) ? "1" : "0");


						xml.WriteEndElement(); // "bannedschools"
					}

					if (reg.BannedFollowers != null)
					{
						xml.WriteStartElement("bannedfollowers");

						xml.WriteAttributeString("summons", (reg.SummonsAreBanned) ? "1" : "0");
						xml.WriteAttributeString("familiars", (reg.FamiliarsAreBanned) ? "1" : "0");
						xml.WriteAttributeString("tammed", (reg.PetsAreBanned) ? "1" : "0");
						xml.WriteAttributeString("tammedlimit", XmlConvert.ToString(reg.TameLimit));


						xml.WriteEndElement(); // "bannedfollowers"
					}

					if (reg.RegionPopulation != null)
					{
						xml.WriteStartElement("races");


						for (var i = 0; i < NRace.AllRaces.Count; i++)
						{
							xml.WriteAttributeString(NRace.AllRaces[i].Name,
								XmlConvert.ToString(reg.RegionPopulation.Proportions[i]));
						}

						xml.WriteAttributeString("female", XmlConvert.ToString(reg.RegionPopulation.Female));

						xml.WriteEndElement(); // "races"
					}

					if (reg.Intolerance != null)
					{
						xml.WriteStartElement("intolerance");

						for (var i = 0; i < NRace.AllRaces.Count; i++)
						{
							xml.WriteAttributeString(NRace.AllRaces[i].Name, reg.Intolerance[i].ToString());
						}

						xml.WriteEndElement(); // "intolerance"
					}

					if (reg.Guards != null)
					{
						var isAnyGuard = false;

						for (var i = 0; i < reg.Guards.Length; i++)
						{
							var g = reg.Guards[i];

							if (g != null)
							{
								if (!isAnyGuard)
								{
									xml.WriteStartElement("guards");
									isAnyGuard = true;
								}

								xml.WriteStartElement("guard");

								xml.WriteAttributeString("type", i.ToString());
								xml.WriteAttributeString("file", g.FileName);
								xml.WriteAttributeString("factor", XmlConvert.ToString(g.Factor));
								xml.WriteAttributeString("span", XmlConvert.ToString(g.Span));
								xml.WriteAttributeString("female", XmlConvert.ToString(g.Female));

								xml.WriteStartElement("races");

								for (var j = 0; j < NRace.AllRaces.Count; j++)
								{
									xml.WriteAttributeString(NRace.Races[j].Name, XmlConvert.ToString(g.Races[j]));
								}

								xml.WriteEndElement(); // "races"

								xml.WriteEndElement(); // "guard"
							}
						}

						if (isAnyGuard)
							xml.WriteEndElement(); // "guards"
					}

					if (reg.DifficultyLevelWeights.Count != 0)
					{
						xml.WriteStartElement("difficultyLevel");
						foreach (var pair in reg.DifficultyLevelWeights)
						{
							if(pair.Value != 0)
								xml.WriteAttributeString(pair.Key.ToString(),XmlConvert.ToString(pair.Value));
						}
						xml.WriteEndElement();
					}

					xml.WriteEndElement(); // "region"
				}

				xml.WriteEndElement(); // "NelderimRegions"

				xml.Flush();
				xml.Close();
			}
			catch (Exception exc)
			{
				Console.WriteLine("Error");
				Console.WriteLine(exc.ToString());
				return false;
			}

			Console.WriteLine("Done");
			return true;
		}

		private static void OnEnterRegion(OnEnterRegionEventArgs e)
		{
			var m = e.From;
			if (e.OldRegion == null)
				if (m is BaseVendor)
					m.Race = GetRace(e.NewRegion.Name);
		}

		private static void OnCreate(MobileCreatedEventArgs e)
		{
			var m = e.Mobile;

			if (m is BaseCreature { Tamable: false } bc)
			{
				var r = GetRegion(m.Region.Name);
				if (r != null && r.DifficultyLevelWeights.Count != 0 && bc.DifficultyLevel == DifficultyLevelValue.Normal)
				{
					bc.DifficultyLevel = Utility.RandomWeigthed(r.DifficultyLevelWeights);
				}
			}
		}


		public static RegionsEngineRegion GetRegion(string regionName)
		{
			RegionsEngineRegion reg = null;

			try
			{
				for (int i = 0, count = NelderimRegions.Count; i < count; i++)
					if (NelderimRegions[i].Name == regionName)
						reg = NelderimRegions[i];

				reg ??= NelderimRegions[0];
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			return reg;
		}

		public static Race GetRace(string regionName)
		{
			var race = Race.DefaultRace;

			try
			{
				var reg = GetRegion(regionName);
				race = (reg.RegionPopulation == null) ? GetRace(reg.Parent) : reg.GetRace;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			return race;
		}

		public static double GetFemaleChance(string regionName)
		{
			try
			{
				var reg = GetRegion(regionName);
				return (reg.RegionPopulation == null) ? GetFemaleChance(reg.Parent) : reg.GetFemaleChance;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			return 0.5;
		}

		public static void MakeGuard(BaseNelderimGuard guard)
		{
			var typ = (int)guard.Type;
			GuardEngine guards = null;
			var region = GetRegion(guard.Region.Name);

			while ((guards = region.Guards[typ]) == null)
				region = GetRegion(region.Parent);

			guards.Make(guard);
		}

		public static bool MakeGuard(BaseNelderimGuard guard, string regionName)
		{
			var typ = (int)guard.Type;
			GuardEngine guards;
			var region = GetRegion(regionName);

			if (region.Name == "Default")
				return false;

			while ((guards = region.Guards[typ]) == null)
				region = GetRegion(region.Parent);

			guards.Make(guard);

			return true;
		}

		public static bool CastIsBanned(string regionName, Spell spell)
		{
			try
			{
				var reg = GetRegion(regionName);

				if (reg.Schools == null)
				{
					return CastIsBanned(reg.Parent, spell);
				}

				if (spell is NecromancerSpell && reg.NecromantionIsBanned)
					return true;

				if (spell is PaladinSpell && reg.ChivalryIsBanned)
					return true;

				if (reg.MageryIsBanned && !(spell is PaladinSpell) && !(spell is NecromancerSpell))
					return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.WriteLine("Debug -> Region =" + regionName + " | Spell = {0}" + spell);
			}

			return false;
		}

		public static bool PetIsBanned(string regionName, Spell spell)
		{
			try
			{
				var reg = GetRegion(regionName);

				if (reg.BannedFollowers == null)
				{
					return PetIsBanned(reg.Parent, spell);
				}

				if (spell is SummonFamiliarSpell && reg.FamiliarsAreBanned)
					return true;

				if ((spell is AirElementalSpell || spell is EarthElementalSpell
				                                || spell is FireElementalSpell || spell is SummonDaemonSpell
				                                || spell is WaterElementalSpell) && reg.SummonsAreBanned)
					return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.WriteLine("Debug -> Region =" + regionName + " | Spell = {0}" + spell);
			}

			return false;
		}

		public static bool PetIsBanned(string regionName, BaseCreature pet)
		{
			try
			{
				var reg = GetRegion(regionName);

				if (reg.BannedFollowers == null)
					return PetIsBanned(reg.Parent, pet);

				if (pet is BaseFamiliar && reg.FamiliarsAreBanned)
					return true;

				if ((pet is SummonedAirElemental || pet is SummonedDaemon || pet is SummonedEarthElemental ||
				     pet is SummonedFireElemental || pet is SummonedWaterElemental) && reg.SummonsAreBanned)
					return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			return false;
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
		
		public static void MentionIntolerance(Mobile source, Mobile target)
		{
			source.Emote(Utility.RandomList(IntoleranceEmote).Invoke(target.Race));
			source.Yell(Utility.RandomList(IntoleranceSaying).Invoke(target.Race));
			target.SendMessage($"Zdaje sie, ze w tej okolicy nie lubi sie {target.Race.GetPluralName(Cases.Dopelniacz)}!",0x25);
		}

		public static bool ActIntolerativeHarmful(Mobile source, Mobile target, bool msg)
		{
			try
			{
				if (source != null && target != null && source.InLOS(target))
				{
					var region = GetRegion(source.Region.Name);

					while (region.Intolerance == null)
						region = GetRegion(region.Parent);

					double intolerance = region.Intolerance[NRace.AllRaces.FindIndex(r => r.Equals(target.Race))];

					if (intolerance >= 30)
					{
						if (msg)
						{
							MentionIntolerance(source, target);
						}

						// szansa na crim
						if (intolerance > 50)
						{
							double distMod = 0;

							// modyfikator szansy dla strazy w zaleznosci od tego jak daleko stoja od celu
							if (source is BaseNelderimGuard)
							{
								var distance = source.GetDistanceToSqrt(target);

								// +10% jesli odleglosc wynosi 0-3
								// 4-6 bez zmian
								// -10% jesli odleglosc wynosi 7+            
								if (distance <= 3)
									distMod = 0.1;
								else if (distance >= 7)
									distMod = -0.1;
							}

							double minVal = source is BaseVendor ? 30 : 50;
							var chance = ((intolerance - minVal) * 2 + distMod) / 100;
							return chance >= Utility.RandomDouble();
						}
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

		public static void GetResourceVeins(string regionName, out List<double> factors)
		{
			factors = null;
			try
			{
				var reg = GetRegion(regionName);
				if (reg == null || reg.ResourceVeins == null)
				{
					return;
				}
				factors = new List<double>(reg.ResourceVeins);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}
	}
}
