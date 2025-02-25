using Nelderim.Towns;
using Server.Gumps;

namespace Server.Commands
{
	public class Armia
	{
		public static void Initialize()
		{
			CommandSystem.Register("ArmiaPosterunki", AccessLevel.Player, ArmiaSpawn_handler);
		}

		[Usage("ArmiaPosterunki")]
		[Description(
			"Wyswietla panel ustawien posterunkow miasta. Wymaga bycia obywatelem miasta oraz posiadania odpowiednich uprawnien (Przywodca lub Glowny Kanclerz lub Kanclerz Armii).")]
		private static void ArmiaSpawn_handler(CommandEventArgs args)
		{
			Mobile from = args.Mobile;
			Towns t = TownDatabase.GetCitizenCurrentCity(from);

			if (from.Account.AccessLevel >= AccessLevel.GameMaster ||
			    (t != Towns.None && (TownDatabase.GetCurrentTownStatus(from) == TownStatus.Leader ||
			                         TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Prime ||
			                         TownDatabase.GetCurrentTownConselourStatus(from) == TownCounsellor.Diplomacy)))
			{
				from.SendGump(new TownPostsGump(t, from));
			}
		}
	}
}
