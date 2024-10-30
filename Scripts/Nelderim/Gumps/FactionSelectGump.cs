using Server.Accounting;
using Server.Commands;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using Server.Targeting;

namespace Server.Nelderim.Gumps
{
	public class FactionSelectGump : Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register("factionselect", AccessLevel.Counselor, ShowGump);
		}

		private static void ShowGump(CommandEventArgs e)
		{
			var from = e.Mobile;
			from.BeginTarget(12,
				false,
				TargetFlags.None,
				(_, targeted) =>
				{
					if (targeted is Mobile m)
					{
						m.SendGump(new FactionSelectGump(m));
					}
				});
		}

		private Mobile _from;
		
		public FactionSelectGump(Mobile from): base(50, 50)
		{
			_from = from;
			_from.Frozen = true;
			
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
			AddPage(0);
			AddBackground(0, 0, 600, 500, 40288);
			AddLabel(260, 20, 47, "Wybor frakcji");
			AddImageTiled(300, 44, 2, 430, 30000);
			AddImageTiled(301, 44, 2, 430, 30000);
			
			AddHtml(20, 40, 280, 30, B(CENTER(FONT("FRAKCJA1", 0x0000BB, 6))), false, false);
			AddHtml(20, 70, 270, 250, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", false, false);
			AddLabel(20, 290, 0, "Dostepne rasy:");
			AddLabel(30, 310, 0, "- Tamael");
			AddLabel(30, 330, 0, "- Jarling");
			AddLabel(30, 350, 0, "- Elf");
			AddLabel(135, 395, 61, "Wybieram");
			AddButton(140, 415, 9004, 9005, 1, GumpButtonType.Reply, 0);
			
			AddHtml(320, 40, 280, 30, B(CENTER(FONT("FRAKCJA2", 0xBB0000, 6))), false, false);
			AddHtml(310, 70, 270, 250, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", false, false);
			AddLabel(310, 290, 0, "Dostepne rasy:");
			AddLabel(320, 310, 0, "- Tamael");
			AddLabel(320, 330, 0, "- Krasnolud");
			AddLabel(320, 350, 0, "- Drow");
			AddLabel(435, 395, 61, "Wybieram");
			AddButton(440, 415, 9004, 9005, 2, GumpButtonType.Reply, 0);
		}
		

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			var acc = _from.Account as Account;
			var fac = Faction.None;
			if (info.ButtonID == 1)
			{
				fac = Faction.West;
			}
			else if (info.ButtonID == 2)
			{
				fac = Faction.East;
			}
			if (info.ButtonID == 0 && _from.IsPlayer())
			{
				_from.SendGump(new FactionSelectGump(_from));
				return;
			}
			if (fac != Faction.None)
			{
				_from.Faction = fac;
				if (acc != null)
				{
					acc.Faction = fac;
				}
			}
			_from.Frozen = false;
			_from.SendMessage("Należysz teraz do frakcji {0}", fac.Name);
			Paperdoll.Send(_from, _from);
			base.OnResponse(sender, info);
		}
	}
}
