#region References

using Server;

#endregion

namespace Nelderim.Towns
{
	class TownsVendorInfo : NExtensionInfo
	{
		public Towns TownAssigned { get; set; }

		public TownBuildingName TownBuildingAssigned { get; set; }

		public bool TradesWithCriminals { get; set; }

		public override void Serialize(GenericWriter writer)
		{
			writer.Write( (int)0 );
			writer.Write((int)TownAssigned);
			writer.Write((int)TownBuildingAssigned);
			writer.Write(TradesWithCriminals);
		}

		public override void Deserialize(GenericReader reader)
		{
			var version = reader.ReadInt();
			TownAssigned = (Towns)reader.ReadInt();
			TownBuildingAssigned = (TownBuildingName)reader.ReadInt();
			TradesWithCriminals = reader.ReadBool();
		}
	}
}
