#region References

using System.Xml;

#endregion

namespace Server.Regions
{
	public class MiningRegion : NBaseRegion
	{
		public MiningRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
		}

		public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
		{
			global = 28;
		}
	}

	public class LumberRegion : Region
	{
		public LumberRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
		}

		public override void OnEnter(Mobile m)
		{
			m.SendMessage("Drzewa w tutejszych lasach maja charakterystyczna barwe kory.");

			base.OnEnter(m);
		}

		public override void OnExit(Mobile m)
		{
			m.SendMessage("Opuszczasz tereny tych specyficznych lasow.");

			base.OnExit(m);
		}
	}
}
