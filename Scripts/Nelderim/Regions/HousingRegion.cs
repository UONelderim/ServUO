#region References

using System.Xml;

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
			return true;
		}
	}
}
