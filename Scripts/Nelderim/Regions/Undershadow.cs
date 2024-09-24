#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#endregion

namespace Server.Regions
{
	public class Undershadow : NBaseRegion
	{
		public static List<Undershadow> _regions = [];
		
		public Undershadow(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
			_regions.Add(this);
		}
		
		public override bool AllowHousing(Mobile from, Point3D p)
		{
			return false;
		}

		public override void OnEnter(Mobile m)
		{
			if (Name != String.Empty)
				m.SendMessage("Twym oczom ukazuje sie {0}", PrettyName);

			base.OnEnter(m);
		}

		public override void OnExit(Mobile m)
		{
			if (Name != String.Empty)
				m.SendMessage("Opuszczasz teren {0}", PrettyName);

			base.OnExit(m);
		}

		public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
		{
			global = LightCycle.DungeonLevel;
		}

		public static bool Contains(Map m, Point3D p)
		{
			return _regions.Any(region => region.Map == m && region.Contains(p));
		}
	}
}
