using System;
using System.Text.RegularExpressions;
using Server.Gumps;
using Server.Multis;

namespace Server.Regions
{
	public partial class HouseRegion
	{
		public void NelderimOnSpeech(SpeechEventArgs e)
		{
			Mobile from = e.Mobile;
            Item sign = House.Sign;

            bool isOwner = House.IsOwner(from);
            bool isCoOwner = isOwner || House.IsCoOwner(from);
            bool isFriend = isCoOwner || House.IsFriend(from);

            if (!isFriend)
                return;

            if (!from.Alive)
                return;

            if (Regex.IsMatch(e.Speech, "chce zmienic rozmiar mojego domu", RegexOptions.IgnoreCase))
            {
                if (from.Map != sign.Map || !from.InRange(sign, 0))
                {
                    from.SendLocalizedMessage(500295); // you are too far away to do that.
                }
                else if (DateTime.UtcNow <= House.BuiltOn.AddHours(1))
                {
                    from.SendLocalizedMessage(1080178); // You must wait one hour between each house demolition.
                }
                else if (isOwner)
                {
                    from.CloseGump(typeof(ConfirmHouseResize));
                    from.CloseGump(typeof(HouseGump));
                    from.SendGump(new ConfirmHouseResize(from, House));
                }
                else
                {
                    from.SendLocalizedMessage(501320); // Only the house owner may do 
                }
            }

            if (!House.IsInside(from) || !House.IsActive)
                return;

            else if (Regex.IsMatch(e.Speech, "usunac sie", RegexOptions.IgnoreCase))
            {
                from.SendLocalizedMessage(501326); // Target the individual to eject from this house.
                from.Target = new HouseKickTarget(House);
            }
            else if (Regex.IsMatch(e.Speech, "wyrzucam cie", RegexOptions.IgnoreCase))
            {
                if (!House.Public)
                {
                    from.SendLocalizedMessage(1062521); // You cannot ban someone from a private house.  Revoke their access instead.
                }
                else
                {
                    from.SendLocalizedMessage(501325); // Target the individual to ban from this house.
                    from.Target = new HouseBanTarget(true, House);
                }
            }
            else if (Regex.IsMatch(e.Speech, "chce to zablokowac", RegexOptions.IgnoreCase))
            {
                from.SendLocalizedMessage(502097); // Lock what down?
                from.Target = new LockdownTarget(false, House);
            }
            else if (Regex.IsMatch(e.Speech, "chce to odblokowac", RegexOptions.IgnoreCase))
            {
                from.SendLocalizedMessage(502100); // Choose the item you wish to release
                from.Target = new LockdownTarget(true, House);
            }
            else if (Regex.IsMatch(e.Speech, "chce to zabezpieczyc", RegexOptions.IgnoreCase))
            {
                if (isCoOwner)
                {
                    from.SendLocalizedMessage(502103); // Choose the item you wish to secure
                    from.Target = new SecureTarget(false, House);
                }
                else
                {
                    from.SendLocalizedMessage(502094); // You must be in your house to do this. 
                }
            }
            else if (Regex.IsMatch(e.Speech, "chce to odbezpieczyc", RegexOptions.IgnoreCase))
            {
                if (isOwner)
                {
                    from.SendLocalizedMessage(502106); // Choose the item you wish to unsecure
                    from.Target = new SecureTarget(true, House);
                }
                else
                {
                    from.SendLocalizedMessage(502094); // You must be in your house to do this. 
                }
            }
            else if (Regex.IsMatch(e.Speech, "chce postawic bezpieczny pojemnik", RegexOptions.IgnoreCase))
            {
                if (isOwner)
                {
                    from.SendLocalizedMessage(502109); // Owners do not get a strongbox of their own.
                }
                else if (isCoOwner)
                {
                    House.AddStrongBox(from);
                }
                else
                {
                    from.SendLocalizedMessage(1010587); // You are not a co-owner of this house.
                }
            }
            else if (Regex.IsMatch(e.Speech, "chce postawic kosz", RegexOptions.IgnoreCase))
            {
                if (isCoOwner)
                {
                    House.AddTrashBarrel(from);
                }
                else
                {
                    from.SendLocalizedMessage(1010587); // You are not a co-owner of this house.
                }
            }
		}
	}
}
