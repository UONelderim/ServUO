#region References

using Server;

#endregion

namespace Nelderim
{
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
