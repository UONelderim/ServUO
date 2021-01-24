using System;
using Server.Items;
using Server.Targeting;
using Server.Commands;
using System.Collections;

namespace Server.Gumps
{
	public class RewardScrollCombineGump : Gump
	{
		public static void Initialize() {
			CommandSystem.Register("RSCombine", AccessLevel.Player, new CommandEventHandler(RewardCombine_OnCommand));
		}

		[Usage("RSCombine")]
		[Description("Otwiera gump umozliwiajacy laczenie zwojow nagrod, otrzymanych za questy lub donacje.")]
		private static void RewardCombine_OnCommand(CommandEventArgs arg) {
			if (arg.Length < 1) {
				arg.Mobile.CloseGump(typeof(RewardScrollCombineGump));
				arg.Mobile.SendGump(new RewardScrollCombineGump(null));
				return;
			} else {
				arg.Mobile.SendMessage("RSCombine");
				return;
			}
		}

		private ArrayList m_AddedScrolls;
		private int m_AfterJoinClass;
		private int m_AfterJoinUses;

		private bool ConsumeAllScrolls(ArrayList Scrolls, Mobile from) {
			ArrayList items = new ArrayList();
			ArrayList m_scrolls = new ArrayList();
			m_scrolls = Scrolls;

			foreach (Item item in from.Backpack.Items) {
				foreach (Item rew in m_scrolls) {
					if (item == rew) {
						items.Add(item);
					}
				}
			}
            if ( m_scrolls.Count == items.Count )
            {
                foreach ( Item item in items )
                    item.Delete();
                return true;
            }
            else
                return false;
		}


		public RewardScrollCombineGump(ArrayList scrolls)
			: base(30, 30) {
			if (scrolls == null)
				m_AddedScrolls = new ArrayList();
			else
				m_AddedScrolls = new ArrayList(scrolls);
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage(0);
			this.AddBackground(131, 48, 536, 341, 9200);
			this.AddAlphaRegion(140, 79, 516, 33);
			this.AddImage(81, 12, 10440);
			this.AddLabel(149, 85, 2591, @"Łączenie Zwoi Nagród");
			this.AddImage(634, 12, 10441);
			this.AddButton(143, 142, 1209, 1210, 1, GumpButtonType.Reply, 0);
			this.AddBackground(281, 125, 373, 180, 3500);
			this.AddLabel(161, 139, 2591, @"Dodaj zwój");
			this.AddButton(143, 164, 1209, 1210, 2, GumpButtonType.Reply, 0);
			this.AddLabel(161, 161, 2591, @"Połącz zwoje");
			this.AddButton(590, 362, 1209, 1210, 0, GumpButtonType.Reply, 0);
			this.AddLabel(608, 359, 2591, @"Anuluj");
			this.AddLabel(311, 137, 0, @"Klasa");
			this.AddLabel(461, 137, 0, @"Wartość");
			this.AddImageTiled(297, 155, 339, 2, 50);
			this.AddLabel(581, 137, 0, @"Usuń");
			//Lista scroll'i
			int sum = 0;
			int cl = 0;
			int h = 155;
			int b = 3;
			int uses = 0;
			foreach (RewardScroll sc in m_AddedScrolls) {
				string tmp = "Zwoj Nagrody Klasy " + sc.Class.ToString();
				this.AddLabel(311, h, 0, tmp);
				this.AddLabel(461, h, 0, sc.Value.ToString());
				this.AddButton(600, h + 4, 1209, 1210, b, GumpButtonType.Reply, 0);
				// stara metoda uwzgledniajaca spadek wartosci zwoju przy laczeniu
				//sum += (int)(((1.0 - (1.35 * (16.0 - (double)sc.Class)) / 100.0)) * (double)sc.Value);
				sum += (int)((double)sc.Value);
				if (sc.Class >= 1 && sc.Class <= 9)
					uses += 6 - sc.Repeat;
				h += 20;
				b++;
			}
			cl = (sum == 0) ? 0 : (16 - (int)Math.Log((double)sum / 250.0, 2.0));
			if (uses > 5)
				m_AfterJoinUses = 1;
			else {
				if (cl >= 1 && cl <= 9)
					m_AfterJoinUses = 6 - uses;
				else
					m_AfterJoinUses = 1;
			}
			//Wartosc
			m_AfterJoinClass = cl;
			//m_AfterJoinUses = uses;
			String value = "Aktualna wartość: " + sum.ToString() + " centarów.";
			String cla = "Klasa zwoju po połączeniu: " + cl.ToString();
			String us = "Ilość użyć po połączeniu: " + m_AfterJoinUses.ToString();
			this.AddLabel(285, 305, 2591, value);
			this.AddLabel(285, 325, 2591, cla);
			this.AddLabel(285, 345, 2591, us);

		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info) {
			Mobile from = sender.Mobile;

			switch (info.ButtonID) {
				case 0: {
						from.SendMessage("Anulowano");
						break;
					}
				case 1: {
						from.Target = new ScrollTarget(m_AddedScrolls);
						from.SendMessage("Który zwój chcesz dodać?");
						break;
					}
				case 2: {
						int cl = 16;
						foreach (RewardScroll rs in m_AddedScrolls) {
							if (rs.Class < cl)
								cl = rs.Class;
						}
						if (m_AfterJoinClass > cl) {
							from.CloseGump(typeof(RSInternalGump));
							from.SendGump(new RSInternalGump());
						} else if (m_AddedScrolls.Count != 0) {
							string scrolllist = "";
							foreach (RewardScroll rs in m_AddedScrolls) {
								scrolllist += rs.Class.ToString() + ", ";
							}
							if (ConsumeAllScrolls(m_AddedScrolls, from)) {
								RewardScroll reward = new RewardScroll(m_AfterJoinClass, m_AfterJoinUses);
								string log = from.AccessLevel + " " + CommandLogging.Format(from);
								log += " combined Reward Scrolls:" + scrolllist + "got a " + m_AfterJoinClass + " Class Reward Scroll (" + CommandLogging.Format(reward) + ") [RS]";
								CommandLogging.WriteLine(from, log);

								from.SendSound(0x3D);
								from.FixedEffect(0x376A, 10, 16);

								from.Backpack.DropItem(reward);
							} else {
								from.SendMessage("Zabrałeś z plecaka któryś zwój! Łączenie przerwane!");
							}
						}
						break;
					}
				default: {
						int i = info.ButtonID - 3;
						m_AddedScrolls.RemoveAt(i);
						from.SendGump(new RewardScrollCombineGump(m_AddedScrolls));
						break;
					}
			}
		}

	}

