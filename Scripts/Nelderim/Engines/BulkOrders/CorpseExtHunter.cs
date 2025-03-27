using System.Collections.Generic;
using Server.Engines.BulkOrders;

namespace Server.Items
{
	public partial class Corpse
	{
		public List<Mobile> HasLootingRights = [];
		public List<Mobile> Hunters = [];
		public List<SmallHunterBOD> HunterBods = [];
	}
}
