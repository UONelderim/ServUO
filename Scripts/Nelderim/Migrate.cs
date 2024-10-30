using System;
using System.Linq;
using Nelderim;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Nelderim;
using Server.Nelderim.Gumps;
using Server.Nelderim.Misc;

namespace Server.Commands
{
	public class Migrate
	{
		public static void Initialize()
		{
			//TODO: CHANGE ME TO OWNER ONLY
			CommandSystem.Register("DoMigrate", AccessLevel.Administrator, DoMigrate);
		}

		private static void DoMigrate(CommandEventArgs e)
		{
			var from = e.Mobile;
			var players = World.Mobiles.Values.OfType<PlayerMobile>();
			
			foreach (var pm in players)
			{
				if(pm.IsStaff())
					continue;
				RefundLanguages(pm);
				pm.Kills = 0;
				pm.Fame = 0;
				pm.Karma = 0;
				pm.Name = "Czlowiek";
				pm.Profile = "";
				var characterSheet = CharacterSheet.Get(pm);
				characterSheet.AppearanceAndCharacteristic = "";
				characterSheet.HistoryAndProfession = "";
				pm.Race = Race.None;
				pm.Faction = Faction.None;

				pm.LogoutMap = AccountHandler.StartingCities[0].Map;
				pm.LogoutLocation = AccountHandler.StartingCities[0].Location;
				BaseMount.Dismount(pm);
				//Stable all pets
				if (pm.AllFollowers.Count > 0)
				{
					for (int i = pm.AllFollowers.Count - 1; i >= 0; --i)
					{
						var pet = pm.AllFollowers[i] as BaseCreature;

						if (pet == null)
						{
							continue;
						}

						pet.ControlTarget = null;
						pet.ControlOrder = OrderType.Stay;
						pet.Internalize();

						pet.SetControlMaster(null);
						pet.SummonMaster = null;

						pet.IsStabled = true;
						pet.StabledBy = null;

						pm.Stabled.Add(pet);
					}
				}

			}
			
			//New Haven -> RaceRoom
			var newHavenMap = Map.Trammel;
			var newHavenPortalLocation = new Point3D(3506, 2574, 18);
			var raceRoomMap = Map.Felucca;
			var raceRoomStartLocation = new Point3D(5176, 230, 0);
			var raceRoomPortalLocation = new Point3D(5156, 254, 50);
			from.SendMessage("Cleaning space for newHaven Portal");
			foreach (var item in newHavenMap.FindItems<Item>(newHavenPortalLocation).ToArray())
			{
				from.SendMessage($"Removing {item}@{item.Location}");
				item.Delete();
			}
			from.SendMessage("Creating newHaven Portal");
			var newHavenMoongate = new ConfirmationMoongate()
			{
				GumpWidth = 300,
				GumpHeight = 200,
				TitleColor = 0xFFFFFF,
				MessageColor = 0xFFFFFF,
				TitleNumber = 1124183,
				MessageNumber = 1124184,
				TargetMap = raceRoomMap,
				Target = raceRoomStartLocation
			};
			newHavenMoongate.MoveToWorld(newHavenPortalLocation, newHavenMap);
			
			from.SendMessage("Cleaning space for RaceRoom Portal");
			foreach (var item in raceRoomMap.FindItems<Item>(raceRoomPortalLocation).ToArray())
			{
				from.SendMessage($"Removing {item}@{item.Location}");
				item.Delete();
			}
			from.SendMessage("Creating RaceRoom Portal");
			var raceRoomMoongate = new RaceRoomMoongate
			{
				//It have to go somewhere to not give an error
				TargetMap = raceRoomMap,
				Target = raceRoomPortalLocation
			};
			raceRoomMoongate.MoveToWorld(raceRoomPortalLocation, raceRoomMap);
		}

		private static void RefundLanguages(PlayerMobile pm)
		{
			foreach (var lang in Enum.GetValues<NLanguage>())
			{
				if (lang is NLanguage.Belkot or NLanguage.Powszechny)
					continue;
				if (pm.Race.DefaultLanguages.ContainsKey(lang))
					continue;
				var value = pm.LanguagesKnown[lang];
				if (value == 1000)
				{
					pm.LanguagesKnown[lang] = 0;
					pm.QuestPoints += 150;
					pm.QuestPointsHistory.Add(
						new QuestPointsHistoryEntry(
							DateTime.Now, "Nelderim", 150, $"Zwrot PD za jezyk: {lang}", pm.Name));
				}
			}
		}
	}
}
