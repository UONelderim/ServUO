using System;
using System.Linq;
using Server.Network;
using Server.Items;

namespace Server.Gumps
{
	public class LetterGump : Gump
	{
		private const int LINE_LENGTH = 32;
		private const int LINES_COUNT = 15;
		private const int LINE_HEIGHT = 18;
		
		private Letter _Letter;
 
		public LetterGump(Letter letter) : base(25, 25)
		{
			_Letter = letter;

			AddPage(0);
			var y = 0;
			AddImage(0, y, 0x820);
			y += 37;
			for (var i = 0; i < 4; i++)
			{
				AddImage(17, y, 0x821 + (i & 0x1)); //Interlaced background
				y += 70;
			}
			AddImage(18, y, 0x823);
			for (var i = 0; i < LINES_COUNT; i++)
			{
				var startIndex = i * LINE_LENGTH;
				var initialText = (letter.Text != null && letter.Text.Length > (startIndex + LINE_LENGTH)) ? letter.Text.Substring(startIndex, LINE_LENGTH) : "";
				AddTextEntry(28, 38 + i * LINE_HEIGHT, 220, 18, 0, i, initialText.Trim(), LINE_LENGTH);
			}
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			var joined = String.Join("", info.TextEntries.Select(x => x.Text.PadRight(LINE_LENGTH)));
			_Letter.Text = joined;
		}
	}
}
