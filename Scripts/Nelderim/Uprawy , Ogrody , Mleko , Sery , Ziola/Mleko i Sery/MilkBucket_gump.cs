#region References

using Server.Network;

#endregion

namespace Server.Gumps
{
	public class LaitageHelpGump : Gump
	{
		private object m_State;

		public LaitageHelpGump(object state) : base(30, 30)
		{
			Closable = true;
			Dragable = true;

			AddPage(0);

			AddBackground(0, 0, 400, 300, 5054);

			Add(new GumpHtml(10, 10, 380, 280,
				"<p><b><u>Mleczny System : Pojemnik Na Mleko</u></b></p><p>Pomoc : </p><p>1 - Dwoklik na pusty pojemnik i zaznaczenie zwierzecia rozpoczyna dojenie.Kiedy rozpoczniesz dojenie nie mzoesz mieszac mleka.<br>Nie mzoesz wydoic krow owiec i koz do jednego pojemnika.</p><p>2 - Pojemnik jest teraz gotow do napelnienia.<br>Zwierze mozna wydoic podwojnym kliknieciem na nim.<br>Dojenie meczy zwierze wiec potrzebuja troche czasu na odnowienie zapasow mleka. Zwierze potrzebuje krotkiej przerwy podczas dojenia.</p><p>3 - Kiedy juz pojemnik jest pelen mleko mozna przelac do formy na ser, butelek i dzbanow.<br>Rozne typy mleka daja roznego rodzaju sery. Na napelnienie formy na ser potrzeba 15 litrow.</p><p>Jesli to czytasz to dlatego ze:</p><p>- Pojemnik jest pusty a Ty prubujesz go napelnic .<br>- Pojemnik jest pelny a Ty probujesz go napelnic<br>- Wybrales niedozwolony cel.",
				true, true));
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
		}
	}
}
