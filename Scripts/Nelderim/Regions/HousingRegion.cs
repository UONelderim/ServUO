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
		
		public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
		{
			base.AlterLightLevel(m, ref global, ref personal);

			// Apply Undershadow light level to house within Undershadow region
			NelderimRegion.CheckUndershadowRules(m, ref global, ref personal);
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			return true;
		}
	}
}
