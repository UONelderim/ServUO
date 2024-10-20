#region References

using System;
using System.Xml;
using Server.Spells;

#endregion

namespace Server.Regions
{
	public class TravelRegion : NBaseRegion
	{
		public TravelRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			return false;
		}

		public override void OnEnter(Mobile m)
		{
			if (this.Name != String.Empty)
				m.SendMessage("Wkroczyles w miejsce, ktorego aura wzmaga zdolnosci translokacyjne.");

			base.OnEnter(m);
		}

		public override void OnExit(Mobile m)
		{
			if (this.Name != String.Empty)
				m.SendMessage("Opuszczasz miejsce, ktorego aura wzmaga zdolnosci translokacyjne.");

			base.OnExit(m);
		}

		public override bool CheckTravel(Mobile traveller, Point3D p, TravelCheckType type)
		{
			if (traveller.Race != Race.NDrow)
			{
				return true;
			}
			return type is TravelCheckType.TeleportFrom or TravelCheckType.TeleportTo;
		}
	}

	public class UndershadowTravelRegion : TravelRegion
	{
		public UndershadowTravelRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent) { }
		
		public override bool CheckTravel(Mobile traveller, Point3D p, TravelCheckType type)
		{
			if (traveller.Race == Race.NDrow)
			{
				return true;
			}
			return type is TravelCheckType.TeleportFrom or TravelCheckType.TeleportTo;
		}
	}
}
