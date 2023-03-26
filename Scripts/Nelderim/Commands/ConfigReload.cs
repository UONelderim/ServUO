namespace Server.Commands
{
	public class ConfigReload
	{
		public static void Initialize()
		{
			CommandSystem.Register("configReload", AccessLevel.Administrator, _ => Config.Unload());
		}

	}
}
