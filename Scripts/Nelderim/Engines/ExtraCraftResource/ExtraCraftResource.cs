using System.Collections.Generic;
using Server;
using Server.Items;

namespace Nelderim.ExtraCraftResource
{
	class ExtraCraftResource : NExtension<ExtraCraftResourceInfo>
	{
		public static string ModuleName = "ExtraCraftResource";

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
			foreach (KeyValuePair<Serial, ExtraCraftResourceInfo> kvp in m_ExtensionInfo)
			{
				if (kvp.Value.Resource2 == CraftResource.None || World.FindItem(kvp.Key) == null)
					toRemove.Add(kvp.Key);
			}

			foreach (Serial serial in toRemove)
			{
				m_ExtensionInfo.TryRemove(serial, out _);
			}
		}
	}
}
