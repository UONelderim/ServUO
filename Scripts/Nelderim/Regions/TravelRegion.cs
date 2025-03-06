#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Server.Nelderim;

#endregion

namespace Server.Regions
{
	public class TravelRegion : NBaseRegion
	{
		private static readonly List<Type> _TravelImpactedRegionTypes =
			[typeof(CityRegion), typeof(HousingRegion), typeof(VillageRegion)];
		
		private static readonly List<TravelRegion> _TravelRegions = [];
		
		public TravelRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
			_TravelRegions.Add(this);
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			return false;
		}
		
		public static bool ValidateTravel(Mobile m, Point3D targetLoc, Map targetMap)
		{
			var fromRegion = _TravelRegions.Find(r => r.Map == m.Map && r.Contains(m.Location));
			if (fromRegion == null) 
				return false; //Traveller not in TravelRegion
			
			var toRegion = _TravelRegions.Find(r => r.Map == targetMap && r.Contains(targetLoc));
			if (toRegion == null)
				return false; //Target not in TravelRegion

			foreach (var type in _TravelImpactedRegionTypes)
			{
				if(TryGetRegions(type, m.Map, m.Location, out var fromRegions))
				{
					if(fromRegions.Any(r => NelderimRegionSystem.GetRegion(r.Name).GetFaction().IsEnemy(m)))
						return false; //Traveller in enemy region
				}
				
				if(TryGetRegions(type, targetMap, targetLoc, out var toRegions))
				{
					if(toRegions.Any(r => NelderimRegionSystem.GetRegion(r.Name).GetFaction().IsEnemy(m)))
						return false; //Target in enemy region
				}
			}

			return true;
		}
	}
}
