using Server;
using System.Collections.Generic;

namespace Nelderim
{
    class ItemHitPoints : NExtension<ItemHitPointsInfo>
    {
		public static string ModuleName = "ItemHitPoints";

		public static void Initialize()
		{
			EventSink.WorldSave += new WorldSaveEventHandler( Save );
			Load( ModuleName );
		}

		public static void Save( WorldSaveEventArgs args )
		{
			Cleanup();
			Save( args, ModuleName );
		}

		public static new ItemHitPointsInfo Get(IEntity entity)
		{
			bool created = !m_ExtensionInfo.ContainsKey(entity.Serial);
			ItemHitPointsInfo info = NExtension<ItemHitPointsInfo>.Get(entity);
			if(created && entity is IHitPoints entity2 ) { 
				info.HitPoints = info.MaxHitPoints = Utility.RandomMinMax(entity2.InitMinHits, entity2.InitMaxHits); 
			}
			return info;
		}

		private static void Cleanup()
		{
			List<Serial> toRemove = new List<Serial>();
			foreach ( KeyValuePair<Serial, ItemHitPointsInfo> kvp in m_ExtensionInfo )
			{
				if ( World.FindItem( kvp.Key ) == null )
					toRemove.Add( kvp.Key );
			}
			foreach ( Serial serial in toRemove )
			{
				m_ExtensionInfo.Remove( serial );
			}
		}
	}
}
