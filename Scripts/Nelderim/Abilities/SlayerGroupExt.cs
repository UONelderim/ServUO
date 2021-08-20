using System;
using System.Linq;
using Server.Mobiles;

namespace Server.Items
{
	public partial class SlayerGroup
	{
		private static void FillNelderimSlayerEntries(ref SlayerGroup humanoid, ref SlayerGroup undead,
			ref SlayerGroup elemental,
			ref SlayerGroup abyss, ref SlayerGroup arachnid, ref SlayerGroup reptilian, ref SlayerGroup fey,
			ref SlayerGroup eodon,
			ref SlayerGroup eodonTribe, ref SlayerGroup dino, ref SlayerGroup myrmidex)
		{
			AddTypes(ref undead, typeof(SaragAwatar), typeof(NSarag));
		}

		private static void AddTypes(ref SlayerGroup group, params Type[] types)
		{
			group.Super = new SlayerEntry(group.Super.Name, group.Super.Types.Concat(types).ToArray());
		}
	}
}
