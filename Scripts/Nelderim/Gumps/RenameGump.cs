using System;
using System.Linq;
using Nelderim;
using Server.Commands;
using Server.Misc;
using Server.Mobiles;
using Server.Nelderim.Gumps;
using Server.Network;
using Server.Targeting;

namespace Server.Gumps
{
	public class RenameGump : Gump
	{
		public const string DEFAULT_PREFIX = "Postac";

		public static void Initialize()
		{
			CommandSystem.Register("rename",
				AccessLevel.GameMaster,
				e =>
				{
					e.Mobile.BeginTarget(16,
						false,
						TargetFlags.None,
						(from, targeted) =>
						{
							if (targeted is PlayerMobile pm)
							{
								if(pm.IsPlayer())
									pm.Frozen = true;
								pm.SendGump(new RenameGump());
							}
						});
				});
		}

		public RenameGump(string reason = "") : base(50, 50)
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
			AddPage(0);
			AddBackground(50, 50, 250, 150, 2620);
			AddLabel(100, 67, 1160, "Nadaj imię twojej postaci");
			AddLabel(110, 87, 0x20, reason);
			AddImageTiled(70, 115, 210, 20, 0xBBC);
			AddTextEntry(72, 115, 206, 20, 1359, 0, "", 30);
			AddButton(145, 160, 238, 239, 2, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			var m = sender.Mobile;
			var newName = info.GetTextEntry(0).Text;
			if (info.ButtonID == 0)
			{
				if (m.IsPlayer())
					m.SendGump(new RenameGump());
			}
			else if (newName.Length <= 3)
			{
				m.SendGump(new RenameGump("Imie jest za krótkie"));
			}
			else if (newName.StartsWith(DEFAULT_PREFIX))
			{
				m.SendGump(new RenameGump("Imie jest niedozwolone"));
			}
			else
			{
				var sanitisedName = String.Join(' ', newName.Split(' ').Select(Translate.CapitalizeFirstLetter));
				m.Name = sanitisedName;
				m.Frozen = false;
				Paperdoll.Send(m, m);
				NewCharSetupGump.Check(m);
			}
		}
	}
}
