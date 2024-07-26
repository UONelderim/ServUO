#region References

using System;
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
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage(0);

			var allLanguages = Enum.GetValues(typeof(NLanguage));
			
			AddBackground(0, 0, 200, 50 + allLanguages.Length * 30, 9260);
			AddLabel(17, 17, 0, @"Wybierz język");
			int y = 40;
			foreach (NLanguage lang in allLanguages)
			{
				
				if (from.LanguageSpeaking == lang || lang == NLanguage.Belkot || from.LanguagesKnown[lang] > 300)
				{
					AddButton(20, y, from.LanguageSpeaking == lang ? 4006 : 4005, 4007, (int)lang + 2,
						GumpButtonType.Reply,
						1);
				}
				else
				{
					AddImage(20, y, 4022, 901);
				}

				AddLabel(60, y, 0, $"{(float)(from.LanguagesKnown[lang])/10:f1}");
				AddLabel(100, y, 0 ,Enum.GetName(typeof(NLanguage), lang));
				y += 30;
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;
			int buttonID = info.ButtonID;
			if (buttonID > 0)
			{
				if (from is PlayerMobile)
				{
					PlayerMobile pm = (PlayerMobile)from;
					NLanguage langToSpeak = (NLanguage)info.ButtonID - 2;
					if (pm.LanguagesKnown[langToSpeak] < 200)
						langToSpeak = NLanguage.Belkot;
					pm.LanguageSpeaking = langToSpeak;
					pm.SendGump(new LanguagesGump(pm));
				}
			}
		}
	}
}
