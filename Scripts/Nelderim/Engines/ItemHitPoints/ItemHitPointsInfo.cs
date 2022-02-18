#region References

using Server;

#endregion

namespace Nelderim
{
	class ItemHitPointsInfo : NExtensionInfo
	{
		public int MaxHitPoints { get; set; }

		public int HitPoints { get; set; }

		public override void Deserialize(GenericReader reader)
		{
			MaxHitPoints = reader.ReadInt();
			HitPoints = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			writer.Write(MaxHitPoints);
			writer.Write(HitPoints);
		}
	}
}
