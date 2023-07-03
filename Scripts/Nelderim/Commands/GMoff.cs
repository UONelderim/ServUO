#region References

using Server.Mobiles;

#endregion

namespace Server.Commands
{
	public class GMoff
	{
		public static void Initialize()
		{
			CommandSystem.Register("GMoff", AccessLevel.Counselor, GMoff_OnCommand);
		}

		[Usage("GMoff")]
		[Description("Ukrycie przed swiatem Accessu GMowego.")]
		private static void GMoff_OnCommand(CommandEventArgs e)
		{
			PlayerMobile pm = e.Mobile as PlayerMobile;

			if (pm == null)
				return;

			pm.HiddenGM = !pm.HiddenGM;

			if (pm.HiddenGM)
			{
				pm.SendMessage(136, "Ukryles swoj status Mistrza Gry!");
			}
			else
				pm.SendMessage(136, "Ochrona Mistrza Gry aktywna!");
			pm.InvalidateProperties();
		}
	}
}
