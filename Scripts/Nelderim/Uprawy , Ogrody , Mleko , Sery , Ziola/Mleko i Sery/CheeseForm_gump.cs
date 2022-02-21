// by Alari - template copied from milkbucket gump.

#region References

using Server.Network;

#endregion

namespace Server.Gumps
{
	public class CheeseFormHelpGump : Gump
	{
		private object m_State;

		public CheeseFormHelpGump(object state) : base(30, 30)
		{
			Closable = true;
			Dragable = true;

			AddPage(0);

			AddBackground(0, 0, 400, 300, 5054);

			Add(new GumpHtml(10, 10, 380, 280,
				"<p><b><u>Mleczny System : Forma Na Ser</u></b></p><p>Pomoc : </p><p>1 - Dwoklik na forme i zaznaczenie pojemnika z mlekiem powoduje rozpoczecie fermetacji.<br>Mozesz zrobic ser z krowiego ,koziego i owczego mleka. Potrzebujesz 15 litrow mleka aby zrobic ser.</p><p>2 - Ser jest gotowy do fermetacji.<br>Dwoklik na forme rozpoczyna proces fermetacji.<br>Jesli proces sie rozpocza ,jego stan pokazany jest w %.</p><p>3 - Kiedy ser juz jest gotow kliknij na forme aby uzyskac nowy ser.<br>Masz szanse na uzyskanie magicznego sera. Masz takze szanse ze nie uda ci sie zrobic zadnego sera. Niewielki skill gotowania pomoze osiagnac ci lepsze efekty.</p><p>Jesli to czytasz to dlatego ze:</p><p>- Forma na ser jest pelna a TY prubujesz ja napelnic.<br>- W pojemniku na mleko nie ma wystarczajacej ilosci mleka.<br>- Wybrals niedozwolony cel.</p>",
				true, true));
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
		}
	}
}
