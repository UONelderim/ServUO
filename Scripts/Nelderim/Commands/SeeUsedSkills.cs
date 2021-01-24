using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class SeeUsedSkills
    {
        public static void Initialize()
        {
			Register( "SeeUsedSkills", AccessLevel.Counselor, new CommandEventHandler( SeeUsedSkillsCommand ) );
			Register( "SUS", AccessLevel.Counselor, new CommandEventHandler( SeeUsedSkillsCommand ) );
        }
		
        public static void Register( string command, AccessLevel access, CommandEventHandler handler )
        {
            CommandSystem.Register( command, access, handler );
        }

        [Usage( "SeeUsedSkills <off>" )]
		[Aliases( "SUS" )]
        [Description( "Pokazuje uzywane przez cel skille do wylaczenia (off) lub przeminiecia 60 sekund" )]
        public static void SeeUsedSkillsCommand( CommandEventArgs e )
        {
			if ( e.GetString( 0 ) == "off" )
				e.Mobile.Target = new SeeUsedSkillsTarget( false );
            else
                e.Mobile.Target = new SeeUsedSkillsTarget( true );
        }
		
        public class SeeUsedSkillsTarget : Target
        {
            private bool m_Start;
            
			
			public SeeUsedSkillsTarget( bool start ) : base ( -1, false, TargetFlags.None )
            {
                m_Start = start;
            }
			
            protected override void OnTarget( Mobile from, object targeted )
            {
                if ( targeted is PlayerMobile )
                {
					PlayerMobile pm = targeted as PlayerMobile;
					
					string log = from.AccessLevel + " " + CommandLogging.Format( from );
					log += " tried to See " + CommandLogging.Format( targeted );
					log += " Used Skills [SUS]";
					
					CommandLogging.WriteLine( from, log );
					
					if ( pm.AccessLevel >= from.AccessLevel )
						from.SendLocalizedMessage( 505849 ); // Masz zbyt niskie uprawnienia do sledzenia poczynan celu!
					else
					{
						if ( m_Start )
						{
							pm.StartTracking( from );
							from.SendLocalizedMessage( 505850 ); // Zaprzestano sledzenia celu.
						}
						else
						{
							pm.StopTracking( from );
						    from.SendLocalizedMessage( 505851, pm.Name ); // "Rozpoczeto sledzenie poczynan celu -> {0}"
						}
					}
                }
                else
                	from.SendLocalizedMessage( 505852 ); // "Cel nie jest graczem."
            }
        }
    }
}
