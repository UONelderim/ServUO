#region References

using System;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Nelderim;
using Server.Regions;
using System.Text.RegularExpressions;

#endregion

namespace Server.Regions
{
	public class CanEnterRegion : NelderimRegion
	{
		public CanEnterRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			return false;
		}

		public override bool CanEnter(IEntity e)
		{
			if (e is Mobile mobile)
			{
				if (!mobile.IsStaff())
				{
					mobile.SendMessage("Nie mozesz przebywac w tym miejscu");
					return false;
				}
			}

			return base.CanEnter(e);
		}
	}
}