	public class ScrollTarget : Target
	{
		private ArrayList m_List;

		public ScrollTarget(ArrayList list) : base(2, false, TargetFlags.None) {
			if (list != null)
				m_List = new ArrayList(list);
			else
				m_List = new ArrayList();
		}

		protected override void OnTarget(Mobile from, object targeted) {
			if (targeted == null || ((targeted is Item) && (targeted as Item).Deleted))
				return;

			if (!(targeted is RewardScroll))
				from.SendMessage("To nie jest zwoj nagrody!");
			else {
				if (m_List.Contains(targeted as RewardScroll))
					from.SendMessage("Ten zwoj zostal juz dodany");
				else {
					if (m_List.Count == 6)
						from.SendMessage("Chwilowo ograniczone do maks. 6 zwojow...");
					else
						m_List.Add(targeted as RewardScroll);
				}
			}
			from.SendGump(new RewardScrollCombineGump(m_List));
		}
	}

	public class RSInternalGump : Gump
	{

		public RSInternalGump() : base(25, 50) {

			AddPage(0);

			AddBackground(25, 10, 420, 200, 5054);

			AddImageTiled(33, 20, 401, 181, 2624);
			AddAlphaRegion(33, 20, 401, 181);

			string komunikat = "<B>Łączenie zwojów</B><BR><BR>" +
							   "Chcesz połączyć zwoje, w takiej konfiguracji, że otrzymałbyś w efekcie zwój gorszy bądź porównywalny z najlepszym, który dodałeś do listy. Jako, że byłoby to marnotrastwo, taka możliwość nie jest dopuszczona.";


			AddHtml(40, 48, 387, 100, komunikat, true, true);

			AddButton(190, 172, 4005, 4007, 0, GumpButtonType.Reply, 0);
			AddHtmlLocalized(230, 172, 120, 20, 505686, 0xFFFFFF, false, false); // Zamknij

			//					AddHtmlLocalized( 40, 20, 380, 20, 505688, 0xFFFFFF, false, false ); // Informacje o systemie zmniejszania zwierząt
		}

		public override void OnResponse(Server.Network.NetState state, RelayInfo info) {
		}
	}

}
