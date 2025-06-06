#region References
using Server.Accounting;
using Server.ContextMenus;
using Server.Items;
using Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using Nelderim;
using Nelderim.Configuration;

#endregion

namespace Server.Mobiles
{
    public partial class Banker : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

        [Constructable]
        public Banker()
            : base("- bankier")
        { }

        public Banker(Serial serial)
            : base(serial)
        { }

        public override NpcGuild NpcGuild => NpcGuild.MerchantsGuild;

        protected override List<SBInfo> SBInfos => m_SBInfos;

		public static double GetFullBalance(Mobile m)
		{
			return GetFullBalance(m, out _, out _, out _, out _, out _);
		}

		public static double GetFullBalance(Mobile m, out Gold[] gold, out BankCheck[] checks)
		{
			return GetFullBalance(m, out gold, out checks, out _, out _, out _);
		}

		public static double GetFullBalance(Mobile m, out long goldTotal, out long checkTotal, out long virtualTotal)
		{
			return GetFullBalance(m, out _, out _, out goldTotal, out checkTotal, out virtualTotal);
		}

		public static double GetFullBalance(Mobile m, out Gold[] gold, out BankCheck[] checks, out long goldTotal, out long checkTotal, out long virtualTotal)
		{
			gold = Array.Empty<Gold>();
			checks = Array.Empty<BankCheck>();

			goldTotal = checkTotal = virtualTotal = 0L;

			var balance = 0.0;

			if (AccountGold.Enabled && m.Account != null)
			{
				m.Account.GetGoldBalance(out virtualTotal, out _);

				balance += virtualTotal;
			}

			var bank = m.Player ? m.BankBox : m.FindBankNoCreate();

			if (bank != null)
			{
				gold = bank.FindItemsByType<Gold>(o => !o.HasLockedParent).ToArray();
				checks = bank.FindItemsByType<BankCheck>(o => !o.HasLockedParent).ToArray();

				balance += goldTotal = gold.Aggregate(0L, (c, t) => c + t.Amount);
				balance += checkTotal = checks.Aggregate(0L, (c, t) => c + t.Worth);
			}

			return balance;
		}

		public static int GetBalance(Mobile m)
		{
			var balance = GetFullBalance(m);

			return (int)Math.Max(0, Math.Min(Int32.MaxValue, balance));
		}

		public static int GetBalance(Mobile m, out Gold[] gold, out BankCheck[] checks)
		{
			var balance = GetFullBalance(m, out gold, out checks);

			return (int)Math.Max(0, Math.Min(Int32.MaxValue, balance));
		}

		public static int GetBalance(Mobile m, out long goldTotal, out long checkTotal, out long virtualTotal)
		{
			var balance = GetFullBalance(m, out goldTotal, out checkTotal, out virtualTotal);

			return (int)Math.Max(0, Math.Min(Int32.MaxValue, balance));
		}

		public static int GetBalance(Mobile m, out Gold[] gold, out BankCheck[] checks, out long goldTotal, out long checkTotal, out long virtualTotal)
		{
			var balance = GetFullBalance(m, out gold, out checks, out goldTotal, out checkTotal, out virtualTotal);

			return (int)Math.Max(0, Math.Min(Int32.MaxValue, balance));
		}

		public static bool Withdraw(Mobile from, long amount)
		{
			return Withdraw(from, amount, false);
		}

		public static bool Withdraw(Mobile from, long amount, bool message)
		{
			var balance = GetFullBalance(from, out var gold, out var checks, out var goldTotal, out var checkTotal, out var virtualTotal);

			if (balance < amount)
			{
				return false;
			}

			var need = amount;

			if (need > 0 && virtualTotal > 0)
			{
				if (virtualTotal < need)
				{
					if (from.Account.WithdrawGold(virtualTotal))
					{
						need -= virtualTotal;
					}
				}
				else
				{
					if (from.Account.WithdrawGold(need))
					{
						need = 0;
					}
				}

				if (need == amount)
				{
					return false;
				}
			}

			if (need > 0 && goldTotal > 0)
			{
				for (var i = 0; need > 0 && i < gold.Length; ++i)
				{
					var g = gold[i];

					if (g.Amount <= need)
					{
						need -= g.Amount;
						g.Delete();
					}
					else
					{
						g.Amount -= (int)need;
						need = 0;
					}
				}
			}

			if (need > 0 && checkTotal > 0)
			{
				for (var i = 0; need > 0 && i < checks.Length; ++i)
				{
					var c = checks[i];

					if (c.Worth <= need)
					{
						need -= c.Worth;
						c.Delete();
					}
					else
					{
						c.Worth -= (int)need;
						need = 0;
					}
				}
			}

			if (message)
			{
				from.SendLocalizedMessage(1155856,
					amount.ToString("N0")); // ~1_AMOUNT~ gold has been removed from your bank box.
			}
			BankLog.Log(from, -amount, "withdraw");
			return true;
		}

