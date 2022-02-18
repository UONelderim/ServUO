#region References

using System;
using System.Collections;
using Server;
using Server.Commands;
using Server.Network;

#endregion

namespace Knives.TownHouses
{
	public class Errors
	{
		public static ArrayList ErrorLog { get; } = new ArrayList();
		public static ArrayList Checked { get; } = new ArrayList();

		public static void Initialize()
		{
			CommandSystem.Register("TownHouseErrors", AccessLevel.Counselor, OnErrors);
			CommandSystem.Register("therrors", AccessLevel.Counselor, OnErrors);

			EventSink.Login += OnLogin;
		}

		private static void OnErrors(CommandEventArgs e)
		{
			if (e.ArgString == null || e.ArgString == "")
				new ErrorsGump(e.Mobile);
			else
				Report(e.ArgString + " - " + e.Mobile.Name);
		}

		private static void OnLogin(LoginEventArgs e)
		{
			if (e.Mobile.AccessLevel != AccessLevel.Player
			    && ErrorLog.Count != 0
			    && !Checked.Contains(e.Mobile))
				new ErrorsNotifyGump(e.Mobile);
		}

		public static void Report(string error)
		{
			ErrorLog.Add(String.Format("<B>{0}</B><BR>{1}<BR>", DateTime.Now, error));

			Checked.Clear();

			Notify();
		}

		private static void Notify()
		{
			foreach (NetState state in NetState.Instances)
			{
				if (state.Mobile == null)
					continue;

				if (state.Mobile.AccessLevel != AccessLevel.Player)
					Notify(state.Mobile);
			}
		}

		public static void Notify(Mobile m)
		{
			if (m.HasGump(typeof(ErrorsGump)))
				new ErrorsGump(m);
			else
				new ErrorsNotifyGump(m);
		}
	}
}
