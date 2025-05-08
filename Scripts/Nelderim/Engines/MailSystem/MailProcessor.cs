using System;
using Server;
using Server.Items;

namespace Server.Mail
{
    /// <summary>
    /// Utility for calculating mail cost, delivery time, scheduling, and distance.
    /// </summary>
    public static class MailProcessor
    {
        /// <summary>
        /// Euclidean distance between two locations.
        /// </summary>
        public static double GetDistance(Point3D a, Point3D b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static int CalculateCost(double distance, double weight)
        {
            return (int)(distance * 20 + weight * 20);
        }

        public static TimeSpan CalculateDelay(double distance, double weight)
        {
            double seconds = 5 + distance * 1 + weight * 0.5;
            return TimeSpan.FromSeconds(seconds);
        }

        public static void ScheduleDelivery(MailItem mail)
        {
            Timer.DelayCall(mail.DeliveryDelay, () => {
                mail.Destination.Accept(mail, mail.Sender);
            });
        }
    }
}
