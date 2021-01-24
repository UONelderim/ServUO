using Server;
using System;
using System.Collections.Generic;
using System.IO;
using Nelderim;

namespace Nelderim.Gains
{
    class Gains : NExtension<GainsInfo>
    {
		public static string ModuleName = "Gains";

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
