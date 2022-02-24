#region References

using System.Collections.Generic;
using Server;
using Server.Items;

#endregion

namespace Nelderim
{
	class ItemHitPoints : NExtension<ItemHitPointsInfo>
	{
		public static string ModuleName = "ItemHitPoints";

		public static void Initialize()
		{
			EventSink.WorldSave += Save;
			Load(ModuleName);
		}

		public static void Save(WorldSaveEventArgs args)
		{
			Save(args, ModuleName);
		}

		public static new ItemHitPointsInfo Get(IEntity entity)
		{
			bool created = !m_ExtensionInfo.ContainsKey(entity.Serial);
			ItemHitPointsInfo info = NExtension<ItemHitPointsInfo>.Get(entity);
			if (created && entity is IDurability durableItem)
				info.HitPoints = info.MaxHitPoints =
					Utility.RandomMinMax(durableItem.InitMinHits, durableItem.InitMaxHits);

			return info;
		}
	}
}
