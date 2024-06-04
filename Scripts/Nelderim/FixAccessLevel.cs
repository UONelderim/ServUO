#region References

using Server.Accounting;

#endregion

namespace Server.Commands
{
	public class FixAccessLevel
	{
		public static void Initialize()
		{
			IAccount iacc = Accounts.GetAccount("owner");
			// iacc.SetPassword("1234");
			CommandSystem.Register("FixAccessLevel", AccessLevel.Seer, FixAccessLevel_OnCommand);
			EventSink.Login += args =>
			{
				var m = args.Mobile;
				var race = m.Race;
				race.AssignDefaultLanguages(m);
			};
		}

		[Usage("FixAccessLevel")]
		[Description("Naprawia accessLevel dla postaci")]
		public static void FixAccessLevel_OnCommand(CommandEventArgs e)
		{
			IAccount iacc = Accounts.GetAccount("owner");
			if (iacc is Account acc && acc.Count > 0 && acc[0].TrueAccessLevel != AccessLevel.Owner)
			{
				foreach (Mobile m in World.Mobiles.Values)
				{
					switch (m.TrueAccessLevel)
					{
						case AccessLevel.VIP:
							m.TrueAccessLevel = AccessLevel.Counselor;
							break;
						case AccessLevel.Counselor:
							m.TrueAccessLevel = AccessLevel.GameMaster;
							break;
						case AccessLevel.Decorator:
							m.TrueAccessLevel = AccessLevel.Seer;
							break;
						case AccessLevel.Spawner:
							m.TrueAccessLevel = AccessLevel.Administrator;
							break;
						case AccessLevel.GameMaster:
							m.TrueAccessLevel = AccessLevel.Developer;
							break;
						case AccessLevel.Seer:
							m.TrueAccessLevel = AccessLevel.Owner;
							break;
					}
				}

				e.Mobile.SendMessage("All fixed");
			}
			else
			{
				e.Mobile.SendMessage("Nothing to do");
			}
		}
	}
}
