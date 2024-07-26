#region References

using System;
using System.Xml;

#endregion

namespace Server.Regions
{
	public class CityRegion : NBaseRegion
	{
		public CityRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			return false;
		}

		public override void OnEnter(Mobile m)
		{
			if (this.Name != String.Empty)
				m.SendMessage("Twym oczom ukazuje sie {0}", this.Name);

			base.OnEnter(m);
		}

		public override void OnExit(Mobile m)
		{
			if (this.Name != String.Empty)
				m.SendMessage("Opuszczasz teren {0}", this.Name);

			base.OnExit(m);
		}
	}
}
