using System;

namespace Server.Mail
{
	public static class MailProcessor
	{
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
			Timer.DelayCall(mail.DeliveryDelay,
				() =>
				{
					mail.Destination.Accept(mail, mail.Sender);
				});
		}
	}
}