		public static bool Deposit(Mobile from, int amount, bool message = false)
        {
            // If for whatever reason the TOL checks fail, we should still try old methods for depositing currency.
            if (AccountGold.Enabled && from.Account != null && from.Account.DepositGold(amount))
            {
                if (message)
                    from.SendLocalizedMessage(1042763, amount.ToString("N0")); // ~1_AMOUNT~ gold was deposited in your account.

                return true;
            }

            BankBox box = from.Player ? from.BankBox : from.FindBankNoCreate();

            if (box == null)
            {
                return false;
            }

            List<Item> items = new List<Item>();

            while (amount > 0)
            {
                Item item;
                if (amount < 5000)
                {
                    item = new Gold(amount);
                    amount = 0;
                }
                else if (amount <= 1000000)
                {
                    item = new BankCheck(amount);
                    amount = 0;
                }
                else
                {
                    item = new BankCheck(1000000);
                    amount -= 1000000;
                }

                if (box.TryDropItem(from, item, false))
                {
                    items.Add(item);
                }
                else
                {
                    item.Delete();
                    foreach (Item curItem in items)
                    {
                        curItem.Delete();
                    }

                    return false;
                }
            }

            if (message)
                from.SendLocalizedMessage(1042763, amount.ToString("N0")); // ~1_AMOUNT~ gold was deposited in your account.
            BankLog.Log(from, amount, "deposit");
            return true;
        }

        public static int DepositUpTo(Mobile from, int amount, bool message = false)
        {
            // If for whatever reason the TOL checks fail, we should still try old methods for depositing currency.
            if (AccountGold.Enabled && from.Account != null && from.Account.DepositGold(amount))
            {
                if (message)
                    from.SendLocalizedMessage(1042763, amount.ToString("N0")); // ~1_AMOUNT~ gold was deposited in your account.

                return amount;
            }

            BankBox box = from.Player ? from.BankBox : from.FindBankNoCreate();

            if (box == null)
            {
                return 0;
            }

            int amountLeft = amount;
            while (amountLeft > 0)
            {
                Item item;
                int amountGiven;

                if (amountLeft < 5000)
                {
                    item = new Gold(amountLeft);
                    amountGiven = amountLeft;
                }
                else if (amountLeft <= 1000000)
                {
                    item = new BankCheck(amountLeft);
                    amountGiven = amountLeft;
                }
                else
                {
                    item = new BankCheck(1000000);
                    amountGiven = 1000000;
                }

                if (box.TryDropItem(from, item, false))
                {
                    amountLeft -= amountGiven;
                }
                else
                {
                    item.Delete();
                    break;
                }
            }

            return amount - amountLeft;
        }

