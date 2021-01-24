// 05.03.29  :: troyan :: polecenia przeniesione na poziom Counselor
// 05.12.18 :: troyan :: dopasowanie do Gmoff

using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targets;
using Server.Network;
using Server.Targeting;

namespace Server.Commands
{
    public class Say
    {
        private static int m_Color;

        public static void Initialize()
        {
            m_Color = 99;
            Register( "Say", AccessLevel.Counselor, new CommandEventHandler( SayCommand ) );
			Register( "S", AccessLevel.Counselor, new CommandEventHandler( SayCommand ) );
            Register( "SetSayColor", AccessLevel.Counselor, new CommandEventHandler( SetSayColorCommand ) );
        }
        public static void Register( string command, AccessLevel access, CommandEventHandler handler )
        {
            CommandSystem.Register( command, access, handler );
        }
        public static int Color
        {
            get { return m_Color; }
        }

        [Usage( "SetSayColor <int>" )]
        [Description( "Defines the color for items speech with the Say command" )]
        public static void SetSayColorCommand( CommandEventArgs e )
        {
            if ( e.Length <= 0 )
                e.Mobile.SendMessage( "Format: SetSayColor \"<int>\"" );
            else
            {
                m_Color = e.GetInt32(0);
                e.Mobile.SendMessage( m_Color, "You choosed this color." );
            }
        }

        [Usage( "Say <text>" )]
        [Description( "Force target to say <text>." )]
        public static void SayCommand( CommandEventArgs e )
        {
            string toSay = e.ArgString.Trim();

            if ( e.Length > 0 )
                e.Mobile.Target =  new SayTarget( toSay );
            else
                e.Mobile.SendMessage( "Format: Say \"<text>\"" );
        }
		
        public class SayTarget : Target
        {
            private string m_toSay;

            public SayTarget( string s ) : base ( -1, false, TargetFlags.None )
            {
                m_toSay = s;
            }
            protected override void OnTarget( Mobile from, object targeted )
            {
                if ( targeted is Mobile )
                {
                    Mobile targ = (Mobile)targeted;

                    if ( from != targ && from.TrueAccessLevel > targ.TrueAccessLevel )
                    {
                        CommandLogging.WriteLine( from, "{0} {1} forcing speech on {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( targ ) ); 
                        targ.Say( m_toSay );
                    }
                }
                else if ( targeted is Item )
                {
                    Item targ = (Item)targeted;

                    targ.PublicOverheadMessage( MessageType.Regular, Say.Color, false, m_toSay );
                }
            }
        }
    }
}
