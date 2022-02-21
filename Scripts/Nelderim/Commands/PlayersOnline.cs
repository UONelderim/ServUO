#region References

using Server.Network;

#endregion

namespace Server.Commands
{
	public class Online
	{
		public static void Initialize()
		{
			CommandSystem.Register("gracze", AccessLevel.Player, Online_OnCommand);
		}

		public static void Online_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendMessage("Graczy online: {0}", NetState.Instances.Count);
		}
	}
}
