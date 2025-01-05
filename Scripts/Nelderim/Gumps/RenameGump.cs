using System;
using System.Linq;
using Knives.TownHouses;
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

		public RenameGump(bool female = false, string reason = "") : base(50, 50)
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
			AddPage(0);
			AddBackground(0, 0, 250, 160, 2620);
			
			AddHtml(0, 15, 250, 20, FONT(CENTER("Nadaj tożsamość twojej postaci"), 0xFFFF00), false, false);
			
			AddLabel(17, 45, 1160, "Imie:");
			AddImageTiled(50, 45, 185, 20, 0xBBC);
			AddTextEntry(52, 45, 183, 20, 1359, 0, "", 30);
			
			AddRadio(17, 70, 9721, 0x86A, !female, 1);
			AddLabel(52, 75, 1160, "Mężczyzna");
			AddRadio(137, 70, 9721, 0x86A, female, 2); 
			AddLabel(172, 75, 1160, "Kobieta");
			
			AddHtml(0, 102, 250, 20,FONT(CENTER(reason), 0xFF0000), false, false);
			
			AddButton(95, 125, 238, 239, 2, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			var m = sender.Mobile;
			var newName = info.GetTextEntry(0).Text;
			var female = info.Switches.Length > 0 && info.Switches[0] == 2;
			if (info.ButtonID == 0)
			{
				if (m.IsPlayer())
					m.SendGump(new RenameGump());
			}
			else if (newName.Length <= 3)
			{
				m.SendGump(new RenameGump(female, "Imie jest za krótkie"));
			}
			else if (newName.StartsWith(DEFAULT_PREFIX))
			{
				m.SendGump(new RenameGump(female, "Imie jest niedozwolone"));
			}
			else
			{
				var sanitisedName = String.Join(' ', newName.Split(' ').Select(Translate.CapitalizeFirstLetter));
				m.Name = sanitisedName;
				m.Female = female;
				m.Frozen = false;
				m.BodyValue = m.Female ? 0x191 : 0x190;
				Paperdoll.Send(m, m);
				NewCharSetupGump.Check(m);
				m.SendMessage(0x40, "Nadales postaci nowa tozsamosc");
			}
		}
	}
}
