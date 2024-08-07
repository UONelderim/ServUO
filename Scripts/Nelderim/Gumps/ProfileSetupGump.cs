using Server.Gumps;
using Server.Misc;
using Server.Network;
using Server.Regions;

namespace Server.Nelderim.Gumps
{
	public class ProfileSetupGump : Gump
	{
		public static void Initialize()
		{
			EventSink.ChangeProfileRequest += e => Apply(e.Beheld);
		}
		
		public static void Apply(Mobile m)
		{
			m.CloseGump<ProfileSetupGump>();
			if (m.Region is not RaceRoomRegion)
				return;
			
			if (m.Profile.Trim().Length < 50 )
			{
				m.Frozen = true;
				if(m.Profile.Trim().Length == 0)
					m.Profile = "...";
				m.SendGump(new ProfileSetupGump(m));
			}
			else
			{
				m.Frozen = false;
			}
		}
		
		private Mobile _from;
		
		public ProfileSetupGump(Mobile from): base(320, 25)
		{
			_from = from;
			
			Profile.EventSink_ProfileRequest(new ProfileRequestEventArgs(_from, _from));
			
			Closable = false;
			Disposable = true;
			Dragable = false;
			Resizable = false;
			AddPage(0);
			AddBackground(0, 0, 550, 450, 302);
			AddImage(15,10, 4506);
			AddImage(15,100, 4506);
			AddImage(15,190, 4506);
			AddImage(10,270, 4507);
			AddHtml(165, 25, 300, 20, B(FONT("UZUPEŁNIJ OPIS POSTACI", 0x00AA00, 8)), false, false);
			AddHtml(80, 50, 480, 300, "Każda postać musi mieć uzupełniony opis zewnętrzny postaci. <br>" +
			                          "Powinien to być ogólny opis wyglądu i cech charakterystycznych widocznych gołym okiem, " +
			                          "pozbawiony informacji na temat charakteru, usposobienia czy pochodzenia postaci " +
			                          "– tylko to, co widać, słychać i czuć.", false, false);
			AddLabel(80, 150, 0, "Co może zawierać opis postaci:");
			AddLabel(90, 170, 66, "- opis twarzy: oczy, włosy, zarost, ewentualne blizny, nos");
			AddLabel(90, 190, 66, "- przybliżony wiek (np: pełen wigoru młodzian lub kulawy starzec)");
			AddLabel(90, 210, 66, "- opis postury, przybliżony wzrost (np: średniego wzrostu kobieta)");
			AddLabel(90, 230, 66, "- charakterystyczne znaki na ciele (blizny, tatuaże)");
			AddLabel(90, 250, 66, "- dodatkowe: zapach, rodzaj spojrzenia, styl chodu");
			AddLabel(80, 290, 0, "Czego nie powinien zawierać opis postaci:");
			AddLabel(90, 310, 37, "- historii postaci");
			AddLabel(90, 330, 37, "- celów i dążeń postaci");
			AddLabel(90, 350, 37, "- wykonywanego zawodu");
			AddLabel(90, 390, 42, "Zamknij opis postaci aby zatwierdzić zmianę.");
		}
		

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (info.ButtonID == 0)
				Apply(_from);
		}
	}
}
