#region References

using System.Collections.Generic;
using Server;
using Server.Items;

#endregion

namespace Nelderim.ExtraCraftResource
{
	class ExtraCraftResource : NExtension<ExtraCraftResourceInfo>
	{
		public static string ModuleName = "ExtraCraftResource";

		public static void Initialize()
		{
			EventSink.WorldSave += Save;
			Load(ModuleName);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Save(args, ModuleName);
		}
	}
}
