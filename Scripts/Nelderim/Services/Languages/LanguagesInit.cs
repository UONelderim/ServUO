using Server;

namespace Nelderim
{
	public class LanguagesInit
	{
		public static string ModuleName = "Languages";

		public static void Initialize()
		{
			EventSink.WorldSave += Save;
			Languages.Load(ModuleName);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Languages.Save(args, ModuleName);
		}
	}
}
