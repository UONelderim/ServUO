using System;

using Server.Accounting;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
	public class BankerGump : BaseGump
	{
		private readonly int TextColor = 0x000F;
		private readonly Banker _banker;

		public BankerGump(Banker banker, PlayerMobile pm)
			: base(pm, 150, 150)
		{
			_banker = banker;
		}

		public override void AddGumpLayout()
		{
			AddBackground(0, 0, 420, 344, 9300);

			AddHtmlLocalized(0, 10, 420, 16, 1113302, "#1156076", 1, false, false); // Bank Actions

			var acct = User.Account;

			AddHtmlLocalized(15, 35, 150, 16, 1156044, false, false); // Total Gold:
			AddHtml(145, 35, 200, 16, $"{acct?.TotalGold:N0}", false, false);

			AddHtmlLocalized(15, 55, 150, 16, 1156045, false, false); // Total Platinum:
			AddHtml(145, 55, 200, 16, $"{acct?.TotalPlat:N0}", false, false);

			AddHtmlLocalized(15, 75, 150, 16, 1157003, false, false); // Secure Account:
			AddHtml(145, 75, 200, 16, $"{acct?.GetSecureBalance(User):N0}", false, false);

			AddHtmlLocalized(15, 95, 150, 16, 1157004, false, false); // Transfer Gold:
			AddHtml(145, 95, 200, 16, "0", false, false);

			AddHtmlLocalized(15, 115, 150, 16, 1157005, false, false); // Transfer Platinum:
			AddHtml(145, 115, 200, 16, "0", false, false);

			AddHtmlLocalized(270, 35, 90, 16, 1114514, "Help", 0, false, false);
			AddButton(370, 35, 4014, 4015, 7, GumpButtonType.Reply, 0);

			// AddHtmlLocalized(60, 150, 360, 16, 1156064, TextColor, false, false); // Deposit Gold into Character Transfer Account
			// AddButton(20, 150, 4005, 4006, 1, GumpButtonType.Reply, 0);
			// AddTooltip(1156070); // Transfers gold from the bank to the character transfer account; capped at 1 billion gold. Any currency that 
			// 					 // a players wishes to transfer to another shard must be placed in character transfer account. Upon transferring 
			// 					 // the currency will be added to player's account on the shard.

			// AddHtmlLocalized(60, 180, 300, 16, 1156065, TextColor, false, false); // Deposit Platinum into Character Transfer Account
			// AddButton(20, 180, 4005, 4006, 2, GumpButtonType.Reply, 0);
			// AddTooltip(1156071); // Transfers platinum from the bank to the character transfer account; capped at 2 billion platinum. Any currency 
			// 					 // that a players wishes to transfer to another shard must be placed in character transfer account. Upon transferring 
			// 					 // the currency will be added to player's account on the shard. 

			AddHtmlLocalized(60, 210, 300, 16, 1156066, TextColor, false, false); // Withdraw Gold from Character Transfer Account
			AddButton(20, 210, 4005, 4006, 3, GumpButtonType.Reply, 0);
			// AddTooltip(1156072); // Transfers gold from the character transfer account to the bank; capped at 1 billion gold.

			// AddHtmlLocalized(60, 240, 300, 16, 1156067, TextColor, false, false); // Withdraw Platinum from Character Transfer Account
			// AddButton(20, 240, 4005, 4006, 4, GumpButtonType.Reply, 0);
			// AddTooltip(1156073); // Transfers platinum from the character transfer account to the bank; capped at 2 billion platinum. Really? Who the fuck has this much?

			AddHtmlLocalized(60, 270, 300, 16, 1156068, TextColor, false, false); // Deposit Gold into Secure Account
			AddButton(20, 270, 4005, 4006, 5, GumpButtonType.Reply, 0);
			AddTooltip(1156074); // Transfers gold from the bank to the player's secure account; capped at 100,000,000 gold. Only funds added 
								 // to the secure account can be added to the wall safe account.

			AddHtmlLocalized(60, 300, 300, 16, 1156069, TextColor, false, false); // Withdraw Gold from Secure Account
			AddButton(20, 300, 4005, 4006, 6, GumpButtonType.Reply, 0);
			AddTooltip(1156075); // Transfers gold from the secure account to the bank; capped at 100,0,000 gold.
		}

		public override void OnResponse(RelayInfo info)
		{
			switch (info.ButtonID)
			{
				case 0: break;
				case 1:
				case 2:
				case 3:
				case 4:
				{
					User.SendLocalizedMessage(1155866); // Enter amount to withdraw:
					User.BeginPrompt((from, text) =>
					{
						if (Int32.TryParse(text, out var amount))
						{
							_banker.WithdrawToBackpack(User, amount);
							Refresh(true);
						}
					});
					Refresh(false);
				}
				break;

				case 5:
				{
					User.SendLocalizedMessage(1155865); // Enter amount to deposit:
					User.BeginPrompt(OnDeposit);
				}
				break;

				case 6:
				{
					User.SendLocalizedMessage(1155866); // Enter amount to withdraw:
					User.BeginPrompt(OnWithdraw);
				}
				break;

				case 7:
				{
					User.CloseGump(typeof(NewCurrencyHelpGump));
					User.SendGump(new NewCurrencyHelpGump());

					Refresh();
				}
				break;
			}
		}

		private void OnDeposit(Mobile from, string input)
		{
			if (from?.Deleted != false || from.Account?.Deleted != false)
			{
				return;
			}

			try
			{
				if (String.IsNullOrWhiteSpace(input))
				{
					return;
				}

				var v = Utility.ToInt32(input);

				if (v > 0)
				{
					var balance = from.Account.GetSecureBalance(from);

					if (balance + v > Account.MaxSecureAmount)
					{
						v = Account.MaxSecureAmount - balance;
					}
				}

				if (v <= 0)
				{
					from.SendLocalizedMessage(1155867); // The amount entered is invalid. Verify that there are sufficient funds to complete this transaction.
					return;
				}

				if (!Banker.Withdraw(from, v))
				{
					from.SendLocalizedMessage(1155867); // The amount entered is invalid. Verify that there are sufficient funds to complete this transaction.
					return;
				}

				if (from.Account.DepositSecure(from, v))
				{
					from.SendLocalizedMessage(1153188); // Transaction successful:
					from.SendLocalizedMessage(1042763, v.ToString("N0")); // ~1_AMOUNT~ gold was deposited in your account.
					return;
				}

				from.SendMessage(0x22, "Could not deposit funds to your secure account.");

				if (Banker.Deposit(from, v))
				{
					from.SendMessage(0x55, "Your funds have been returned to your account.");
					return;
				}

				from.SendMessage(0x22, "Could not deposit funds to your account.");

				while (v > 0)
				{
					var amount = Math.Min(60000, v);

					from.Backpack.DropItem(new Gold(amount));

					v -= amount;
				}

				from.SendMessage(0x55, "Your funds have been returned to your backpack.");
			}
			finally
			{
				Refresh();
			}
		}

		private void OnWithdraw(Mobile from, string input)
		{
			if (from?.Deleted != false || from.Account?.Deleted != false)
			{
				return;
			}

			try
			{
				if (String.IsNullOrWhiteSpace(input))
				{
					return;
				}

				var v = Utility.ToInt32(input);

				if (v <= 0)
				{
					from.SendLocalizedMessage(1155867); // The amount entered is invalid. Verify that there are sufficient funds to complete this transaction.
					return;
				}

				if (!from.Account.WithdrawSecure(from, v))
				{
					from.SendLocalizedMessage(1155867); // The amount entered is invalid. Verify that there are sufficient funds to complete this transaction.
					return;
				}

				if (Banker.Deposit(from, v))
				{
					from.SendLocalizedMessage(1153188); // Transaction successful:
					from.SendLocalizedMessage(1042763, v.ToString("N0")); // ~1_AMOUNT~ gold was deposited in your account.
					return;
				}

				from.SendMessage(0x22, "Could not deposit funds to your account.");

				if (from.Account.DepositSecure(from, v))
				{
					from.SendMessage(0x55, "Your funds have been returned to your secure account.");
					return;
				}

				from.SendMessage(0x22, "Could not deposit funds to your secure account.");

				while (v > 0)
				{
					var amount = Math.Min(60000, v);

					from.Backpack.DropItem(new Gold(amount));

					v -= amount;
				}

				from.SendMessage(0x55, "Your funds have been returned to your backpack.");
			}
			finally
			{
				Refresh();
			}
		}

		private void Refresh()
		{
			Timer.DelayCall(TimeSpan.FromSeconds(2), Refresh, true, true);
		}
	}

	public class NewCurrencyHelpGump : Gump
	{
		public NewCurrencyHelpGump() : base(50, 75)
		{
			AddBackground(0, 0, 875, 480, 5170);
			AddHtmlLocalized(50, 40, 775, 440, 1156048, Utility.ToColor16(0x6495ED), false, false);
		}
	}
}
