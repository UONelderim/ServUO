#region References

using Server.Mobiles;

#endregion

namespace Server.Commands
{
	public class StatusCommand
	{
		public static void Initialize()
		{
			CommandSystem.Register("status", AccessLevel.Player, Status_OnCommand);
		}

		[Usage("Status")]
		[Description("Wyswietla informacje o postaci.")]
		public static void Status_OnCommand(CommandEventArgs e)
		{
			PlayerMobile pm = (PlayerMobile)e.Mobile;

			pm.SendMessage("Slawa: {0}", e.Mobile.Fame);
			pm.SendMessage("Karma: {0}", e.Mobile.Karma);
			pm.SendMessage("Morderstwa: {0}", e.Mobile.Kills);
		}
	}
}
