#region References

using Server;
using Server.Items;

#endregion

namespace Nelderim
{
	class ItemHitPoints() : NExtension<ItemHitPointsInfo>("ItemHitPoints")
	{
		public static new void Initialize()
		{
			Register(new ItemHitPoints());
		}
		
		public override ItemHitPointsInfo InternalGet(IEntity entity)
		{
			bool created = !ExtensionInfo.ContainsKey(entity.Serial);
			ItemHitPointsInfo info = base.InternalGet(entity);
			if (created && entity is IDurability durableItem)
				info.HitPoints = info.MaxHitPoints =
					Utility.RandomMinMax(durableItem.InitMinHits, durableItem.InitMaxHits);

			return info;
		}
	}
	
	class ItemHitPointsInfo : NExtensionInfo
	{
		public int MaxHitPoints { get; set; }

		public int HitPoints { get; set; }

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 ); //version
			writer.Write(MaxHitPoints);
			writer.Write(HitPoints);
		}

		public override void Deserialize(GenericReader reader)
		{
			int version = 0;
			if (Fix)
				version = reader.ReadInt(); //version
			MaxHitPoints = reader.ReadInt();
			HitPoints = reader.ReadInt();
		}
	}
}
