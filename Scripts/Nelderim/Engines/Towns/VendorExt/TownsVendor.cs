#region References

using System.Collections.Generic;
using Server;

#endregion

namespace Nelderim.Towns
{
	class TownsVendor : NExtension<TownsVendorInfo>
	{
		public static string ModuleName = "TownsVendor";

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
