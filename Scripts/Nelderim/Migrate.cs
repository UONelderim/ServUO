using System.Linq;
using Server.Items;
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
			CreateMoongates(e.Mobile);
		}

		private static void CreateMoongates(Mobile from)
		{
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
	}
}
