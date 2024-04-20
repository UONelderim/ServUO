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
		private PlayerMobile _from;
		private PlayerMobile _player;
		private int _page;

		public QuestPointsHistoryGump(PlayerMobile from, PlayerMobile player, int page) 
			: base(0, 0)
		{
			_from = from;
			_player = player;
			_page = page;
			var gmRequested = from.AccessLevel > AccessLevel.VIP;
			Dragable = true;
			Closable = true;
			Resizable = false;
			Disposable = false;

			AddPage(0);
			var padding = 10;
			var dateWidth = 85;
			var gmWidth = 75;
			var charWidth = 85;
			var pointsWidth = 50;
			AddBackground(0, 0, 700, 600, 5054);
			var x = 10;
			AddImageTiled(x, 10, 85, 550, 1416);
			x += dateWidth + (gmRequested ? gmWidth : charWidth); 
			AddImageTiled(x, 10, gmRequested ? charWidth : pointsWidth, 550, 1416);
			if (gmRequested)
			{
				x += charWidth + pointsWidth;
				AddImageTiled(x, 10, 386, 550, 1416);
			}

			x = 13;
			AddLabel(x, 13, 0x480, "Kiedy");
			x += dateWidth;
			if (gmRequested)
			{
				AddLabel(x, 13, 0x480, "GM");
				x += gmWidth;
			}
			AddLabel(x, 13, 0x480, "Postac");
			x += charWidth;
			AddLabel(x, 13, 0x480, "Punkty");
			x += pointsWidth;
			AddLabel(x, 13, 0x480, "Powód");

			var y = 38;
			var startIndex = page * pageSize;
			foreach (var qphe in player.QuestPointsHistory.Reverse().ToList().GetRange(startIndex, Math.Min(pageSize, player.QuestPointsHistory.Count - startIndex)))
			{
				x = 13;
				AddLabel(x, y, 0x480, qphe.DateTime.ToShortDateString());
				x += dateWidth;
				if(gmRequested)
				{
					AddLabel(x, y, 0x480, qphe.GameMaster);
					x += gmWidth;
				}
				AddLabel(x, y, 0x480, qphe.CharName);
				x += charWidth;
				AddLabel(x, y, 0x480, qphe.Points.ToString("+#;-#;0")); //Formatting to always add sign
				x += pointsWidth;
				AddLabelCropped(x, y, 386, 18, 0x480, qphe.Reason);
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
				sender.Mobile.SendGump(new QuestPointsHistoryGump(_from, _player, _page - 1));
			}
			if (buttonId == 2)
			{
				sender.Mobile.SendGump(new QuestPointsHistoryGump(_from, _player, _page + 1));
			}
		}
	}
}
