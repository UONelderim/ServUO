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

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			if (from.IsStaff())
				return true;
			
			var regionFaction = NelderimRegionSystem.GetRegion(Name).GetFaction();
			return regionFaction != Faction.None && from.Faction == regionFaction;
		}
	}
}
