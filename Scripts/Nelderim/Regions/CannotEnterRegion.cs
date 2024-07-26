#region References

using System.Xml;
using Server.Spells;

#endregion

namespace Server.Regions
{
	public class CannotEnterRegion : NBaseRegion
	{
		public CannotEnterRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
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

		public override bool CheckTravel(Mobile m, Point3D p, TravelCheckType type)
		{
			return m.IsStaff();
		}
	}
}