        public static void Deposit(Container cont, int amount)
        {
            while (amount > 0)
            {
                Item item;

                if (amount < 5000)
                {
                    item = new Gold(amount);
                    amount = 0;
                }
                else if (amount <= 1000000)
                {
                    item = new BankCheck(amount);
                    amount = 0;
                }
                else
                {
                    item = new BankCheck(1000000);
                    amount -= 1000000;
                }

                cont.DropItem(item);
            }
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBanker());
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(Location, 12))
            {
                return true;
            }

            return base.HandlesOnSpeech(from);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
	        if (NConfig.CustomOnSpeech)
	        {
		        NelderimOnSpeech(e);
	        }
	        else
	        {
		        HandleSpeech(this, e);
	        }

	        base.OnSpeech(e);
        }

        private static void HandleSpeech(Banker vendor, SpeechEventArgs e)
        {
	        if (vendor == null)
		        return;
	        
	        if (!e.Handled && e.Mobile.InRange(vendor, 12))
            {
                if (e.Mobile.Map.Rules != MapRules.FeluccaRules && !vendor.CheckVendorAccess(e.Mobile, true))
                {
                    return;
                }

                foreach (int keyword in e.Keywords)
                {
                    switch (keyword)
                    {
                        case 0x0000: // *withdraw*
                            {
                                e.Handled = true;

                                if (e.Mobile.Criminal)
                                {
                                    // I will not do business with a criminal!
                                    vendor.Say(500389);
                                    break;
                                }

                                string[] split = e.Speech.Split(' ');

                                if (split.Length >= 2)
                                {
                                    int amount;

                                    if (!int.TryParse(split[1], out amount))
                                    {
                                        break;
                                    }

                                    vendor.WithdrawToBackpack(e.Mobile, amount);
                                }
                            }
                            break;
                        case 0x0001: // *balance*
                            {
                                e.Handled = true;

                                if (e.Mobile.Criminal)
                                {
                                    // I will not do business with a criminal!
                                    vendor.Say(500389);
                                    break;
                                }

                                if (AccountGold.Enabled && e.Mobile.Account != null)
                                {
                                    vendor.Say(1155855, string.Format("{0:#,0}\t{1:#,0}",
                                        e.Mobile.Account.TotalPlat,
                                        e.Mobile.Account.TotalGold), 0x3BC);

                                    vendor.Say(1155848, string.Format("{0:#,0}", e.Mobile.Account.GetSecureBalance(e.Mobile)), 0x3BC);
                                }
                                else
                                {
                                    // Thy current bank balance is ~1_AMOUNT~ gold.
                                    vendor.Say(1042759, GetBalance(e.Mobile).ToString("#,0"));
                                }
                            }
                            break;
                        case 0x0002: // *bank*
                            {
                                e.Handled = true;

                                if (e.Mobile.Criminal)
                                {
                                    // Thou art a criminal and cannot access thy bank box.
                                    vendor.Say(500378);
                                    break;
                                }

                                e.Mobile.BankBox.Open();
                            }
                            break;
                        case 0x0003: // *check*
                            {
                                e.Handled = true;

                                if (e.Mobile.Criminal)
                                {
                                    // I will not do business with a criminal!
                                    vendor.Say(500389);
                                    break;
                                }

                                if (AccountGold.Enabled && e.Mobile.Account != null)
                                {
                                    vendor.Say("We no longer offer a checking service.");
                                    break;
                                }

                                string[] split = e.Speech.Split(' ');

                                if (split.Length >= 2)
                                {
                                    int amount;

                                    if (!int.TryParse(split[1], out amount))
                                    {
                                        break;
                                    }

                                    if (amount < 5000)
                                    {
                                        // We cannot create checks for such a paltry amount of gold!
                                        vendor.Say(1010006);
                                    }
                                    else if (amount > 1000000)
                                    {
                                        // Our policies prevent us from creating checks worth that much!
                                        vendor.Say(1010007);
                                    }
                                    else
                                    {
                                        BankCheck check = new BankCheck(amount);

                                        BankBox box = e.Mobile.BankBox;

                                        if (!box.TryDropItem(e.Mobile, check, false))
                                        {
                                            // There's not enough room in your bankbox for the check!
                                            vendor.Say(500386);
                                            check.Delete();
                                        }
                                        else if (!box.ConsumeTotal(typeof(Gold), amount))
                                        {
                                            // Ah, art thou trying to fool me? Thou hast not so much gold!
                                            vendor.Say(500384);
                                            check.Delete();
                                        }
                                        else
                                        {
                                            // Into your bank box I have placed a check in the amount of:
                                            vendor.Say(1042673, AffixType.Append, amount.ToString("#,0"), "");
                                            BankLog.Log(e.Mobile, amount, "check");
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }

        public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (from.Alive)
            {
                bool enabled = CheckVendorAccess(from);

                OpenBankEntry entry = new OpenBankEntry(this)
                {
                    Enabled = enabled
                };
                
                if (InsuranceEnabled && from is PlayerMobile pm)
                {
	                list.Add(new PlayerMobile.CallbackEntry(1114299, pm.OpenItemInsuranceMenu) { Enabled = enabled });
	                list.Add(new PlayerMobile.CallbackEntry(6201, pm.ToggleItemInsurance) { Enabled = enabled });
                }

                list.Add(entry);
            }

            base.AddCustomContextEntries(from, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();
        }
    }
}
