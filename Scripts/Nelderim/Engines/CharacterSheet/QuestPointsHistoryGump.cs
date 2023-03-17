#region References

using Nelderim;
using Server.Mobiles;

#endregion

namespace Server.Gumps
{
	public class QuestPointsHistoryGump : Gump
	{
		public QuestPointsHistoryGump(PlayerMobile player) 
			: base(0, 0)
		{
			Dragable = true;
			Closable = true;
			Resizable = false;
			Disposable = false;

			AddPage(0);
			AddBackground(0, 0, 600, 600, 5054);
            
			AddImageTiled(10, 10, 85, 550, 1416); 
			AddImageTiled(171, 10, 60, 550, 1416); 
			AddLabel(13, 13, 0x480, "Kiedy");
			AddLabel(98, 13, 0x480, "GM");
			AddLabel(173, 13, 0x480, "Punkty");
			AddLabel(233, 13, 0x480, "Powód");

			int y = 38;
			foreach (var qphe in player.QuestPointsHistory.Reverse())
			{
				AddLabel(13, y, 0x480, qphe.DateTime.ToShortDateString());
				AddLabel(98, y, 0x480, qphe.GameMaster);
				AddLabel(173, y, 0x480, qphe.Points.ToString("+#;-#;0")); //Formatting to always add sign
				AddLabelCropped(233, y, 350, 18, 0x480, qphe.Reason);
				y += 25;
				if (y > 560) break;
			}
            
			AddButton(558, 568, 4005, 4007, 0, GumpButtonType.Reply, 0);
			AddLabel(500, 568, 925, "Zamknij");
		}
	}
}
