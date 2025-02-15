#region References

using System;
using System.Collections;
using Server;
using Server.Accounting;
using Server.Gumps;
using Server.Network;

#endregion

namespace Arya.Jail
{
	/// <summary>
	///     This gump is used to display the search results for the jail search engine
	/// </summary>
	public class JailSearchGump : Gump
	{
		private const int LabelHue = 0x480;
		private const int GreenHue = 0x40;
		private const int RedHue = 0x20;

		private readonly ArrayList m_List;
		private readonly Mobile m_User;
		private readonly int m_Page;
		private readonly JailGumpCallback m_Callback;

		public JailSearchGump(ArrayList items, Mobile user, JailGumpCallback callback) : this(items, user, 0, callback)
		{
		}

		public JailSearchGump(ArrayList items, Mobile user, int page, JailGumpCallback callback) : base(100, 100)
		{
			user.CloseGump(typeof(JailSearchGump));
			m_Callback = callback;

			m_List = items;
			m_User = user;
			m_Page = page;

			MakeGump();
		}

		private void MakeGump()
		{
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage(0);
			this.AddBackground(0, 0, 485, 315, 9250);

			// Title
			this.AddAlphaRegion(20, 15, 445, 20);
			this.AddLabel(25, 15, LabelHue, String.Format("Jail System Search: Displaying {0} results.", m_List.Count));

			// Table header
			this.AddImageTiled(20, 40, 120, 20, 9304);
			this.AddAlphaRegion(20, 40, 120, 20);
			this.AddLabel(25, 40, LabelHue, @"Account");

			this.AddImageTiled(145, 40, 120, 20, 9304);
			this.AddAlphaRegion(145, 40, 120, 20);
			this.AddLabel(150, 40, LabelHue, @"Player");

			this.AddImageTiled(270, 40, 90, 20, 9304);
			this.AddAlphaRegion(270, 40, 90, 20);
			this.AddLabel(275, 40, LabelHue, @"Status");

			this.AddImageTiled(365, 40, 35, 20, 9304);
			this.AddAlphaRegion(365, 40, 35, 20);
			this.AddLabel(370, 40, LabelHue, @"Jail");

			this.AddImageTiled(405, 40, 60, 20, 9304);
			this.AddAlphaRegion(405, 40, 60, 20);
			this.AddLabel(410, 40, LabelHue, "History");

			// Table background
			this.AddImageTiled(20, 65, 120, 210, 9274);
			this.AddAlphaRegion(20, 65, 120, 210);

			this.AddAlphaRegion(145, 65, 120, 210);

			this.AddImageTiled(270, 65, 90, 210, 9274);
			this.AddAlphaRegion(270, 65, 90, 210);

			this.AddAlphaRegion(365, 65, 35, 210);

			this.AddImageTiled(405, 65, 60, 210, 9274);
			this.AddAlphaRegion(405, 65, 60, 210);

			// Load data
			for (int i = m_Page * 10; i < m_Page * 10 + 10 && i < m_List.Count; i++)
			{
				// i is the absolute index on the array list
				Account acc = m_List[i] as Account;
				Mobile m = m_List[i] as Mobile;

				if (acc == null && m != null)
				{
					acc = m.Account as Account;
				}

				if (acc != null && m == null)
				{
					m = JailSystem.GetOnlineMobile(acc);
				}

				int index = i - (m_Page * 10);

				// Account label and button
				if (acc != null)
				{
					this.AddLabelCropped(35, 70 + index * 20, 100, 20, LabelHue, acc.Username);

					if (m_User.AccessLevel == AccessLevel.Administrator)
					{
						// View account details button : buttons from 10 to 19
						this.AddButton(15, 75 + index * 20, 2224, 2224, 10 + index, GumpButtonType.Reply, 0);
					}
				}

				// Player label and button
				if (m != null)
				{
					this.AddLabelCropped(160, 70 + index * 20, 100, 20, LabelHue, m.Name);

					// View props button: buttons from 20 to 29
					this.AddButton(140, 75 + index * 20, 2224, 2224, 20 + index, GumpButtonType.Reply, 0);
				}

				// Status
				if (m != null && m.NetState != null)
				{
					// A mobile is online
					this.AddLabel(285, 70 + index * 20, GreenHue, @"Online");

					// View Client button: buttons from 30 to 39
					this.AddButton(265, 75 + index * 20, 2224, 2224, 30 + index, GumpButtonType.Reply, 0);
				}
				else
				{
					// Offline
					this.AddLabel(285, 70 + index * 20, RedHue, @"Offline");
				}

				// Jail button: buttons from 40 to 49
				if ((m != null && m.AccessLevel == AccessLevel.Player) || (acc != null && !acc.Banned))
				{
					this.AddButton(375, 73 + index * 20, 5601, 5605, 40 + index, GumpButtonType.Reply, 0);
				}

				// History button: buttons from 50 to 59
				if (acc != null && m_User.AccessLevel >= JailSystem.m_HistoryLevel)
				{
					this.AddButton(428, 73 + index * 20, 5601, 5605, 50 + index, GumpButtonType.Reply, 0);
				}
			}

			this.AddAlphaRegion(20, 280, 445, 20);

			if (m_Page * 10 + 10 < m_List.Count)
			{
				this.AddButton(270, 280, 4005, 4006, 2, GumpButtonType.Reply, 0);
				this.AddLabel(305, 280, LabelHue, @"Next"); // Next: B2
			}

			if (m_Page > 0)
			{
				this.AddButton(235, 280, 4014, 4015, 1, GumpButtonType.Reply, 0);
				this.AddLabel(175, 280, LabelHue, @"Previous"); // Prev: B1
			}

			this.AddButton(20, 280, 4017, 4018, 0, GumpButtonType.Reply, 0);
			this.AddLabel(55, 280, LabelHue, @"Close"); // Close: B0
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			switch (info.ButtonID)
			{
				case 1: // Previous page

					m_User.SendGump(new JailSearchGump(m_List, m_User, m_Page - 1, m_Callback));
					break;

				case 2: // Next page

					m_User.SendGump(new JailSearchGump(m_List, m_User, m_Page + 1, m_Callback));
					break;

				case 0: // Close

					if (m_Callback != null)
					{
						try
						{
							m_Callback.Invoke(m_User);
						}
						catch { }
					}

					break;

				default:

					int type = info.ButtonID / 10;
					int index = info.ButtonID % 10;

					Mobile m = m_List[m_Page * 10 + index] as Mobile;
					Account acc = m_List[m_Page * 10 + index] as Account;

					if (acc == null && m != null)
					{
						acc = m.Account as Account;
					}

					if (m == null && acc != null)
					{
						m = JailSystem.GetOnlineMobile(acc);
					}

					switch (type)
					{
						case 1: // View Account details

							m_User.SendGump(new JailSearchGump(m_List, m_User, m_Page, m_Callback));

							if (acc != null)
							{
								m_User.SendGump(new AdminGump(m_User, AdminGumpPage.AccountDetails_Information, 0, null,
									"Returned from the jail system", acc));
							}

							break;

						case 2: // View Mobile props

							m_User.SendGump(new JailSearchGump(m_List, m_User, m_Page, m_Callback));

							if (m != null)
							{
								m_User.SendGump(new PropertiesGump(m_User, m));
							}

							break;

						case 3: // View Client gump

							m_User.SendGump(new JailSearchGump(m_List, m_User, m_Page, m_Callback));

							if (m != null && m.NetState != null)
							{
								m_User.SendGump(new ClientGump(m_User, m.NetState));
							}
							else
							{
								m_User.SendMessage("That mobile is no longer online");
							}

							break;

						case 4: // Jail button

							object obj = m_List[m_Page * 10 + index];

							if (obj is Mobile)
							{
								// Requesting a jail on a mobile
								JailSystem.InitJail(m_User, obj as Mobile);
							}
							else if (obj is Account)
							{
								// Jail an account
								JailSystem.InitJail(m_User, obj as Account);
							}

							break;

						case 5: // History button

							if (acc != null)
							{
								ArrayList history = JailSystem.SearchHistory(acc);

								if (history.Count > 0)
								{
									m_User.SendGump(new JailListingGump(m_User, history, JailSearchCallback));
								}
								else
								{
									m_User.SendMessage("No jail entries have been found for that account");
									m_User.SendGump(new JailSearchGump(m_List, m_User, m_Page, m_Callback));
								}
							}

							break;
					}

					break;
			}
		}

		private void JailSearchCallback(Mobile user)
		{
			user.SendGump(new JailSearchGump(m_List, user, m_Page, m_Callback));
		}
	}
}
