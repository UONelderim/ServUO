using System;
using Server;
using Server.Items;
using Server.Network;
using Solaris.BoardGames;

namespace Server.Gumps
{
	//oferuje graczowi oczekiwanie na dołączenie do gry
	public class AwaitRecruitmentGump : BoardGameGump
	{
		public override int Height { get { return 200; } }
		public override int Width  { get { return 400; } }

		public AwaitRecruitmentGump(Mobile owner, BoardGameControlItem controlitem) : base(owner, controlitem)
		{
			//zablokuj zamykanie tego gumpa
			Closable = false;

			AddLabel(40, 20, 1152, "Gra:");
			AddLabel(140, 20, 1172, _ControlItem.GameName);

			AddHtml(
				40, 50,
				Width - 80, 80,
				"Czekasz na dołączenie kolejnych graczy do tej gry. Gdy będzie ich wystarczająco dużo, to okno zamknie się automatycznie, a gra się rozpocznie. Jeśli chcesz anulować oczekiwanie, kliknij przycisk Anuluj.",
				true, false
			);

			AddButton(160, 160, 0xF1, 0xF2, 1, GumpButtonType.Reply, 0);
		}

		protected override void DeterminePageLayout()
		{
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			int buttonid = info.ButtonID;

			//przycisk Anuluj
			if (buttonid == 1)
			{
				_Owner.CloseGump(typeof(SelectStyleGump));
				_ControlItem.RemovePlayer(_Owner);

				_Owner.SendMessage("Nie czekasz już na rozpoczęcie tej gry.");
			}
		}
	}
}
