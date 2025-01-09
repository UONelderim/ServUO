using Server.Gumps;
using Server.Misc;
using Server.Regions;

namespace Server.Nelderim.Gumps
{
	public class ProfileSetupGump : Gump
	{
		public static void Initialize()
		{
			EventSink.ChangeProfileRequest += e => Check(e.Beholder, true);
			EventSink.Login += e => Check(e.Mobile);
		}
		
		public static void Check(Mobile m, bool showErrorMessage = false)
		{
			m.CloseGump<ProfileSetupGump>();
			
			if (m.IsPlayer() && (m.Region is RaceRoomRegion || m.Region is Jail) && (m.Profile == null || m.Profile.Trim().Length < 50 ))
			{
				m.Frozen = true;
				if(m.Profile == null || m.Profile.Trim().Length == 0)
					m.Profile = "...";
				m.SendGump(new ProfileSetupGump(m, showErrorMessage));
			}
			else
			{
				m.Frozen = false;
			}
		}
		
		private Mobile _from;
		
		public ProfileSetupGump(Mobile from, bool showErrorMessage = false): base(320, 25)
		{
			_from = from;
			
			Profile.EventSink_ProfileRequest(new ProfileRequestEventArgs(_from, _from));
			
			Closable = false;
			Disposable = true;
			Dragable = false;
			Resizable = false;
			AddPage(0);
			AddBackground(0, 0, 550, 480, 302);
			AddImage(15,10, 4506);
			AddImage(15,100, 4506);
			AddImage(15,190, 4506);
			AddImage(10,270, 4507);
			AddHtml(165, 25, 300, 20, B(FONT("UZUPEŁNIJ OPIS POSTACI", 0x00AA00, 8)), false, false);
			AddHtml(80, 50, 480, 300, "Każda postać musi mieć uzupełniony opis zewnętrzny postaci. <br>" +
			                          "Powinien to być ogólny opis wyglądu i cech charakterystycznych widocznych gołym okiem, " +
			                          "pozbawiony informacji na temat charakteru, usposobienia czy pochodzenia postaci " +
			                          "– tylko to, co widać, słychać i czuć.", false, false);
			
			if (showErrorMessage)
			{
				AddHtml(80, 135, 300, 20, B(FONT("!!! OPIS JEST ZA KRÓTKI !!!",0xCC0000, 0)), false, false);
			}
			var y = 160;
			AddLabel(80, PostInc(ref y), 0, "Co może zawierać opis postaci:");
			AddLabel(90, PostInc(ref y), 66, "- opis twarzy: oczy, włosy, zarost, ewentualne blizny, nos");
			AddLabel(90, PostInc(ref y), 66, "- przybliżony wiek (np: pełen wigoru młodzian lub kulawy starzec)");
			AddLabel(90, PostInc(ref y), 66, "- opis postury, przybliżony wzrost (np: średniego wzrostu kobieta)");
			AddLabel(90, PostInc(ref y), 66, "- charakterystyczne znaki na ciele (blizny, tatuaże)");
			AddLabel(90, PostInc(ref y, 40), 66, "- dodatkowe: zapach, rodzaj spojrzenia, styl chodu");
			AddLabel(80, PostInc(ref y), 0, "Czego nie powinien zawierać opis postaci:");
			AddLabel(90, PostInc(ref y), 37, "- historii postaci");
			AddLabel(90, PostInc(ref y), 37, "- celów i dążeń postaci");
			AddLabel(90, PostInc(ref y, 40), 37, "- wykonywanego zawodu");
			AddLabel(72, y, 66, "!");
			AddLabel(90, PostInc(ref y, 30), 43, "Zamknij opis postaci aby zatwierdzić zmianę.");
			AddLabel(70, y, 66, "?");
			// AddHtml(80, y, 20, 20, B(FONT("?", 0x00ff00, 0)), false, false);
			AddLabel(90, PostInc(ref y, -35), 43, "Opis postaci można edytować otwierajac lewy zwój w paperdoll.");
			AddImage(485, y - 18, 2437);
			AddImage(480, y, 2002);
			AddImage(499, y, 2002);
		}

		private int PostInc(ref int val, int inc = 20)
		{
			var oldVal = val;
			val += inc;
			return oldVal;
		}
	}
}
