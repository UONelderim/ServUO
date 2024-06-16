using System;
using System.Linq;
using Server.Gumps;
using Server.Network;

namespace Server.Nelderim.Gumps
{
	public class SkillSelectGump : Gump
	{
		public static readonly SkillName[] DefaultExcludedSkills =
		{
			SkillName.ItemID, SkillName.TasteID, SkillName.Begging, SkillName.Spellweaving, SkillName.Mysticism,
			SkillName.Imbuing
		};

		public struct SkillSelectConfiguration
		{
			public string Prompt = "Wybierz umiejetnosc";
			public Action CancelCallback = null;
			public int[] ExcludedSkills = DefaultExcludedSkills.Select(s => (int)s).ToArray();
			public int[] DisabledSkills = Array.Empty<int>();

			public SkillSelectConfiguration()
			{
			}
		}

		private Action<SkillInfo> _callback;
		private SkillSelectConfiguration _config;

		public SkillSelectGump(Action<SkillInfo> callback, SkillSelectConfiguration config = default) : base(0, 0)
		{
			int ENTRY_WIDTH = 200;
			int ENTRY_HEIGHT = 25;
			int MARGIN = 10;
			int PADDING = 5;
			int ROWS = 18;
			int COLUMNS = 3;
			int TEXT_HEIGHT = 18;
			int BUTTON_SIZE = 25;

			_callback = callback;
			_config = config;

			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = true;
			AddPage(0);
			AddBackground(
				0,
				0,
				MARGIN * 2 + PADDING * 2 + COLUMNS * ENTRY_WIDTH,
				MARGIN * 2 + TEXT_HEIGHT + PADDING * 2 + ROWS * ENTRY_HEIGHT,
				9200
			);
			AddLabel(MARGIN, MARGIN, 1153, config.Prompt);
			AddBackground(
				MARGIN,
				MARGIN + TEXT_HEIGHT,
				PADDING * 2 + COLUMNS * ENTRY_WIDTH,
				PADDING * 2 + ROWS * ENTRY_HEIGHT,
				9350
			);

			var x = 0;
			var y = 0;
			var filteredSkills = SkillInfo.Table.Where(s => !config.ExcludedSkills.Contains(s.SkillID));
			var sortedSkills = filteredSkills.OrderBy(s => s.Name).ToArray();
			foreach (var skill in sortedSkills)
			{
				if (config.DisabledSkills.Contains(skill.SkillID))
				{
					AddImage(
						MARGIN + PADDING + x * ENTRY_WIDTH,
						MARGIN + PADDING + TEXT_HEIGHT + y * ENTRY_HEIGHT,
						211
					);
				}
				else
				{
					AddButton(
						MARGIN + PADDING + x * ENTRY_WIDTH,
						MARGIN + PADDING + TEXT_HEIGHT + y * ENTRY_HEIGHT,
						210,
						211,
						skill.SkillID + 1,
						GumpButtonType.Reply,
						0
					);
				}

				AddLabel(
					MARGIN + PADDING + BUTTON_SIZE + x * ENTRY_WIDTH,
					MARGIN + PADDING + TEXT_HEIGHT + y * ENTRY_HEIGHT,
					0,
					skill.Name
				);
				y++;
				if (y >= ROWS)
				{
					x++;
					y = 0;
				}
			}
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			var m = state.Mobile;
			var button = info.ButtonID;
			if (button == 0)
			{
				_config.CancelCallback?.Invoke();
				return;
			}

			_callback.Invoke(SkillInfo.Table[button - 1]);
		}
	}
}
