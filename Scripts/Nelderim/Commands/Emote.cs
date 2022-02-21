// 05.03.29  :: troyan :: polecenia przeniesione na poziom Counselor
// 05.12.18 :: troyan :: dopasowanie do Gmoff

#region References

using Server.Network;
using Server.Targeting;

#endregion

namespace Server.Commands
{
	public class emote
	{
		public static void Initialize()
		{
			Color = 99;
			Register("Em", AccessLevel.Counselor, emoteCommand);
			Register("SetemoteColor", AccessLevel.Counselor, SetemoteColorCommand);
		}

		public static void Register(string command, AccessLevel access, CommandEventHandler handler)
		{
			CommandSystem.Register(command, access, handler);
		}

		public static int Color { get; private set; }

		[Usage("SetemoteColor <int>")]
		[Description("Defines the color for items speech with the emote command")]
		public static void SetemoteColorCommand(CommandEventArgs e)
		{
			if (e.Length <= 0)
				e.Mobile.SendMessage("Format: SetemoteColor \"<int>\"");
			else
			{
				Color = e.GetInt32(0);
				e.Mobile.SendMessage(Color, "You choosed this color.");
			}
		}

		[Usage("em <text>")]
		[Description("Force target to emote <text>.")]
		public static void emoteCommand(CommandEventArgs e)
		{
			string toemote = e.ArgString.Trim();

			if (e.Length > 0)
				e.Mobile.Target = new emoteTarget(toemote);
			else
				e.Mobile.SendMessage("Format: emote \"<text>\"");
		}

		public class emoteTarget : Target
		{
			private readonly string m_toemote;

			public emoteTarget(string s) : base(-1, false, TargetFlags.None)
			{
				m_toemote = s;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Mobile)
				{
					Mobile targ = (Mobile)targeted;

					if (from != targ && from.TrueAccessLevel > targ.TrueAccessLevel)
					{
						CommandLogging.WriteLine(from, "{0} {1} forcing speech on {2}", from.TrueAccessLevel,
							CommandLogging.Format(from), CommandLogging.Format(targ));
						targ.Say("*" + m_toemote + "*");
					}
				}
				else if (targeted is Item)
				{
					Item targ = (Item)targeted;

					targ.PublicOverheadMessage(MessageType.Regular, Say.Color, false, "*" + m_toemote + "*");
				}
			}
		}
	}
}
