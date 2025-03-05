#region References

using System;
using Server.Mobiles;
using Server.Targeting;

#endregion

namespace Server.Commands
{
	public class BondTime
	{
		public static void Initialize()
		{
			CommandSystem.Register("BondTime", AccessLevel.Player, BondTime_Command);
		}

		public static void BondTime_Command(CommandEventArgs e)
		{
			e.Mobile.SendMessage("Ktore zwierze chcesz sprawdzic?");
			e.Mobile.Target = new BondTimeTarget();
		}

		public class BondTimeTarget : Target
		{
			public BondTimeTarget() : base(15, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object obj)
			{
				if (!(obj is Mobile) || !(obj is BaseCreature))
				{
					from.SendMessage("Musisz wybrac zwirze.");
					return;
				}

				BaseCreature m = (BaseCreature)obj;

				if (m.Controlled == false)
				{
					from.SendMessage("Zwierze nie jest oswojone!");
					return;
				}

				if (m.IsBonded)
				{
					from.SendMessage("Ten zwierzak jest juz przywiazany.");
					return;
				}

				if (!(m.ControlMaster == from) && from.AccessLevel < AccessLevel.Player)
				{
					from.SendMessage("Ten zwierzak nie nalezy do ciebie!");
					return;
				}

				if (DateTime.UtcNow - m.BondingBegin > TimeSpan.FromDays(7))
				{
					from.SendMessage(
						"Przywiazywanie zwierzaka jeszcze sie nie rozpoczelo. Nakarm go po ponownym oswojeniu jesli potrafisz.");
					return;
				}

				TimeSpan timeleft = m.BondingBegin + TimeSpan.FromDays(7) - DateTime.UtcNow;
				from.SendMessage(
					"{0} Dni, {1} godzin, {2} minut i {3} sekund zostalo do przywiazania sie zwierzaka do ciebie. Zierzak przywiaze sie do ciebie: {4}",
					timeleft.Days, timeleft.Hours, timeleft.Minutes, timeleft.Seconds,
					m.BondingBegin + TimeSpan.FromDays(7));
			}
		}
	}
}
