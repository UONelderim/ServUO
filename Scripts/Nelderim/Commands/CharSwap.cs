#region References

using System;
using Server.Accounting;
using Server.Commands;
using Server.Network;

#endregion

namespace Server.Gumps
{
	public class CharControlGump : Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register("CharControl", AccessLevel.Administrator, CharControl_OnCommand);
		}

		[Usage("CharControl")]
		[Description(
			"Brings up a gump which allows deletion of characters and swapping of characters between accounts.")]
		private static void CharControl_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendGump(new CharControlGump());
		}

		Account a1;
		Account a2;

		SwapInfo swap;

		public string Color(string text, int color)
		{
			return $"<BASEFONT COLOR=#{color:X6}>{text}</BASEFONT>";
		}

		public string Center(string text)
		{
			return $"<CENTER>{text}</CENTER>";
		}

		public CharControlGump() : this(null, null, null, null)
		{
		}

		protected bool InSwapMode => (swap != null);

		public CharControlGump(Account first, Account second, string ErrorMessage) : this(first, second,
			ErrorMessage, null)
		{
		}

		public CharControlGump(Account first, Account second, string ErrorMessage, SwapInfo s) : base(50, 50)
		{
			Closable = true;
			Disposable = false;
			Dragable = true;
			Resizable = false;

			a1 = first;
			a2 = second;
			swap = s;

			AddPage(0);

			#region Gump Prettification

			AddBackground(16, 12, 350, 450, 9270);

			AddImage(190, 22, 9273);
			AddImage(128, 385, 9271);
			AddImage(180, 22, 9275);
			AddImage(190, 100, 9273);
			AddImage(180, 100, 9275);
			AddImage(233, 385, 9271);
			AddImage(26, 385, 9271);

			AddAlphaRegion(15, 10, 352, 454);

			#endregion

			if (!InSwapMode)
			{
				AddButton(176, 49, 4023, 4025, 1, GumpButtonType.Reply, 0); //Okay for acct names button

				AddHtml(30, 395, 325, 56, Color(Center(ErrorMessage), 0xFF0000), false, false);


				AddImageTiled(33, 50, 140, 20, 0xBBC);
				AddImageTiled(209, 50, 140, 20, 0xBBC);

				AddTextEntry(33, 50, 140, 20, 1152, 2, "");
				AddTextEntry(209, 50, 140, 20, 1152, 3, "");
			}

			AddLabel(58, 28, 1152, (first == null) ? "1st Acct Name" : first.ToString());
			AddLabel(232, 28, 1152, (second == null) ? "2nd Acct Name" : second.ToString());

			#region Create Character Buttons

			int x = 50; //x is 225 for 2nd...

			for (int h = 0; h < 2; h++)
			{
				if (first != null)
				{
					int y = 87;
					for (int i = 0; i < first.Length; i++)
					{
						Mobile m = first[i];

						if (m == null) continue;

						if (!(InSwapMode && swap.AlreadyChose(first)))
							AddButton(x - 20, y + 3, 5601, 5605, 10 * i + h * 100 + 5, GumpButtonType.Reply,
								0); //The Swap Select button

						AddLabel(x, y, 1152, String.Format("{0} (0x{1:X})", m.Name, m.Serial.Value));

						int labelY = y + 23;
						int buttonY = y + 27;

						AddLabel(x + 1, labelY, 1152, "Swap");

						if (second != null && !InSwapMode && HasSpace(second) != 0)
							AddButton(x - 15, buttonY, 11400, 11402, 10 * i + h * 100 + 6, GumpButtonType.Reply, 0);
						else
							AddImage(x - 15, buttonY, 11412);


						AddLabel(x + 54, labelY, 1152, "Del");
						if (!InSwapMode)
							AddButton(x + 36, buttonY, 11400, 11402, 10 * i + h * 100 + 7, GumpButtonType.Reply, 0);
						else
							AddImage(x + 36, buttonY, 11412);


						AddLabel(x + 95, labelY, 1152, "Move");

						if (!InSwapMode && second != null && HasSpace(second) >= 0)
							AddButton(x + 78, buttonY, 11400, 11402, 10 * i + h * 100 + 8, GumpButtonType.Reply, 0);
						else
							AddImage(x + 78, buttonY, 11412);

						y += 48;
					}
				}

				x += 175;

				Account temp = first;
				first = second;
				second = temp;
			}

			#endregion
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			bool SendGumpAgain = true;

			Mobile m = state.Mobile;

			if (m.AccessLevel < AccessLevel.Administrator)
				return;

			#region Sanity

			if (IsDeleted(a1))
				a1 = null;

			if (IsDeleted(a2))
				a2 = null;

			#endregion

			int id = info.ButtonID;

			if (id == 0)
			{
				if (InSwapMode)
					m.SendGump(new CharControlGump(a1, a2, "Character swap canceled"));
			}

			if (id == 1)
			{
				#region Find Acct from Input

				string firstStr = info.GetTextEntry(2).Text;
				string secondStr = info.GetTextEntry(3).Text;

				Account first = (Account)Accounts.GetAccount(firstStr);
				Account second = (Account)Accounts.GetAccount(secondStr);

				string ErrorMessage = "";

				if (first == null || second == null)
				{
					if (first == null && firstStr != "" && secondStr == "")
						ErrorMessage = String.Format("Account: '{0}' NOT found", firstStr);
					else if (second == null && secondStr != "" && firstStr == "")
						ErrorMessage = String.Format("Account: '{0}' NOT found", secondStr);
					else if (firstStr == "" && secondStr == "")
						ErrorMessage = "Please enter in an Account name";
					else if (second == null && first == null)
						ErrorMessage = String.Format("Accounts: '{0}' and '{1}' NOT found", firstStr, secondStr);
				}

				if (a1 != null && first == null)
					first = a1;

				if (a2 != null && second == null)
					second = a2;

				m.SendGump(new CharControlGump(first, second, ErrorMessage));

				#endregion
			}
			else if (id > 4) //left side
			{
				#region Sanity & Declarations

				int button = id % 10;
				int charIndex = ((id < 100) ? id : (id - 100)) / 10;

				string error = "Invalid Button";

				Account acct;
				Account secondAcct;

				acct = (id >= 100) ? a2 : a1;
				secondAcct = (id < 100) ? a2 : a1;

				if (IsDeleted(acct))
					error = "Selected Account is null or Deleted";
				else if (acct[charIndex] == null)
					error = "That character is not found";

				#endregion

				else
				{
					Mobile mob = acct[charIndex];
					switch (button)
					{
						#region Swap

						case 5: //Swap Selection And/Or Props
						{
							if (InSwapMode)
							{
								if (!swap.AlreadyChose(acct) && !swap.AlreadyChose(secondAcct))
								{
									//Both Empty, even though this should NEVER happen.  Just a sanity check
									swap.a1 = acct;
									swap.a1CharIndex = charIndex;
									error = "Please choose a character from the Other acct to swap with";
								}
								else if ((swap.AlreadyChose(swap.a1) && !swap.AlreadyChose(swap.a2)) ||
								         (swap.AlreadyChose(swap.a2) && !swap.AlreadyChose(swap.a1)))
								{
									//First is filled, second is empty
									if (swap.AlreadyChose(swap.a1))
									{
										swap.a2 = acct;
										swap.a2CharIndex = charIndex;
									}
									else
									{
										swap.a1 = acct;
										swap.a1CharIndex = charIndex;
									}

									if (swap.SwapEm())
									{
										error = String.Format(
											"Mobile {0} (0x{1:X}) and Mobile {2} (0x{3:X}) sucessfully swapped between Accounts {4} and {5}",
											swap.a1[swap.a1CharIndex], swap.a1[swap.a1CharIndex].Serial.Value,
											swap.a2[swap.a2CharIndex], swap.a2[swap.a2CharIndex].Serial.Value, swap.a2,
											swap.a1);
										CommandLogging.WriteLine(m, error);
									}
									else
										error = "Swap unsucessful";

									swap = null;
								}
							}
							else
							{
								m.SendGump(new PropertiesGump(m, mob));
								error = "Properties gump sent";
							}

							break;
						}
						case 6: //Swap
						{
							if (IsDeleted(secondAcct))
							{
								error = "Both accounts must exist to swap characters";
							}
							else if (HasSpace(acct) == 0 || HasSpace(secondAcct) == 0)
							{
								error = "Both accounts must have at least one character to swap.";
							}
							else
							{
								error = "Please Choose the other character to swap.";
								swap = new SwapInfo(a1, a2);

								if (acct == a1)
									swap.a1CharIndex = charIndex;
								else
									swap.a2CharIndex = charIndex;
							}

							break;
						}

						#endregion

						#region Delete Character

						case 7: //Del
						{
							object[] o = { acct, mob, this };

							m.SendGump(
								new WarningGump(1060635, 30720,
									String.Format(
										"You are about to delete Mobile {0} (0x{1:X}) of Acct {2}.  This can not be reversed without a complete server revert.  Please note that this'll delete any items on that Character, but it'll still leave their house standing.  Do you wish to continue?",
										mob.Name, mob.Serial.Value, acct),
									0xFFC000, 360, 260, CharacterDelete_Callback, o));

							SendGumpAgain = false;

							break;
						}

						#endregion

						#region Move Character

						case 8: //Move
						{
							if (secondAcct == null)
							{
								error = String.Format(
									"Can't move Mobile {0} (0x{1:X} because the other account is null", mob.Name,
									mob.Serial.Value);
								break;
							}

							int newCharLocation = HasSpace(secondAcct);

							if (newCharLocation < 0)
							{
								error = String.Format(
									"Can't move Mobile {0} (0x{1:X}) to account {2} because that account is full",
									mob.Name, mob.Serial.Value, secondAcct);
								break;
							}

							acct[charIndex] = null;
							secondAcct[newCharLocation] = mob;

							mob.Say("I've been moved to another Account!");

							if (mob.NetState != null)
								mob.NetState.Dispose();

							error = String.Format("Mobile {0} (0x{1:X}) of Account {2} moved to Account {3}.", mob.Name,
								mob.Serial.Value, acct, secondAcct);

							CommandLogging.WriteLine(m, error);
							break;
						}

						#endregion
					}
				}

				if (SendGumpAgain)
					m.SendGump(new CharControlGump(a1, a2, error, swap));
			}
		}

		public bool IsDeleted(Account a)
		{
			return (a == null || Accounts.GetAccount(a.Username) == null);
		}

		/// <summary>
		///     Checks to see if an Account used up all of it's character slots.
		///     Only currently supports 5 chars.  will hafta change this when it's changed to 6.
		///     Returns -1 if there's no room, and the location of the first free slot if there is room.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public int HasSpace(Account a)
		{
			if (IsDeleted(a))
				return -1;

			for (int i = 0; i < 5; i++)
			{
				if (a[i] == null)
				{
					return i;
				}
			}

			return -1;
		}


		protected void CharacterDelete_Callback(Mobile from, bool okay, object state)
		{
			object[] states = (object[])state;
			Account acct = (Account)states[0];
			Mobile mob = (Mobile)states[1];
			CharControlGump g = (CharControlGump)states[2];

			string error;

			if (mob == null || acct == null)
			{
				error = "Mobile or Acct is null"; //SafeGuard
				return;
			}

			if (okay)
			{
				mob.Say("I've been Deleted!");

				if (mob.NetState != null)
					mob.NetState.Dispose();

				mob.Delete();
			}

			error = String.Format("Mobile {0} (0x{1:X}) of Acct {2} {3} Deleted.", mob.Name, mob.Serial.Value, acct,
				okay ? "" : "not");

			if (okay)
				CommandLogging.WriteLine(from, error);

			from.SendGump(new CharControlGump(g.a1, g.a2, error));
		}


		public class SwapInfo
		{
			public Account a1;
			public Account a2;

			public int a1CharIndex;

			public int a2CharIndex;
			//TODO: MAke getters & setters


			public SwapInfo(Account firstAcct, Account secondAcct)
			{
				a1 = firstAcct;
				a2 = secondAcct;

				a1CharIndex = -1;
				a2CharIndex = -1;
			}

			public bool AlreadyChose(Account acct)
			{
				if (acct == null)
					return false;

				if (acct == a1)
					return (a1CharIndex >= 0);

				if (acct == a2)
					return (a2CharIndex >= 0);

				return false;
			}

			public bool SwapEm()
			{
				if (a1 == null || a2 == null)
					return false;

				Mobile mob = a1[a1CharIndex];
				Mobile mob2 = a2[a2CharIndex];

				a1[a1CharIndex] = mob2;
				a2[a2CharIndex] = mob;

				if (mob == null || mob2 == null)
					return false;

				mob.Say("I've been Swapped to another Account!");
				mob.Say("I've been Swapped to another Account!");

				if (mob.NetState != null)
					mob.NetState.Dispose();

				if (mob2.NetState != null)
					mob2.NetState.Dispose();

				return true;
			}
		}
	}
}
