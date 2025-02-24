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
			AddBackground(0, 0, 600, 540, 40288);
			AddLabel(250, 20, 47, "WYBÓR FRAKCJI");
			AddImageTiled(300, 44, 2, 470, 30000);
			AddImageTiled(301, 44, 2, 470, 30000);
			
			AddHtml(21, 41, 280, 30, B(CENTER(FONT("SOJUSZ ZACHODNI", 0x000000, 6))), false, false);
			AddHtml(20, 40, 280, 30, B(CENTER(FONT("SOJUSZ ZACHODNI", 0x2222CC, 6))), false, false);
			AddHtml(20, 70, 270, 300, "Sojusz Jarlingów, Tamaelów i Elfów powstał z potrzeby ochrony dawnego porządku i zachowania równowagi w świecie stojącym na krawędzi chaosu. Jarlingowie, dumni wojownicy z północy, zawiązali przymierze z elitarnymi wojownikami Tamaelów, którzy wciąż pamiętają dawne przysięgi, oraz z Elfami – mistrzami magii i strategii. Razem postanowili stawić czoła nowemu zagrożeniu. Ich siła tkwi w dzikiej determinacji Jarlingów, mądrości Elfów oraz niezłomnej lojalności Tamaelów wobec tradycji przodków. Celem sojuszu jest obrona starego porządku świata oraz powstrzymanie ekspansji zbuntowanych krasnoludów, Mrocznych Elfów i Tamaelów, którzy porzucili swoje ideały w poszukiwaniu nowej nadziei. Dążą do utrzymania równowagi i dominacji na niezajętych jeszcze terenach, a także do całkowitego unicestwienia sił nieprzyjaciela, które zagrażają stabilności Nelderim.", 
				true, true);
			var racesBaseY = 380;
			AddLabel(20, racesBaseY, 0, "Dostępne rasy:");
			AddLabel(30, racesBaseY + 20, 0, "- Tamael");
			AddLabel(30, racesBaseY + 40, 0, "- Jarling");
			AddLabel(30, racesBaseY + 60, 0, "- Elf");
			AddLabel(135, 435, 61, "Wybieram");
			AddButton(145, 455, 9004, 9005, 1, GumpButtonType.Reply, 0);
			
			
			AddHtml(311, 41, 280, 30, B(CENTER(FONT("SOJUSZ WSCHODNI", 0x000000, 6))), false, false);
			AddHtml(310, 40, 280, 30, B(CENTER(FONT("SOJUSZ WSCHODNI", 0xCC2222, 6))), false, false);
			AddHtml(310, 70, 270, 300, "Sojusz Krasnoludów, Tamaelów i Mrocznych Elfów zrodził się z goryczy, zdrady i pragnienia zemsty. Krasnoludy, porzucone przez sojuszników, odrzuciły złudzenia dawnych przysiąg i w obliczu zdrady postanowiły wziąć sprawy w swoje ręce. W tej chwili słabości niespodziewanym sojusznikiem okazały się Mroczne Elfy – istoty znane z bezwzględności i politycznej przebiegłości. To przymierze oparte na chłodnej kalkulacji, wzajemnych korzyściach i wspólnej ambicji odbudowy utraconej potęgi. Krasnoludy wniosły do niego swoją niezłomną siłę, wytrzymałość i zdolności wojenne, Mroczne Elfy – mistrzowskie zdolności skrytobójcze oraz potężną magię, a Tamaelowie, którzy odwrócili się od dawnego porządku, kierują się zimną logiką i pragnieniem stworzenia nowego świata, uwolnionego od ograniczeń dawnych sojuszy. Ich celem jest zdobycie potęgi, uzyskanie militarnej przewagi i umocnienie swojej pozycji w Nelderim, aż do całkowitej dominacji nad wszystkimi, którzy odważyli się ich zdradzić lub stanąć na drodze ich ambicjom.", 
				true, true);
			AddLabel(310, racesBaseY, 0, "Dostępne rasy:");
			AddLabel(320, racesBaseY + 20, 0, "- Tamael");
			AddLabel(320, racesBaseY + 40, 0, "- Krasnolud");
			AddLabel(320, racesBaseY + 60, 0, "- Mroczny Elf");
			AddLabel(435, 435, 61, "Wybieram");
			AddButton(445, 455, 9004, 9005, 2, GumpButtonType.Reply, 0);
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
