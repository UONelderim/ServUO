#region References

using Server.Mobiles;

#endregion

namespace Server.Commands
{
	public class KoxCommand
	{
		public static void Initialize()
		{
			CommandSystem.Register("kox", AccessLevel.Player, Kox_OnCommand);
		}

		[Usage("Kox")]
		[Description("Wlacza/wylacza wyswietlanie informacji o przyrostach umiejetnosci")]
		public static void Kox_OnCommand(CommandEventArgs e)
		{
			PlayerMobile pm = (PlayerMobile)e.Mobile;
			
			pm.GainDebug = !pm.GainDebug;
			
			pm.SendMessage(pm.GainDebug ? 0x40 : 0x20, "{0} sledzenie przyrostow umiejetnosci.",
				pm.GainDebug ? "Wlaczyles" : "Wylaczyles");
		}
	}
}
