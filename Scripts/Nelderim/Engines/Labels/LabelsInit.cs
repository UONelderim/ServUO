using Server;
using Server.Commands;
using Server.Nelderim;

namespace Nelderim
{
    class LabelsInit
    {
        public static string ModuleName = "Labels";

        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler( Save ) ;
            Labels.Load( ModuleName );
        }

        public static void Save( WorldSaveEventArgs args )
        {
            Labels.Cleanup();
            Labels.Save( args, ModuleName );
        }
    }
}
