using System;
using System.Text.RegularExpressions;
using Nelderim;
using Server.Accounting;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	public partial class Banker
	{
		private void NelderimOnSpeech(SpeechEventArgs e)
		{
			if (!e.Handled && e.Mobile.InRange(this, 3))
            {
                if (!CheckVendorAccess(e.Mobile))
                {
                    return;
                }
                
		        if ( new Regex("pobrac", RegexOptions.IgnoreCase).IsMatch(e.Speech) )
                {
                    e.Handled = true;

                    string[] split = e.Speech.Split(' ');

                    if (split.Length >= 2)
                    {
                        int amount;

                        Container pack = e.Mobile.Backpack;

                        if (Int32.TryParse(split[1], out amount))
                        {
	                        WithdrawToBackpack(e.Mobile, amount);
                        }
                    }
                }
		        else if(new Regex("saldo", RegexOptions.IgnoreCase).IsMatch(e.Speech) )
		        {
                    e.Handled = true;
                    
                    if (e.Speech.ToLower() == "saldo")
                    {
	                    OnLazySpeech();
                    }
                    else
                    {
	                    if (AccountGold.Enabled && e.Mobile.Account != null)
	                    {
		                    Say(1155855, string.Format("{0:#,0}\t{1:#,0}",
			                    e.Mobile.Account.TotalPlat,
			                    e.Mobile.Account.TotalGold), 0x3BC);

		                    Say(1155848, string.Format("{0:#,0}", e.Mobile.Account.GetSecureBalance(e.Mobile)), 0x3BC);
	                    }
	                    else
	                    {
		                    // Thy current bank balance is ~1_AMOUNT~ gold.
		                    Say(1042759, GetBalance(e.Mobile).ToString("#,0"));
	                    }
                    }
		        }
		        else if(new Regex("bank", RegexOptions.IgnoreCase).IsMatch(e.Speech) && new Regex("skrzyn", RegexOptions.IgnoreCase).IsMatch(e.Speech))
                {
                    e.Handled = true;
                    Say ( 505691 ); // Zaczekaj chwile, zaraz ja znajde.
                    Timer.DelayCall(TimeSpan.FromSeconds(3), () => {
	                    if (Deleted)
	                    {
		                    return;
	                    }

	                    if (e.Mobile == null || !e.Mobile.Alive || GetDistanceToSqrt(e.Mobile) > 4)
	                    {
		                    Say( 505694 ); // Gdziez on polazl?! Nie bede biegal ze skrzynia!
	                    }
	                    else
	                    {
		                    e.Mobile.RevealingAction();
		                    BankBox box = e.Mobile.BankBox;

		                    if ( box != null )
		                    {
			                    box.Open();
		                    }
	                    }
                    });
                } 
		        else if(new Regex("czek", RegexOptions.IgnoreCase).IsMatch(e.Speech) )
		        {
                    e.Handled = true;
                    
                    if (AccountGold.Enabled && e.Mobile.Account != null)
                    {
                        Say("Nie wystawiamy czeków");
                    }
                    else
                    {
	                    var split = e.Speech.Split(' ');

	                    if (split.Length >= 2)
	                    {
		                    int amount;

		                    if (int.TryParse(split[1], out amount))
		                    {
			                   
		                    }
	                    }
                    }
		        }
            }
		}

		public bool WithdrawToBackpack(Mobile from, int amount)
		{
			Container pack = from.Backpack;
			if (amount > 60000)
			{
				// Thou canst not withdraw so much at one time!
				Say(500381);
				return false;
			}
			if (pack == null || pack.Deleted || !(pack.TotalWeight < pack.MaxWeight) ||
			         !(pack.TotalItems < pack.MaxItems))
			{
				// Your backpack can't hold anything else.
				Say(1048147);
				return false;
			}

			if (amount <= 0)
			{
				return false;
			}
			BankBox box = from.Player ? from.BankBox : from.FindBankNoCreate();

			if (box == null || !Withdraw(from, amount))
			{
				// Ah, art thou trying to fool me? Thou hast not so much gold!
				Say(500384);
				return false;
			}
			pack.DropItem(new Gold(amount));

			// Thou hast withdrawn gold from thy account.
			Say(1010005);
			return true;
		}
	}
}
