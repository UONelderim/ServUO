// 05.03.29  :: troyan :: polecenia przeniesione na poziom Counselor
// 05.12.18 :: troyan :: dopasowanie do Gmoff

#region References

using Server.Network;
using Server.Targeting;

#endregion

namespace Server.Commands
{
	public class Say
	{
		public static void Initialize()
		{
			Color = 99;
			Register("Say", AccessLevel.Counselor, SayCommand);
			Register("SetSayColor", AccessLevel.Counselor, SetSayColorCommand);
		}

		public static void Register(string command, AccessLevel access, CommandEventHandler handler)
		{
			CommandSystem.Register(command, access, handler);
		}

		public static int Color { get; private set; }

		[Usage("SetSayColor <int>")]
		[Description("Defines the color for items speech with the Say command")]
		public static void SetSayColorCommand(CommandEventArgs e)
		{
			if (e.Length <= 0)
				e.Mobile.SendMessage("Format: SetSayColor \"<int>\"");
			else
			{
				Color = e.GetInt32(0);
				e.Mobile.SendMessage(Color, "You choosed this color.");
			}
		}

		[Usage("Say <text>")]
		[Description("Force target to say <text>.")]
		public static void SayCommand(CommandEventArgs e)
		{
			string toSay = e.ArgString.Trim();

			if (e.Length > 0)
				e.Mobile.Target = new SayTarget(toSay);
			else
				e.Mobile.SendMessage("Format: Say \"<text>\"");
		}

		public class SayTarget : Target
		{
			private readonly string m_toSay;

			public SayTarget(string s) : base(-1, false, TargetFlags.None)
			{
				m_toSay = s;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Mobile)
				{
					Mobile targ = (Mobile)targeted;

					if (from != targ && from.TrueAccessLevel > targ.TrueAccessLevel)
					{
						CommandLogging.WriteLine(from, "{0} {1} forcing speech on {2}", from.AccessLevel,
							CommandLogging.Format(from), CommandLogging.Format(targ));
						targ.Say(m_toSay);
					}
				}
				else if (targeted is Item)
				{
					Item targ = (Item)targeted;

					targ.PublicOverheadMessage(MessageType.Regular, Color, false, m_toSay);
				}
			}
		}
	}
}
