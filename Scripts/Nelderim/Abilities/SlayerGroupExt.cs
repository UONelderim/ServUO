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
			AddSuperTypes(undead, typeof(SaragAwatar), typeof(NSarag));
			AddSuperTypes(elemental, typeof(AgapiteColossus), typeof(BronzeColossus),
				typeof(BronzeColossus), typeof(DullCopperColossus), typeof(GoldenColossus), typeof(ShadowIronColossus),
				typeof(ValoriteColossus), typeof(VeriteColossus));
			AddEntryTypes(elemental, SlayerName.EarthShatter, typeof(AgapiteColossus), typeof(BronzeColossus),
				typeof(BronzeColossus), typeof(DullCopperColossus), typeof(GoldenColossus), typeof(ShadowIronColossus),
				typeof(ValoriteColossus), typeof(VeriteColossus));
		}

		private static void AddSuperTypes(SlayerGroup group, params Type[] newTypes)
		{
			group.Super = new SlayerEntry(group.Super.Name, group.Super.Types.Concat(newTypes).ToArray());
		}

		private static void AddEntryTypes(SlayerGroup group, SlayerName slayerName, params Type[] newTypes)
		{
			for (int i = 0; i < group.Entries.Length; i++)
			{
				if (group.Entries[i].Name == slayerName)
				{
					var slayerEntry = group.Entries[i];
					group.Entries[i] = new SlayerEntry(slayerEntry.Name, slayerEntry.Types.Concat(newTypes).ToArray());
				}
			}
		}
	}
}
