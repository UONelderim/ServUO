using System;
using System.Linq;
using Server.Network;
using Server.Items;

namespace Server.Gumps
{
	public class LetterGump : Gump
	{
		// private const int LINE_LENGTH = 32;
		
		private Letter _Letter;

		public LetterGump(Letter letter) : base(25, 25)
		{
			var LINE_LENGTH = 32;
			_Letter = letter;

			AddPage(0);
			AddBackground(0, 0, 260, 320, 1579);
			for (var i = 0; i < 15; i++)
			{
				var startIndex = i * LINE_LENGTH;
				var initialText = (letter.Text != null && letter.Text.Length > (startIndex + LINE_LENGTH)) ? letter.Text.Substring(startIndex, LINE_LENGTH) : "";
				AddTextEntry(20, 20 + i * 18, 220, 18, 0, i, initialText.Trim(), LINE_LENGTH);
			}
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			var joined = String.Join("", info.TextEntries.Select(x => x.Text.PadRight(32)));
			_Letter.Text = joined;
		}
	}
}
