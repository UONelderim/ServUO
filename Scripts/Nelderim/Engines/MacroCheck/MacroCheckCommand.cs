using System;
using Server;
using Server.Targeting;
using Server.Mobiles;
using Server.Gumps;
using Server.Engines;

namespace Server.Commands
{
	public class MacroCheckCommand
	{
        public static TimeSpan TimeBetweenMacroChecks = TimeSpan.FromMinutes( 5 );

        public static void Initialize()
		{
			CommandSystem.Register( "MC", AccessLevel.Counselor, new CommandEventHandler( MacroCheck_OnCommand ) );
            CommandSystem.Register( "MacroCheck", AccessLevel.Counselor, new CommandEventHandler( MacroCheck_OnCommand ) );
		}
		
		[Usage( "MacroCheck" )]
		[Description( "Wysylamy gump graczowi, ktory moze makrowac." )]
		private static void MacroCheck_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new MacroCheckTarget();
		}
		
		private class MacroCheckTarget : Target
		{
			public MacroCheckTarget( ) : base( -1, true, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if( o is PlayerMobile )
				{
					PlayerMobile player = o as PlayerMobile;
					
						String text = CheckOpenGump( player , from );
						if( text != null )
						{
							from.SendMessage( text );
						}
						else if( text == null )
						{
							CheckPlayer  check = new CheckPlayer( (PlayerMobile)o, from );
						}
				}
			}
		}
		
		public static string CheckOpenGump( Mobile m , Mobile GM )
		{
			PlayerMobile pm = m as PlayerMobile;
			if( m.NetState == null )
			{
				return "Ten gracz juz sie wylogowal.";
			}
			else if ( GM == m )
			{
				return "Nie mozesz sobie samemu wyslac macrochecka.";
			}
			else if( m.HasGump( typeof (MacroCheckGump) ) ) 
			{
				return "Ten gracz ma juz jednego macrochecka na ekranie!";
			}
			else if( pm.LastMacroCheck + TimeBetweenMacroChecks > DateTime.Now )
			{
				return String.Format("Ten gracz niedawno mial macrochecka. Musisz poczekac jeszcze {0}.", TimeFormat( (  TimeBetweenMacroChecks - ( DateTime.Now - pm.LastMacroCheck ) ) ) );
			}
			return null;
		}

		public static string TimeFormat( TimeSpan time )
		{
			return String.Format( "{0} min i {1} sek", time.Minutes , time.Seconds );
		}
	}
}
