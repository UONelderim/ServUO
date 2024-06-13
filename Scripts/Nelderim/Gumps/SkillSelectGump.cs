using System;
using System.Linq;
using Server.Gumps;
using Server.Network;

namespace Server.Nelderim.Gumps
{
	public class SkillSelectGump : Gump
	{
		public static readonly SkillName[] ExcludedSkills =
		{
			SkillName.ItemID, SkillName.TasteID, SkillName.Begging, SkillName.Spellweaving, SkillName.Mysticism,
			SkillName.Imbuing
		};

		private Action<SkillInfo> _callback;

		public SkillSelectGump(Action<SkillInfo> callback) : this(callback, ExcludedSkills)
		{
		}
		
		public SkillSelectGump(Action<SkillInfo> callback, SkillName[] excludedSkills) : base(0, 0)
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
			AddLabel(MARGIN, MARGIN, 1153, "Wybierz umiejetnosc");
			AddBackground(
				MARGIN,
				MARGIN + TEXT_HEIGHT,
				PADDING * 2 + COLUMNS * ENTRY_WIDTH,
				PADDING * 2 + ROWS * ENTRY_HEIGHT,
				9350
			);

			var x = 0;
			var y = 0;
			for (var index = 0; index < SkillInfo.Table.Length; index++)
			{
				if (excludedSkills.Contains((SkillName)index))
				{
					continue;
				}

				AddButton(
					MARGIN + PADDING + x * ENTRY_WIDTH,
					MARGIN + PADDING + TEXT_HEIGHT + y * ENTRY_HEIGHT,
					210,
					211,
					index + 1,
					GumpButtonType.Reply,
					0
				);
				AddLabel(
					MARGIN + PADDING + BUTTON_SIZE + x * ENTRY_WIDTH,
					MARGIN + PADDING + TEXT_HEIGHT + y * ENTRY_HEIGHT,
					0,
					SkillInfo.Table[index].Name
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
				m.SendMessage("Wybor anulowany");
				return;
			}

			_callback.Invoke(SkillInfo.Table[button - 1]);
		}
	}
}
