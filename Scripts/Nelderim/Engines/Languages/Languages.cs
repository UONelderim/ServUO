using Server;

namespace Nelderim
{
    class Languages : NExtension<LanguagesInfo>
    {
		public static string ModuleName = "Speech";

		public static void Initialize()
		{
			EventSink.WorldSave += new WorldSaveEventHandler( Save );
			Load( ModuleName );
		}

		public static void Save( WorldSaveEventArgs args )
		{
			Save( args, ModuleName );
		}
	}
}
