using System.Xml;

namespace Server.Regions
{
	public class GeographicRegion : NelderimRegion
	{
		public GeographicRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			return false;
		}
	}
}
