using System.Collections.Generic;
using Server;

namespace Nelderim.Towns
{
	class TownsVendor : NExtension<TownsVendorInfo>
	{
		public static string ModuleName = "TownsVendor";

		public static void Initialize()
		{
			EventSink.WorldSave += new WorldSaveEventHandler(Save);
			Load(ModuleName);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Cleanup();
			Save(args, ModuleName);
		}

		private static void Cleanup()
		{
			List<Serial> toRemove = new List<Serial>();
			foreach (Serial serial in m_ExtensionInfo.Keys)
			{
				if (World.FindMobile(serial) == null)
					toRemove.Add(serial);
			}

			foreach (Serial serial in toRemove)
			{
				m_ExtensionInfo.TryRemove(serial, out _);
			}
		}
	}
}
