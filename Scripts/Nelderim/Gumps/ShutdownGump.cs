using System;
using Server.Network;

namespace Server.Gumps
{
	public class ShutdownGump : Gump
	{
		public ShutdownGump(int minutes, AccessLevel accessLevel) : base(200, 200)
		{
			Closable = false;
			Disposable = true;
			Dragable = true;
			Resizable = false;
			AddPage(0);
			AddBackground(0, 0, 310, 100, 9270);
			AddLabel(40, 15, 138, $"Zasniecie swiata nastapi za {minutes} Minut!");
			AddLabel(40, 40, 173, $"{DateTime.Now.Add(TimeSpan.FromMinutes(minutes))}");
			if(accessLevel >= AccessLevel.Administrator) AddButton(180, 38, 0xF2, 0xF1, 1, GumpButtonType.Reply, 0);
			AddLabel(40, 65, 255, "Prosimy wszystkich o rozlaczenie sie");
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			int buttonId = info.ButtonID;
			if (buttonId == 1)
			{
				AdminGump.AdminShutdownTimer?.Stop();
				foreach (var ns in NetState.Instances)
				{
					ns.Mobile?.CloseGump(typeof(ShutdownGump));
					ns.Mobile?.SendMessage(20, "Zasniecie swiata anulowane");
				}
			}
		}
	}
}
