// -------------------------------
// File: IMailDestination.cs
// -------------------------------
using Server.Items;
using Server.Mobiles;

namespace Server.Mail
{
    /// <summary>
    /// Interface for any mail drop destination (house box, bank box, etc.).
    /// </summary>
    public interface IMailDestination
    {
        bool CanAccept(MailItem mail, Mobile sender);
        void Accept(MailItem mail, Mobile sender);
        Item ContainerItem { get; }
    }
}
