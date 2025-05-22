using System.Linq;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mail
{
	public static class MailUtility
	{
		public static List<IMailDestination> GetAllDestinations(Mobile recipient, Mobile sender)
		{
			var list = new List<IMailDestination>();

			list.AddRange(
				World.Items.OfType<HouseMailBox>()
					.Where(b => b.CanAccept(null, recipient))
					.OrderBy(b => Utility.GetDistanceToSqrt(recipient.Location, b.Location))
			);

			list.AddRange(
				World.Items.OfType<BankMailBox>()
					.OrderBy(b => Utility.GetDistanceToSqrt(sender.Location, b.Location))
			);

			list.Add(new DirectBackpackDestination(recipient));

			return list;
		}

		public static IMailDestination FindBestDestination(Mobile recipient, Mobile sender)
		{
			return GetAllDestinations(recipient, sender).First();
		}
	}
}
