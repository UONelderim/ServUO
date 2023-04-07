#region References

using System;
using System.Linq;
using Server.Mobiles;
using Server.Network;

#endregion

namespace Server.Gumps
{
	public class QuestPointsHistoryGump : Gump
	{
		private const int pageSize = 21;
		private int _page;
		private PlayerMobile _player;
		public QuestPointsHistoryGump(PlayerMobile player, int page) 
			: base(0, 0)
		{
			_page = page;
			_player = player;
			Dragable = true;
			Closable = true;
			Resizable = false;
			Disposable = false;

			AddPage(0);
			AddBackground(0, 0, 700, 600, 5054);
            
			AddImageTiled(10, 10, 85, 550, 1416); 
			AddImageTiled(171, 10, 85, 550, 1416); 
			AddImageTiled(306, 10, 386, 550, 1416); 

			AddLabel(13, 13, 0x480, "Kiedy");
			AddLabel(98, 13, 0x480, "GM");
			AddLabel(173, 13, 0x480, "Postac");
			AddLabel(258, 13, 0x480, "Punkty");
			AddLabel(308, 13, 0x480, "Powód");

			var y = 38;
			var startIndex = page * pageSize;
			foreach (var qphe in player.QuestPointsHistory.Reverse().ToList().GetRange(startIndex, Math.Min(pageSize, player.QuestPointsHistory.Count - startIndex)))
			{
				AddLabel(13, y, 0x480, qphe.DateTime.ToShortDateString());
				AddLabel(98, y, 0x480, qphe.GameMaster);
				AddLabel(173, y, 0x480, qphe.CharName);
				AddLabel(258, y, 0x480, qphe.Points.ToString("+#;-#;0")); //Formatting to always add sign
				AddLabelCropped(308, y, 386, 18, 0x480, qphe.Reason);
				y += 25;
			}
			
			AddLabel(400, 568, 0x480, $"Strona: {page + 1}/{(player.QuestPointsHistory.Count / pageSize) + 1}");

			if (page > 0)
			{
				AddButton(500, 568, 0x15E3,0x15E7, 1,GumpButtonType.Reply, 0);
			}

			if (startIndex + pageSize < player.QuestPointsHistory.Count)
			{
				AddButton(550, 568, 0x15E1,0x15E5, 2, GumpButtonType.Reply, 0);
			}
			
			AddButton(658, 568, 4005, 4007, 0, GumpButtonType.Reply, 0);
			AddLabel(600, 568, 925, "Zamknij");
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			int buttonId = info.ButtonID;
			if (buttonId == 1)
			{
				sender.Mobile.SendGump(new QuestPointsHistoryGump(_player, _page - 1));
			}
			if (buttonId == 2)
			{
				sender.Mobile.SendGump(new QuestPointsHistoryGump(_player, _page + 1));
			}
		}
	}
}
