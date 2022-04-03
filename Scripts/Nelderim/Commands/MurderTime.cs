#region References

using Server.Mobiles;

#endregion

namespace Server.Commands
{
	public class MurderTimeCommand
	{
		public static void Initialize()
		{
			CommandSystem.Register("murdertime", AccessLevel.Player, Status_OnCommand);
		}

		[Usage("MurderTime")]
		[Description("Wyswietla informacje o czasie pozostałym do przedawnienia ostatniego morderstwa")]
		public static void Status_OnCommand(CommandEventArgs e)
		{
			PlayerMobile pm = (PlayerMobile)e.Mobile;

			pm.SendMessage("MurderTime: {0}", pm.LongTermElapse - pm.GameTime);
		}
	}
}
