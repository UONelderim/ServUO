using System;
using System.Collections;

using Server;
using Server.Gumps;

namespace Arya.Jail
{
	/// <summary>
	/// This gump displays a jailing entry
	/// </summary>
	public class JailCommentGump : Gump
	{
		private const int LabelHue = 1152;
		private const int GreenHue = 64;
		private const int RedHue = 32;

		private Mobile m_Player;
		private Mobile m_Admin;

		public JailCommentGump( Mobile player , Mobile admin ) : base( 50, 50 )
		{
			admin.CloseGump( typeof( JailCommentGump ) );

			m_Player = player;
			m_Admin = admin;

			MakeGump();
		}

		private void MakeGump()
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);

			this.AddBackground(0, 0, 600, 400, 9250);
			this.AddAlphaRegion(15, 15, 570, 370);

			// Title
			this.AddLabel(20, 20, RedHue, @"Player Comments");

			// Player
			this.AddLabel(290, 20, LabelHue, @"Player:");
			this.AddButton(269, 20, 5601, 5605, 1, GumpButtonType.Reply, 0);
			this.AddLabel(350, 20, GreenHue, m_Player.Name );
			
			// Comments
			
			string html = JailSystem.CheckComments( m_Player );
			
			
			this.AddLabel(20, 40, LabelHue, @"Comments:");
			this.AddImageTiled(19, 59, 562, 152, 3604);
			this.AddAlphaRegion(20, 60, 560, 150);
			this.AddHtml( 20, 60, 560, 150, html, false, true);
			
			
			// New comment: Text 2
			this.AddLabel(290, 210, LabelHue, @"New comment");
			this.AddImageTiled(289, 229, 292, 102, 5154);
			this.AddAlphaRegion(290, 230, 290, 100);
			this.AddTextEntry(290, 230, 290, 100, LabelHue, 1, @"");

			// Add comment: Button 5
			this.AddLabel(330, 360, LabelHue, @"Add comment");
			this.AddButton(290, 360, 4011, 4012, 2, GumpButtonType.Reply, 0);

			// Close: button 0
			this.AddButton(450, 360, 4023, 4024, 0, GumpButtonType.Reply, 0);
			this.AddLabel(490, 360, LabelHue, @"Close");
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			
			switch ( info.ButtonID )
			{
				case 1: // Display player props

					m_Admin.SendGump( new JailCommentGump( m_Player , m_Admin ) );
					
					if ( m_Player != null )
					{
						m_Admin.SendGump( new PropertiesGump( m_Admin, m_Player ) );
					}
					break;

				case 2: // Add comment
					
					
					TextRelay te1 = info.GetTextEntry( 1 );
					string comment = te1.Text;

					if ( comment != null && comment.Length > 0 )
					{
						
						PlayerCommentEntry entry = new PlayerCommentEntry( m_Player , comment );
						JailSystem.JailComments.Add( entry );
					}
					else
					{
						m_Admin.SendMessage( "Can't add an empty comment" );
					}
					
					
					m_Admin.SendGump( new JailCommentGump( m_Player , m_Admin ) );
					break;

				
			}
			
		}

	}
}