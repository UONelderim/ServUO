#region References

using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

#endregion

namespace Nelderim
{
	public class LanguagesGump : Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register("jezyk", AccessLevel.Player, Jezyk_OnCommand);
		}

		[Usage("Usage: Jezyk")]
		[Description("Ustawaia jezyk jakim bedzie mowic postac.")]
		public static void Jezyk_OnCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;

			if (from is PlayerMobile)
			{
				PlayerMobile pm = (PlayerMobile)from;
				pm.SendGump(new LanguagesGump(pm));
			}
		}

		public LanguagesGump(PlayerMobile from) : base(0, 0)
		{
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;

			AddPage(0);

			List<Language> languages = from.LanguagesKnown.List;
			AddBackground(0, 0, 200, 50 + languages.Count * 30, 9260);
			AddLabel(17, 17, 0, @"Wybierz język");
			int y = 40;
			foreach (Language lang in languages)
			{
				AddButton(20, y, from.LanguageSpeaking == lang ? 4006 : 4005, 4007, (int)lang + 1, GumpButtonType.Reply,
					1);
				AddLabel(60, y, 0, Enum.GetName(typeof(Language), lang));
				y += 30;
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;
			int buttonID = info.ButtonID;
			if (buttonID > 0)
			{
				// ButtonID 0 to zamkniecie gumpa
				if (from is PlayerMobile)
				{
					PlayerMobile pm = (PlayerMobile)from;
					Language langToSpeak = (Language)info.ButtonID - 1;
					pm.LanguageSpeaking = langToSpeak;
					pm.SendGump(new LanguagesGump(pm));
				}
			}
		}
	}
}
