using System;
using Server.Items;
using Server.Targeting;
using Server.Commands;
using System.Collections;

namespace Server.Gumps
{
	public class RewardScrollCombineGump : Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register("RSCombine", AccessLevel.Player, RewardCombine_OnCommand);
		}

		[Usage("RSCombine")]
		[Description("Otwiera gump umozliwiajacy laczenie zwojow nagrod, otrzymanych za questy lub donacje.")]
		private static void RewardCombine_OnCommand(CommandEventArgs arg)
		{
			if (arg.Length < 1)
			{
				arg.Mobile.CloseGump(typeof(RewardScrollCombineGump));
				arg.Mobile.SendGump(new RewardScrollCombineGump(null));
				return;
			}
			else
			{
				arg.Mobile.SendMessage("RSCombine");
				return;
			}
		}

		private ArrayList m_AddedScrolls;
		private int m_AfterJoinClass;
		private int m_AfterJoinUses;

		private bool ConsumeAllScrolls(ArrayList Scrolls, Mobile from)
		{
			var items = new ArrayList();
			var m_scrolls = new ArrayList();
			m_scrolls = Scrolls;

			foreach (var item in from.Backpack.Items)
			{
				foreach (Item rew in m_scrolls)
					if (item == rew)
						items.Add(item);
			}

			if (m_scrolls.Count == items.Count)
			{
				foreach (Item item in items)
					item.Delete();
				return true;
			}
			else
				return false;

			return false;
		}


		public RewardScrollCombineGump(ArrayList scrolls)
			: base(30, 30)
		{
			if (scrolls == null)
				m_AddedScrolls = new ArrayList();
			else
				m_AddedScrolls = new ArrayList(scrolls);
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
			AddPage(0);
			AddBackground(131, 48, 536, 341, 9200);
			AddAlphaRegion(140, 79, 516, 33);
			AddImage(81, 12, 10440);
			AddLabel(149, 85, 2591, @"Łączenie Zwoi Nagród");
			AddImage(634, 12, 10441);
			AddButton(143, 142, 1209, 1210, 1, GumpButtonType.Reply, 0);
			AddBackground(281, 125, 373, 180, 3500);
			AddLabel(161, 139, 2591, @"Dodaj zwój");
			AddButton(143, 164, 1209, 1210, 2, GumpButtonType.Reply, 0);
			AddLabel(161, 161, 2591, @"Połącz zwoje");
			AddButton(590, 362, 1209, 1210, 0, GumpButtonType.Reply, 0);
			AddLabel(608, 359, 2591, @"Anuluj");
			AddLabel(311, 137, 0, @"Klasa");
			AddLabel(461, 137, 0, @"Wartość");
			AddImageTiled(297, 155, 339, 2, 50);
			AddLabel(581, 137, 0, @"Usuń");
			//Lista scroll'i
			var sum = 0;
			var cl = 0;
			var h = 155;
			var b = 3;
			var uses = 0;
			foreach (RewardScroll sc in m_AddedScrolls)
			{
				var tmp = "Zwoj Nagrody Klasy " + sc.Class.ToString();
				AddLabel(311, h, 0, tmp);
				AddLabel(461, h, 0, sc.Value.ToString());
				AddButton(600, h + 4, 1209, 1210, b, GumpButtonType.Reply, 0);
				// stara metoda uwzgledniajaca spadek wartosci zwoju przy laczeniu
				//sum += (int)(((1.0 - (1.35 * (16.0 - (double)sc.Class)) / 100.0)) * (double)sc.Value);
				sum += (int)(double)sc.Value;
				if (sc.Class >= 1 && sc.Class <= 9)
					uses += 6 - sc.Repeat;
				h += 20;
				b++;
			}

			cl = sum == 0 ? 0 : 16 - (int)Math.Log((double)sum / 250.0, 2.0);
			if (uses > 5)
				m_AfterJoinUses = 1;
			else
			{
				if (cl >= 1 && cl <= 9)
					m_AfterJoinUses = 6 - uses;
				else
					m_AfterJoinUses = 1;
			}

			//Wartosc
			m_AfterJoinClass = cl;
			//m_AfterJoinUses = uses;
			var value = "Aktualna wartość: " + sum.ToString() + " centarów.";
			var cla = "Klasa zwoju po połączeniu: " + cl.ToString();
			var us = "Ilość użyć po połączeniu: " + m_AfterJoinUses.ToString();
			AddLabel(285, 305, 2591, value);
			AddLabel(285, 325, 2591, cla);
			AddLabel(285, 345, 2591, us);
		}

		public override void OnResponse(Network.NetState sender, RelayInfo info)
		{
			var from = sender.Mobile;

			switch (info.ButtonID)
			{
				case 0:
				{
					from.SendMessage("Anulowano");
					break;
				}
				case 1:
				{
					from.Target = new ScrollTarget(m_AddedScrolls);
					from.SendMessage("Który zwój chcesz dodać?");
					break;
				}
				case 2:
				{
					var cl = 16;
					foreach (RewardScroll rs in m_AddedScrolls)
					{
						if (rs.Class < cl)
							cl = rs.Class;
					}

					if (m_AfterJoinClass > cl)
					{
						from.CloseGump(typeof(RSInternalGump));
						from.SendGump(new RSInternalGump());
					}
					else if (m_AddedScrolls.Count != 0)
					{
						var scrolllist = "";
						foreach (RewardScroll rs in m_AddedScrolls) scrolllist += rs.Class.ToString() + ", ";
						if (ConsumeAllScrolls(m_AddedScrolls, from))
						{
							var reward = new RewardScroll(m_AfterJoinClass, m_AfterJoinUses);
							var log = from.AccessLevel + " " + CommandLogging.Format(from);
							log += " combined Reward Scrolls:" + scrolllist + "got a " + m_AfterJoinClass +
							       " Class Reward Scroll (" + CommandLogging.Format(reward) + ") [RS]";
							CommandLogging.WriteLine(from, log);

							from.SendSound(0x3D);
							from.FixedEffect(0x376A, 10, 16);

							from.Backpack.DropItem(reward);
						}
						else
							from.SendMessage("Zabrałeś z plecaka któryś zwój! Łączenie przerwane!");
					}

					break;
				}
				default:
				{
					var i = info.ButtonID - 3;
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

		public ScrollTarget(ArrayList list) : base(2, false, TargetFlags.None)
		{
			if (list != null)
				m_List = new ArrayList(list);
			else
				m_List = new ArrayList();
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			if (targeted == null || (targeted is Item && (targeted as Item).Deleted))
				return;

			if (!(targeted is RewardScroll))
				from.SendMessage("To nie jest zwoj nagrody!");
			else
			{
				if (m_List.Contains(targeted as RewardScroll))
					from.SendMessage("Ten zwoj zostal juz dodany");
				else
				{
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
		public RSInternalGump() : base(25, 50)
		{
			AddPage(0);

			AddBackground(25, 10, 420, 200, 5054);

			AddImageTiled(33, 20, 401, 181, 2624);
			AddAlphaRegion(33, 20, 401, 181);

			var komunikat = "<B>Łączenie zwojów</B><BR><BR>" +
			                "Chcesz połączyć zwoje, w takiej konfiguracji, że otrzymałbyś w efekcie zwój gorszy bądź porównywalny z najlepszym, który dodałeś do listy. Jako, że byłoby to marnotrastwo, taka możliwość nie jest dopuszczona.";


			AddHtml(40, 48, 387, 100, komunikat, true, true);

			AddButton(190, 172, 4005, 4007, 0, GumpButtonType.Reply, 0);
			AddHtmlLocalized(230, 172, 120, 20, 505686, 0xFFFFFF, false, false); // Zamknij

			//					AddHtmlLocalized( 40, 20, 380, 20, 505688, 0xFFFFFF, false, false ); // Informacje o systemie zmniejszania zwierząt
		}

		public override void OnResponse(Network.NetState state, RelayInfo info)
		{
		}
	}
}
