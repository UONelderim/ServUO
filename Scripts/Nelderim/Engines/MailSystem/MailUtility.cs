using System.Linq;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Mail;

namespace Server.Mail
{
    public static class MailUtility
    {
        /// <summary>
        /// Zwraca listę wszystkich możliwych destynacji: skrzynki domowe, bankowe i plecak.
        /// </summary>
        public static List<IMailDestination> GetAllDestinations(Mobile recipient, Mobile sender)
        {
            var list = new List<IMailDestination>();

            // 1) Skrzynki domowe
            list.AddRange(
                World.Items.OfType<HouseMailBox>()
                     .Where(b => b.CanAccept(null, recipient))
                     .OrderBy(b => MailProcessor.GetDistance(recipient.Location, b.Location))
            );

            // 2) Skrzynki bankowe
            list.AddRange(
                World.Items.OfType<BankMailBox>()
                     .OrderBy(b => MailProcessor.GetDistance(sender.Location, b.Location))
            );

            // 3) Fallback – plecak
            list.Add(new DirectBackpackDestination(recipient));

            return list;
        }

        /// <summary>
        /// Domyślna destynacja – pierwsza z listy.
        /// </summary>
        public static IMailDestination FindBestDestination(Mobile recipient, Mobile sender)
        {
            return GetAllDestinations(recipient, sender).First();
        }
    }
}
