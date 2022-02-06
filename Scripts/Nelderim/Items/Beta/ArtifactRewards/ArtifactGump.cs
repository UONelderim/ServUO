using System;
using System.Collections.Generic;
using Server.Network;

namespace Server.Gumps
{
	public abstract class ArtifactGump : Gump
	{
		protected Item m_Deed;

		protected virtual SortedList<string, Type[]> artifacts { get; }

		protected virtual Type gumpType { get; }

		public ArtifactGump(Mobile from, Item deed) : base(30, 20)
		{
			m_Deed = deed;

			AddPage(1);

			AddBackground(0, 0, 300, 400, 5054);
			AddBackground(8, 8, 284, 384, 3000);

			AddLabel(40, 12, 0, "Artifact List");

			for (var catIndex = 0; catIndex < artifacts.Keys.Count; catIndex++)
			{
				var category = artifacts.Keys[catIndex];
				var yOffset = catIndex * 20;
				AddLabel(52, 40 + yOffset, 0, $"{category} Menu");
				AddButton(12, 40 + yOffset, 4005, 4007, 0, GumpButtonType.Page, 2 + catIndex); // Page starts from two
			}

			AddLabel(52, 340, 0, "Close");
			AddButton(12, 340, 4005, 4007, 0, GumpButtonType.Reply, 0);


			for (var catIndex = 0; catIndex < artifacts.Keys.Count; catIndex++)
			{
				var category = artifacts.Keys[catIndex];
				AddPage(2 + catIndex);

				AddBackground(0, 0, 300, 400, 5054);
				AddBackground(8, 8, 284, 384, 3000);

				AddLabel(40, 12, 0, $"{category} List");

				for (var artIndex = 0; artIndex < artifacts[category].Length; artIndex++)
				{
					var type = artifacts[category][artIndex];
					var yOffset = artIndex * 20;
					AddLabel(52, 40 + yOffset, 0, Utility.SplitCamelCase(type.Name));
					AddButton(12, 40 + yOffset, 4005, 4007, (1 + catIndex) * 1000 + artIndex, GumpButtonType.Reply, 0);
				}

				AddLabel(52, 340, 0, "Main Menu");
				AddButton(12, 340, 4005, 4007, 0, GumpButtonType.Page, 1);
			}
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			if (info.ButtonID == 0)
			{
				from.CloseGump(gumpType);
				return;
			}

			var categoryIndex = (info.ButtonID / 1000) - 1;
			var artifactIndex = info.ButtonID % 1000;

			if (categoryIndex < artifacts.Count)
			{
				var category = artifacts.Keys[categoryIndex];
				if (artifactIndex < artifacts[category].Length)
				{
					var type = artifacts[category][artifactIndex];
					var item = Activator.CreateInstance(type) as Item;
					if (item != null)
					{
						from.AddToBackpack(item);
						from.CloseGump(gumpType);
						m_Deed.Delete();
					}
				}
			}
		}
	}
}
