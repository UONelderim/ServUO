#region References

using System;
using Server;
using Server.Commands;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Mobiles;

#endregion

namespace Nelderim
{
	class LabelsConfig
	{
		public static void AddCreationMark(IEntity entity, Mobile from)
		{
			if (ShouldMark(entity, from))
			{
				var labelsInfo = Labels.Get(entity);
				labelsInfo.Labels[4] = CommandLogging.Format(from) as string;
			}
		}
		
		public static void AddTamperingMark(object o, Mobile from)
		{
			if (o is not IEntity entity)
				return;
			
			if (ShouldMark(entity, from))
			{
				var labelsInfo = Labels.Get(entity);
				labelsInfo.ModifiedBy = from.Account.Username;
				labelsInfo.ModifiedDate = DateTime.Now;
			}
		}
				
		private static bool ShouldMark(IEntity o, Mobile from)
		{
			if(o is BaseVendor || o is PlayerMobile || o is Teleporter || o is Spawner || o is Static || o is ChampionSpawn )
				return false;

			return true;
		}
	}
}
