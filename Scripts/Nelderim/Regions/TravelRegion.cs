#region References

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Server.Nelderim;

#endregion

namespace Server.Regions
{
	public class TravelRegion : NBaseRegion
	{
		private static readonly List<TravelRegion> _Regions = [];
		
		public TravelRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
			_Regions.Add(this);
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			return false;
		}
		
		public static bool ValidateTravel(Mobile traveller, Point3D targetLoc, Map targetMap)
		{
			//Traveller in TravelRegion
			if (!_Regions.Any(region => region.Map == traveller.Map && region.Contains(traveller.Location)))
				return false;
			//Target in TravelRegion
			if (!_Regions.Any(region => region.Map == targetMap && region.Contains(targetLoc))) 
				return false;
			
			return traveller.Faction != Faction.East;
		}
	}

	public class UndershadowTravelRegion : NBaseRegion
	{
		private static readonly List<UndershadowTravelRegion> _Regions = [];

		public UndershadowTravelRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
			_Regions.Add(this);
		}
		
		public override bool AllowHousing(Mobile from, Point3D p)
		{
			return false;
		}

		public static bool ValidateTravel(Mobile traveller, Point3D targetLoc, Map targetMap)
		{
			//Traveller in TravelRegion
			if (!_Regions.Any(region => region.Map == traveller.Map && region.Contains(traveller.Location)))
				return false;
			//Target in TravelRegion
			if (!_Regions.Any(region => region.Map == targetMap && region.Contains(targetLoc))) 
				return false;
			
			return traveller.Faction == Faction.East;
		}
	}
}
