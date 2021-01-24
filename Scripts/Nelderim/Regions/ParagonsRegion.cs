using System.Xml;

namespace Server.Regions
{
	public class ParagonsRegion : Region
	{
		public ParagonsRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent) {
		}
		public override void OnEnter(Mobile m) {
			if (m.AccessLevel > AccessLevel.Player)
				m.SendMessage("Wkroczyles na obszar, gdzie jest szansa znalezienia paragonow.");
		}

		public override void OnExit(Mobile m) {
			if (m.AccessLevel > AccessLevel.Player)
				m.SendMessage("Opusciles obszar, gdzie jest szansa znalezienia paragonow.");
		}
	}
}


