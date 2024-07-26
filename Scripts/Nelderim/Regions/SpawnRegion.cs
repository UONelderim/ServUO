#region References

using System.Xml;

#endregion

namespace Server.Regions
{
	public class SpawnRegion : NBaseRegion
	{
		public SpawnRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
		}
	}
}
