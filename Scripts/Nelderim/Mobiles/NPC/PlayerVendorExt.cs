using System.Text.RegularExpressions;

namespace Server.Mobiles
{
	public partial class PlayerVendor
	{
		public override string NGetName(Mobile witness)
		{
			return Name;
		}

		private void NelderimOnSpeech(SpeechEventArgs e)
		{
			Mobile from = e.Mobile;

			if (e.Handled || !from.Alive || from.GetDistanceToSqrt(this) > 3)
				return;

			if (Regex.IsMatch(e.Speech, "kupi", RegexOptions.IgnoreCase) && WasNamed(e.Speech))
			{
				if (IsOwner(from))
				{
					SayTo(from, 503212); // You own this shop, just take what you want.
				}
				else if (House == null || !House.IsBanned(from))
				{
					from.SendLocalizedMessage(503213); // Select the item you wish to buy.
					from.Target = new PVBuyTarget();

					e.Handled = true;
				}
			}
			else if (Regex.IsMatch(e.Speech, "pokaz", RegexOptions.IgnoreCase) &&
			         WasNamed(e.Speech))
			{
				if (House != null && House.IsBanned(from) && !IsOwner(from))
				{
					SayTo(from,
						1062674); // You can't shop from this home as you have been banned from this establishment.
				}
				else
				{
					if (WasNamed(e.Speech))
						OpenBackpack(from);
					else
					{
						IPooledEnumerable mobiles = e.Mobile.GetMobilesInRange(2);

						foreach (Mobile m in mobiles)
							if (m is PlayerVendor && m.CanSee(e.Mobile) && m.InLOS(e.Mobile))
								((PlayerVendor)m).OpenBackpack(from);

						mobiles.Free();
					}

					e.Handled = true;
				}
			}
			else if (Regex.IsMatch(e.Speech, "oddaj", RegexOptions.IgnoreCase) &&
			         WasNamed(e.Speech))
			{
				if (IsOwner(from))
				{
					CollectGold(from);

					e.Handled = true;
				}
			}
			else if (Regex.IsMatch(e.Speech, "status", RegexOptions.IgnoreCase) &&
			         WasNamed(e.Speech))
			{
				if (IsOwner(from))
				{
					SendOwnerGump(from);

					e.Handled = true;
				}
				else
				{
					SayTo(from, 503226);
				}
			}
			else if (Regex.IsMatch(e.Speech, "zwalniam cie", RegexOptions.IgnoreCase) &&
			         WasNamed(e.Speech))
			{
				if (IsOwner(from))
				{
					Dismiss(from);

					e.Handled = true;
				}
			}
			else if (Regex.IsMatch(e.Speech, "obroc", RegexOptions.IgnoreCase) &&
			         WasNamed(e.Speech))
			{
				if (IsOwner(from))
				{
					Direction = GetDirectionTo(from);

					e.Handled = true;
				}
			}
		}
	}
}
