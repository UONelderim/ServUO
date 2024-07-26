#region References

using System;
using System.Xml;

#endregion

namespace Server.Regions
{
	public class TavernRegion : NBaseRegion
	{
		public TavernRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			return false;
		}

		public override void OnEnter(Mobile m)
		{
			if (this.Name != String.Empty)
				m.SendMessage("Witam w karczmie {0}", Name);

			base.OnEnter(m);
		}
	}
}
