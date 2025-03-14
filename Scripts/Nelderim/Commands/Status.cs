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
			if (e.Mobile is PlayerMobile pm)
			{
				pm.SendMessage("Slawa: {0}", pm.Fame);
				pm.SendMessage("Karma: {0}", pm.Karma);
				pm.SendMessage("Morderstwa: {0}", pm.Kills);
				if (pm.Kills > 0)
				{
					pm.SendMessage("Ostatnie morderstwo sie przedawni za okolo {0} godzin.", pm.LongTermElapse.TotalHours);
				}
			}
		}
	}
}
