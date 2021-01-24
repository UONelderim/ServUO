using Server;
using System;
using System.Collections.Generic;
using System.IO;
using Nelderim;

namespace Nelderim
{
	class CharacterSheet : NExtension<CharacterSheetInfo>
	{
		public static string ModuleName = "CharacterSheet";

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
