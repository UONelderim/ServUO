#region References

using System.Xml;
using Server.Nelderim;

#endregion

namespace Server.Regions
{
	public class HousingRegion : NBaseRegion
	{
		public HousingRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
		}

		public static bool ValidateHousing(Mobile from, Point3D targetLoc)
		{
			if (from.IsStaff())
				return true;
			
			if(TryGetRegions(typeof(HousingRegion), from.Map, targetLoc, out var regions))
			{
				foreach (var region in regions)
				{
					var regionFaction = NelderimRegionSystem.GetRegion(region.Name).GetFaction();
					return regionFaction != Faction.None && from.Faction == regionFaction;
				}
			}

			return false;
			
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			if (from.IsStaff())
				return true;
			
			var regionFaction = NelderimRegionSystem.GetRegion(Name).GetFaction();
			return regionFaction != Faction.None && from.Faction == regionFaction;
		}
	}
}
