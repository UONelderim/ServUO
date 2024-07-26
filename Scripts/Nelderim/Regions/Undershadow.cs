#region References

using System;
using System.Xml;

#endregion

namespace Server.Regions
{
	public class Undershadow : NBaseRegion
	{
		public Undershadow(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
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
	}
}
