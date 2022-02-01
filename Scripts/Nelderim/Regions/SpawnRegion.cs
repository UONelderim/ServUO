using System.Xml;

namespace Server.Regions
{
	public class SpawnRegion : NelderimRegion
	{
		public SpawnRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
		}
	}
}
